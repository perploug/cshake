using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.CSharp;
using System.Reflection;
using System.IO;

namespace Cshake.Core.Hosts
{
    public class MonoHost
    {
        public bool Initialized { get; set; }
        public DirectoryInfo BaseDir { get; set; }
        public DirectoryInfo PluginsDir { get; set; }
        public string TasksSearchPattern = "*.Tasks.dll";

        public MonoHost(string basedir)
        {
            BaseDir = new DirectoryInfo(basedir);
            PluginsDir = new DirectoryInfo(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
        }

        public void Load(string path)
        {
            if(!Initialized)
                Init();

            var buildscript = File.ReadAllText(path);
            Evaluator.Run(buildscript);
        }


        public void Run(string statement)
        {
            Evaluator.Run(statement);        
        }


        public void Init()
        {
            Evaluator.Init(new string[0]);
            Evaluator.ReferenceAssembly(typeof(MonoHost).Assembly);

            foreach (var assembly in PluginsDir.GetFiles(TasksSearchPattern))
            {
                if (assembly.Name.Contains(".Tasks."))
                {
                    Evaluator.ReferenceAssembly(Assembly.LoadFile(assembly.FullName));
                }
                Console.WriteLine("loaded " + assembly.Name);
            }

            Evaluator.Run(@"  
            using System;
            using System.Collections.Generic;
            using System.Text;
            using Cshake.Core;
            using Cshake.Core.Runners;");

            Initialized = true;
        }
    }
}
