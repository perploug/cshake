// -----------------------------------------------------------------------
// <copyright file="MsBuild.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Cshake.Core.Compilers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MsBuild
    {
        string app = "";

        public static bool CompileProject(string directory, string file)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.WorkingDirectory = directory;
            psi.FileName = @"c:\windows\microsoft.net\framework\v4.0.30319\msbuild.exe";
            psi.Arguments = string.Join(" ", new string[]{ 
                                                file + ".csproj",
                                                "/v:n",
                                                "/p:WarningLevel=0",
                                                "/p:ToolsVersion=4.0",
                                                "/p:Configuration=Release"});

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

            return true;
        }

        static void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        static void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}
