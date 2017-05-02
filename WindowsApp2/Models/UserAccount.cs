using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSharp;


namespace WindowsApp2.Models
{
    static class UserAccount
    {
        public static string LoggedInUsername;
        public static bool LoggedIn = false;
        public static string Summoner = "No Summoner";
        private static RiotApi api = Api.GetApi();
        public static void LogIn(string username)
        {
            LoggedInUsername = username;
            LoggedIn = true;
        }

        public static void LogOut()
        {
            LoggedInUsername = "";
            LoggedIn = false;
        }
        public static void SetSummoner(string summ)
        {
            Summoner = summ;
        }
        public static string GetSummoner()
        {
            return Summoner;
        }
    }
}
