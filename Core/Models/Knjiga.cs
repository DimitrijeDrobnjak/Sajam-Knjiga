using System.Collections.Generic;
using Core.Interfaces;
using Core.Utils;

namespace Core.Models;

public class Knjiga : IEntity, ICsvRecord {
    public int Id { get; set; }
    public string Isbn { get; set; } = "";
    public string Naziv { get; set; } = "";
    public string Zanr { get; set; } = "";
    public int GodinaIzdanja { get; set; }
    public double Cena { get; set; }
    public int BrojStrana { get; set; }
    public List<int> AutorIds { get; set; } = new List<int>();
    public int IzdavacId { get; set; }
    public List<int> PosetiociKupiliIds { get; set; } = new List<int>();
    public List<int> PosetiociZeljaIds { get; set; } = new List<int>();

    public string[] ToFields() => new[] {
        CsvConvert.ToText(Id),
        Isbn,
        Naziv,
        Zanr,
        CsvConvert.ToText(GodinaIzdanja),
        CsvConvert.ToText(Cena),
        CsvConvert.ToText(BrojStrana),
        CsvConvert.ToText(AutorIds),
        CsvConvert.ToText(IzdavacId),
        CsvConvert.ToText(PosetiociKupiliIds),
        CsvConvert.ToText(PosetiociZeljaIds)
    };

    public void FromFields(string[] f) {
        Id = CsvConvert.ToInt(f[0]);
        Isbn = f[1];
        Naziv = f[2];
        Zanr = f[3];
        GodinaIzdanja = CsvConvert.ToInt(f[4]);
        Cena = CsvConvert.ToDouble(f[5]);
        BrojStrana = CsvConvert.ToInt(f[6]);
        AutorIds = CsvConvert.ToIntList(f[7]);
        IzdavacId = CsvConvert.ToInt(f[8]);
        PosetiociKupiliIds = CsvConvert.ToIntList(f[9]);
        PosetiociZeljaIds = CsvConvert.ToIntList(f[10]);
    }
}
