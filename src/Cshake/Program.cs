using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cshake.Core.Hosts;
using Cshake.Core.Extensions;
using System.IO;

namespace Cshake
{
    class Program
    {
        static void Main(string[] args)
        {
            MonoHost host = new MonoHost(Environment.CurrentDirectory);
            
            try
            {
                host.Init();
                var usings = Path.Combine(Environment.CurrentDirectory + "\\usings.cshake");
                var build = Path.Combine(Environment.CurrentDirectory + "\\build.cshake");

                host.Load(usings);
                host.Load(build);
            }
            catch (Exception ex)
            {
                Console.WriteLine("INIT: " + ex.ToString());
            }

            while (true)
            {
                Console.Write("->");
                string input = Console.ReadLine();
                if (input.Equals("@@")) return;
                try { host.Run(input); }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }  
        }
    }
}
