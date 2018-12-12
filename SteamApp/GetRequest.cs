using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace SteamApp
{
    static class GetRequest
    {
        private static string url => "https://api.steampowered.com/IDOTA2Match_570/";
        public static string getMatchHistory(string key, long id)
        {
            string method = "GetMatchHistory/";
            string json = string.Empty;
            string uri = join(key, id, method);

            json = HttpRequest.Get(uri);

            return json;
        }

        private static string join(string key, long id, string method)
        {
            string uri = string.Empty;
            string options = "V001/" + "?key=" + key + "&account_id=" + id;
            uri = url + method + options;
            return uri;
        }
    }
}
