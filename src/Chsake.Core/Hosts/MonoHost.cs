using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.CSharp;
using System.Reflection;
using System.IO;
using System.Dynamic;

namespace Cshake.Core.Hosts
{
    public class MonoHost
    {
        public bool Initialized { get; set; }
        public DirectoryInfo BaseDir { get; set; }
        public DirectoryInfo PluginsDir { get; set; }
        public string TasksSearchPattern = "*.dll";

        public MonoHost(string basedir)
        {
            BaseDir = new DirectoryInfo(basedir);
            PluginsDir = new DirectoryInfo(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
        }

        public void BuildProject(string projectName)
        {
            if(!Initialized)
                Init();

            var name = Path.Combine(BaseDir.ToString() , projectName + ".dll");
            Assembly a = Assembly.LoadFile(name);
            List<Type> n = new List<Type>(a.GetExportedTypes());

            foreach (var project in n.FindAll(OnlyConcreteClasses(typeof(IBuild<IContext>), true)))
            {
                IBuild<IContext> build = Activator.CreateInstance(project) as IBuild<IContext>;
                var ctx = build.CreateContext();
                build.Execute(ctx);
            }
        }

        private static Predicate<Type> OnlyConcreteClasses(Type type, bool onlyConcreteClasses)
        {
            return t => (type.IsAssignableFrom(t) && (onlyConcreteClasses ? (t.IsClass && !t.IsAbstract) : true));
        }


        public void BuildProject2(string projectName)
        {
            string name = projectName + "Build";
            string buildFile = name + ".cs";
            string modelFile = name + "Model.cs";

            string modelCreation = "var model = " + name + ".CreateContent();";
            string execution = name + ".Execute(model);";

            if (!Initialized)
                Init();

            if (File.Exists(modelFile))
            {
                var modelDefinition = File.ReadAllText(modelFile);
                var buildDefinition = File.ReadAllText(buildFile);

                var one1 = Evaluator.Compile(modelDefinition);
                var one2 = Evaluator.Compile(buildDefinition);

                Console.WriteLine(one1);
                Console.WriteLine(one2);

                Evaluator.Run(modelCreation);
                Evaluator.Run(execution);
            }
            else if (File.Exists(projectName))
            {
                var buildDefinition = File.ReadAllText(projectName);
                Evaluator.Run(buildDefinition);
            }
            else
            {
                Evaluator.Run(projectName);
            }
        }

        public void Run(string statement)
        {
            Evaluator.Run(statement);        
        }

        public void Init()
        {
            Evaluator.Init(new string[0]);
            foreach (var assembly in PluginsDir.GetFiles(TasksSearchPattern))
            {
                if (!assembly.Name.Contains("Mono.CSharp"))
                {
                    Console.WriteLine("loading " + assembly.Name);
                    Evaluator.ReferenceAssembly(Assembly.LoadFile(assembly.FullName));
                    Console.Write(" ...DONE ");
                } 
            }
            

            Evaluator.Run(@"  
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using Cshake.Core;
            using Cshake.Core.Runners;
            using Cshake.Tasks;
            using Fluent.IO;
            using System.Dynamic;    
            ");

            Console.Write("...Usings done");

            Initialized = true;
        }
    }
}
