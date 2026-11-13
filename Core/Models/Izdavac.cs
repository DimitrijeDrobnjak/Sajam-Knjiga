using System.Collections.Generic;
using Core.Interfaces;

namespace Core.Models
{
    public class Izdavac : IEntity
    {
        public int Id { get; set; }
        public string Sifra { get; set; } = "";
        public string Naziv { get; set; } = "";
        public int SefId { get; set; } // autor sa >= 5 godina iskustva
        public List<int> AutorIds { get; set; } = new List<int>();
        public List<int> KnjigaIds { get; set; } = new List<int>();
    }
}
