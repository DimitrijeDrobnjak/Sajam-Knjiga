using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Utils;

// Spajanje polja u jedan red i razdvajanje reda na polja (separator je ';').
// Posebni znakovi unutar teksta (\ ; novi red) se "escape-uju" kosom crtom,
// pa komentar poput "super; odlicno" ne pokvari red.
public static class CsvFile {
    public const char Separator = ';';

    public static string JoinLine(IEnumerable<string> fields) {
        return string.Join(Separator, fields.Select(Escape));
    }

    public static string[] SplitLine(string line) {
        List<string> fields = new List<string>();
        StringBuilder current = new StringBuilder();
        bool escaped = false;

        foreach (char c in line) {
            if (escaped) {
                current.Append(c == 'n' ? '\n' : c == 'r' ? '\r' : c);
                escaped = false;
            } else if (c == '\\') {
                escaped = true;
            } else if (c == Separator) {
                fields.Add(current.ToString());
                current.Clear();
            } else {
                current.Append(c);
            }
        }

        fields.Add(current.ToString());
        return fields.ToArray();
    }

    private static string Escape(string value) {
        StringBuilder sb = new StringBuilder();
        foreach (char c in value) {
            switch (c) {
                case '\\': sb.Append("\\\\"); break;
                case Separator: sb.Append("\\;"); break;
                case '\n': sb.Append("\\n"); break;
                case '\r': sb.Append("\\r"); break;
                default: sb.Append(c); break;
            }
        }
        return sb.ToString();
    }
}
