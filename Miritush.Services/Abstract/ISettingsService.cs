using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Miritush.DAL.Model;

namespace Miritush.Services.Abstract
{
    public interface ISettingsService
    {
        Task<Setting> Get(string settingName);
        Task<string> GetValue(string settingName);
    }
}