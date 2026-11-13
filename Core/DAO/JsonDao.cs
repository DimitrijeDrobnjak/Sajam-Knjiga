using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Interfaces;

namespace Core.DAO
{
    // Genericki DAO: cuva listu entiteta u jedan JSON fajl.
    public class JsonDao<T> where T : class, IEntity
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        private readonly string _filePath;
        private List<T> _items = new List<T>();

        public JsonDao(string fileName)
        {
            string dir = Path.Combine(AppContext.BaseDirectory, "Data");
            Directory.CreateDirectory(dir);
            _filePath = Path.Combine(dir, fileName);
            Load();
        }

        public List<T> GetAll() => _items.ToList();

        public T? GetById(int id) => _items.FirstOrDefault(e => e.Id == id);

        public int NextId() => _items.Count == 0 ? 1 : _items.Max(e => e.Id) + 1;

        public T Add(T item)
        {
            item.Id = NextId();
            _items.Add(item);
            Save();
            return item;
        }

        public bool Update(T item)
        {
            int index = _items.FindIndex(e => e.Id == item.Id);
            if (index < 0) return false;
            _items[index] = item;
            Save();
            return true;
        }

        public bool Delete(int id)
        {
            int removed = _items.RemoveAll(e => e.Id == id);
            if (removed == 0) return false;
            Save();
            return true;
        }

        public void Save()
        {
            File.WriteAllText(_filePath, JsonSerializer.Serialize(_items, Options));
        }

        private void Load()
        {
            if (!File.Exists(_filePath)) return;
            string json = File.ReadAllText(_filePath);
            _items = JsonSerializer.Deserialize<List<T>>(json, Options) ?? new List<T>();
        }
    }
}
