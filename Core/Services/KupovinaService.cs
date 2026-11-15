using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Models;
using Core.Utils;

namespace Core.Services;

public class KupovinaService {
    private readonly IDao<Kupovina> _kupovine;
    private readonly IDao<Posetilac> _posetioci;
    private readonly IDao<Knjiga> _knjige;

    public KupovinaService(
        IDao<Kupovina> kupovine,
        IDao<Posetilac> posetioci,
        IDao<Knjiga> knjige) {
        _kupovine = kupovine;
        _posetioci = posetioci;
        _knjige = knjige;
    }

    public List<Kupovina> GetAll() => _kupovine.GetAll();

    public Kupovina? GetById(int id) => _kupovine.GetById(id);

    public List<Posetilac> GetPosetioci() => _posetioci.GetAll();

    public List<Knjiga> GetKnjige() => _knjige.GetAll();

    public string ImePosetioca(int id) {
        Posetilac? p = _posetioci.GetById(id);
        return p == null ? "(obrisan)" : $"{p.Ime} {p.Prezime} ({p.BrojClanskeKarte})";
    }

    public string NazivKnjige(int id) {
        Knjiga? k = _knjige.GetById(id);
        return k == null ? "(obrisana)" : k.Naziv;
    }

    public Kupovina Add(Kupovina kupovina) {
        Posetilac posetilac = _posetioci.GetById(kupovina.PosetilacId)
            ?? throw new ValidacijaException("Posetilac sa datim Id ne postoji.");
        Knjiga knjiga = _knjige.GetById(kupovina.KnjigaId)
            ?? throw new ValidacijaException("Knjiga sa datim Id ne postoji.");

        ValidirajPolja(kupovina.DatumKupovine, kupovina.Ocena);

        _kupovine.Add(kupovina);

        posetilac.KupovinaIds.Add(kupovina.Id);
        if (!knjiga.PosetiociKupiliIds.Contains(posetilac.Id))
            knjiga.PosetiociKupiliIds.Add(posetilac.Id);

        AzurirajProsek(posetilac);
        _posetioci.Update(posetilac);
        _knjige.Update(knjiga);
        return kupovina;
    }

    // posetilac i knjiga se ne menjaju; za to obrisati i ponovo uneti kupovinu
    public void Update(int id, DateTime datum, int ocena, string komentar) {
        Kupovina kupovina = _kupovine.GetById(id)
            ?? throw new ValidacijaException("Kupovina sa datim Id ne postoji.");

        ValidirajPolja(datum, ocena);

        kupovina.DatumKupovine = datum;
        kupovina.Ocena = ocena;
        kupovina.Komentar = komentar;
        _kupovine.Update(kupovina);

        Posetilac? posetilac = _posetioci.GetById(kupovina.PosetilacId);
        if (posetilac != null) {
            AzurirajProsek(posetilac);
            _posetioci.Update(posetilac);
        }
    }

    public void Delete(int id) {
        Kupovina kupovina = _kupovine.GetById(id)
            ?? throw new ValidacijaException("Kupovina sa datim Id ne postoji.");

        _kupovine.Delete(id);

        Posetilac? posetilac = _posetioci.GetById(kupovina.PosetilacId);
        if (posetilac != null) {
            posetilac.KupovinaIds.Remove(id);
            AzurirajProsek(posetilac);
            _posetioci.Update(posetilac);
        }

        Knjiga? knjiga = _knjige.GetById(kupovina.KnjigaId);
        bool istaKnjigaJosKupljena = _kupovine.GetAll().Any(x =>
            x.PosetilacId == kupovina.PosetilacId && x.KnjigaId == kupovina.KnjigaId);
        if (knjiga != null && !istaKnjigaJosKupljena) {
            knjiga.PosetiociKupiliIds.Remove(kupovina.PosetilacId);
            _knjige.Update(knjiga);
        }
    }

    private void AzurirajProsek(Posetilac posetilac) {
        List<int> ocene = _kupovine.GetAll()
            .Where(x => x.PosetilacId == posetilac.Id)
            .Select(x => x.Ocena)
            .ToList();
        posetilac.ProsecnaOcena = ocene.Count == 0 ? 0 : Math.Round(ocene.Average(), 2);
    }

    private static void ValidirajPolja(DateTime datum, int ocena) {
        if (ocena < 1 || ocena > 5)
            throw new ValidacijaException("Ocena mora biti izmedju 1 i 5.");
        if (datum.Date > DateTime.Today)
            throw new ValidacijaException("Datum kupovine ne moze biti u buducnosti.");
    }
}
