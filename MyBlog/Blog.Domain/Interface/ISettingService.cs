using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blog.Model;

namespace Blog.Domain.Interface
{
    /// <summary>
    /// 配置管理
    /// </summary>
    public interface ISettingService
    {
        /// <summary>
        /// 取得全部配置
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> GetAllSetting();

        /// <summary>
        /// 根据配置名取得配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetSetting(string name);

        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetSetting(string key, string value);
    }
}
