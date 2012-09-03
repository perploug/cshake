using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cshake.Core.Runners
{
    public class TargetRunner
    {
        public IList<ITarget> Visited { get; set; }
        public IContext Context { get; set; }

        public TargetRunner(IContext context)
        {
            Visited = new List<ITarget>();
            Context = context;
        }

        public void Run(ITarget root)
        {
            if (root.HasDependencies)
            {
                foreach (var dep in root.Dependencies)
                    if (!HasVisited(dep))
                        Run(dep);
            }

            Visited.Add(root);
            root.Run(Context);
        }

        public bool HasVisited(ITarget target){
            if (Visited != null && Visited.Contains(target))
                return true;
            else
                return false;
        }
    }
}
