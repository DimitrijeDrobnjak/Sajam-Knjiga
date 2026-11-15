using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleClient.Utils;
using Core.Models;
using Core.Services;
using Core.Utils;

namespace ConsoleClient.Controllers;

public class PosetilacController {
    private readonly PosetilacService _service;

    public PosetilacController(PosetilacService service) {
        _service = service;
    }

    public void Meni() {
        while (true) {
            Console.WriteLine();
            Console.WriteLine("=== POSETIOCI ===");
            Console.WriteLine("1. Prikaz svih posetilaca");
            Console.WriteLine("2. Dodavanje posetioca");
            Console.WriteLine("3. Izmena posetioca");
            Console.WriteLine("4. Brisanje posetioca");
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
        string[] zaglavlje = {
            "Id", "Br. clanske karte", "Ime", "Prezime", "Datum rodjenja", "Adresa",
            "Telefon", "E-mail", "God. clanstva", "Status", "Pros. ocena", "Br. kupovina"
        };

        List<string[]> redovi = _service.GetAll().Select(p => new[] {
            p.Id.ToString(),
            p.BrojClanskeKarte,
            p.Ime,
            p.Prezime,
            p.DatumRodjenja.ToString("dd.MM.yyyy."),
            _service.AdresaTekst(p),
            p.Telefon,
            p.Email,
            p.GodinaClanstva.ToString(),
            p.Status == StatusPosetioca.Redovan ? "Redovan" : "VIP",
            p.ProsecnaOcena.ToString("0.00"),
            p.KupovinaIds.Count.ToString()
        }).ToList();

        Console.WriteLine();
        ConsoleTable.Print(zaglavlje, redovi);
    }

    private void Dodavanje() {
        Console.WriteLine("\n--- Dodavanje posetioca ---");
        Posetilac posetilac = new Posetilac {
            BrojClanskeKarte = ConsoleInput.ReadString("Broj clanske karte (npr. CK-1-2023)"),
            Ime = ConsoleInput.ReadString("Ime"),
            Prezime = ConsoleInput.ReadString("Prezime"),
            DatumRodjenja = ConsoleInput.ReadDate("Datum rodjenja"),
            Telefon = ConsoleInput.ReadString("Kontakt telefon"),
            Email = ConsoleInput.ReadString("E-mail"),
            GodinaClanstva = ConsoleInput.ReadInt("Trenutna godina clanstva", 1, 3000),
            Status = ConsoleInput.ReadStatus()
        };

        Adresa adresa = new Adresa {
            Ulica = ConsoleInput.ReadString("Adresa - ulica"),
            Broj = ConsoleInput.ReadString("Adresa - broj"),
            Grad = ConsoleInput.ReadString("Adresa - grad"),
            Drzava = ConsoleInput.ReadString("Adresa - drzava")
        };

        Posetilac dodat = _service.Add(posetilac, adresa);
        Console.WriteLine($"Posetilac je dodat (Id = {dodat.Id}).");
    }

    private void Izmena() {
        Console.WriteLine("\n--- Izmena posetioca ---");
        Prikaz();
        int id = ConsoleInput.ReadInt("Id posetioca za izmenu", 1, int.MaxValue);

        Posetilac stari = _service.GetById(id)
            ?? throw new ValidacijaException("Posetilac sa datim Id ne postoji.");
        Adresa staraAdresa = _service.GetAdresa(stari) ?? new Adresa();

        Console.WriteLine("(Enter zadrzava trenutnu vrednost)");
        Posetilac izmene = new Posetilac {
            BrojClanskeKarte = ConsoleInput.ReadString("Broj clanske karte", stari.BrojClanskeKarte),
            Ime = ConsoleInput.ReadString("Ime", stari.Ime),
            Prezime = ConsoleInput.ReadString("Prezime", stari.Prezime),
            DatumRodjenja = ConsoleInput.ReadDate("Datum rodjenja", stari.DatumRodjenja),
            Telefon = ConsoleInput.ReadString("Kontakt telefon", stari.Telefon),
            Email = ConsoleInput.ReadString("E-mail", stari.Email),
            GodinaClanstva = ConsoleInput.ReadInt("Trenutna godina clanstva", 1, 3000, stari.GodinaClanstva),
            Status = ConsoleInput.ReadStatus(stari.Status)
        };

        Adresa adresa = new Adresa {
            Ulica = ConsoleInput.ReadString("Adresa - ulica", staraAdresa.Ulica),
            Broj = ConsoleInput.ReadString("Adresa - broj", staraAdresa.Broj),
            Grad = ConsoleInput.ReadString("Adresa - grad", staraAdresa.Grad),
            Drzava = ConsoleInput.ReadString("Adresa - drzava", staraAdresa.Drzava)
        };

        _service.Update(id, izmene, adresa);
        Console.WriteLine("Posetilac je izmenjen.");
    }

    private void Brisanje() {
        Console.WriteLine("\n--- Brisanje posetioca ---");
        Prikaz();
        int id = ConsoleInput.ReadInt("Id posetioca za brisanje", 1, int.MaxValue);

        Posetilac posetilac = _service.GetById(id)
            ?? throw new ValidacijaException("Posetilac sa datim Id ne postoji.");

        Console.WriteLine($"Brise se: {posetilac.Ime} {posetilac.Prezime} ({posetilac.BrojClanskeKarte}).");
        Console.WriteLine("Obrisace se i sve njegove kupovine.");
        if (!ConsoleInput.Confirm("Da li ste sigurni")) {
            Console.WriteLine("Brisanje je otkazano.");
            return;
        }

        _service.Delete(id);
        Console.WriteLine("Posetilac je obrisan.");
    }
}
