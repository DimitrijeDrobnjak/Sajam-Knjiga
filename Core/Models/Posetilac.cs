using System;
using System.Collections.Generic;
using Core.Interfaces;
using Core.Utils;

namespace Core.Models;

public class Posetilac : IEntity, ICsvRecord {
    public int Id { get; set; }
    public string Ime { get; set; } = "";
    public string Prezime { get; set; } = "";
    public DateTime DatumRodjenja { get; set; }
    public int AdresaId { get; set; }
    public string Telefon { get; set; } = "";
    public string Email { get; set; } = "";
    public string BrojClanskeKarte { get; set; } = ""; // npr. CK-1-2023
    public int GodinaClanstva { get; set; }
    public StatusPosetioca Status { get; set; }
    public double ProsecnaOcena { get; set; }
    public List<int> KupovinaIds { get; set; } = new List<int>();
    public List<int> ListaZeljaKnjigaIds { get; set; } = new List<int>();

    public string[] ToFields() => new[] {
        CsvConvert.ToText(Id),
        BrojClanskeKarte,
        Ime,
        Prezime,
        CsvConvert.ToText(DatumRodjenja),
        CsvConvert.ToText(AdresaId),
        Telefon,
        Email,
        CsvConvert.ToText(GodinaClanstva),
        Status.ToString(),
        CsvConvert.ToText(ProsecnaOcena),
        CsvConvert.ToText(KupovinaIds),
        CsvConvert.ToText(ListaZeljaKnjigaIds)
    };

    public void FromFields(string[] f) {
        Id = CsvConvert.ToInt(f[0]);
        BrojClanskeKarte = f[1];
        Ime = f[2];
        Prezime = f[3];
        DatumRodjenja = CsvConvert.ToDate(f[4]);
        AdresaId = CsvConvert.ToInt(f[5]);
        Telefon = f[6];
        Email = f[7];
        GodinaClanstva = CsvConvert.ToInt(f[8]);
        Status = Enum.Parse<StatusPosetioca>(f[9]);
        ProsecnaOcena = CsvConvert.ToDouble(f[10]);
        KupovinaIds = CsvConvert.ToIntList(f[11]);
        ListaZeljaKnjigaIds = CsvConvert.ToIntList(f[12]);
    }
}
