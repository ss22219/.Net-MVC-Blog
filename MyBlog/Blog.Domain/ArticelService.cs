using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Domain.Interface;
using Blog.Repository.Interface;
using Blog.Model;
using System.Web;
using System.Web.Caching;

namespace Blog.Domain
{
	/// <summary>
	/// 文章业务类
	/// </summary>
	public class ArticleService : IArticleService
	{
		/// <summary>
		/// 依赖注入赋值
		/// </summary>
		private IArticleRepository _articleRepository;
		private IRoleService _roleService;
		private ISettingService _settingService;
		private IUserService _userService;
		private ICategoryService _categoryService;

		public ArticleService (ICategoryService categoryService, IArticleRepository articleRepository, IRoleService roleService, ISettingService settingService, IUserService userService)
		{
			_userService = userService;
			_articleRepository = articleRepository;
			_roleService = roleService;
			_settingService = settingService;
			_categoryService = categoryService;
		}

		public int AddArticle (Model.Article article)
		{

			if (_roleService.AddArticlePower ()) {
				if (string.IsNullOrEmpty (article.Title)) {
					throw new DomainException ("Title", "文章标题不能为空！");
				} else if (string.IsNullOrEmpty (article.Content)) {
					throw new DomainException ("Content", "文章内容不能为空！");
				} else if (article.Categorys.Count == 0) {
					throw new DomainException ("Category", "文章必须要有一个分类！");
				}
				article.Status = ArticleStatus.Open;
				article.Author = _userService.GetLoginUser ();
				IList<Category> categorys = article.Categorys;
				article.Categorys = null;
				int id = _articleRepository.Add (article);
				article.Categorys = categorys;
				_articleRepository.Update (article);
				return id;
			} else {
				throw new DomainException ("NoPower", "对不起，您没有权限添加文章。");
			}
		}

		public void UpdateArticle (Model.Article article)
		{
			Article article2 = _articleRepository.Get (article.ArticleId);
			if (article2 == null) {
				throw new DomainException ("NoFind", "对不起，该文章不存在或已经被删除。");
			}
			if (!_roleService.EditArticlePower ()) {
				throw new DomainException ("NoPower", "对不起，您没有权限编辑该文章。");
			} else {
				article.ModifyDate = DateTime.Now;
				_articleRepository.Update (article);
			}
		}

		public void DeleteArticle (int id)
		{
			Article article = _articleRepository.Get (id);
			if (article == null) {
				throw new DomainException ("NoFind", "对不起，该文章不存在或已经被删除。");
			}
			if (_roleService.DeleteArticlePower ()) {
				_articleRepository.BeginTransaction ();
				try {
					_articleRepository.Update ("delete from Comment where ArticleId=" + id);
					_articleRepository.Update ("delete from Attach where ArticleId=" + id);
					_articleRepository.Update ("delete from CategoryRelationShip where ArticleId=" + id);
					_articleRepository.Update ("delete from Article where ArticleId=" + id);
					foreach (var category in article.Categorys) {
						category.Count = category.Count - 1 >= 0 ? category.Count - 1 : 0;
						_categoryService.UpdateCategory (category);
					}
				} catch (Exception ex) {
					_articleRepository.RollbackTransaction ();
					throw new DomainException ("DatabaseError", ex.Message);
				}
				_articleRepository.CommitTransaction ();
			} else {
				throw new DomainException ("NoPower", "对不起，您没有权限删除该文章。");
			}
		}

		public Model.Article GetArticle (int id)
		{
			Article article = (Article)HttpRuntime.Cache.Get ("Article_" + id);
			if (article != null)
				return article;
			article = _articleRepository.Get (id);
			if (article == null) {
				throw new DomainException ("NoFind", "对不起，该文章不存在或已经被删除。");
			}
			HttpRuntime.Cache.Insert ("Article_" + id, article, new SqlCacheDependency ("Default", "Article"), DateTime.MaxValue, TimeSpan.FromSeconds (30));
			return article;
		}

