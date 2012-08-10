using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cshake.Core.Tests
{
    public class CourierBuild : Project
    {
        public CourierBuild()
        {
            ITarget zip = new Target("Runs the zipping of the assemblies");
            ITarget build = new Target("Runs the compiler actions");
            ITarget test = new Target("Runs tests");

            ITarget defaultTarget = new Target("This is my default target", () =>
            {
                Console.WriteLine("This is my default delegate");
            }
            ).Depends(build);

            build.Execute(() =>
                            {
                                Console.WriteLine("This is my build delegate");
                            })
                 .Depends(test);

            zip.Execute(() =>
                {
                    Console.WriteLine("This is my test delegate");
                }
            ).Depends(build, test);
        }
    }
}
