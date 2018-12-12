using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SteamApp
{
    class Program
    {
        public static string key;
        public static string lang="us";
        static void Main(string[] args)
        {
            key = args[0];
            string matchHistoryJson;
            GetMatchHistory.Rootobject matchHistory = new GetMatchHistory.Rootobject();
            long id;
            bool alive = true;
            while (alive)
            {
                try
                {
                    Console.WriteLine();
                    Console.WriteLine("Введите id челика: ");
                    id = Convert.ToInt64(Console.ReadLine());

                    matchHistoryJson = GetRequest.getMatchHistory(key, id);
                    matchHistory = JsonConvert.DeserializeObject<GetMatchHistory.Rootobject>(matchHistoryJson);

                    if (matchHistory.result.status == 1)
                    {
                        List<GetMatchHistory.Player> MyPlayer = new List<GetMatchHistory.Player>();
                        for (int i = 0; i < matchHistory.result.matches.Length; i++)
                        {
                            MyPlayer.Add(matchHistory.result.matches[i].players
                                .Single(player => player.account_id == id));
                        }

                        Dictionary<string, int> heroPickList = getHeroPickList(MyPlayer);

                        var groupList = from pair in heroPickList
                            orderby pair.Value descending
                            select pair;

                        using (var file = new System.IO.StreamWriter(id + ".txt"))
                        {
                            var x = 0;
                            foreach (KeyValuePair<string, int> pair in groupList)
                            {
                                if (x < 10)
                                {
                                    Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
                                    x++;
                                }
                                file.WriteLine("{0}: {1}", pair.Key, pair.Value);
                            }
                        }

                        Console.WriteLine("Челик записан в файл {0}.txt", id);
                    }
                    else if (matchHistory.result.status == 15)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Профиль челика скрыт");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                /* Console.WriteLine("Чтобы продолжить, нажмите ENTER. Чтобы выйти, введите любое значение:");
                string end = Console.ReadLine();
                if (end.Length > 0)
                {
                    alive = false;
                }*/
            }

            Console.WriteLine("Работа программы окончена!");
            Console.ReadLine();
        }

        private static Dictionary<string, int> getHeroPickList(List<GetMatchHistory.Player> playerList)
        {
            var heroCount = new Dictionary<int, int>();
            var namedHeroCount = new Dictionary<string,int>();
            var j = 0;
            foreach (var player in playerList)
            {
                if (!heroCount.Keys.Contains(player.hero_id))
                {
                    heroCount.Add(player.hero_id, 1);
                    for (int i = j + 1; i < playerList.Count; i++)
                    {
                        if (player.hero_id == playerList[i].hero_id)
                        {
                            heroCount[player.hero_id]++;
                        }
                    }
                }
                j++;
            }

            namedHeroCount = idToName(heroCount);

            return namedHeroCount;
        }

        private static Dictionary<string, int> idToName(Dictionary<int, int> heroCount)
        {
            var namedHeroCount = new Dictionary<string, int>();

            HeroesList.Hero tempHero;
            string heroesListJson = HeroesList.Get.getHeroesList(key, lang);
            HeroesList.Rootobject heroesList = JsonConvert.DeserializeObject<HeroesList.Rootobject>(heroesListJson);

            foreach (var hero in heroCount)
            {
                tempHero = heroesList.result.heroes.Single(h => h.id == hero.Key);
                namedHeroCount.Add(tempHero.localized_name, hero.Value);
            }

            return namedHeroCount;
        }
    }
}
