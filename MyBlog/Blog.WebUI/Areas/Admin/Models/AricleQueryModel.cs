using Blog.Domain;
using Blog.Model;
using Blog.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog.WebUI.Areas.Admin.Models
{
    public enum ArticleOrder
    {
        Title,
        CommentCount,
        CreateDate
    }

    public enum OrderType
    {
        Asc,
        Desc
    }

    /// <summary>
    /// 文章查询模型
    /// </summary>
    public class ArticleQueryModel
    {
        /// <summary>
        /// 操作
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int? Category { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 按月份分档
        /// </summary>
        public string MonthCategory { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public OrderType? OrderType { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public ArticleOrder? OrderBy { get; set; }

        /// <summary>
        /// 按标题查找
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 文章ID列表
        /// </summary>
        public int[] ArticleIds { get; set; }


        /// <summary>
        /// 摘要视图
        /// </summary>
        public bool ExcerptMode { get; set; }

        /// <summary>
        /// 当前页数
        /// </summary>
        public int Page
        {
            get
            {
                return _page;
            }
            set
            {
                if (value > 0)
                {
                    _page = value;
                }
            }
        }

        private int _page = 1;
        /// <summary>
        /// 每页显示数目
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 查询结果
        /// </summary>
        public PageInfo<Article> PageInfo { get; set; }

        /// <summary>
        /// 全部文章数量
        /// </summary>
        public int AllCount { get; set; }

        /// <summary>
        /// 已发布数量
        /// </summary>
        public int OpenCount { get; set; }

        /// <summary>
        /// 回收站数量
        /// </summary>
        public int DeleteCount { get; set; }

        /// <summary>
        /// 审核评论的数量 key ArticelId,value VerifyCommentCount
        /// </summary>
        public IDictionary<int, int> VerifyComment { get; set; }

        /// <summary>
        /// 月分档列表
        /// </summary>
        public IDictionary<string, string> MonthCategorys { get; set; }

        /// <summary>
        /// 所有分类
        /// </summary>
        public IList<Category> Categorys { get; set; }

        public ArticleRequestModel BuildRequest(int page = 0)
        {
            page = page <= 0 ? this.Page : page;
            page = page > PageInfo.TotalPage ? PageInfo.TotalPage : page;
            return new ArticleRequestModel()
            {
                Category = this.Category,
                PageSize = this.PageSize,
                ExcerptMode = this.ExcerptMode,
                MonthCategory = this.MonthCategory,
                OrderBy = this.OrderBy,
                OrderType = this.OrderType,
                Page = page,
                Tag = this.Tag,
                Status = this.Status,
                Title = this.Title
            };
        }
    }
}