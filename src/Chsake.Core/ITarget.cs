using System;
namespace Cshake.Core
{
    public interface ITarget
    {
        System.Collections.Generic.IEnumerable<ITarget> Dependencies { get; }
        Action ExecutionAction { get; }
        Func<bool> IfFunc { get; }
        Func<bool> UnlessFunc { get; }
        string Description { get; set; }
        bool HasDependencies { get; }

        ITarget DependsOn(params ITarget[] dependencies);
        ITarget Run(IContext context);
        ITarget If(Func<bool> action);
        ITarget Unless(Func<bool> action);
        ITarget Execute(Action action);

        
    }
}
