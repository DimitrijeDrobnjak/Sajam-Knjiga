using System;
using System.Collections.Generic;
using System.Text;
using Core.Interfaces;
using Core.Models;
using Core.Utils;

namespace Core.Services;

public class AdresaService
{
    private readonly IDao<Adresa> _adrese;
    public AdresaService(IDao<Adresa> adrese)
    {
        _adrese = adrese;
    }

    public List<Adresa> GetAll()
    {
        return _adrese.GetAll();
    }

    public Adresa? GetById(int id)
    {
        return _adrese.GetById(id);
    }
    public Adresa Add(Adresa adresa)
    {
        Provera(adresa);
        return _adrese.Add(adresa);
    }

    public void Update(Adresa adresa)
    {
        if (_adrese.GetById(adresa.Id) == null)
            throw new ValidacijaException("Adresa sa tim ID-om ne postoji!");

        Provera(adresa);
        _adrese.Update(adresa);
    }

    public void Delete(int id)
    {
        if (_adrese.GetById(id) == null)
            throw new ValidacijaException("Adresa sa tim ID-om ne postoji!");

        _adrese.Delete(id);
    }

    private void Provera(Adresa adresa)
    {
        Validator.Obavezno(adresa.Ulica, "Ulica");
        Validator.Obavezno(adresa.Broj, "Broj");
        Validator.Obavezno(adresa.Grad, "Grad");
        Validator.Obavezno(adresa.Drzava, "Drzava");
    }
}