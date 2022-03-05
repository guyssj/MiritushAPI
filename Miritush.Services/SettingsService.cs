using System;
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
            var setting = await dbContext.Settings.FindAsync(settingName);

            return setting.SettingValue;
        }
    }
}