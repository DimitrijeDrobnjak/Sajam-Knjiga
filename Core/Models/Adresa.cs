using Core.Interfaces;

namespace Core.Models
{
    public class Adresa : IEntity
    {
        public int Id { get; set; }
        public string Ulica { get; set; } = "";
        public string Broj { get; set; } = "";
        public string Grad { get; set; } = "";
        public string Drzava { get; set; } = "";
    }
}
