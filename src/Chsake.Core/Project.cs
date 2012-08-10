using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cshake.Core
{
    public class Project
    {
        public void Test()
        {
            Console.WriteLine("WONKERS");
        }

        public ITarget defaultTarget = null;

        public void run()
        {
            if (defaultTarget == null)
                Console.WriteLine("No default Target found");
            else
            {
                Runners.TargetRunner r = new Runners.TargetRunner();
                r.Run(defaultTarget);
            }
        }
    }
}
