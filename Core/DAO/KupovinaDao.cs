using System.Collections.Generic;
using System.Linq;
using Core.Models;

namespace Core.DAO;

public class KupovinaDao : CsvDao<Kupovina> {
    public const string NazivDatoteke = "kupovine.csv";

    public KupovinaDao() : base(NazivDatoteke) { }

    public List<Kupovina> GetByPosetilac(int posetilacId) {
        return _items.Where(k => k.PosetilacId == posetilacId).ToList();
    }

    public List<Kupovina> GetByKnjiga(int knjigaId) {
        return _items.Where(k => k.KnjigaId == knjigaId).ToList();
    }
}
