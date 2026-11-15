using System;
using System.Text.RegularExpressions;

namespace Core.Utils;

public static class Validator {
    private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    private static readonly Regex TelefonRegex = new Regex(@"^[0-9+\-/ ()]{6,20}$");
    private static readonly Regex ClanskaKartaRegex = new Regex(@"^CK-\d+-\d{4}$", RegexOptions.IgnoreCase);

    public static void Obavezno(string? vrednost, string nazivPolja) {
        if (string.IsNullOrWhiteSpace(vrednost))
            throw new ValidacijaException($"Polje '{nazivPolja}' je obavezno.");
    }

    public static void ProveriEmail(string? email) {
        Obavezno(email, "E-mail");
        if (!EmailRegex.IsMatch(email!.Trim()))
            throw new ValidacijaException("E-mail adresa nije u ispravnom formatu (npr. ime@domen.rs).");
    }

    public static void ProveriTelefon(string? telefon) {
        Obavezno(telefon, "Kontakt telefon");
        if (!TelefonRegex.IsMatch(telefon!.Trim()))
            throw new ValidacijaException("Telefon moze sadrzati samo cifre, +, -, /, razmak i zagrade (6-20 karaktera).");
    }

    public static void ProveriClanskuKartu(string? broj) {
        Obavezno(broj, "Broj clanske karte");
        if (!ClanskaKartaRegex.IsMatch(broj!.Trim()))
            throw new ValidacijaException("Broj clanske karte mora biti u formatu CK-<broj>-<godina>, npr. CK-1-2023.");
    }

    public static void ProveriDatumRodjenja(DateTime datum) {
        if (datum.Year < 1900 || datum.Date >= DateTime.Today)
            throw new ValidacijaException("Datum rodjenja mora biti u proslosti (posle 1900. godine).");
    }
}
