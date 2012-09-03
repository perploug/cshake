// -----------------------------------------------------------------------
// <copyright file="CompiledHost.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Cshake.Core.Hosts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using System.IO;
    using System.Reflection;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CompiledHost
    {
        public bool Initialized { get; set; }
        public DirectoryInfo BaseDir { get; set; }
        public DirectoryInfo PluginsDir { get; set; }
        public string TasksSearchPattern = "*.dll";

        public CompiledHost(string basedir)
        {
            BaseDir = new DirectoryInfo(basedir);
            PluginsDir = new DirectoryInfo(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
        }

        public void BuildProject(string projectName)
        {
            if (Compilers.MsBuild.CompileProject(BaseDir.ToString(), projectName))
            {
                //open mono host
                var name = Path.Combine(BaseDir.ToString() + @"\bin\release\", projectName + ".dll");
                Console.WriteLine("loading: " + name);

                Assembly a = Assembly.LoadFile(name);
                List<Type> n = new List<Type>(a.GetExportedTypes());


                foreach (var project in n.Where(x => x.GetInterface("IBuild`1") != null))
                {
                    Console.WriteLine("Instantiating: " + project.FullName);
                    var build = Activator.CreateInstance(project);
                    build.GetType().GetMethod("Run").Invoke(build, null);
                    
//                    Console.WriteLine(build.GetType().get);
                    //var ctx = build.CreateContext();
                    //build.Execute(ctx);
                }
            }
        }

        private static Predicate<Type> OnlyConcreteClasses(Type type, bool onlyConcreteClasses)
        {
            return t => (type is IBuild<IContext>); // .IsAssignableFrom(t) && (onlyConcreteClasses ? (t.IsClass && !t.IsAbstract) : true));
        }



    }
}
