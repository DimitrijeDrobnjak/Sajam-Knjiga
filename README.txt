SAJAM KNJIGA
OISISI 2026/2027 - informacioni sistem uprave sajma knjiga (C#, WPF, MVC)

TIM
Student 1: Dimitrije Drobnjak RA119/2024
Student 2: Bogdan Ilijev RA186/2024

STRUKTURA RESENJA
SajamKnjiga.slnx
  Core           - modeli, interfejsi, DAO, servisi (poslovna pravila), pomocne klase
  ConsoleClient  - konzolna aplikacija (kontroleri, unos i ispis)
  WpfClient      - WPF aplikacija (drugi domaci zadatak)
  Data           - CSV fajlovi sa podacima (separator ';')

POKRETANJE
Otvoriti SajamKnjiga.slnx u Visual Studio-u, postaviti ConsoleClient kao Startup Project i pokrenuti (F5).
Podaci se citaju iz foldera Data pored .slnx fajla i upisuju nakon svake izmene.

FORMAT DATOTEKA
Jedan red = jedan entitet, polja razdvojena sa ';'. Znaci \ ; i novi red u tekstu se upisuju sa prefiksom \.
Liste identifikatora se upisuju odvojene zarezom (npr. 1,4,7).
Datumi su u formatu yyyy-MM-dd. Datoteke: posetioci.csv, kupovine.csv, izdavaci.csv, adrese.csv, autori.csv, knjige.csv.

RASPODELA POSLA

Student 1 - Dimitrije Drobnjak (#posetilac, #kupovina, #izdavac)
- Pokretanje projekta: Git repozitorijum, povezivanje sa GitHub-om, kreiranje solution-a (Core, ConsoleClient, WpfClient)
- Modeli: Posetilac (sa statusom i listom zelja), Kupovina (ocena, komentar, datum), Izdavac (sa sefom)
- Interfejsi i DAO sloj: IEntity, ICsvRecord, IDao, genericki CsvDao<T>, PosetilacDao, KupovinaDao, IzdavacDao
- Pomocne klase: CsvFile, CsvConvert, DataPaths, Validator, ValidacijaException
- Servisi: PosetilacService, KupovinaService, IzdavacService (validacija, jedinstvenost, referencijalni integritet,
  prosecna ocena posetioca, veza knjiga-izdavac)
- Kontroleri i konzolni ispis: PosetilacController, KupovinaController, IzdavacController, ConsoleInput, ConsoleTable, glavni meni
- Funkcionalnosti 8.1-8.4 (dodavanje, izmena, brisanje, prikaz) za posetioce, kupovine i izdavace

Student 2 - Bogdan Ilijev (#autor, #knjiga, #adresa)
- Modeli: Autor (podaci o autoru i godine iskustva), Knjiga (ISBN, žanr, cena, izdavač), Adresa (ulica, broj, mesto i poštanski broj)
- DAO sloj: AutorDao, KnjigaDao, AdresaDao
- Servisi: AutorService, KnjigaService, AdresaService (validacija podataka, jedinstvenost i provera postojanja povezanih entiteta)
- Kontroleri i ispis u konzoli: AutorController, KnjigaController, AdresaController
- Funkcionalnosti 8.1–8.4 (dodavanje, izmena, brisanje i prikaz) za autore, knjige i adrese
