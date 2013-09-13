using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    /// <summary>
    /// 领域异常类，凡是在领域验证中出现错误，将会抛出此异常！需要catch处理
    /// </summary>
    public class DomainException : Exception
    {
        //存放错误信息的List
        public Dictionary<string, string> Errors = new Dictionary<string, string>();

        //判断是否有错误
        public bool IsValid
        {
            get
            {
                return Errors.Count == 0 ? true : false;
            }
        }

        //增加错误信息
        public void AddError(string name, string message)
        {
            this.Errors.Add(name, message);
        }

        public DomainException()
        {
        }

        public DomainException(string name, string message):base(message)
        {
            AddError(name, message);
        }
    }
}
