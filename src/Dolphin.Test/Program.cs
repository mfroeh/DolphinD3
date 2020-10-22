using System;
using WK.Libraries.HotkeyListenerNS;

namespace Dolphin.Test
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var x = new Hotkey();
            var y = new Hotkey();

            x = null;
            // System.Console.WriteLine(hotkey.Equals(null));
            System.Console.WriteLine(x == y);
            Console.ReadLine();
        }

    }

    public interface Interface { }

    public class InterfaceImplementationA : Interface { }

    public interface IGenric
    {
    }

    public class Generic<T> : IGenric where T : Interface
    {
    }
}