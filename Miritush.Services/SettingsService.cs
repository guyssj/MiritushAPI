using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Miritush.DAL.Model;
using Miritush.Services.Abstract;

namespace Miritush.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly booksDbContext dbContext;
        private readonly ConcurrentDictionary<string, string> settingValueCache = new ConcurrentDictionary<string, string>();

        public SettingsService(booksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Setting> Get(string settingName)
        {
            var setting = await dbContext.Settings.FindAsync(settingName);

            return setting;
        }

        public async Task<string> GetValue(string settingName)
        {
            if (settingValueCache.TryGetValue(settingName, out var cachedValue))
                return cachedValue;

            var setting = await dbContext.Settings.FindAsync(settingName);

            if (setting == null)
                return string.Empty;

            settingValueCache[settingName] = setting.SettingValue;
            return setting.SettingValue;
        }
    }
}