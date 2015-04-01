namespace THZ.App.Template.Helpers.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Uninf.Cache;
    using Uninf.CacheData;
    using Uninf.Data;
    using Uninf.Model;

    public abstract class ObjDetailBase<TDbModel,TCacheModel,Tkey>:ObjBase<TCacheModel,Tkey> where TDbModel:class
    {
        protected IUnitOfWork uow;

        protected IModelConverter<TDbModel, TCacheModel> converter;

        public ObjDetailBase(ICache cache, IUnitOfWork uow, IModelConverter<TDbModel, TCacheModel> converter)
            : base(cache)
        {
            this.uow = uow;
            this.converter = converter;
        }

        protected override TCacheModel Rebuild(Tkey key)
        {
            var obj = this.uow.GetRepository<TDbModel>().Find(key);
            return this.converter.Convert(obj);
        }

        protected override Dictionary<Tkey, TCacheModel> Rebuild(params Tkey[] keys)
        {
            return this.uow.GetRepository<TDbModel>()
                .AsQueryable()
                .Where(this.GetWhere(keys))
                .ToDictionary(this.KeySelector(), x => this.converter.Convert(x));
        }

        protected abstract Expression<Func<TDbModel, bool>> GetWhere(params Tkey[] keys);

        protected abstract Func<TDbModel, Tkey> KeySelector();

    }
}