using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace Cshake.Core
{
    public class Context : Cshake.Core.IContext
    {
        public string CurrentDirectory { get; set; }

        public Context()
        {
            CurrentDirectory = Environment.CurrentDirectory;    
        }
        
        public void Output(object o)
        {
            Console.WriteLine(o);
        }

        public void Error(object o)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(o);
            Console.ResetColor();
        }

        public void Success(object o)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(o);
            Console.ResetColor();
        }

        public void Important(object o)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(o);
            Console.ResetColor();
        }

        public dynamic Settings{get;set;}
    }
}
