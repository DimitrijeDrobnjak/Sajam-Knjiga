using System.Collections.Generic;

namespace Core.Interfaces;

public interface IDao<T> where T : class, IEntity {
    List<T> GetAll();
    T? GetById(int id);
    T Add(T item);
    bool Update(T item);
    bool Delete(int id);
    void Save();
}
