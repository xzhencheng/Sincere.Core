using Sincere.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sincere.Core.IRepository.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<int> Execute(string sql, List<SqlParameter> parms, CommandType cmdType = CommandType.Text);
        Task<List<TEntity>> Query(string sql, List<SqlParameter> parms, CommandType cmdType = CommandType.Text);
        Task<bool> Insert(TEntity model);
        Task<bool> InsertRange(List<TEntity> datas);

        Task<int> Del(TEntity model);

        Task<int> DelBy(Expression<Func<TEntity, bool>> delWhere);

        Task<int> Modify(TEntity model);

        Task<int> Modify(TEntity model, params string[] propertyNames);

        Task<int> ModifyBy(TEntity model, Expression<Func<TEntity, bool>> whereLambda, params string[] modifiedPropertyNames);

        Task<List<TEntity>> GetList();

        Task<List<TEntity>> GetListBy(Expression<Func<TEntity, bool>> whereLambda);

        Task<TEntity> GetModelById(Expression<Func<TEntity, bool>> whereLambda);

        Task<List<TEntity>> GetListBy<TKey>(Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey>> orderLambda, bool isAsc = true);

        Task<List<TEntity>> GetListBy<TKey>(int top, Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey>> orderLambda, bool isAsc = true);

        Task<List<TEntity>> GetListBy<TKey1, TKey2>(Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey1>> orderLambda1, Expression<Func<TEntity, TKey2>> orderLambda2, bool isAsc1 = true, bool isAsc2 = true);

        Task<List<TEntity>> GetListBy<TKey1, TKey2>(int top, Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey1>> orderLambda1, Expression<Func<TEntity, TKey2>> orderLambda2, bool isAsc1 = true, bool isAsc2 = true);

        Task<List<TEntity>> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey>> orderByLambda, bool isAsc = true);

        Task<PageModel<TEntity>> GetPagedList<TKey>(Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey>> orderByLambda, bool isAsc = true, int pageIndex = 1, int pageSize = 20);

        void RollBackChanges();

    }
}