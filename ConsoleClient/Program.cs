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
        IDao<Adresa> adrese = new CsvDao<Adresa>("adrese.csv");
        IDao<Posetilac> posetioci = new PosetilacDao();
        IDao<Autor> autori = new CsvDao<Autor>("autori.csv");
        IDao<Knjiga> knjige = new CsvDao<Knjiga>("knjige.csv");
        IDao<Kupovina> kupovine = new KupovinaDao();
        IDao<Izdavac> izdavaci = new IzdavacDao();

        PosetilacController posetilacController =
            new PosetilacController(new PosetilacService(posetioci, adrese, kupovine, knjige));
        KupovinaController kupovinaController =
            new KupovinaController(new KupovinaService(kupovine, posetioci, knjige));
        IzdavacController izdavacController =
            new IzdavacController(new IzdavacService(izdavaci, autori, knjige));

        while (true) {
            Console.WriteLine();
            Console.WriteLine("===== SAJAM KNJIGA =====");
            Console.WriteLine("1. Posetioci");
            Console.WriteLine("2. Kupovine");
            Console.WriteLine("3. Izdavaci");
            Console.WriteLine("9. [TEST] Ubaci probne autore i knjige");
            Console.WriteLine("0. Izlaz");

            string izbor = ConsoleInput.ReadString("Izbor");
            switch (izbor) {
                case "1": posetilacController.Meni(); break;
                case "2": kupovinaController.Meni(); break;
                case "3": izdavacController.Meni(); break;
                case "9": TestPodaci.Ubaci(adrese, autori, knjige); break;
                case "0": return;
                default: Console.WriteLine("Nepoznata opcija."); break;
            }
        }
    }
}
