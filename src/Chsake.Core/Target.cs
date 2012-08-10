using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cshake.Core
{
    public class Target : ITarget
    {
        public Action ExecutionAction { get; private set; }
        public Func<bool> IfFunc { get; private set; }
        public Func<bool> UnlessFunc { get; private set; }
        public string Description { get; set; }

        private IEnumerable<ITarget> _dependencies = null;

        public Target(string description){
            Description = description;
        }
        public Target(string description, Action action)
        {
            ExecutionAction = action;
            Description = description;
        }

        public Target(Action action)
        {
            ExecutionAction = action;
        }

        public ITarget Depends(params ITarget[] dependencies)
        {
            _dependencies = dependencies.Cast<Target>();
            return this;
        }

        public ITarget If(Func<bool> func)
        {
            IfFunc = func;
            return this;
        }

        public ITarget Unless(Func<bool> func)
        {
            UnlessFunc = func;
            return this;
        }

        public ITarget Execute(Action action)
        {
            ExecutionAction = action;
            return this;
        }

        public ITarget Run(Context context)
        {
            Console.WriteLine("Trying to run: " + this.GetType().Name);
            Console.WriteLine(this.Description);

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

            if (ExecutionAction != null)
                ExecutionAction.DynamicInvoke(null);


            return this;
        }
               

        public IEnumerable<ITarget> Dependencies
        {
            get
            {
                return _dependencies;
            }
        }

        public bool HasDependencies
        {
            get
            {
                if (_dependencies == null || !_dependencies.Any())
                    return false;

                return true;
            }
        }
       


    }
}
