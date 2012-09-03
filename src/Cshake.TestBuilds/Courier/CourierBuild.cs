   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;
   using Cshake.Core;
   using Cshake.Core.Runners;
   using Cshake.Tasks;
   using Fluent.IO;
   using Cshake.TestBuilds.Courier;

namespace Cshake.TestBuilds.Courier
{
    public class CourierBuild : IBuild<CourierBuildModel>
    {
        public CourierBuildModel CreateContext()
        {
            CourierBuildModel b = new CourierBuildModel();

            b.RootDir = b.CurrentDirectory;
            b.WinDir = @"c:\WINDOWS";
            b.MsBuildApp = b.WinDir + @"\microsoft.net\framework\v4.0.30319\msbuild.exe";

            b.PackageDir = b.RootDir + @"\package";
            b.ZipDir = b.RootDir + @"\zip";
            b.PluginDir = b.PackageDir + @"\umbraco\plugins\courier";
            

            //solution
            b.SolutionDir = @"C:\Users\Hartvig\Documents\Source Code\Umbraco Pro\Courier 2";
            b.Solution = @"umbraco.courier.sln";
            b.AssetsDir = b.SolutionDir + @"\assets";

            //projects
            b.DllsDir = b.SolutionDir + @"\contrib\3rd party assemblies";
            b.CoreDir = b.SolutionDir + @"\Core\Umbraco.Courier.Core";
            b.UIDir = b.SolutionDir + @"\Core\Umbraco.Courier.UI";
            b.ProvidersDir = b.SolutionDir + @"\Core\Umbraco.Courier.Providers";
            b.NhibernateDir = b.SolutionDir + @"\Core\Umbraco.Courier.Persistence.NHibernate";
            b.SvnDir = b.SolutionDir + @"\Contrib\Samples\Umbraco.Courier.SubversionRepository";
            b.ContribDir = b.SolutionDir + @"\Contrib\Providers";

            //courier app settings 
            b.Version = "2.7.2";
            b.Name = "Courier";

            b.ConfigFile = b.CoreDir + @"\configuration files\Courier.config";
            
            return b;
        }

        public void Execute(CourierBuildModel context)
        {
            //Define targets
            ITarget package = new Target("Packages a new version of courier");
            ITarget build = new Target("Builds some shit");
            ITarget test = new Target("Runs tests");
            ITarget handleFiles = new Target("Moves files around");
            ITarget zip = new Target("Zips the files");
            
            build.Execute(() =>
            {   
                new Exec(
                        program: context.MsBuildApp,
                        workingDirectory: context.SolutionDir,
                        args: new string[]{ 
                                        context.Solution,
                                        "/v:n",
                                        "/p:WarningLevel=0",
                                        "/p:ToolsVersion=4.0",
                                        "/p:Configuration=Release"})
                        .Run(context);
            });

            //will package the entire courier application and send it to a public download directory
            package.Execute(() =>
            {
                context.Success("Hey we are done");
            }
            ).DependsOn(zip);


            zip.DependsOn(handleFiles);


            handleFiles.Execute(() =>
            {
                //.aspx pages to /pages
                Path.Get(context.UIDir + @"\pages")
                            .Files("*.aspx", false)
                            .Copy(Path.Get(context.PackageDir + @"\umbraco\plugins\courier\pages").CreateDirectory());

                context.Success("Moved pages");


                //.master to /masterpages
                Path.Get(context.UIDir + @"\masterpages")
                    .Files("*.master", false)
                    .Copy(Path.Get(context.PackageDir + @"\umbraco\plugins\courier\masterpages").CreateDirectory());

                context.Success("Moved masterpages");

                //.aspx pages to /dialogs
                Path.Get(context.UIDir + @"\dialogs")
                            .Files("*.aspx", false)
                            .Copy(Path.Get(context.PackageDir + @"\umbraco\plugins\courier\dialogs").CreateDirectory());
                
                context.Success("Moved dialogs");

                //images
                Path.Get(context.UIDir + @"\images")
                            .Files("*.*", false)
                            .Copy(Path.Get(context.PackageDir + @"\umbraco\plugins\courier\images").CreateDirectory());

                context.Success("Moved images");

                Path.Get(context.UIDir + @"\scripts")
                            .Files("*.js", false)
                            .Copy(Path.Get(context.PackageDir + @"\umbraco\plugins\courier\scripts").CreateDirectory());

                Path.Get(context.UIDir + @"\css")
                            .Copy(Path.Get(context.PackageDir + @"\umbraco\plugins\courier\css").CreateDirectory());

                //usercontrols
                Path.Get(context.UIDir + @"\usercontrols")
                            .Files("*.ascx", false)
                            .Copy(Path.Get(context.PackageDir + @"\umbraco\plugins\courier\usercontrols").CreateDirectory());

                context.Success("Moved usercontrols");


                //dashboards
                Path.Get(context.UIDir + @"\dashboard")
                            .Files("*.ascx", false)
                            .Copy(Path.Get(context.PackageDir + @"\umbraco\plugins\courier\dashboard").CreateDirectory());

                context.Success("Moved dashboards");


                Path.Get(context.UIDir + @"\images")
                    .Files("courier.jpg", false)
                    .Copy(Path.Get(context.PackageDir + @"\umbraco\images\tray").CreateDirectory());

                context.Success("Moved tray icon");

                
                //webservices
                Path.Get(context.ContribDir)
                    .AllFiles()
                    .WhereExtensionIs("ashx", "asmx")
                    .Copy( Path.Get(context.PackageDir + @"\umbraco\plugins\courier\webservices").CreateDirectory());


                context.Success("Moved webservices");


                //nhibernate dlls
                Path.Get(context.NhibernateDir + @"\packages")
                    .AllFiles()
                    .WhereExtensionIs("dll", "pdb", "xml")
                    .Copy(Path.Get(context.PackageDir + @"\bin").CreateDirectory());

                //courier dlls
                Path.Get(context.CoreDir, context.ContribDir)
                    .Files("umbraco.courier.*", true)
                    .WhereExtensionIs("dll","pdb","xml")
                    .Copy(Path.Get(context.PackageDir + @"\bin").CreateDirectory(), Overwrite.IfNewer);
                

                context.Success("Moved dlls and pdbs");

                //package file
                Path.Get(context.AssetsDir + @"\package.xml")
                    .Copy(
                            Path.Get(context.PackageDir)
                                .Files("package.xml", false)
                                .XmlPoke("/umbPackage/info/package/version", context.Version)
                                .XmlPoke("/umbPackage/files/file [orgName = 'Umbraco.Courier.Core.dll']/orgPath", "bin")
                            );


                //configuration file
                Path.Get(context.ConfigFile)
                    .Copy(
                            Path.Get(context.PackageDir + @"\config").CreateDirectory());
            
            }
            ).DependsOn(build, test);

            TargetRunner tr = new TargetRunner(context);
            tr.Run(package);
        }


        public void Run()
        {
            var ctx = CreateContext();
            Execute(ctx);
        }
    }

}