using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;

namespace Core.DAO;

public class KnjigaDao : CsvDao<Knjiga>
{
    public const string NazivDatoteke = "knjige.csv";

    public KnjigaDao() : base(NazivDatoteke) { }
}
