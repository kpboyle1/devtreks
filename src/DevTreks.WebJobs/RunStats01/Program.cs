using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DevTreks.WebJobs.RunStats01
{
    /// <summary>
    ///Purpose:		Run webjobs using netcore apps
    ///Author:		www.devtreks.org
    ///Date:		2016, September
    ///References:	CTA examples 2 and 3
    ///</summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            string sScriptExecutable = string.Empty;
            if (args.Count() > 0)
            {
                sScriptExecutable = args[0];
            }
            string sScriptPath = string.Empty;
            if (args.Count() > 1)
            {
                sScriptPath = args[1];
            }
            string sInputPath = string.Empty;
            if (args.Count() > 2)
            {
                sInputPath = args[2];
            }
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = sScriptExecutable;
            start.RedirectStandardOutput = true;
            start.UseShellExecute = false;
            start.Arguments = string.Format("{0} {1}", sScriptPath, sInputPath);
            start.CreateNoWindow = true;
            string sLastLine = string.Empty;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    while (!reader.EndOfStream)
                    {
                        sLastLine = reader.ReadLine();
                        sb.AppendLine(sLastLine);
                    }

                    //sb.Append(reader.ReadToEnd());
                    //retain for writing to file
                    //using (StreamWriter sw = new StreamWriter(output3))
                    //{
                    //    sb.Append(reader.ReadToEnd());
                    //    sw.Write(sb.ToString());
                    //}
                }
                process.WaitForExit();
            }
        }
    }
}
