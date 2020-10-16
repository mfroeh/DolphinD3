using Newtonsoft.Json;
using System;
using System.IO;

namespace Dolphin.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var property = typeof(Condition).GetProperty("Punishment", typeof(Func<Player, World, bool>));

        }        
    }
}