		public PageInfo<Model.Article> FindArticleByTitle (string title, int pageIndex)
		{
			int pageSize = int.Parse (_settingService.GetSetting ("ArticlePageSize"));
			PageInfo<Article> pageInfo = new PageInfo<Article> ();
			pageInfo.PageSize = pageSize;

			int totalItem = (int)_articleRepository.Single (
				qeruy => qeruy
                    .Where (a => a.Type == ArticleType.Blog && a.Status == ArticleStatus.Open && a.Title.Contains (title))
                    .Count ()
			);

			pageInfo.TotalItem = totalItem;
			pageIndex = pageIndex > pageInfo.TotalPage ? pageInfo.TotalPage : pageIndex;
			pageIndex = pageIndex <= 0 ? 1 : pageIndex;
			IList<Article> list = _articleRepository.Find (
				Query => Query
                .Where (a => a.Type == ArticleType.Blog && a.Status == ArticleStatus.Open && a.Title.Contains (title))
                .OrderByDescending (a => a.CreateDate)
                .Skip ((pageIndex - 1) * pageSize).Take (pageSize)
			);

			pageInfo.PageItems = list;
			pageInfo.PageIndex = pageIndex;

			return pageInfo;
		}

		public PageInfo<Model.Article> GetArticleByCategory (int categoryId, int pageIndex)
		{
			PageInfo<Article> articles = (PageInfo<Article>)HttpRuntime.Cache.Get ("Article_Category" + categoryId + "_" + pageIndex);
			if (articles != null)
				return articles;
			int pageSize = int.Parse (_settingService.GetSetting ("ArticlePageSize"));
			PageInfo<Article> pageInfo = new PageInfo<Article> ();
			pageInfo.PageSize = pageSize;
			int totalItem = (int)_articleRepository.Single (
				qeruy => qeruy
                    .Where (a => a.Type == ArticleType.Blog && a.Status == ArticleStatus.Open && a.Categorys.Where (c => c.CategoryId == categoryId).Count () > 0)
                    .Count ()
			);
			pageInfo.TotalItem = totalItem;
			pageIndex = pageIndex > pageInfo.TotalPage ? pageInfo.TotalPage : pageIndex;

			IList<Article> list = _articleRepository.Find (
				Query => Query
                .Where (a => a.Type == ArticleType.Blog && a.Status == ArticleStatus.Open && a.Categorys.Where (c => c.CategoryId == categoryId).Count () > 0)
                .OrderByDescending (a => a.CreateDate)
                .Skip ((pageIndex - 1) * pageSize).Take (pageSize)
			);
			pageInfo.PageItems = list;
			pageInfo.PageIndex = pageIndex;
			HttpRuntime.Cache.Insert ("Article_Category" + categoryId + "_" + pageIndex, pageInfo, new SqlCacheDependency ("Default", "Article"), DateTime.MaxValue, TimeSpan.FromSeconds (30));
			return pageInfo;
		}

		public PageInfo<Model.Article> FindArticleByMonth (DateTime month, int pageIndex)
		{
			int pageSize = int.Parse (_settingService.GetSetting ("ArticlePageSize"));

			PageInfo<Article> pageInfo = new PageInfo<Article> ();
			pageInfo.PageSize = pageSize;

			int totalItem = (int)_articleRepository.Single (
				qeruy => qeruy
                    .Where (a => a.Type == ArticleType.Blog && a.Status == ArticleStatus.Open && a.CreateDate.Year == month.Year && a.CreateDate.Month == month.Month)
                    .Count ()
			);
			pageInfo.TotalItem = totalItem;
			pageIndex = pageIndex > pageInfo.TotalPage ? pageInfo.TotalPage : pageIndex;
			pageIndex = pageIndex <= 0 ? 1 : pageIndex;
			IList<Article> list = _articleRepository.Find (
				Query => Query
                    .Where (a => a.Type == ArticleType.Blog && a.Status == ArticleStatus.Open && a.CreateDate.Year == month.Year && a.CreateDate.Month == month.Month)
			);
			pageInfo.PageItems = list;
			pageInfo.PageIndex = pageIndex;
			return pageInfo;
		}

