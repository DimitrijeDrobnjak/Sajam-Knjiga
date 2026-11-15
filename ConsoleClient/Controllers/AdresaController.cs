
using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleClient.Utils;
using Core.Models;
using Core.Services;
using Core.Utils;

namespace ConsoleClient.Controllers;

public class AdresaController
{
    private readonly AdresaService _service;

    public AdresaController(AdresaService service)
    {
        _service = service;
    }

    public void Meni()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== ADRESE ===");
            Console.WriteLine("1. Prikaz svih adresa");
            Console.WriteLine("2. Dodavanje adrese");
            Console.WriteLine("3. Izmena adrese");
            Console.WriteLine("4. Brisanje adrese");
            Console.WriteLine("0. Nazad");

            string izbor = ConsoleInput.ReadString("Izbor");

            try
            {
                switch (izbor)
                {
                    case "1": Prikaz(); break;
                    case "2": Dodavanje(); break;
                    case "3": Izmena(); break;
                    case "4": Brisanje(); break;
                    case "0": return;
                    default: Console.WriteLine("Nepoznata opcija."); break;
                }
            }
            catch (ValidacijaException ex)
            {
                Console.WriteLine($"Greska: {ex.Message}");
            }
        }
    }

    public void Prikaz()
    {
        string[] zaglavlje = {
            "Id", "Ulica", "Broj", "Grad", "Drzava"
        };

        List<string[]> redovi = _service.GetAll().Select(a => new[] {
            a.Id.ToString(),
            a.Ulica,
            a.Broj,
            a.Grad,
            a.Drzava
        }).ToList();

        Console.WriteLine();
        ConsoleTable.Print(zaglavlje, redovi);
    }

    private void Dodavanje()
    {
        Console.WriteLine("\n--- Dodavanje adrese ---");

        Adresa adresa = new Adresa
        {
            Ulica = ConsoleInput.ReadString("Ulica"),
            Broj = ConsoleInput.ReadString("Broj"),
            Grad = ConsoleInput.ReadString("Grad"),
            Drzava = ConsoleInput.ReadString("Drzava")
        };

        Adresa dodata = _service.Add(adresa);

        Console.WriteLine($"Adresa je dodata (Id = {dodata.Id}).");
    }

    private void Izmena()
    {
        Console.WriteLine("\n--- Izmena adrese ---");
        Prikaz();

        int id = ConsoleInput.ReadInt("Id adrese za izmenu", 1, int.MaxValue);

        Adresa stara = _service.GetById(id)
            ?? throw new ValidacijaException("Adresa sa datim Id ne postoji.");

        Console.WriteLine("(Enter zadrzava trenutnu vrednost)");

        Adresa izmene = new Adresa
        {
            Id = id,
            Ulica = ConsoleInput.ReadString("Ulica", stara.Ulica),
            Broj = ConsoleInput.ReadString("Broj", stara.Broj),
            Grad = ConsoleInput.ReadString("Grad", stara.Grad),
            Drzava = ConsoleInput.ReadString("Drzava", stara.Drzava)
        };

        _service.Update(izmene);

        Console.WriteLine("Adresa je izmenjena.");
    }

    private void Brisanje()
    {
        Console.WriteLine("\n--- Brisanje adrese ---");
        Prikaz();

        int id = ConsoleInput.ReadInt("Id adrese za brisanje", 1, int.MaxValue);

        Adresa adresa = _service.GetById(id)
            ?? throw new ValidacijaException("Adresa sa datim Id ne postoji.");

        Console.WriteLine($"Brise se: {adresa.Ulica} {adresa.Broj}, {adresa.Grad}, {adresa.Drzava}");

        if (!ConsoleInput.Confirm("Da li ste sigurni?"))
        {
            Console.WriteLine("Brisanje je otkazano.");
            return;
        }

        _service.Delete(id);

        Console.WriteLine("Adresa je obrisana.");
    }
}
