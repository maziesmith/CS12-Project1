using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS12_Project_1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            //PersonsDatabase debugpd = new PersonsDatabase(debugpath,null);
            string debugpath = @"Z:\hmnt\CS12 Project 1";
            PersonsDatabase debugpd = new PersonsDatabase(debugpath,null);
            LoginSystem ls = new LoginSystem(debugpd);
            HeadEssay headEssay = new HeadEssay(debugpd,ls);
            //Application.Run();
            headEssay.ShowLoginForm();
        }
        public static void Exit(ISystem.ExitStatus t_exitStatus)
        {
            switch(t_exitStatus)
            {
                case ISystem.ExitStatus.userClosed:
                    Application.Exit();
                    return;
                case ISystem.ExitStatus.errors:
                    Application.Exit();
                    return;
                case ISystem.ExitStatus.noError:
                    Application.Exit();
                    return;
            }
        }
    }
}
