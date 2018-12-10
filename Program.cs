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
            string debugpath = @"Z:\hmnt\CS12 Project 1\";
            PersonsDatabase debugpd = new PersonsDatabase(debugpath,null);
            LoginSystem ls = new LoginSystem(debugpd);
            HeadEssay headEssay = new HeadEssay(debugpd,ls);



            Person baz = debugpd.BloomFilterSearch("asdf");
            baz.AddFriend(debugpd.BloomFilterSearch("alice"));
            baz.AddFriend(debugpd.BloomFilterSearch("tom"));

            Person foo = debugpd.BloomFilterSearch("alice");
            foo.AddFriend(debugpd.BloomFilterSearch("tom"));




            //debugpd.BloomFilterSearch("asdf").AddFriend(debugpd.BloomFilterSearch("tom"));
            //debugpd.BloomFilterSearch("alice").AddFriend(debugpd.BloomFilterSearch("tom"));

            headEssay.ShowLoginForm();
            //Application.Run();
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
