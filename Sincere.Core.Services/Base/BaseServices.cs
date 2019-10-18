using Sincere.Core.IRepository.Base;
using Sincere.Core.IServices.Base;
using Sincere.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sincere.Core.Services.Base
{
    public class BaseServices<TEntity> : IBaseServices<TEntity> where TEntity : class, new()
    {
        //public IBaseRepository<TEntity> baseDal = new BaseRepository<TEntity>();
        public IBaseRepository<TEntity> BaseDal;//通过在子类的构造函数中注入，这里是基类，不用构造函数
         
        public async Task<int> Del(TEntity model)
        {
           return await BaseDal.Del(model);
        }

        public async Task<int> DelBy(Expression<Func<TEntity, bool>> delWhere)
        {
            return await BaseDal.DelBy(delWhere);
        }

        public async Task<int> Execute(string sql, List<SqlParameter> parms, CommandType cmdType = CommandType.Text)
        {
            return await BaseDal.Execute(sql,parms,cmdType);
        }

        public async Task<List<TEntity>> GetList()
        {
           return await BaseDal.GetList();
        }

        public async Task<List<TEntity>> GetListBy(Expression<Func<TEntity, bool>> whereLambda)
        {
            return await BaseDal.GetListBy(whereLambda);
        }

        public async Task<List<TEntity>> GetListBy<TKey>(Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey>> orderLambda, bool isAsc = true)
        {
            return await BaseDal.GetListBy(whereLambda,orderLambda,isAsc);
        }

        public async Task<List<TEntity>> GetListBy<TKey>(int top, Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey>> orderLambda, bool isAsc = true)
        {
            return await BaseDal.GetListBy(top, whereLambda, orderLambda, isAsc);
        }

        public async Task<List<TEntity>> GetListBy<TKey1, TKey2>(Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey1>> orderLambda1, Expression<Func<TEntity, TKey2>> orderLambda2, bool isAsc1 = true, bool isAsc2 = true)
        {
            return await BaseDal.GetListBy( whereLambda, orderLambda1,orderLambda2, isAsc1,isAsc2);
        }

        public async Task<List<TEntity>> GetListBy<TKey1, TKey2>(int top, Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey1>> orderLambda1, Expression<Func<TEntity, TKey2>> orderLambda2, bool isAsc1 = true, bool isAsc2 = true)
        {
            return await BaseDal.GetListBy(top, whereLambda, orderLambda1, orderLambda2, isAsc1, isAsc2);
        }

        public async Task<TEntity> GetModelById(Expression<Func<TEntity, bool>> whereLambda)
        {
            return await BaseDal.GetModelById( whereLambda);
        }

        public async Task<List<TEntity>> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey>> orderByLambda, bool isAsc = true)
        {
            return await BaseDal.GetPagedList(pageIndex,pageSize, whereLambda,orderByLambda,isAsc);
        }

        public async Task<PageModel<TEntity>> GetPagedList<TKey>(Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey>> orderByLambda, bool isAsc = true, int pageIndex = 1, int pageSize = 20)
        {
            return await BaseDal.GetPagedList(  whereLambda, orderByLambda, isAsc, pageIndex, pageSize);
        }

        public async Task<bool> Insert(TEntity model)
        {
            return await BaseDal.Insert(model);
        }

        public async Task<bool> InsertRange(List<TEntity> datas)
        {
            return await BaseDal.InsertRange(datas);
        }

        public async Task<int> Modify(TEntity model)
        {
            return await BaseDal.Modify(model);
        }

        public async Task<int> Modify(TEntity model, params string[] propertyNames)
        {
            return await BaseDal.Modify(model,propertyNames);
        }

        public async Task<int> ModifyBy(TEntity model, Expression<Func<TEntity, bool>> whereLambda, params string[] modifiedPropertyNames)
        {
            return await BaseDal.ModifyBy(model, whereLambda,modifiedPropertyNames);
        }

        public async Task<List<TEntity>> Query(string sql, List<SqlParameter> parms, CommandType cmdType = CommandType.Text)
        {
            return await BaseDal.Query(sql, parms, cmdType);
        }

        public  void RollBackChanges()
        {
              BaseDal.RollBackChanges();
        }
    }

}
