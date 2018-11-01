using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Framework.DATA
{
    public abstract class Context<TEntity, TObject> where TEntity : class
    {
        protected baseTransacoesEntities contex;

        public Context()
        {
            this.contex = new baseTransacoesEntities();

            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<TEntity, TObject>().IgnoreAllPropertiesWithAnInaccessibleSetter();
                cfg.CreateMap<TObject, TEntity>().IgnoreAllPropertiesWithAnInaccessibleSetter();
            });
        }

        protected IList<TObject> GetAll()
        {
            return Mapper.Map<IList<TEntity>, IList<TObject>>(this.contex.Set<TEntity>().ToList());
        }

        protected IList<TObject> GetAllFunction(Func<TEntity, bool> predicate)
        {
            var objs = this.contex.Set<TEntity>().Where(predicate).ToList();
            return Mapper.Map<IList<TEntity>, IList<TObject>>(objs);
        }

        protected TObject Find(params object[] ID)
        {
            var obj = contex.Set<TEntity>().Find(ID);
            contex.Entry(obj).State = EntityState.Detached;
            return Mapper.Map<TEntity, TObject>(obj);
        }

        protected TObject Update(TObject obj)
        {
            contex.Entry(Mapper.Map<TObject, TEntity>(obj)).State = EntityState.Modified;
            contex.SaveChanges();
            return obj;
        }

        protected void UpdateRange(List<TObject> objs)
        {
            contex.Entry(Mapper.Map<List<TObject>, List<TEntity>>(objs)).State = EntityState.Modified;
            contex.SaveChanges();
        }

        protected void Save()
        {
            contex.SaveChanges();
        }

        protected TObject Insert(TObject obj)
        {
            TEntity obj2 = contex.Set<TEntity>().Add(Mapper.Map<TObject, TEntity>(obj));
            contex.SaveChanges();
            return Mapper.Map<TEntity, TObject>(obj2);
        }

        protected List<TObject> InsertRange(List<TObject> objs)
        {
            List<TEntity> obj2 = contex.Set<TEntity>().AddRange(Mapper.Map<List<TObject>, List<TEntity>>(objs)).ToList();
            contex.SaveChanges();
            return Mapper.Map<List<TEntity>, List<TObject>>(obj2);
        }

        protected void Delete(TObject obj)
        {
            contex.Set<TEntity>().Remove(Mapper.Map<TObject, TEntity>(obj));
            contex.SaveChanges();
        }

        protected void DeleteRange(List<TObject> objs)
        {
            contex.Set<TEntity>().RemoveRange(Mapper.Map<List<TObject>, List<TEntity>>(objs));
            contex.SaveChanges();
        }

        protected void Dispose()
        {
            contex.Dispose();
        }
    }
}