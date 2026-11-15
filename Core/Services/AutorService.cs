using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Models;
using Core.Utils;

namespace Core.Services;

public class AutorService
{
    private readonly IDao<Autor> _autori;

    public AutorService(IDao<Autor> autori)
    {
        _autori = autori;
    }

    public List<Autor> GetAll()
    {
        return _autori.GetAll();
    }

    public Autor? GetById(int id)
    {
        return _autori.GetById(id);
    }

    public Autor Add(Autor autor)
    {
        Provera(autor, 0);

        autor.KnjigaIds = new List<int>();

        return _autori.Add(autor);
    }

    public void Update(Autor autor)
    {
        Autor? postojeci = _autori.GetById(autor.Id);

        if (postojeci == null)
            throw new ValidacijaException("Autor sa tim ID-om ne postoji!");

        Provera(autor, autor.Id);

        postojeci.Ime = autor.Ime;
        postojeci.Prezime = autor.Prezime;
        postojeci.DatumRodjenja = autor.DatumRodjenja;
        postojeci.AdresaId = autor.AdresaId;
        postojeci.Telefon = autor.Telefon;
        postojeci.Email = autor.Email;
        postojeci.BrojLicneKarte = autor.BrojLicneKarte;
        postojeci.GodineIskustva = autor.GodineIskustva;

        _autori.Update(postojeci);
    }

    public void Delete(int id)
    {
        if (_autori.GetById(id) == null)
            throw new ValidacijaException("Autor sa tim ID-om ne postoji!");

        _autori.Delete(id);
    }

    private void Provera(Autor autor, int excludeId)
    {
        Validator.Obavezno(autor.Ime, "Ime");
        Validator.Obavezno(autor.Prezime, "Prezime");
        Validator.ProveriDatumRodjenja(autor.DatumRodjenja);
        Validator.ProveriTelefon(autor.Telefon);
        Validator.ProveriEmail(autor.Email);
        Validator.Obavezno(autor.BrojLicneKarte, "Broj licne karte");

        if (autor.GodineIskustva < 0)
            throw new ValidacijaException("Godine iskustva ne mogu biti negativne.");

        bool licnaKartaZauzeta = _autori.GetAll().Any(x =>
            x.Id != excludeId &&
            string.Equals(x.BrojLicneKarte, autor.BrojLicneKarte, StringComparison.OrdinalIgnoreCase));

        if (licnaKartaZauzeta)
            throw new ValidacijaException("Autor sa tim brojem licne karte vec postoji.");
    }
}
