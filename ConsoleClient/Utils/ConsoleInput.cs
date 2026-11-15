using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Core.Models;

namespace ConsoleClient.Utils;

// Ako je prosledjena trenutna vrednost (izmena), Enter je zadrzava.
public static class ConsoleInput {
    private static readonly string[] DatumFormati = { "D.M.YYYY", "D.M.YYYY.", "DD.MM.YYYY", "DD.MM.YYYY." };

    public static string ReadString(string prompt, string? current = null) {
        while (true) {
            Console.Write(current == null ? $"{prompt}: " : $"{prompt} [{current}]: ");
            string input = (Console.ReadLine() ?? "").Trim();

            if (input.Length == 0) {
                if (current != null)
                    return current;
                Console.WriteLine("  Polje je obavezno.");
                continue;
            }
            return input;
        }
    }

    // Opciono polje: prazan unos = zadrzi trenutnu (ili prazno), "-" = obrisi
    public static string ReadOptionalString(string prompt, string? current = null) {
        Console.Write(current == null ? $"{prompt} (moze prazno): " : $"{prompt} [{current}] ('-' za brisanje): ");
        string input = (Console.ReadLine() ?? "").Trim();

        if (input.Length == 0)
            return current ?? "";
        if (input == "-")
            return "";
        return input;
    }

    public static int ReadInt(string prompt, int min, int max, int? current = null) {
        while (true) {
            Console.Write(current == null ? $"{prompt}: " : $"{prompt} [{current}]: ");
            string input = (Console.ReadLine() ?? "").Trim();

            if (input.Length == 0 && current != null)
                return current.Value;

            if (int.TryParse(input, out int value) && value >= min && value <= max)
                return value;

            Console.WriteLine($"  Unesite ceo broj izmedju {min} i {max}.");
        }
    }

    public static DateTime ReadDate(string prompt, DateTime? current = null) {
        while (true) {
            string tekuci = current == null ? "" : $" [{current.Value:dd.MM.yyyy.}]";
            Console.Write($"{prompt} (dd.MM.yyyy){tekuci}: ");
            string input = (Console.ReadLine() ?? "").Trim();

            if (input.Length == 0 && current != null)
                return current.Value;

            if (DateTime.TryParseExact(input, DatumFormati, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datum))
                return datum;

            Console.WriteLine("  Datum nije u ispravnom formatu (npr. 25.12.1990).");
        }
    }

    public static StatusPosetioca ReadStatus(StatusPosetioca? current = null) {
        while (true) {
            string tekuci = current == null ? "" : $" [{(current == StatusPosetioca.Redovan ? "R" : "V")}]";
            Console.Write($"Status (R - redovan, V - VIP){tekuci}: ");
            string input = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();

            if (input.Length == 0 && current != null)
                return current.Value;
            if (input == "R")
                return StatusPosetioca.Redovan;
            if (input == "V")
                return StatusPosetioca.VIP;

            Console.WriteLine("  Unesite R ili V.");
        }
    }

    // Lista Id-jeva odvojenih zarezom, npr. "1,2,5". Prazno = zadrzi trenutnu, "-" = prazna lista.
    public static List<int> ReadIdList(string prompt, List<int>? current = null) {
        while (true) {
            string tekuci = current == null ? "" : $" [{string.Join(",", current)}]";
            Console.Write($"{prompt} (Id-jevi odvojeni zarezom, '-' za praznu listu){tekuci}: ");
            string input = (Console.ReadLine() ?? "").Trim();

            if (input.Length == 0) {
                if (current != null)
                    return current.ToList();
                return new List<int>();
            }
            if (input == "-")
                return new List<int>();

            string[] delovi = input.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<int> rezultat = new List<int>();
            bool ispravno = true;
            foreach (string deo in delovi) {
                if (int.TryParse(deo, out int id) && id > 0)
                    rezultat.Add(id);
                else
                    ispravno = false;
            }

            if (ispravno)
                return rezultat;
            Console.WriteLine("  Unesite samo pozitivne cele brojeve odvojene zarezom.");
        }
    }

    public static bool Confirm(string prompt) {
        while (true) {
            Console.Write($"{prompt} (d/n): ");
            string input = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
            if (input == "d" || input == "da")
                return true;
            if (input == "n" || input == "ne")
                return false;
        }
    }
}
