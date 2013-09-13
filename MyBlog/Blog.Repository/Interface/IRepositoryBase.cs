using System;
using System.Linq;
using System.Collections.Generic;
namespace Blog.Repository.Interface
{
    public enum OrderType
    {
        desc,
        asc
    }
    /// <summary>
    /// 数据操作存储父类
    /// </summary>
    public interface IRepositoryBase<T> where T : class
    {
        /// <summary>
        /// 通过委托查询
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        IList<T> Find(Func<IQueryable<T>, IQueryable<T>> expr);

        /// <summary>
        /// 通过委托查询单个结果
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        object Single(Func<IQueryable<T>, object> expr);

        /// <summary>
        /// 使用Sql更新或删除
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        int Update(string sql, object[] args = null);

        /// <summary>
        /// 根据ID取得实体（因为设计表时为每一个表设置了自增主键，所以直接使用Int id）
        /// </summary>
        /// <param name="id">主键</param>
        T Get(int id);

        /// <summary>
        /// 取得所有实体
        /// </summary>
        /// <returns></returns>
        IList<T> GetAll();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entiry">实体数据</param>
        int Add(T entiry);

        /// <summary>
        /// 删除实体（因为设计每一个表都有主键，所以使用Int型）
        /// </summary>
        /// <param name="id">主键</param>
        void Delete(int id);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体数据</param>
        void Update(T entity);

        /// <summary>
        /// 保存修改
        /// </summary>
        void Save();

        /// <summary>
        /// 开启事务
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTransaction();
    }
}
