using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blog.Domain.Interface;
using Blog.Repository.Interface;
using Blog.Model;
using System.Web;
using System.Web.Caching;

namespace Blog.Domain
{
    /// <summary>
    /// 分类业务类
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository _categoryRepository;
        private IRoleService _roleService;

        /// <summary>
        /// 唉，这个缓存应该让工厂生成的
        /// </summary>
        private IList<Category> _categorys
        {
            get;
            set;
        }

        public CategoryService(ICategoryRepository categoryRepository, IRoleService roleService)
        {
            _categoryRepository = categoryRepository;
            _roleService = roleService;
        }

        public IList<Model.Category> GetAllCategory()
        {
            if (_categorys == null)
            {
                _categorys = _categoryRepository.GetAll();
            }
            return _categorys;
        }


        public IDictionary<string, string> GetMonthCategory()
        {
            IList<string> months = _categoryRepository.GetMonthCategory();
            IDictionary<string, string> monthCategory = new Dictionary<string, string>();
            foreach (string month in months)
            {
                monthCategory.Add(month, month.Insert(4, " 年") + "月");
            }
            return monthCategory;
        }


        public Model.Category GetCategoryById(int categoryId)
        {
            if (_categorys == null)
            {
                _categorys = _categoryRepository.GetAll();
            }
            if (_categorys.Where(c => c.CategoryId == categoryId).Count() > 0)
            {
                return _categorys.Where(c => c.CategoryId == categoryId).First();
            }
            else
            {
                return null;
            }
        }

        public Model.Category FindTagByName(string name)
        {
            IList<Category> categorys =  this.GetAllCategory().Where(c => c.Name == name).ToList();

            if (categorys.Count > 0)
            {
                return categorys.First();
            }
            else
            {
                return null;
            }
        }


        public int AddCategory(Category category)
        {
            if (!_roleService.AdminPower())
            {
                throw new DomainException("NoPower", "对不起，您没有权限！");
            }
            if (string.IsNullOrEmpty(category.Name))
            {
                throw new DomainException("Name", "分类名称不能为空！");
            }
            if (category.Parent != null && GetCategoryById(category.Parent.CategoryId) == null)
            {
                throw new DomainException("NoCategory", "没有找到父分类！");
            }
            if ((int)_categoryRepository.Single(query => query.Where(c => (c.Name == category.Name) && (c.Parent == category.Parent) && (c.Type == category.Type)).Count()) > 0)
            {
                throw new DomainException("NoCategory", "发现同名分类！");
            }
            int lint = _categoryRepository.Add(category);
            _categoryRepository.Save();
            _categorys = null;
            return lint;
        }

        public void UpdateCategory(Category category)
        {
            if (!_roleService.AdminPower())
            {
                throw new DomainException("NoPower", "对不起，您没有权限！");
            }
            if (GetCategoryById(category.CategoryId) == null)
            {
                throw new DomainException("NoFind", "没有找到该分类！");
            }
            _categorys = null;
            _categoryRepository.Update(category);
        }


        public IList<Category> GetLastTag()
        {
            return _categoryRepository.Find(query => query.Where(c => c.Type == CategoryType.Tag).OrderByDescending(c => c.Count).Take(15));
        }


        public void DeleteCaetegory(int id)
        {
            if (_roleService.AdminPower())
            {
                _categoryRepository.Delete(id);
                _categorys = null;
            }
            else
            {
                throw new DomainException("NoPower", "对不起，您没有权限！");
            }
        }
    }
}