		public PageInfo<Model.Article> FindArticleByDay (DateTime day, int pageIndex)
		{

			PageInfo<Article> articles = (PageInfo<Article>)HttpRuntime.Cache.Get ("Article_day" + day + "_" + pageIndex);
			if (articles != null)
				return articles;

			int pageSize = int.Parse (_settingService.GetSetting ("ArticlePageSize"));
			PageInfo<Article> pageInfo = new PageInfo<Article> ();
			pageInfo.PageSize = pageSize;
			int totalItem = (int)_articleRepository.Single (
				qeruy => qeruy
                    .Where (
				a => a.Type == ArticleType.Blog && a.Status == ArticleStatus.Open && a.CreateDate.Day == day.Day && a.CreateDate.Month == day.Month && a.CreateDate.Year == day.Year
			)
                    .Count ()
			);
			pageInfo.TotalItem = totalItem;
			pageIndex = pageIndex > pageInfo.TotalPage ? pageInfo.TotalPage : pageIndex;

			IList<Article> list = _articleRepository.Find (
				query => query.Where (
				a => a.Type == ArticleType.Blog && a.Status == ArticleStatus.Open && a.CreateDate.Day == day.Day && a.CreateDate.Month == day.Month && a.CreateDate.Year == day.Year
			)
                 .OrderByDescending (a => a.CreateDate)
                 .Skip ((pageIndex - 1) * pageSize).Take (pageSize)
			);
			pageInfo.PageItems = list;
			pageInfo.PageIndex = pageIndex;
			HttpRuntime.Cache.Insert ("Article_day" + day + "_" + pageIndex, pageInfo, new SqlCacheDependency ("Default", "Article"), DateTime.MaxValue, TimeSpan.FromSeconds (30));
			return pageInfo;
		}

		public IList<Article> GetLastBlog ()
		{
			IList<Article> LastBlog = null;
			int count = int.Parse (_settingService.GetSetting ("LastArticleCount"));
			LastBlog = _articleRepository.Find (
				query => query.Where (
				a => a.Type == ArticleType.Blog && a.Status == ArticleStatus.Open
				)
				.OrderByDescending (a => a.CreateDate)
				.Take (count)
				);
			return LastBlog;
		}

		public IList<Article> GetLastPage ()
		{
			IList<Article> LastPage = (IList<Article>)HttpRuntime.Cache.Get ("Article_LastPage");
			if (LastPage != null) {
				return LastPage;
			}
			int count = int.Parse (_settingService.GetSetting ("LastArticleCount"));
			LastPage = _articleRepository.Find (
				query => query.Where (
				a => a.Type == ArticleType.Page && a.Status == ArticleStatus.Open
			)
                    .OrderByDescending (a => a.CreateDate)
                    .Take (count)
			);
			HttpRuntime.Cache.Insert ("Article_LastPage", LastPage, new SqlCacheDependency ("Default", "Article"), DateTime.MaxValue, TimeSpan.FromMinutes (1));
			return LastPage;
		}

		public Article BrowseArticle (int articleId)
		{
			Article article = GetArticle (articleId);

			if (article.Status == ArticleStatus.Delete) {
				throw new DomainException ("NoFind", "对不起，该文章不存在或已经被删除。");
			} else if (_roleService.ReadArticelPower (article)) {
				article.Browse++;
				_articleRepository.Update (article);
				return article;
			} else {
				throw new DomainException ("NoPower", "对不起，您没有权限查看该文章。");
			}
		}

		public PageInfo<Article> FindArticlesByQuery (Func<IQueryable<Article>, IQueryable<Article>> expr, int pageIndex, int pageSize)
		{
			if (!_roleService.AddArticlePower ()) {
				throw new DomainException ("NoPower", "对不起，您没有权限！");
			}

			pageIndex = pageIndex > 0 ? pageIndex : 1;

			int count = (int)_articleRepository.Single (query => expr (query).Count ());

			PageInfo<Article> info = new PageInfo<Article> () {
				PageIndex = pageIndex,
				PageSize = pageSize,
				TotalItem = count
			};
			info.PageIndex = info.PageIndex > info.TotalPage ? info.TotalPage : info.PageIndex;

			IList<Article> list = _articleRepository.Find (query => expr (query).Skip ((pageIndex - 1) * pageSize).Take (pageSize));

			info.PageItems = list;
			return info;
		}

		public object GetArticleSingle (Func<IQueryable<Article>, object> expr)
		{
			return _articleRepository.Single (expr);
		}
	}
}
