namespace Core.Interfaces;

// Entitet koji zna da se pretvori u jedan red CSV fajla (kao niz polja) i nazad.
public interface ICsvRecord {
    string[] ToFields();
    void FromFields(string[] fields);
}
