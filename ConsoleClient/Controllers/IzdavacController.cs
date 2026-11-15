using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleClient.Utils;
using Core.Models;
using Core.Services;
using Core.Utils;

namespace ConsoleClient.Controllers;

public class IzdavacController {
    private readonly IzdavacService _service;

    public IzdavacController(IzdavacService service) {
        _service = service;
    }

    public void Meni() {
        while (true) {
            Console.WriteLine();
            Console.WriteLine("=== IZDAVACI ===");
            Console.WriteLine("1. Prikaz svih izdavaca");
            Console.WriteLine("2. Dodavanje izdavaca");
            Console.WriteLine("3. Izmena izdavaca");
            Console.WriteLine("4. Brisanje izdavaca");
            Console.WriteLine("0. Nazad");

            string izbor = ConsoleInput.ReadString("Izbor");
            try {
                switch (izbor) {
                    case "1": Prikaz(); break;
                    case "2": Dodavanje(); break;
                    case "3": Izmena(); break;
                    case "4": Brisanje(); break;
                    case "0": return;
                    default: Console.WriteLine("Nepoznata opcija."); break;
                }
            } catch (ValidacijaException ex) {
                Console.WriteLine($"Greska: {ex.Message}");
            }
        }
    }

    public void Prikaz() {
        string[] zaglavlje = { "Id", "Sifra", "Naziv", "Sef", "Autori", "Knjige" };

        List<string[]> redovi = _service.GetAll().Select(i => new[] {
            i.Id.ToString(),
            i.Sifra,
            i.Naziv,
            _service.ImeAutora(i.SefId),
            i.AutorIds.Count == 0 ? "-" : string.Join(", ", i.AutorIds.Select(_service.ImeAutora)),
            i.KnjigaIds.Count == 0 ? "-" : string.Join(", ", i.KnjigaIds.Select(_service.NazivKnjige))
        }).ToList();

        Console.WriteLine();
        ConsoleTable.Print(zaglavlje, redovi);
    }

    private void PrikaziAutoreIKnjige() {
        Console.WriteLine("Autori:");
        ConsoleTable.Print(
            new[] { "Id", "Ime", "Prezime", "God. iskustva" },
            _service.GetAutori().Select(a => new[] { a.Id.ToString(), a.Ime, a.Prezime, a.GodineIskustva.ToString() }).ToList());

        Console.WriteLine("Knjige:");
        ConsoleTable.Print(
            new[] { "Id", "ISBN", "Naziv" },
            _service.GetKnjige().Select(k => new[] { k.Id.ToString(), k.Isbn, k.Naziv }).ToList());
    }

    private void Dodavanje() {
        Console.WriteLine("\n--- Dodavanje izdavaca ---");
        if (_service.GetAutori().Count == 0)
            throw new ValidacijaException("Nema autora u sistemu, pa ne moze da se odredi sef (njih dodaje kolega).");

        PrikaziAutoreIKnjige();
        Console.WriteLine("Sef mora biti autor sa najmanje 5 godina iskustva. Sef se automatski dodaje i u listu autora.");

        Izdavac izdavac = new Izdavac {
            Sifra = ConsoleInput.ReadString("Sifra"),
            Naziv = ConsoleInput.ReadString("Naziv"),
            SefId = ConsoleInput.ReadInt("Id autora koji je sef", 1, int.MaxValue),
            AutorIds = ConsoleInput.ReadIdList("Autori"),
            KnjigaIds = ConsoleInput.ReadIdList("Knjige")
        };

        Izdavac dodat = _service.Add(izdavac);
        Console.WriteLine($"Izdavac je dodat (Id = {dodat.Id}).");
    }

    private void Izmena() {
        Console.WriteLine("\n--- Izmena izdavaca ---");
        Prikaz();
        int id = ConsoleInput.ReadInt("Id izdavaca za izmenu", 1, int.MaxValue);

        Izdavac stari = _service.GetById(id)
            ?? throw new ValidacijaException("Izdavac sa datim Id ne postoji.");

        PrikaziAutoreIKnjige();
        Console.WriteLine("(Enter zadrzava trenutnu vrednost)");

        Izdavac izmene = new Izdavac {
            Sifra = ConsoleInput.ReadString("Sifra", stari.Sifra),
            Naziv = ConsoleInput.ReadString("Naziv", stari.Naziv),
            SefId = ConsoleInput.ReadInt("Id autora koji je sef", 1, int.MaxValue, stari.SefId),
            AutorIds = ConsoleInput.ReadIdList("Autori", stari.AutorIds),
            KnjigaIds = ConsoleInput.ReadIdList("Knjige", stari.KnjigaIds)
        };

        _service.Update(id, izmene);
        Console.WriteLine("Izdavac je izmenjen.");
    }

    private void Brisanje() {
        Console.WriteLine("\n--- Brisanje izdavaca ---");
        Prikaz();
        int id = ConsoleInput.ReadInt("Id izdavaca za brisanje", 1, int.MaxValue);

        Izdavac izdavac = _service.GetById(id)
            ?? throw new ValidacijaException("Izdavac sa datim Id ne postoji.");

        Console.WriteLine($"Brise se izdavac: {izdavac.Naziv} ({izdavac.Sifra}). Njegove knjige ostaju bez izdavaca.");
        if (!ConsoleInput.Confirm("Da li ste sigurni")) {
            Console.WriteLine("Brisanje je otkazano.");
            return;
        }

        _service.Delete(id);
        Console.WriteLine("Izdavac je obrisan.");
    }
}
