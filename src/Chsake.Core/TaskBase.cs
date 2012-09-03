using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cshake.Core.Exceptions;

namespace Cshake.Core
{
    public abstract class Task : Cshake.Core.ITask
    {
        public Func<bool> IfFunc { get; private set; }
        public Func<bool> UnlessFunc { get; private set; }
        public bool ThrowExceptionOnError { get; set; }


        public bool Initialized { get; set; }


        public ITask Run(IContext context)
        {
            Console.WriteLine("Running task: " + this.GetType().Name);

            if (!Initialized)
                this.Init(context);


            if (IfFunc != null)
            {
                bool result = IfFunc.Invoke();
                if (!result)
                    return this;
            }
            if (UnlessFunc != null)
            {
                bool result = UnlessFunc.Invoke();
                if (result)
                    return this;
            }

            try
            {
                ExecuteTask(context);
            }
            catch (Exception ex)
            {
                context.Error(ex);

                if (ThrowExceptionOnError)
                    throw new TaskExecutionException();
            }
            
            return this;
        }

        public ITask If(Func<bool> func)
        {
            IfFunc = func;
            return this;
        }

        public ITask Unless(Func<bool> func)
        {
            UnlessFunc = func;
            return this;
        }

        public ITask FailOnError(bool fail = true)
        {
            this.ThrowExceptionOnError = fail;
            return this;
        }

        public abstract void Init(IContext cts);

        public abstract void ExecuteTask(IContext ctx);
        
    }
}
