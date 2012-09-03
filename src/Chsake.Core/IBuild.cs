using System;
namespace Cshake.Core
{
    public interface IBuild<T> where T : IContext
    {
        T CreateContext();
        void Execute(T context);
    }
}
