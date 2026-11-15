using System;
using System.Linq;
using Core.Models;

namespace Core.DAO;

public class PosetilacDao : CsvDao<Posetilac> {
    public const string NazivDatoteke = "posetioci.csv";

    public PosetilacDao() : base(NazivDatoteke) { }

    public Posetilac? GetByBrojClanskeKarte(string brojClanskeKarte) {
        return _items.FirstOrDefault(p =>
            string.Equals(p.BrojClanskeKarte, brojClanskeKarte, StringComparison.OrdinalIgnoreCase));
    }
}
