using System;

namespace S4Pay.Factory.Repository
{
    public interface IRepositoryBase<TDocument>
    {
        bool Save(TDocument document);
        bool Delete(Guid document);
        bool Set<TValue>(Guid id, string key, TValue value);
    }
}