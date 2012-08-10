using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cshake.Core;
using System.Threading;
using System.Diagnostics;

namespace Cshake.Tasks
{
    public class Exec : Task
    {
        public List<string> Args { get; set; }
        public string Program { get; set; }
        public string WorkingDirectory { get; set; }

        private Context _ctx = null;

        public Exec(string program, string workingDirectory = "", params string[] args)
        {
            Args = new List<string>();
            Program = program;
            Args.AddRange(args);
            WorkingDirectory = Environment.CurrentDirectory;
            if (!string.IsNullOrEmpty(workingDirectory))
                WorkingDirectory = workingDirectory;

        }

        public override void ExecuteTask(Context ctx)
        {
            _ctx = ctx;

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.WorkingDirectory = WorkingDirectory;
            psi.FileName = Program;
            psi.Arguments = string.Join(" ", Args);
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.CreateNoWindow = true;

            Process p = new Process();
            p.StartInfo = psi;

            p.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);
            p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);

            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            p.WaitForExit();
        }

        void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        public override void Init(Context ctx)
        {
           
        }
    }
}
