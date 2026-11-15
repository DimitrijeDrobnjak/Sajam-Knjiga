using System;
using System.Text;
using ConsoleClient.Controllers;
using ConsoleClient.Utils;
using Core.DAO;
using Core.Interfaces;
using Core.Models;
using Core.Services;

namespace ConsoleClient;

public static class Program {
    public static void Main() {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        // Jedna instanca DAO-a po entitetu, deli se izmedju svih servisa
        // (dva DAO objekta za isti fajl bi se razilazila u memoriji).
        IDao<Adresa> adrese = new AdresaDao();
        IDao<Posetilac> posetioci = new PosetilacDao();
        IDao<Autor> autori = new AutorDao();
        IDao<Knjiga> knjige = new KnjigaDao();
        IDao<Kupovina> kupovine = new KupovinaDao();
        IDao<Izdavac> izdavaci = new IzdavacDao();

        PosetilacController posetilacController =
            new PosetilacController(new PosetilacService(posetioci, adrese, kupovine, knjige));
        KupovinaController kupovinaController =
            new KupovinaController(new KupovinaService(kupovine, posetioci, knjige));
        IzdavacController izdavacController =
            new IzdavacController(new IzdavacService(izdavaci, autori, knjige));

        AdresaController adresaController = new AdresaController(new AdresaService(adrese));
        AutorController autorController = new AutorController(new AutorService(autori), new AdresaService(adrese));
        KnjigaController knjigaController = new KnjigaController(new KnjigaService(knjige, izdavaci));

        while (true) {
            Console.WriteLine();
            Console.WriteLine("===== SAJAM KNJIGA =====");
            Console.WriteLine("1. Posetioci");
            Console.WriteLine("2. Kupovine");
            Console.WriteLine("3. Izdavaci");
            Console.WriteLine("4. Adrese");
            Console.WriteLine("5. Autori");
            Console.WriteLine("6. Knjige");
            Console.WriteLine("0. Izlaz");

            string izbor = ConsoleInput.ReadString("Izbor");
            switch (izbor) {
                case "1": posetilacController.Meni(); break;
                case "2": kupovinaController.Meni(); break;
                case "3": izdavacController.Meni(); break;
                case "4": adresaController.Meni(); break;
                case "5": autorController.Meni(); break;
                case "6": knjigaController.Meni(); break;
                case "0": return;
                default: Console.WriteLine("Nepoznata opcija."); break;
            }
        }
    }
}
