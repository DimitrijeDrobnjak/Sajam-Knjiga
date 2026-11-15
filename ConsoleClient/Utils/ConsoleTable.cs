using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleClient.Utils;

public static class ConsoleTable {
    public static void Print(string[] zaglavlje, List<string[]> redovi) {
        if (redovi.Count == 0) {
            Console.WriteLine("  (nema podataka)");
            return;
        }

        int[] sirine = zaglavlje.Select(h => h.Length).ToArray();
        foreach (string[] red in redovi) {
            for (int i = 0; i < sirine.Length; i++)
                sirine[i] = Math.Max(sirine[i], red[i].Length);
        }

        string separator = "+" + string.Join("+", sirine.Select(s => new string('-', s + 2))) + "+";

        Console.WriteLine(separator);
        Console.WriteLine(Linija(zaglavlje, sirine));
        Console.WriteLine(separator);
        foreach (string[] red in redovi)
            Console.WriteLine(Linija(red, sirine));
        Console.WriteLine(separator);
        Console.WriteLine($"Ukupno: {redovi.Count}");
    }

    private static string Linija(string[] celije, int[] sirine) {
        return "| " + string.Join(" | ", celije.Select((c, i) => c.PadRight(sirine[i]))) + " |";
    }
}
