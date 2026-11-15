using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleClient.Utils;

// Prvi element svakog reda je Id. Siroki redovi se ispisuju kao "kartice",
// a kratki (do 4 kolone) kao jedna linija po stavci.
public static class ConsoleTable {
    private const int KompaktnoDo = 4;

    public static void Print(string[] zaglavlje, List<string[]> redovi) {
        if (redovi.Count == 0) {
            Console.WriteLine("  (nema podataka)");
            return;
        }

        foreach (string[] red in redovi) {
            if (zaglavlje.Length <= KompaktnoDo)
                PrintLinija(red);
            else
                PrintKartica(zaglavlje, red);
        }

        Console.WriteLine($"  Ukupno: {redovi.Count}");
    }

    private static void PrintLinija(string[] red) {
        Console.Write("  ");
        Boja(ConsoleColor.Cyan, $"#{red[0],-3}");
        Console.WriteLine(string.Join("  ·  ", red.Skip(1)));
    }

    private static void PrintKartica(string[] zaglavlje, string[] red) {
        int sirina = zaglavlje.Skip(1).Max(h => h.Length);

        Boja(ConsoleColor.Cyan, $"┌─ #{red[0]} {red[1]} ");
        Console.WriteLine();
        for (int i = 2; i < zaglavlje.Length; i++) {
            Console.Write("│ ");
            Boja(ConsoleColor.DarkGray, zaglavlje[i].PadRight(sirina));
            Console.WriteLine("  " + red[i]);
        }
        Boja(ConsoleColor.Cyan, "└────");
        Console.WriteLine();
    }

    private static void Boja(ConsoleColor boja, string tekst) {
        Console.ForegroundColor = boja;
        Console.Write(tekst);
        Console.ResetColor();
    }
}
