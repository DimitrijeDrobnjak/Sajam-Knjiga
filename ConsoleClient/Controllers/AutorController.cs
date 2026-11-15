
using ConsoleClient.Utils;
using Core.Models;
using Core.Services;
using Core.Utils;
using System;

namespace ConsoleClient.Controllers;

public class AutorController
{
    private readonly AutorService _service;
    private readonly AdresaService _adresaService;

    public AutorController(AutorService service, AdresaService adresaService)
    {
        _service = service;
        _adresaService = adresaService;
    }

    public void Meni()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("===== AUTORI =====");
            Console.WriteLine("1. Prikaz svih autora");
            Console.WriteLine("2. Dodavanje autora");
            Console.WriteLine("3. Izmena autora");
            Console.WriteLine("4. Brisanje autora");
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
                Console.WriteLine($"Greška: {e.Message}");
            }
        }
    }

    public void Prikaz()
    {
        string[] zaglavlje = {
        "Id", "Ime", "Prezime", "Datum rodjenja",
        "Adresa", "Telefon", "Email",
        "Broj licne karte", "Godine iskustva", "Broj knjiga"
    };

        List<string[]> redovi = _service.GetAll().Select(autor =>
        {
            Adresa? adresa = _adresaService.GetById(autor.AdresaId);

            string tekstAdrese = adresa == null
                ? "Nepoznata adresa"
                : $"{adresa.Ulica} {adresa.Broj}, {adresa.Grad}, {adresa.Drzava}";

            return new[]
            {
            autor.Id.ToString(),
            autor.Ime,
            autor.Prezime,
            autor.DatumRodjenja.ToString("dd.MM.yyyy"),
            tekstAdrese,
            autor.Telefon,
            autor.Email,
            autor.BrojLicneKarte,
            autor.GodineIskustva.ToString(),
            autor.KnjigaIds.Count.ToString()
        };
        }).ToList();

        Console.WriteLine();
        ConsoleTable.Print(zaglavlje, redovi);
    }

    private void Dodavanje()
    {
        Console.WriteLine();
        Console.WriteLine("===== DODAVANJE AUTORA =====");

        Autor autor = new Autor();

        autor.Ime = ConsoleInput.ReadString("Ime");
        autor.Prezime = ConsoleInput.ReadString("Prezime");

        autor.DatumRodjenja = UnesiDatum("Datum rođenja (mm.dd.yyyy)");

        autor.AdresaId = UnesiInt("ID adrese");

        autor.Telefon = ConsoleInput.ReadString("Telefon");
        autor.Email = ConsoleInput.ReadString("Email");
        autor.BrojLicneKarte = ConsoleInput.ReadString("Broj lične karte");

        autor.GodineIskustva = UnesiInt("Godine iskustva");

        _service.Add(autor);

        Console.WriteLine("Autor je uspešno dodat.");
    }

    private void Izmena()
    {
        Console.WriteLine();
        Console.WriteLine("===== IZMENA AUTORA =====");

        Prikaz();

        int id = UnesiInt("Unesite ID autora");

        Autor? autor = _service.GetById(id);

        if (autor == null)
        {
            Console.WriteLine("Autor sa tim ID-om ne postoji.");
            return;
        }

        Console.WriteLine("Unesite nove podatke (Enter za zadržavanje stare vrednosti).");

        string unos = ConsoleInput.ReadString($"Ime ({autor.Ime})");
        if (unos != "") autor.Ime = unos;

        unos = ConsoleInput.ReadString($"Prezime ({autor.Prezime})");
        if (unos != "") autor.Prezime = unos;

        unos = ConsoleInput.ReadString($"Datum rođenja ({autor.DatumRodjenja:d})");
        if (unos != "" && DateTime.TryParse(unos, out DateTime datum))
            autor.DatumRodjenja = datum;

        unos = ConsoleInput.ReadString($"ID adrese ({autor.AdresaId})");
        if (unos != "" && int.TryParse(unos, out int adresaId))
            autor.AdresaId = adresaId;

        unos = ConsoleInput.ReadString($"Telefon ({autor.Telefon})");
        if (unos != "") autor.Telefon = unos;

        unos = ConsoleInput.ReadString($"Email ({autor.Email})");
        if (unos != "") autor.Email = unos;

        unos = ConsoleInput.ReadString($"Broj lične karte ({autor.BrojLicneKarte})");
        if (unos != "") autor.BrojLicneKarte = unos;

        unos = ConsoleInput.ReadString($"Godine iskustva ({autor.GodineIskustva})");
        if (unos != "" && int.TryParse(unos, out int iskustvo))
            autor.GodineIskustva = iskustvo;

        _service.Update(autor);

        Console.WriteLine("Autor je uspešno izmenjen.");
    }

    private void Brisanje()
    {
        Console.WriteLine();
        Console.WriteLine("===== BRISANJE AUTORA =====");

        Prikaz();

        int id = UnesiInt("Unesite ID autora za brisanje");

        Autor? autor = _service.GetById(id);

        if (autor == null)
        {
            Console.WriteLine("Autor sa tim ID-om ne postoji.");
            return;
        }

        bool potvrda = ConsoleInput.Confirm(
            $"Da li ste sigurni da želite da obrišete autora {autor.Ime} {autor.Prezime}?"
        );

        if (potvrda)
        {
            _service.Delete(id);
            Console.WriteLine("Autor je uspešno obrisan.");
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

    private DateTime UnesiDatum(string poruka)
    {
        while (true)
        {
            string unos = ConsoleInput.ReadString(poruka);

            if (DateTime.TryParse(unos, out DateTime datum))
                return datum;

            Console.WriteLine("Unesite ispravan datum.");
        }
    }
}
