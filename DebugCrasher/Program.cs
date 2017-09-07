using System;
using System.Collections.Generic;
using Library1;

namespace DebugCrasher
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("In Main");
            new Class1().M();
        }
    }
}