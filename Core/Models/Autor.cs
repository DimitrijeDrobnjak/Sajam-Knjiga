using System;
using System.Collections.Generic;
using Core.Interfaces;

namespace Core.Models
{
    public class Autor : IEntity
    {
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
    }
}
