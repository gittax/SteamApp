using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SteamApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string key = args[0];
            string matchHistoryJson;
            GetMatchHistory.Rootobject matchHistory = new GetMatchHistory.Rootobject();
            string id;
            bool alive = true;
            while (alive)
            {
                id = Console.ReadLine();

                matchHistoryJson = GetRequest.getMatchHistory(key, id);
                matchHistory = JsonConvert.DeserializeObject<GetMatchHistory.Rootobject>(matchHistoryJson);
                GetMatchHistory.Player[] MyPlayer = matchHistory.result.matches[0].players
                    .Select(player=>player)
                    .Where(player => player.account_id == 34326248)
                    .ToArray();
                Console.WriteLine(MyPlayer[0].account_id);

                Console.WriteLine("Чтобы продолжить, нажмите ENTER. Чтобы выйти, введите любое значение:");
                string end = Console.ReadLine();
                if (end.Length > 0)
                {
                    alive = false;
                }
            }

            Console.WriteLine("Работа программы окончена!");
        }
    }
}
