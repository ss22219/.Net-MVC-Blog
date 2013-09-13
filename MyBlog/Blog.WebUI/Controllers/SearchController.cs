using System.Collections.Generic;
using System.Web.Mvc;
using Blog.Domain.Interface;
using Blog.Model;
using Blog.Domain;
namespace Blog.WebUI.Controllers
{
    public class SearchController : Controller
    {
        private IArticleService _articleService;

        public SearchController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public ActionResult Index(string id, int page = 1)
        {
            page = page > 0 ? page : 1;
            ViewBag.Page = page;
            PageInfo<Article> list = _articleService.FindArticleByTitle(id, page);
            return View(list);
        }

    }
}
