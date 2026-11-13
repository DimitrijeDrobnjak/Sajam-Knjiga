using System;
using System.Collections.Generic;
using Core.Interfaces;

namespace Core.Models
{
    public class Posetilac : IEntity
    {
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
    }
}
