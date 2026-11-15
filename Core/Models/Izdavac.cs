using System.Collections.Generic;
using Core.Interfaces;
using Core.Utils;

namespace Core.Models;

public class Izdavac : IEntity, ICsvRecord {
    public int Id { get; set; }
    public string Sifra { get; set; } = "";
    public string Naziv { get; set; } = "";
    public int SefId { get; set; } // autor sa >= 5 godina iskustva
    public List<int> AutorIds { get; set; } = new List<int>();
    public List<int> KnjigaIds { get; set; } = new List<int>();

    public string[] ToFields() => new[] {
        CsvConvert.ToText(Id),
        Sifra,
        Naziv,
        CsvConvert.ToText(SefId),
        CsvConvert.ToText(AutorIds),
        CsvConvert.ToText(KnjigaIds)
    };

    public void FromFields(string[] f) {
        Id = CsvConvert.ToInt(f[0]);
        Sifra = f[1];
        Naziv = f[2];
        SefId = CsvConvert.ToInt(f[3]);
        AutorIds = CsvConvert.ToIntList(f[4]);
        KnjigaIds = CsvConvert.ToIntList(f[5]);
    }
}
