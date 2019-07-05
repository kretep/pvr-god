using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace PVR_God
{
    class ProcessManager
    {
        private Process mongodProcess;
        private Process restheartProcess;
        private Process serverProcess;

        private DispatcherTimer timer;

        public ProcessManager()
        {
            // See which processes are already running
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                if (process.ProcessName == "mongod")
                {
                    Console.WriteLine("Database already running");
                    mongodProcess = process;
                }
                if (process.ProcessName == "java")
                {
                    if (process.GetCommandLine().EndsWith("restheart.jar")) {
                        Console.WriteLine("API already running");
                        restheartProcess = process;
                    }
                }
                if (process.ProcessName == "pvr-server")
                {
                    Console.WriteLine("Server already running");
                    serverProcess = process;
                }
            }

            // Set up timer
            timer = new DispatcherTimer();
            timer.Tick += OnTimer;
            timer.Interval = new TimeSpan(0, 0, 1);
        }

        public void RunMongod()
        {
            if (mongodProcess == null)
            {
                Console.WriteLine("Starting database");
                mongodProcess = CreateProcess("mongod", "");
            }
        }

        public void RunRestheart()
        {
            if (restheartProcess == null)
            {
                Console.WriteLine("Starting API");
                restheartProcess = CreateProcess("java", "-server -jar lib/restheart.jar");
            }
        }

        public void RunServer()
        {
            if (serverProcess == null)
            {
                Console.WriteLine("Starting server");
                serverProcess = CreateProcess("lib\\pvr-server.exe", "");
            }
        }

        private Process CreateProcess(string fileName, string arguments)
        {
            Process process = new Process();
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            process.Start();
            return process;
        }

        public void EndMongod()
        {
            Console.WriteLine("Stopping database");
            EndProcess(mongodProcess);
            mongodProcess = null;
        }

        public void EndRestheart()
        {
            Console.WriteLine("Stopping API");
            EndProcess(restheartProcess);
            restheartProcess = null;
        }

        public void EndServer()
        {
            Console.WriteLine("Stopping server");
            EndProcess(serverProcess);
            serverProcess = null;
        }

        private void EndProcess(Process process)
        {
            if (!process.HasExited)
            {
                process.CloseMainWindow();
            }
            process.Close();
        }

        public void RestartDatabase()
        {
            EndMongod();
            RunMongod();
        }

        public void RestartAPI()
        {
            EndRestheart();
            RunRestheart();
        }

        public void RestartServer()
        {
            EndServer();
            RunServer();
        }

        public void RestartAll()
        {
            EndAll();
            RunAll();
        }

        public void EndAll()
        {
            EndMongod();
            EndRestheart();
            EndServer();
        }

        public void RunAll()
        {
            if (mongodProcess == null)
            {
                RunMongod();
                Thread.Sleep(3000);
            }
            if (restheartProcess == null)
            {
                RunRestheart();
            }
            if (serverProcess == null) {
                RunServer();
            }
            Thread.Sleep(1000);
            timer.Start();
        }

        public void OnTimer(Object source, EventArgs e)
        {
            RunNonRunning();
        }

        private void RunNonRunning()
        {
            // Check if the required processes are running
            bool seenMongod = false;
            bool seenRestheart = false;
            bool seenServer = false;
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                if (process.ProcessName == "mongod")
                {
                    seenMongod = true;
                }
                if (process.ProcessName == "java")
                {
                    if (process.GetCommandLine().EndsWith("restheart.jar"))
                    {
                        seenRestheart = true;
                    }
                }
                if (process.ProcessName == "pvr-server")
                {
                    seenServer = true;
                }
            }

            // If any process is not running, restart by 'RunAll'
            // (already running will not be run again).
            if (!seenMongod)
            {
                Console.WriteLine("Database stopped unexpectedly");
                mongodProcess = null;
            }
            if (!seenRestheart)
            {
                Console.WriteLine("API stopped unexpectedly");
                restheartProcess = null;
            }
            if (!seenServer)
            {
                Console.WriteLine("Server stopped unexpectedly");
                serverProcess = null;
            }
            if (!seenMongod || !seenRestheart || !seenServer)
            {
                timer.Stop();
                RunAll();
            }
        }
    }
}
