using System;
using Cshake.Core;
namespace Cshake.TestBuilds.Courier
{
    public interface ICourierBuildModel : IContext
    {
        string ContribDir { get; set; }
        string CoreDir { get; set; }
        string DllsDir { get; set; }
        string MsBuildApp { get; set; }
        string Name { get; set; }
        string NhibernateDir { get; set; }
        string PackageDir { get; set; }
        string PluginDir { get; set; }
        string ProvidersDir { get; set; }
        string RootDir { get; set; }
        string Solution { get; set; }
        string SolutionDir { get; set; }
        string SvnDir { get; set; }
        string UIDir { get; set; }
        string Version { get; set; }
        string WinDir { get; set; }
        string ZipDir { get; set; }
    }
}
