using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cshake.Core
{
    public abstract class Task : Cshake.Core.ITask
    {
        public bool Initialized { get; set; }
        public Task Run(Context ctx)
        {
            if (!Initialized)
                this.Init(ctx);

            ExecuteTask(ctx);

            return this;
        }

        public abstract void Init(Context cts);

        public abstract void ExecuteTask(Context ctx);
        
    }
}
