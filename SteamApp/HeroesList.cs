using System;
using System.Collections.Generic;
using System.Text;

namespace SteamApp
{
    class HeroesList
    {
        public class Rootobject
        {
            public Result result { get; set; }
        }

        public class Result
        {
            public Hero[] heroes { get; set; }
            public int status { get; set; }
            public string statusDetail { get; set; }
            public int count { get; set; }
        }

        public class Hero
        {
            public string name { get; set; }
            public int id { get; set; }
            public string localized_name { get; set; }
        }

        public static class Get
        {
            private static string url => "https://api.steampowered.com/IEconDOTA2_570/";
            public static string getHeroesList(string key, string lang)
            {
                string method = "GetHeroes/";
                string json = string.Empty;
                string uri = join(key, lang, method);

                json = HttpRequest.Get(uri);

                return json;
            }

            private static string join(string key, string lang, string method)
            {
                string uri = string.Empty;
                string options = "v1/" + "?key=" + key + "&language=" + lang;
                uri = url + method + options;
                return uri;
            }
        }
    }
}
