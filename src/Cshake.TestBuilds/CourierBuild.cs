using System.Linq;

using System;
using System.Collections.Generic;
using System.Text;
using Cshake.Core;
using Cshake.Core.Runners;
using Cshake.Tasks;
using Cshake.Courier.Tasks;


namespace Cshake.TestBuilds
{
    public class Class1
    {

        public Class1()
        {
        //get the context
        Context ctx = new Context();

        //variables, dirs and files
        string rootdir = ctx.CurrentDirectory;
        string windir = @"c:\WINDOWS";
        string msbuildapp = windir + @"\microsoft.net\framework\v4.0.30319\msbuild.exe";
        
        string solutionDir = @"C:\Users\per\Documents\Projects\umbraco PRO\Courier 2";
        string solution = solutionDir + @"\umbraco.courier.sln";
                       
        //Define targets
        ITarget package = new Target("Packages a site with courier");
        ITarget build = new Target("Runs the compiler actions");
        ITarget test = new Target("Runs tests");

        //trigger the build to run
        ITarget defaultTarget = new Target("This is my default target", () =>
        {
            Console.WriteLine("This is my default delegate");
        }
        ).Depends(build);

        build.Execute(() =>
                        {
                            new Exec(
                                    program: msbuildapp,
                                    workingDirectory: solutionDir,
                                    args: new string[]{ 
                                                solution,
                                                "/v:n",
                                                "/p:WarningLevel=0",
                                                "/p:ToolsVersion=4.0",
                                                "/p:Configuration=Release"})
                                    .Run(ctx);

                                    ctx.Success("Build is done");
                        })
                .Depends(test)
                .If(() =>
                {
                    //perform some logic
                    return true;
                });


        //will package our entire site with Courier
        package.Execute(() =>
            {
                new Package(
                        config: "courier.config",
                        manifest: "manifest.xml",
                        source: "clean",
                        revision: "CompleteSite"
                        ).Run(ctx);
            }
        ).Depends(test);

        test.Execute(() =>
        {
            //You shall not pass!!
            ctx.Error("Sorry, I'm failing, so you cant build or package");
            throw new Exception("fail");
        }
        ).Depends(test);   

        TargetRunner tr = new TargetRunner();
        tr.Run(defaultTarget);


        }
    }
}
