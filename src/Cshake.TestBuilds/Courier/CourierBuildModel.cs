// -----------------------------------------------------------------------
// <copyright file="CourierBuildModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Cshake.Core;
    using System.Dynamic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>

namespace Cshake.TestBuilds.Courier
{
    public class CourierBuildModel : Context, Cshake.TestBuilds.Courier.ICourierBuildModel
    {
        public string RootDir { get; set; }
        public string WinDir { get; set; }

        public string MsBuildApp { get; set; }
        public string SmartAssemblyApp { get; set; }
        public string addFilesToPackageApp { get; set; }

        public string PackageDir { get; set; }
        public string ZipDir { get; set; }
        public string PluginDir { get; set; }

        public string SolutionDir { get; set; }
        public string Solution { get; set; }

        public string AssetsDir { get; set; }
        public string DllsDir { get; set; }
        public string CoreDir { get; set; }
        public string UIDir { get; set; }
        public string ProvidersDir { get; set; }
        public string NhibernateDir { get; set; }
        public string SvnDir { get; set; }
        public string ContribDir { get; set; }

        public string Version { get; set; }
        public string Name { get; set; }

        public string ConfigFile { get; set; }
    }
}