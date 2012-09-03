using System;
namespace Cshake.Core
{
    public interface ITask
    {
        void ExecuteTask(IContext ctx);
        void Init(IContext ctx);
        bool Initialized { get; set; }
        ITask Run(IContext ctx);
    }
}
