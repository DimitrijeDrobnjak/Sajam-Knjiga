using System.Collections.Generic;
using Core.Interfaces;

namespace Core.Models
{
    public class Knjiga : IEntity
    {
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
    }
}
