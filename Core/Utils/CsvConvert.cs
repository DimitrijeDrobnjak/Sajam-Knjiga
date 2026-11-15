using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Core.Utils;

// Pretvaranje brojeva, datuma i listi u tekst i nazad, nezavisno od regionalnih podesavanja racunara.
public static class CsvConvert {
    private const string DatumFormat = "yyyy-MM-dd";

    public static string ToText(int value) => value.ToString(CultureInfo.InvariantCulture);

    public static string ToText(double value) => value.ToString(CultureInfo.InvariantCulture);

    public static string ToText(DateTime value) => value.ToString(DatumFormat, CultureInfo.InvariantCulture);

    public static string ToText(IEnumerable<int> values) => string.Join(",", values);

    public static int ToInt(string text) => int.Parse(text, CultureInfo.InvariantCulture);

    public static double ToDouble(string text) => double.Parse(text, CultureInfo.InvariantCulture);

    public static DateTime ToDate(string text) => DateTime.ParseExact(text, DatumFormat, CultureInfo.InvariantCulture);

    public static List<int> ToIntList(string text) {
        if (string.IsNullOrWhiteSpace(text))
            return new List<int>();

        return text.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(ToInt).ToList();
    }
}
