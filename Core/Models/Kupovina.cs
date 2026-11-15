using System;
using Core.Interfaces;
using Core.Utils;

namespace Core.Models;

public class Kupovina : IEntity, ICsvRecord {
    public int Id { get; set; }
    public int PosetilacId { get; set; }
    public int KnjigaId { get; set; }
    public DateTime DatumKupovine { get; set; }
    public int Ocena { get; set; }
    public string Komentar { get; set; } = "";

    public string[] ToFields() => new[] {
        CsvConvert.ToText(Id),
        CsvConvert.ToText(PosetilacId),
        CsvConvert.ToText(KnjigaId),
        CsvConvert.ToText(DatumKupovine),
        CsvConvert.ToText(Ocena),
        Komentar
    };

    public void FromFields(string[] f) {
        Id = CsvConvert.ToInt(f[0]);
        PosetilacId = CsvConvert.ToInt(f[1]);
        KnjigaId = CsvConvert.ToInt(f[2]);
        DatumKupovine = CsvConvert.ToDate(f[3]);
        Ocena = CsvConvert.ToInt(f[4]);
        Komentar = f[5];
    }
}
