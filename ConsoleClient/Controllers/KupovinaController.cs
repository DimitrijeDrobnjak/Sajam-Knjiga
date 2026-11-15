using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleClient.Utils;
using Core.Models;
using Core.Services;
using Core.Utils;

namespace ConsoleClient.Controllers;

public class KupovinaController {
    private readonly KupovinaService _service;

    public KupovinaController(KupovinaService service) {
        _service = service;
    }

    public void Meni() {
        while (true) {
            Console.WriteLine();
            Console.WriteLine("=== KUPOVINE ===");
            Console.WriteLine("1. Prikaz svih kupovina");
            Console.WriteLine("2. Dodavanje kupovine");
            Console.WriteLine("3. Izmena kupovine");
            Console.WriteLine("4. Brisanje kupovine");
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
        string[] zaglavlje = { "Id", "Posetilac", "Knjiga", "Datum kupovine", "Ocena", "Komentar" };

        List<string[]> redovi = _service.GetAll().Select(k => new[] {
            k.Id.ToString(),
            _service.ImePosetioca(k.PosetilacId),
            _service.NazivKnjige(k.KnjigaId),
            k.DatumKupovine.ToString("dd.MM.yyyy."),
            k.Ocena.ToString(),
            k.Komentar
        }).ToList();

        Console.WriteLine();
        ConsoleTable.Print(zaglavlje, redovi);
    }

    private void Dodavanje() {
        Console.WriteLine("\n--- Dodavanje kupovine ---");

        List<Posetilac> posetioci = _service.GetPosetioci();
        if (posetioci.Count == 0)
            throw new ValidacijaException("Nema posetilaca. Prvo dodajte posetioca.");
        List<Knjiga> knjige = _service.GetKnjige();
        if (knjige.Count == 0)
            throw new ValidacijaException("Nema knjiga u sistemu (njih dodaje kolega ili opcija 9 u glavnom meniju).");

        Console.WriteLine("Posetioci:");
        ConsoleTable.Print(
            new[] { "Id", "Br. clanske karte", "Ime", "Prezime" },
            posetioci.Select(p => new[] { p.Id.ToString(), p.BrojClanskeKarte, p.Ime, p.Prezime }).ToList());

        Console.WriteLine("Knjige:");
        ConsoleTable.Print(
            new[] { "Id", "ISBN", "Naziv", "Cena" },
            knjige.Select(k => new[] { k.Id.ToString(), k.Isbn, k.Naziv, k.Cena.ToString("0.00") }).ToList());

        Kupovina kupovina = new Kupovina {
            PosetilacId = ConsoleInput.ReadInt("Id posetioca", 1, int.MaxValue),
            KnjigaId = ConsoleInput.ReadInt("Id knjige", 1, int.MaxValue),
            DatumKupovine = ConsoleInput.ReadDate("Datum kupovine"),
            Ocena = ConsoleInput.ReadInt("Ocena (1-5)", 1, 5),
            Komentar = ConsoleInput.ReadOptionalString("Komentar")
        };

        Kupovina dodata = _service.Add(kupovina);
        Console.WriteLine($"Kupovina je dodata (Id = {dodata.Id}).");
    }

    private void Izmena() {
        Console.WriteLine("\n--- Izmena kupovine ---");
        Prikaz();
        int id = ConsoleInput.ReadInt("Id kupovine za izmenu", 1, int.MaxValue);

        Kupovina stara = _service.GetById(id)
            ?? throw new ValidacijaException("Kupovina sa datim Id ne postoji.");

        Console.WriteLine("(Enter zadrzava trenutnu vrednost; posetilac i knjiga se ne menjaju)");
        DateTime datum = ConsoleInput.ReadDate("Datum kupovine", stara.DatumKupovine);
        int ocena = ConsoleInput.ReadInt("Ocena (1-5)", 1, 5, stara.Ocena);
        string komentar = ConsoleInput.ReadOptionalString("Komentar", stara.Komentar);

        _service.Update(id, datum, ocena, komentar);
        Console.WriteLine("Kupovina je izmenjena.");
    }

    private void Brisanje() {
        Console.WriteLine("\n--- Brisanje kupovine ---");
        Prikaz();
        int id = ConsoleInput.ReadInt("Id kupovine za brisanje", 1, int.MaxValue);

        if (_service.GetById(id) == null)
            throw new ValidacijaException("Kupovina sa datim Id ne postoji.");

        if (!ConsoleInput.Confirm("Da li ste sigurni")) {
            Console.WriteLine("Brisanje je otkazano.");
            return;
        }

        _service.Delete(id);
        Console.WriteLine("Kupovina je obrisana.");
    }
}
