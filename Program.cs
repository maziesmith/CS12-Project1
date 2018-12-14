using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS12_Project_1
{
    static class Program
    {
        private static string PD_DEBUGPATH = @"D:\CS12 Project 1";
        private static string PD_FILENAME = "AIDAN BIRD - HeadEssayData";
        private static readonly PersonsDatabase pd = new PersonsDatabase(PD_DEBUGPATH,null);
        private static readonly LoginSystem ls = new LoginSystem(pd);
        private static readonly HeadEssay headEssay = new HeadEssay(pd,ls);

        // The main entry point for the application.
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            pd.Initalize(PD_FILENAME);


            pd.AddPerson("asdf","asdf","asdf","asdf","asdfasdfasdf");
            pd.AddPerson("baz","baz","baz","alice","asdfasdfasdf");
            pd.AddPerson("baz","baz","baz","tom","asdfasdfasdf");
            pd.AddPerson("baz","baz","baz","bigbob","asdfasdfasdf");
            
            Person baz = pd.BloomFilterSearch("asdf");
            Person bar = pd.BloomFilterSearch("tom");
            Person foo = pd.BloomFilterSearch("alice");
            Person bigbob = pd.BloomFilterSearch("bigbob");

            baz.AddInterest("comsci","comeng","AI","ML");
            bar.AddInterest("comsci","comeng","ML","IT");
            foo.AddInterest("comeng","IT","osdev","comsec");
            bigbob.AddInterest("comeng","webdev","blockchain","crypto");
            baz.AddFriend(foo);
            baz.AddFriend(bar);
            foo.AddFriend(bar);
            bigbob.AddFriend(foo);
            bar.AddFriend(bigbob);
            pd.FullRebuildInterestTable();
            headEssay.ShowLoginForm();
        }
        public static void Exit(ISystem.ExitStatus t_exitStatus)
        {
            pd.Save(PD_FILENAME);
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
