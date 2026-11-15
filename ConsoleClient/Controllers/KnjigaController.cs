
using ConsoleClient.Utils;
using Core.Models;
using Core.Services;
using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleClient.Controllers;

public class KnjigaController
{
    private readonly KnjigaService _service;

    public KnjigaController(KnjigaService service)
    {
        _service = service;
    }

    public void Meni()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("===== KNJIGE =====");
            Console.WriteLine("1. Prikaz svih knjiga");
            Console.WriteLine("2. Dodavanje knjige");
            Console.WriteLine("3. Izmena knjige");
            Console.WriteLine("4. Brisanje knjige");
            Console.WriteLine("0. Povratak");

            string izbor = ConsoleInput.ReadString("Izbor");

            try
            {
                switch (izbor)
                {
                    case "1":
                        Prikaz();
                        break;

                    case "2":
                        Dodavanje();
                        break;

                    case "3":
                        Izmena();
                        break;

                    case "4":
                        Brisanje();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Nepoznata opcija.");
                        break;
                }
            }
            catch (ValidacijaException e)
            {
                Console.WriteLine($"Greska: {e.Message}");
            }
        }
    }

    public void Prikaz()
    {
        string[] zaglavlje = {
            "Id", "ISBN", "Naziv", "Zanr",
            "Godina izdanja", "Cena", "Broj strana",
            "ID izdavaca", "Broj autora"
        };

        List<string[]> redovi = _service.GetAll().Select(knjiga => new[]
        {
            knjiga.Id.ToString(),
            knjiga.Isbn,
            knjiga.Naziv,
            knjiga.Zanr,
            knjiga.GodinaIzdanja.ToString(),
            knjiga.Cena.ToString("0.00"),
            knjiga.BrojStrana.ToString(),
            knjiga.IzdavacId.ToString(),
            knjiga.AutorIds.Count.ToString()
        }).ToList();

        Console.WriteLine();
        ConsoleTable.Print(zaglavlje, redovi);
    }

    private void Dodavanje()
    {
        Console.WriteLine();
        Console.WriteLine("===== DODAVANJE KNJIGE =====");

        Knjiga knjiga = new Knjiga();

        knjiga.Isbn = ConsoleInput.ReadString("ISBN");
        knjiga.Naziv = ConsoleInput.ReadString("Naziv");
        knjiga.Zanr = ConsoleInput.ReadString("Zanr");
        knjiga.GodinaIzdanja = UnesiInt("Godina izdanja");
        knjiga.Cena = UnesiDouble("Cena");
        knjiga.BrojStrana = UnesiInt("Broj strana");
        knjiga.IzdavacId = UnesiInt("ID izdavaca");

        _service.Add(knjiga);

        Console.WriteLine("Knjiga je uspesno dodata.");
    }

    private void Izmena()
    {
        Console.WriteLine();
        Console.WriteLine("===== IZMENA KNJIGE =====");

        Prikaz();

        int id = UnesiInt("Unesite ID knjige");

        Knjiga? knjiga = _service.GetById(id);

        if (knjiga == null)
        {
            Console.WriteLine("Knjiga sa tim ID-om ne postoji.");
            return;
        }

        Console.WriteLine("Unesite nove podatke (Enter za zadržavanje stare vrednosti).");

        string unos = ConsoleInput.ReadString($"ISBN ({knjiga.Isbn})");
        if (unos != "") knjiga.Isbn = unos;

        unos = ConsoleInput.ReadString($"Naziv ({knjiga.Naziv})");
        if (unos != "") knjiga.Naziv = unos;

        unos = ConsoleInput.ReadString($"Zanr ({knjiga.Zanr})");
        if (unos != "") knjiga.Zanr = unos;

        unos = ConsoleInput.ReadString($"Godina izdanja ({knjiga.GodinaIzdanja})");
        if (unos != "" && int.TryParse(unos, out int godina))
            knjiga.GodinaIzdanja = godina;

        unos = ConsoleInput.ReadString($"Cena ({knjiga.Cena})");
        if (unos != "" && double.TryParse(unos, out double cena))
            knjiga.Cena = cena;

        unos = ConsoleInput.ReadString($"Broj strana ({knjiga.BrojStrana})");
        if (unos != "" && int.TryParse(unos, out int brojStrana))
            knjiga.BrojStrana = brojStrana;

        unos = ConsoleInput.ReadString($"ID izdavaca ({knjiga.IzdavacId})");
        if (unos != "" && int.TryParse(unos, out int izdavacId))
            knjiga.IzdavacId = izdavacId;

        _service.Update(knjiga);

        Console.WriteLine("Knjiga je uspesno izmenjena.");
    }

    private void Brisanje()
    {
        Console.WriteLine();
        Console.WriteLine("===== BRISANJE KNJIGE =====");

        Prikaz();

        int id = UnesiInt("Unesite ID knjige za brisanje");

        Knjiga? knjiga = _service.GetById(id);

        if (knjiga == null)
        {
            Console.WriteLine("Knjiga sa tim ID-om ne postoji.");
            return;
        }

        bool potvrda = ConsoleInput.Confirm(
            $"Da li ste sigurni da zelite da obrisete knjigu {knjiga.Naziv}?"
        );

        if (potvrda)
        {
            _service.Delete(id);
            Console.WriteLine("Knjiga je uspesno obrisana.");
        }
        else
        {
            Console.WriteLine("Brisanje je otkazano.");
        }
    }

    private int UnesiInt(string poruka)
    {
        while (true)
        {
            string unos = ConsoleInput.ReadString(poruka);

            if (int.TryParse(unos, out int vrednost))
                return vrednost;

            Console.WriteLine("Unesite ispravan ceo broj.");
        }
    }

    private double UnesiDouble(string poruka)
    {
        while (true)
        {
            string unos = ConsoleInput.ReadString(poruka);

            if (double.TryParse(unos, out double vrednost))
                return vrednost;

            Console.WriteLine("Unesite ispravnu cenu.");
        }
    }
}
