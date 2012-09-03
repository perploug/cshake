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
            try
            {
                var host = new CompiledHost(Environment.CurrentDirectory);
                host.BuildProject(args[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

              
        }
    }
}
