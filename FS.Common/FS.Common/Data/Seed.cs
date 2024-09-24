using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FS.Common.Data
{
    public static class Seed
    {
        private static Random random = new Random();
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        
        public static string RandomString(int length)
        {
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static int RandomInt(int max)
        {
            return random.Next(0, max);
        }
        public static int RandomInt(int min, int max)
        {
            return random.Next(min, max);
        }

    }
}
