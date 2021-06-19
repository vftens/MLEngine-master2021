using System;

using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Gremlin.Net.Process.Traversal;
using OpenXmlPowerTools;
using System.Windows.Controls;

namespace OsEngine.Robots.IrPython
{
    class IronPython
    {
    }
    class Program
    {
        static void MyMain(string[] args)
        {
            ScriptEngine engine = Python.CreateEngine();
            engine.Execute("print 'hello, world'");     // Python 2
        }
    }
    class Script
    {
        static void MioMain(string args)
        {
            ScriptEngine engine = Python.CreateEngine();
            engine.ExecuteFile("D://hello.py");
            Console.Read();
        }
    }
    class Run_cmd //()
    {

        private string fileName = @"D:\test.py";

        Process p = new Process();

        /* // доделать 
        p.StartInfo = new ProcessStartInfo
            (@"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python36_64\python.exe", fileName)
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        p.Start();

        string output = p.StandardOutput.ReadToEnd();
        p.WaitForExit();

        Button.Text = output;
        */

    }


}
