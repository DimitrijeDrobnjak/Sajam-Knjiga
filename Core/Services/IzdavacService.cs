using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Models;
using Core.Utils;

namespace Core.Services;

public class IzdavacService {
    private const int MinGodinaIskustvaSefa = 5;

    private readonly IDao<Izdavac> _izdavaci;
    private readonly IDao<Autor> _autori;
    private readonly IDao<Knjiga> _knjige;

    public IzdavacService(
        IDao<Izdavac> izdavaci,
        IDao<Autor> autori,
        IDao<Knjiga> knjige) {
        _izdavaci = izdavaci;
        _autori = autori;
        _knjige = knjige;
    }

    public List<Izdavac> GetAll() => _izdavaci.GetAll();

    public Izdavac? GetById(int id) => _izdavaci.GetById(id);

    public List<Autor> GetAutori() => _autori.GetAll();

    public List<Knjiga> GetKnjige() => _knjige.GetAll();

    public string ImeAutora(int id) {
        Autor? a = _autori.GetById(id);
        return a == null ? "(obrisan)" : $"{a.Ime} {a.Prezime}";
    }

    public string NazivKnjige(int id) {
        Knjiga? k = _knjige.GetById(id);
        return k == null ? "(obrisana)" : k.Naziv;
    }

    public Izdavac Add(Izdavac izdavac) {
        Validate(izdavac, 0);
        _izdavaci.Add(izdavac);
        PovezKnjige(izdavac, new List<int>());
        return izdavac;
    }

    public void Update(int id, Izdavac izmene) {
        Izdavac postojeci = _izdavaci.GetById(id)
            ?? throw new ValidacijaException("Izdavac sa datim Id ne postoji.");

        Validate(izmene, id);

        List<int> stareKnjige = postojeci.KnjigaIds.ToList();

        postojeci.Sifra = izmene.Sifra;
        postojeci.Naziv = izmene.Naziv;
        postojeci.SefId = izmene.SefId;
        postojeci.AutorIds = izmene.AutorIds;
        postojeci.KnjigaIds = izmene.KnjigaIds;
        _izdavaci.Update(postojeci);

        PovezKnjige(postojeci, stareKnjige);
    }

    // knjige ostaju, ali bez izdavaca
    public void Delete(int id) {
        if (_izdavaci.GetById(id) == null)
            throw new ValidacijaException("Izdavac sa datim Id ne postoji.");

        foreach (Knjiga knjiga in _knjige.GetAll().Where(x => x.IzdavacId == id)) {
            knjiga.IzdavacId = 0;
            _knjige.Update(knjiga);
        }

        _izdavaci.Delete(id);
    }

    private void Validate(Izdavac i, int excludeId) {
        Validator.Obavezno(i.Sifra, "Sifra");
        Validator.Obavezno(i.Naziv, "Naziv");

        bool sifraZauzeta = _izdavaci.GetAll().Any(x =>
            x.Id != excludeId &&
            string.Equals(x.Sifra, i.Sifra, StringComparison.OrdinalIgnoreCase));
        if (sifraZauzeta)
            throw new ValidacijaException("Izdavac sa tom sifrom vec postoji.");

        Autor sef = _autori.GetById(i.SefId)
            ?? throw new ValidacijaException("Autor koji je zadat kao sef ne postoji.");
        if (sef.GodineIskustva < MinGodinaIskustvaSefa)
            throw new ValidacijaException(
                $"Sef izdavaca mora imati najmanje {MinGodinaIskustvaSefa} godina iskustva (ovaj autor ima {sef.GodineIskustva}).");

        i.AutorIds = i.AutorIds.Distinct().ToList();
        foreach (int autorId in i.AutorIds) {
            if (_autori.GetById(autorId) == null)
                throw new ValidacijaException($"Autor sa Id {autorId} ne postoji.");
        }
        if (!i.AutorIds.Contains(i.SefId))
            i.AutorIds.Add(i.SefId);

        i.KnjigaIds = i.KnjigaIds.Distinct().ToList();
        foreach (int knjigaId in i.KnjigaIds) {
            if (_knjige.GetById(knjigaId) == null)
                throw new ValidacijaException($"Knjiga sa Id {knjigaId} ne postoji.");
        }
    }

    // Odrzava obe strane veze: Izdavac.KnjigaIds <-> Knjiga.IzdavacId
    private void PovezKnjige(Izdavac izdavac, IEnumerable<int> stareKnjige) {
        foreach (int knjigaId in stareKnjige.Except(izdavac.KnjigaIds)) {
            Knjiga? uklonjena = _knjige.GetById(knjigaId);
            if (uklonjena != null && uklonjena.IzdavacId == izdavac.Id) {
                uklonjena.IzdavacId = 0;
                _knjige.Update(uklonjena);
            }
        }

        foreach (int knjigaId in izdavac.KnjigaIds) {
            Knjiga? knjiga = _knjige.GetById(knjigaId);
            if (knjiga == null)
                continue;

            if (knjiga.IzdavacId != 0 && knjiga.IzdavacId != izdavac.Id) {
                Izdavac? stariIzdavac = _izdavaci.GetById(knjiga.IzdavacId);
                if (stariIzdavac != null) {
                    stariIzdavac.KnjigaIds.Remove(knjiga.Id);
                    _izdavaci.Update(stariIzdavac);
                }
            }

            if (knjiga.IzdavacId != izdavac.Id) {
                knjiga.IzdavacId = izdavac.Id;
                _knjige.Update(knjiga);
            }
        }
    }
}
