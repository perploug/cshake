using System;
namespace Cshake.Core
{
    interface ITask
    {
        void ExecuteTask(Context ctx);
        void Init(Context ctx);
        bool Initialized { get; set; }
        Task Run(Context ctx);
    }
}
