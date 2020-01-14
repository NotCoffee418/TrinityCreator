using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrinityCreator.Helpers
{
    class GHelper
    {
        private static ConcurrentBag<Thread> _threadBag = new ConcurrentBag<Thread>();
        public static void SafeThreadStart(ThreadStart method)
        {
            // Create the thread & start it
            Thread thread = new Thread(method);
            thread.Start();

            // Add to the list (so garbagecollection doesn't eat them)
            _threadBag.Add(thread);
        }

        public static List<T> EnumToList<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
