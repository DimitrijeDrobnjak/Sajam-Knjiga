using System;
using System.Collections.Generic;
using System.Text;
using Core.Models;

namespace Core.DAO;

public class AutorDao : CsvDao<Autor>
{
    public const string NazivDatoteke = "autori.csv";

    public AutorDao() : base(NazivDatoteke) { }
}