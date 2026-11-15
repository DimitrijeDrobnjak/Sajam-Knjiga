
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Core.Models;
using Core.Utils;

namespace Core.Services;

public class KnjigaService
{
    private readonly IDao<Knjiga> _knjige;
    private readonly IDao<Izdavac> _izdavaci;

    public KnjigaService(IDao<Knjiga> knjige, IDao<Izdavac> izdavaci)
    {
        _knjige = knjige;
        _izdavaci = izdavaci;
    }

    public List<Knjiga> GetAll()
    {
        return _knjige.GetAll();
    }

    public Knjiga? GetById(int id)
    {
        return _knjige.GetById(id);
    }

    public Knjiga Add(Knjiga knjiga)
    {
        Provera(knjiga, 0);

        knjiga.AutorIds = new List<int>();
        knjiga.PosetiociKupiliIds = new List<int>();
        knjiga.PosetiociZeljaIds = new List<int>();

        return _knjige.Add(knjiga);
    }

    public void Update(Knjiga knjiga)
    {
        Knjiga? postojeca = _knjige.GetById(knjiga.Id);

        if (postojeca == null)
            throw new ValidacijaException("Knjiga sa tim ID-om ne postoji!");

        Provera(knjiga, knjiga.Id);

        postojeca.Isbn = knjiga.Isbn;
        postojeca.Naziv = knjiga.Naziv;
        postojeca.Zanr = knjiga.Zanr;
        postojeca.GodinaIzdanja = knjiga.GodinaIzdanja;
        postojeca.Cena = knjiga.Cena;
        postojeca.BrojStrana = knjiga.BrojStrana;
        postojeca.IzdavacId = knjiga.IzdavacId;

        _knjige.Update(postojeca);
    }

    public void Delete(int id)
    {
        if (_knjige.GetById(id) == null)
            throw new ValidacijaException("Knjiga sa tim ID-om ne postoji!");

        _knjige.Delete(id);
    }

    private void Provera(Knjiga knjiga, int excludeId)
    {
        Validator.Obavezno(knjiga.Isbn, "ISBN");
        Validator.Obavezno(knjiga.Naziv, "Naziv");
        Validator.Obavezno(knjiga.Zanr, "Zanr");

        if (knjiga.GodinaIzdanja < 1 || knjiga.GodinaIzdanja > DateTime.Now.Year)
            throw new ValidacijaException("Godina izdanja nije ispravna!");

        if (knjiga.Cena < 0)
            throw new ValidacijaException("Cena ne moze biti negativna!");

        if (knjiga.BrojStrana <= 0)
            throw new ValidacijaException("Broj strana mora biti veci od nule!");

        if (_izdavaci.GetById(knjiga.IzdavacId) == null)
            throw new ValidacijaException("Izdavac sa tim ID-om ne postoji!");

        bool isbnZauzet = _knjige.GetAll().Any(x =>
            x.Id != excludeId &&
            string.Equals(x.Isbn, knjiga.Isbn, StringComparison.OrdinalIgnoreCase));

        if (isbnZauzet)
            throw new ValidacijaException("Knjiga sa tim ISBN brojem vec postoji.");
    }
}
