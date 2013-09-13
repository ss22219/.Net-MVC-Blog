using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blog.Repository.Interface;
using Blog.Domain.Interface;
using Blog.Model;
using System.Runtime.Caching;
using System.Web.Caching;
using System.Web;

namespace Blog.Domain
{
    public class SettingService : ISettingService
    {
        private ISettingRepository _settingRepository;
        /// <summary>
        /// 这个应该缓存到缓存模块中
        /// </summary>

        private IDictionary<string, string> _settings;

        public SettingService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public IDictionary<string, string> GetAllSetting()
        {
            if (_settings == null || _settings.Count <= 1)
            {
                _settings = new Dictionary<string, string>();
                IList<Setting> list = _settingRepository.GetAll();
                foreach (var setting in list)
                {
                    if (!string.IsNullOrEmpty(setting.SettingKey))
                        _settings[setting.SettingKey] = setting.SettingValue;
                }
            }
            return _settings;
        }


        public string GetSetting(string name)
        {
            if (_settings == null)
            {
                _settings = new Dictionary<string, string>();
                IList<Setting> list = _settingRepository.GetAll();
                foreach (var setting in list)
                {
                    _settings[setting.SettingKey] = setting.SettingValue;
                }
            }
            if (_settings.ContainsKey(name))
            {
                return _settings[name];
            }
            else
            {
                return string.Empty;
            }
        }

        public void SetSetting(string key, string value)
        {
            IList<Setting> settings = _settingRepository.Find(query => query.Where(s => s.SettingKey == key));
            foreach (var setting in settings)
            {
                setting.SettingValue = value;
            }
            _settings[key] = value;
        }
    }
}
