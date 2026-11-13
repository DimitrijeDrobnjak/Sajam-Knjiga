using System;
using Core.Interfaces;

namespace Core.Models
{
    public class Kupovina : IEntity
    {
        public int Id { get; set; }
        public int PosetilacId { get; set; }
        public int KnjigaId { get; set; }
        public DateTime DatumKupovine { get; set; }
        public int Ocena { get; set; } // 1-5
        public string Komentar { get; set; } = "";
    }
}
