using System;
using System.Collections.Generic;
using Core.Interfaces;
using Core.Models;

namespace ConsoleClient.Utils;

// PRIVREMENO: samo da mozes da testiras Izdavaca i Kupovinu dok kolega ne zavrsi Autora i Knjigu.
// Obrisati (zajedno sa opcijom u Program.cs) pre predaje.
public static class TestPodaci {
    public static void Ubaci(IDao<Adresa> adrese, IDao<Autor> autori, IDao<Knjiga> knjige) {
        if (autori.GetAll().Count > 0 || knjige.GetAll().Count > 0) {
            Console.WriteLine("Autori ili knjige vec postoje, nista nije dodato.");
            return;
        }

        Autor a1 = NapraviAutora(adrese, autori, "Petar", "Petrovic", "001", 30);
        Autor a2 = NapraviAutora(adrese, autori, "Jovana", "Jovanovic", "002", 12);
        Autor a3 = NapraviAutora(adrese, autori, "Milan", "Milic", "003", 2);

        NapraviKnjigu(autori, knjige, "978-86-0000-001-1", "Prva knjiga", "Roman", 2015, 1200, 320, a1);
        NapraviKnjigu(autori, knjige, "978-86-0000-002-2", "Druga knjiga", "Poezija", 2018, 800, 120, a2);
        NapraviKnjigu(autori, knjige, "978-86-0000-003-3", "Treca knjiga", "Drama", 2021, 950, 210, a3);

        Console.WriteLine("Dodata 3 autora (Id 1 i 2 imaju >= 5 godina iskustva) i 3 knjige.");
    }

    private static Autor NapraviAutora(IDao<Adresa> adrese, IDao<Autor> autori,
        string ime, string prezime, string lk, int iskustvo) {
        Adresa adresa = adrese.Add(new Adresa { Ulica = "Probna ulica", Broj = "1", Grad = "Novi Sad", Drzava = "Srbija" });
        return autori.Add(new Autor {
            Ime = ime,
            Prezime = prezime,
            DatumRodjenja = new DateTime(1970, 1, 1),
            AdresaId = adresa.Id,
            Telefon = "021123456",
            Email = $"{ime.ToLowerInvariant()}@primer.rs",
            BrojLicneKarte = lk,
            GodineIskustva = iskustvo
        });
    }

    private static void NapraviKnjigu(IDao<Autor> autori, IDao<Knjiga> knjige,
        string isbn, string naziv, string zanr, int godina, double cena, int strana, Autor autor) {
        Knjiga knjiga = knjige.Add(new Knjiga {
            Isbn = isbn,
            Naziv = naziv,
            Zanr = zanr,
            GodinaIzdanja = godina,
            Cena = cena,
            BrojStrana = strana,
            AutorIds = new List<int> { autor.Id }
        });
        autor.KnjigaIds.Add(knjiga.Id);
        autori.Update(autor);
    }
}
