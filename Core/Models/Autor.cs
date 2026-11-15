using System;
using System.Collections.Generic;
using Core.Interfaces;
using Core.Utils;

namespace Core.Models;

public class Autor : IEntity, ICsvRecord {
    public int Id { get; set; }
    public string Ime { get; set; } = "";
    public string Prezime { get; set; } = "";
    public DateTime DatumRodjenja { get; set; }
    public int AdresaId { get; set; }
    public string Telefon { get; set; } = "";
    public string Email { get; set; } = "";
    public string BrojLicneKarte { get; set; } = "";
    public int GodineIskustva { get; set; }
    public List<int> KnjigaIds { get; set; } = new List<int>();

    public string[] ToFields() => new[] {
        CsvConvert.ToText(Id),
        Ime,
        Prezime,
        CsvConvert.ToText(DatumRodjenja),
        CsvConvert.ToText(AdresaId),
        Telefon,
        Email,
        BrojLicneKarte,
        CsvConvert.ToText(GodineIskustva),
        CsvConvert.ToText(KnjigaIds)
    };

    public void FromFields(string[] f) {
        Id = CsvConvert.ToInt(f[0]);
        Ime = f[1];
        Prezime = f[2];
        DatumRodjenja = CsvConvert.ToDate(f[3]);
        AdresaId = CsvConvert.ToInt(f[4]);
        Telefon = f[5];
        Email = f[6];
        BrojLicneKarte = f[7];
        GodineIskustva = CsvConvert.ToInt(f[8]);
        KnjigaIds = CsvConvert.ToIntList(f[9]);
    }
}
