using System;
using System.Linq;
using Core.Models;

namespace Core.DAO;

public class IzdavacDao : CsvDao<Izdavac> {
    public const string NazivDatoteke = "izdavaci.csv";

    public IzdavacDao() : base(NazivDatoteke) { }

    public Izdavac? GetBySifra(string sifra) {
        return _items.FirstOrDefault(i =>
            string.Equals(i.Sifra, sifra, StringComparison.OrdinalIgnoreCase));
    }
}
