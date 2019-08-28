using Microsoft.EntityFrameworkCore;
using Sincere.Core.IRepository.Base;
using Sincere.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sincere.Core.Repository.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        private BaseCoreContext _db;
        private readonly DbSet<TEntity> _dbSet;

        internal BaseCoreContext Db
        {
            get { return _db; }
            private set { _db = value; }
        }
        public BaseRepository(IBaseContext mydbcontext)
        {
            this._db = mydbcontext as BaseCoreContext;
            this._dbSet = _db.Set<TEntity>();
        }

        #region INSERT

        /// <summary>
        /// 新增 实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public async Task<bool> Insert(TEntity model, bool isSaveChanges = false)
        public async Task<bool> Insert(TEntity model)
        {
            _db.Set<TEntity>().Add(model);
            //if (isSaveChanges)
            //{
            return await _db.SaveChangesAsync() > 0;
            //}
            //else
            //{
            //    return false;
            //}

        }

        /// <summary>
        /// 普通批量插入
        /// </summary>
        /// <param name="datas"></param>
        public async Task<bool> InsertRange(List<TEntity> datas)
        {
            await _db.Set<TEntity>().AddRangeAsync(datas);
            return await _db.SaveChangesAsync() == datas.Count;
        }

        #endregion INSERT

        #region Delete

        #region 2.0 根据id删除 +  int Del(T model)
        /// <summary>
        /// 2.0 根据id删除
        /// </summary>
        /// <param name="model">必须包含要删除id的对象</param>
        /// <returns></returns>
        public async Task<int> Del(TEntity model)
        {
            _db.Set<TEntity>().Attach(model);
            _db.Set<TEntity>().Remove(model);
            return await _db.SaveChangesAsync();
        }
        #endregion

        #region 2.1 根据条件删除 + int DelBy(Expression<Func<T, bool>> delWhere)
        /// <summary>
        /// 2.1 根据条件删除
        /// </summary>
        /// <param name="delWhere"></param>
        /// <returns>返回受影响的行数</returns>
        public async Task<int> DelBy(Expression<Func<TEntity, bool>> delWhere)
        {
            //2.1.1 查询要删除的数据
            List<TEntity> listDeleting = _db.Set<TEntity>().Where(delWhere).ToList();
            //2.1.2 将要删除的数据 用删除方法添加到 EF 容器中
            listDeleting.ForEach(u =>
            {
                _db.Set<TEntity>().Attach(u);  //先附加到EF 容器
                _db.Set<TEntity>().Remove(u); //标识为删除状态
            });
            //2.1.3 一次性生成sql语句 到数据库执行删除
            return await _db.SaveChangesAsync();
        }
        #endregion


        #endregion

        #region UPDATE

        #region 3.0 修改实体 +  int Modify(T model)
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> Modify(TEntity model)
        {
            //EntityEntry entry = _db.Entry<TEntity>(model);
            _db.Set<TEntity>().Update(model);
            return await _db.SaveChangesAsync();
        }
        #endregion

        #region 3.1 修改实体，可修改指定属性 + int Modify(T model, params string[] propertyNames)
        /// <summary>
        /// 3.1 修改实体，可修改指定属性
        /// </summary>
        /// <param name="model"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public async Task<int> Modify(TEntity model, params string[] propertyNames)
        {
            //3.1.1 将对象添加到EF中
            EntityEntry entry = _db.Entry<TEntity>(model);
            //3.1.2 先设置对象的包装状态为 Unchanged
            entry.State = EntityState.Unchanged;
            //3.1.3 循环被修改的属性名数组
            foreach (string propertyName in propertyNames)
            {
                //将每个被修改的属性的状态设置为已修改状态；这样在后面生成的修改语句时，就只为标识为已修改的属性更新
                entry.Property(propertyName).IsModified = true;
            }
            return await _db.SaveChangesAsync();
        }
        #endregion

        #region 3.2 批量修改 + int ModifyBy(T model, Expression<Func<T, bool>> whereLambda, params string[] modifiedPropertyNames)
        /// <summary>
        /// 3.2 批量修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="whereLambda"></param>
        /// <param name="modifiedPropertyNames"></param>
        /// <returns></returns>
        public async Task<int> ModifyBy(TEntity model, Expression<Func<TEntity, bool>> whereLambda, params string[] modifiedPropertyNames)
        {
            //3.2.1 查询要修改的数据
            List<TEntity> listModifing = _db.Set<TEntity>().Where(whereLambda).ToList();
            //3.2.2 获取实体类类型对象
            Type t = typeof(TEntity);
            //3.2.3 获取实体类所有的公共属性
            List<PropertyInfo> propertyInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            //3.2.4 创建实体属性字典集合
            Dictionary<string, PropertyInfo> dicPropertys = new Dictionary<string, PropertyInfo>();
            //3.2.5 将实体属性中要修改的属性名 添加到字典集合中  键：属性名  值：属性对象
            propertyInfos.ForEach(p =>
            {
                if (modifiedPropertyNames.Contains(p.Name))
                {
                    dicPropertys.Add(p.Name, p);
                }
            });
            //3.2.6 循环要修改的属性名
            foreach (string propertyName in modifiedPropertyNames)
            {
                //判断要修改的属性名是否在实体类的属性集合中存在
                if (dicPropertys.ContainsKey(propertyName))
                {
                    //如果存在，则取出要修改的属性对象
                    PropertyInfo proInfo = dicPropertys[propertyName];
                    //取出要修改的值
                    object newValue = proInfo.GetValue(model, null);
                    //批量设置要修改对象的属性
                    foreach (TEntity item in listModifing)
                    {
                        //为要修改的对象的要修改的属性设置新的值
                        proInfo.SetValue(item, newValue, null);
                    }
                }
            }
            //一次性生成sql语句 到数据库执行
            return await _db.SaveChangesAsync();
        }
        #endregion


        #endregion UPDATE

        #region SELECT

        #region  5.0 根据条件查询 + List<TEntity> GetListBy(Expression<Func<T, bool>> whereLambda)
        /// <summary>
        /// 5.0 根据条件查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetListBy(Expression<Func<TEntity, bool>> whereLambda)
        {
            return await _db.Set<TEntity>().Where(whereLambda).AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> GetModelById(Expression<Func<TEntity, bool>> whereLambda)
        {
            return await _db.Set<TEntity>().Where(whereLambda).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<List<TEntity>> GetList()
        {
            return await _db.Set<TEntity>().AsNoTracking().ToListAsync();
        }
        #endregion

        #region 5.1 根据条件查询，并排序 +  List<TEntity> GetListBy<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc = true)
        /// <summary>
        /// 5.1 根据条件查询，并排序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetListBy<TKey>(Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey>> orderLambda, bool isAsc = true)
        {
            if (isAsc)
            {
                return await _db.Set<TEntity>().Where(whereLambda).OrderBy(orderLambda).AsNoTracking().ToListAsync();
            }
            else
            {
                return await _db.Set<TEntity>().Where(whereLambda).OrderByDescending(orderLambda).AsNoTracking().ToListAsync();
            }
        }
        #endregion

        #region 5.2 根据条件查询Top多少个，并排序 + List<TEntity> GetListBy<TKey>(int top, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc = true)
        /// <summary>
        /// 5.2 根据条件查询Top多少个，并排序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="top"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetListBy<TKey>(int top, Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey>> orderLambda, bool isAsc = true)
        {
            if (isAsc)
            {
                return await _db.Set<TEntity>().Where(whereLambda).OrderBy(orderLambda).Take(top).AsNoTracking().ToListAsync();
            }
            else
            {
                return await _db.Set<TEntity>().Where(whereLambda).OrderByDescending(orderLambda).Take(top).AsNoTracking().ToListAsync();
            }
        }
        #endregion

        #region  5.3 根据条件排序查询  双排序 + List<TEntity> GetListBy<TKey1, TKey2>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey1>> orderLambda1, Expression<Func<T, TKey2>> orderLambda2, bool isAsc1 = true, bool isAsc2 = true)
        /// <summary>
        /// 5.3 根据条件排序查询  双排序
        /// </summary>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda1"></param>
        /// <param name="orderLambda2"></param>
        /// <param name="isAsc1"></param>
        /// <param name="isAsc2"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetListBy<TKey1, TKey2>(Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey1>> orderLambda1, Expression<Func<TEntity, TKey2>> orderLambda2, bool isAsc1 = true, bool isAsc2 = true)
        {
            if (isAsc1)
            {
                if (isAsc2)
                {
                    return await _db.Set<TEntity>().Where(whereLambda).OrderBy(orderLambda1).ThenBy(orderLambda2).AsNoTracking().ToListAsync();
                }
                else
                {
                    return await _db.Set<TEntity>().Where(whereLambda).OrderBy(orderLambda1).ThenByDescending(orderLambda2).AsNoTracking().ToListAsync();
                }
            }
            else
            {
                if (isAsc2)
                {
                    return await _db.Set<TEntity>().Where(whereLambda).OrderByDescending(orderLambda1).ThenBy(orderLambda2).AsNoTracking().ToListAsync();
                }
                else
                {
                    return await _db.Set<TEntity>().Where(whereLambda).OrderByDescending(orderLambda1).ThenByDescending(orderLambda2).AsNoTracking().ToListAsync();
                }
            }
        }
        #endregion

        #region 5.3 根据条件排序查询Top个数  双排序 + List<TEntity> GetListBy<TKey1, TKey2>(int top, Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, TKey1>> orderLambda1, Expression<Func<T, TKey2>> orderLambda2, bool isAsc1 = true, bool isAsc2 = true)
        /// <summary>
        ///  5.3 根据条件排序查询Top个数  双排序
        /// </summary>
        /// <typeparam name="TKey1"></typeparam>
        /// <typeparam name="TKey2"></typeparam>
        /// <param name="top"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda1"></param>
        /// <param name="orderLambda2"></param>
        /// <param name="isAsc1"></param>
        /// <param name="isAsc2"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetListBy<TKey1, TKey2>(int top, Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey1>> orderLambda1, Expression<Func<TEntity, TKey2>> orderLambda2, bool isAsc1 = true, bool isAsc2 = true)
        {
            if (isAsc1)
            {
                if (isAsc2)
                {
                    return await _db.Set<TEntity>().Where(whereLambda).OrderBy(orderLambda1).ThenBy(orderLambda2).Take(top).AsNoTracking().ToListAsync();
                }
                else
                {
                    return await _db.Set<TEntity>().Where(whereLambda).OrderBy(orderLambda1).ThenByDescending(orderLambda2).Take(top).AsNoTracking().ToListAsync();
                }
            }
            else
            {
                if (isAsc2)
                {
                    return await _db.Set<TEntity>().Where(whereLambda).OrderByDescending(orderLambda1).ThenBy(orderLambda2).Take(top).AsNoTracking().ToListAsync();
                }
                else
                {
                    return await _db.Set<TEntity>().Where(whereLambda).OrderByDescending(orderLambda1).ThenByDescending(orderLambda2).Take(top).AsNoTracking().ToListAsync();
                }
            }
        }
        #endregion

        #endregion SELECT

        #region 分页

        #region 6.0 分页查询 + List<T> GetPagedList<TKey>
        /// <summary>
        /// 分页查询 + List<TEntity> GetPagedList
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="whereLambda">条件 lambda表达式</param>
        /// <param name="orderBy">排序 lambda表达式</param>
        /// <returns></returns>
        public async Task<List<TEntity>> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey>> orderByLambda, bool isAsc = true)
        {
            // 分页 一定注意： Skip 之前一定要 OrderBy
            if (isAsc)
            {
                return await _db.Set<TEntity>().Where(whereLambda).OrderBy(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
            }
            else
            {
                return await _db.Set<TEntity>().Where(whereLambda).OrderByDescending(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
            }
        }
        #endregion

        #region 6.1分页查询 带输出 +List<TEntity> GetPagedList<TKey>
        /// <summary>
        /// 分页查询 带输出
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<PageModel<TEntity>> GetPagedList<TKey>(Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey>> orderByLambda, bool isAsc = true, int pageIndex = 1, int pageSize = 20)
        {
            var rowCount = await _db.Set<TEntity>().Where(whereLambda).CountAsync();

            int pageCount = (Math.Ceiling(rowCount.ObjToDecimal() / pageSize.ObjToDecimal())).ObjToInt();

            List<TEntity> list = null;

            if (isAsc)
            {
                list = await _db.Set<TEntity>().OrderBy(orderByLambda).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
            }
            else
            {
                list = await _db.Set<TEntity>().OrderByDescending(orderByLambda).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
            }

            return new PageModel<TEntity>() { dataCount = rowCount, pageCount = pageCount, page = pageIndex, PageSize = pageSize, data = list };
        }
        #endregion

        #endregion

        #region ORTHER

        /// <summary>
        /// 执行存储过程或自定义sql语句--返回集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> Query(string sql, List<SqlParameter> parms, CommandType cmdType = CommandType.Text)
        {
            //存储过程（exec getActionUrlId @name,@ID）
            if (cmdType == CommandType.StoredProcedure)
            {
                StringBuilder paraNames = new StringBuilder();
                foreach (var sqlPara in parms)
                {
                    paraNames.Append($" @{sqlPara},");
                }
                sql = paraNames.Length > 0 ? $"exec {sql} {paraNames.ToString().Trim(',')}" : $"exec {sql} ";
            }

            return await _db.Set<TEntity>().FromSql(sql).ToListAsync();

        }


        /// <summary>
        /// 回滚
        /// </summary>
        public void RollBackChanges()
        {
            var items = _db.ChangeTracker.Entries().ToList();
            items.ForEach(o => o.State = EntityState.Unchanged);
        }

        /// <summary>
        /// 自定义语句和存储过程的增删改--返回影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public async Task<int> Execute(string sql, List<SqlParameter> parms, CommandType cmdType = CommandType.Text)
        {
            //存储过程（exec getActionUrlId @name,@ID）
            if (cmdType == CommandType.StoredProcedure)
            {
                StringBuilder paraNames = new StringBuilder();
                foreach (var sqlPara in parms)
                {
                    paraNames.Append($" @{sqlPara},");
                }
                sql = paraNames.Length > 0 ?
                    $"exec {sql} {paraNames.ToString().Trim(',')}" :
                    $"exec {sql} ";
            }

            int ret = await _db.Database.ExecuteSqlCommandAsync(sql, parms.ToArray());
            return 0;
        }


        #endregion ORTHER
    }
}
