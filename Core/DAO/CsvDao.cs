using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Core.Interfaces;
using Core.Utils;

namespace Core.DAO;

// Genericki DAO za sve entitete. Pri pravljenju ucita CSV fajl (jedan red = jedan entitet),
// a posle svakog dodavanja, izmene i brisanja ponovo upise ceo fajl.
public class CsvDao<T> : IDao<T> where T : class, IEntity, ICsvRecord, new() {
    private readonly string _filePath;
    protected readonly List<T> _items = new List<T>();

    public CsvDao(string fileName) {
        _filePath = DataPaths.GetFilePath(fileName);
        Load();
    }

    public List<T> GetAll() {
        return _items.ToList();
    }

    public T? GetById(int id) {
        return _items.FirstOrDefault(x => x.Id == id);
    }

    public T Add(T item) {
        item.Id = _items.Count == 0 ? 1 : _items.Max(x => x.Id) + 1;
        _items.Add(item);
        Save();
        return item;
    }

    public bool Update(T item) {
        int index = _items.FindIndex(x => x.Id == item.Id);
        if (index < 0)
            return false;

        _items[index] = item;
        Save();
        return true;
    }

    public bool Delete(int id) {
        int obrisano = _items.RemoveAll(x => x.Id == id);
        if (obrisano == 0)
            return false;

        Save();
        return true;
    }

    public void Save() {
        IEnumerable<string> lines = _items.Select(x => CsvFile.JoinLine(x.ToFields()));
        File.WriteAllLines(_filePath, lines, new UTF8Encoding(false));
    }

    private void Load() {
        if (!File.Exists(_filePath))
            return;

        string[] lines = File.ReadAllLines(_filePath, Encoding.UTF8);
        for (int i = 0; i < lines.Length; i++) {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            try {
                T item = new T();
                item.FromFields(CsvFile.SplitLine(lines[i]));
                _items.Add(item);
            } catch (Exception ex) when (ex is FormatException || ex is OverflowException
                                       || ex is IndexOutOfRangeException || ex is ArgumentException) {
                throw new InvalidDataException($"Neispravan red {i + 1} u fajlu '{_filePath}': {ex.Message}", ex);
            }
        }
    }
}
