using Core.Interfaces;
using Core.Utils;

namespace Core.Models;

public class Adresa : IEntity, ICsvRecord {
    public int Id { get; set; }
    public string Ulica { get; set; } = "";
    public string Broj { get; set; } = "";
    public string Grad { get; set; } = "";
    public string Drzava { get; set; } = "";

    public string[] ToFields() => new[] {
        CsvConvert.ToText(Id), Ulica, Broj, Grad, Drzava
    };

    public void FromFields(string[] f) {
        Id = CsvConvert.ToInt(f[0]);
        Ulica = f[1];
        Broj = f[2];
        Grad = f[3];
        Drzava = f[4];
    }
}
