using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Manager
{
    internal static class FileManager
    {
        public static string GetToken()
        {
            return File.ReadAllText("../../Files/token.txt");
        }

        public static ulong GetGuildId()
        {
            return ulong.Parse(File.ReadAllText("../../Files/server.txt"));
        }
    }
}
