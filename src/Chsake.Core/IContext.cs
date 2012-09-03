using System;
namespace Cshake.Core
{
    public interface IContext
    {
        string CurrentDirectory { get; set; }
        void Error(object o);
        void Important(object o);
        void Output(object o);
        void Success(object o);
        dynamic Settings { get; set; }
    }
}
