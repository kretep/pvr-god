using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.ComponentModel;
using System.Diagnostics;

namespace PVR_God
{
    public static class Extensions
    {
        // Define an extension method for type System.Process that returns the command 
        // line via WMI.
        public static string GetCommandLine(this Process process)
        {
            try
            {
                string cmdLine = null;
                using (var searcher = new ManagementObjectSearcher(
                  $"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {process.Id}"))
                {
                    // By definition, the query returns at most 1 match, because the process 
                    // is looked up by ID (which is unique by definition).
                    using (var matchEnum = searcher.Get().GetEnumerator())
                    {
                        if (matchEnum.MoveNext()) // Move to the 1st item.
                        {
                            cmdLine = matchEnum.Current["CommandLine"]?.ToString();
                        }
                    }
                }
                if (cmdLine == null)
                {
                    // Not having found a command line implies 1 of 2 exceptions, which the
                    // WMI query masked:
                    // An "Access denied" exception due to lack of privileges.
                    // A "Cannot process request because the process (<pid>) has exited."
                    // exception due to the process having terminated.
                    // We provoke the same exception again simply by accessing process.MainModule.
                    var dummy = process.MainModule; // Provoke exception.
                }
                return cmdLine;
            }
            catch(Exception)
            {

            }
            return "exception";
        }
    }
}
