using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Repository.Interface;
using Blog.Model;
namespace Blog.Domain.Interface
{
    public interface IArticleService
    {
        /// <summary>
        /// 浏览文章
        /// </summary>
        /// <param name="articleId"></param>
        Article BrowseArticle(int articleId);

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="article"></param>
        int AddArticle(Article article);

        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="article"></param>
        void UpdateArticle(Article article);

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id"></param>
        void DeleteArticle(int id);

        /// <summary>
        /// 取得文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Article GetArticle(int id);

        /// <summary>
        /// 取得最新文章列表
        /// </summary>
        /// <param name="count">文章个数</param>
        /// <returns></returns>
        IList<Article> GetLastBlog();

        /// <summary>
        /// 取得最新页面列表
        /// </summary>
        /// <param name="count">页面个数</param>
        /// <returns></returns>
        IList<Article> GetLastPage();

        /// <summary>
        /// 根据某一天取得那天发表的文章
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        PageInfo<Article> FindArticleByDay(DateTime day, int pageIndex);

        /// <summary>
        /// 根据某一个月取得那一个月发表的文章
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        PageInfo<Article> FindArticleByMonth(DateTime month, int pageInde);

        /// <summary>
        /// 根据文章标题搜索文章
        /// </summary>
        /// <param name="title">文章标题</param>
        /// <param name="pageIndex">当前页数</param>
        /// <returns></returns>
        PageInfo<Article> FindArticleByTitle(string title, int pageIndex);

        /// <summary>
        /// 根据分类Id取得该分类下所有文章
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        PageInfo<Article> GetArticleByCategory(int categoryId, int pageIndex);

        /// <summary>
        /// 根据查询条件分页查询
        /// </summary>
        /// <param name="expr">查询委托</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PageInfo<Article> FindArticlesByQuery(Func<IQueryable<Article>, IQueryable<Article>> expr, int pageIndex, int pageSize);

        /// <summary>
        /// 查询首横首列
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        object GetArticleSingle(Func<IQueryable<Article>, object> expr);
    }
}
