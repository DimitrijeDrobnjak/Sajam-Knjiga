using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;

namespace Core.DAO;

public class AdresaDao : CsvDao<Adresa>
{
    public const string NazivDatoteke = "adrese.csv";

    public AdresaDao() : base(NazivDatoteke) { }
}
