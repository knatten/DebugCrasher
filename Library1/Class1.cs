using System;
using Library2;

namespace Library1
{
    public class Class1
    {
        public void M()
        {
            Console.WriteLine("In Library 1");
            Action a = () =>
            {
                var c = new Class2();
            };
        }
    }
}