using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miritush.Services.Helpers
{
    public class MessageHelper
    {
        public static string ReplacePlaceHolder(
            string messageTemplate,
            Dictionary<string, string> placeHolders)
        {
            foreach (var place in placeHolders)
            {
                messageTemplate = messageTemplate.Replace(place.Key, place.Value);
            }
            return messageTemplate;
        }
    }
}