using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Models;
using Core.Utils;

namespace Core.Services;

public class PosetilacService {
    private readonly IDao<Posetilac> _posetioci;
    private readonly IDao<Adresa> _adrese;
    private readonly IDao<Kupovina> _kupovine;
    private readonly IDao<Knjiga> _knjige;

    public PosetilacService(
        IDao<Posetilac> posetioci,
        IDao<Adresa> adrese,
        IDao<Kupovina> kupovine,
        IDao<Knjiga> knjige) {
        _posetioci = posetioci;
        _adrese = adrese;
        _kupovine = kupovine;
        _knjige = knjige;
    }

    public List<Posetilac> GetAll() => _posetioci.GetAll();

    public Posetilac? GetById(int id) => _posetioci.GetById(id);

    public Adresa? GetAdresa(Posetilac posetilac) => _adrese.GetById(posetilac.AdresaId);

    public string AdresaTekst(Posetilac posetilac) {
        Adresa? a = GetAdresa(posetilac);
        return a == null ? "-" : $"{a.Ulica} {a.Broj}, {a.Grad}, {a.Drzava}";
    }

    public Posetilac Add(Posetilac posetilac, Adresa adresa) {
        Validate(posetilac, adresa, 0);

        Adresa sacuvanaAdresa = _adrese.Add(adresa);
        posetilac.AdresaId = sacuvanaAdresa.Id;
        posetilac.KupovinaIds = new List<int>();
        posetilac.ListaZeljaKnjigaIds = new List<int>();
        posetilac.ProsecnaOcena = 0;
        return _posetioci.Add(posetilac);
    }

    // kupovine, lista zelja i prosecna ocena se ne menjaju rucno
    public void Update(int id, Posetilac izmene, Adresa adresa) {
        Posetilac postojeci = _posetioci.GetById(id)
            ?? throw new ValidacijaException("Posetilac sa datim Id ne postoji.");

        Validate(izmene, adresa, id);

        postojeci.BrojClanskeKarte = izmene.BrojClanskeKarte;
        postojeci.Ime = izmene.Ime;
        postojeci.Prezime = izmene.Prezime;
        postojeci.DatumRodjenja = izmene.DatumRodjenja;
        postojeci.Telefon = izmene.Telefon;
        postojeci.Email = izmene.Email;
        postojeci.GodinaClanstva = izmene.GodinaClanstva;
        postojeci.Status = izmene.Status;

        Adresa? staraAdresa = _adrese.GetById(postojeci.AdresaId);
        if (staraAdresa != null) {
            staraAdresa.Ulica = adresa.Ulica;
            staraAdresa.Broj = adresa.Broj;
            staraAdresa.Grad = adresa.Grad;
            staraAdresa.Drzava = adresa.Drzava;
            _adrese.Update(staraAdresa);
        } else {
            Adresa nova = _adrese.Add(adresa);
            postojeci.AdresaId = nova.Id;
        }

        _posetioci.Update(postojeci);
    }

    // brise i njegove kupovine i uklanja ga sa knjiga
    public void Delete(int id) {
        Posetilac posetilac = _posetioci.GetById(id)
            ?? throw new ValidacijaException("Posetilac sa datim Id ne postoji.");

        foreach (Kupovina kupovina in _kupovine.GetAll().Where(x => x.PosetilacId == id))
            _kupovine.Delete(kupovina.Id);

        foreach (Knjiga knjiga in _knjige.GetAll()) {
            int uklonjenoKupili = knjiga.PosetiociKupiliIds.RemoveAll(x => x == id);
            int uklonjenoZelja = knjiga.PosetiociZeljaIds.RemoveAll(x => x == id);
            if (uklonjenoKupili + uklonjenoZelja > 0)
                _knjige.Update(knjiga);
        }

        _adrese.Delete(posetilac.AdresaId);
        _posetioci.Delete(id);
    }

    private void Validate(Posetilac p, Adresa a, int excludeId) {
        Validator.ProveriClanskuKartu(p.BrojClanskeKarte);
        bool kartaZauzeta = _posetioci.GetAll().Any(x =>
            x.Id != excludeId &&
            string.Equals(x.BrojClanskeKarte, p.BrojClanskeKarte, StringComparison.OrdinalIgnoreCase));
        if (kartaZauzeta)
            throw new ValidacijaException("Posetilac sa tim brojem clanske karte vec postoji.");

        Validator.Obavezno(p.Ime, "Ime");
        Validator.Obavezno(p.Prezime, "Prezime");
        Validator.ProveriDatumRodjenja(p.DatumRodjenja);
        Validator.ProveriTelefon(p.Telefon);
        Validator.ProveriEmail(p.Email);

        if (p.GodinaClanstva <= 0)
            throw new ValidacijaException("Godina clanstva mora biti pozitivan broj.");

        Validator.Obavezno(a.Ulica, "Ulica");
        Validator.Obavezno(a.Broj, "Broj");
        Validator.Obavezno(a.Grad, "Grad");
        Validator.Obavezno(a.Drzava, "Drzava");
    }
}
