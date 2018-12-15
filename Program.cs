using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CS12_Project_1
{
    static class Program
    {
        private static string PD_DEBUGPATH = string.Concat(Directory.GetCurrentDirectory(),'\\');   // path of database file
        private static string PD_FILENAME = "AIDAN BIRD - HeadEssayData";   // name of database file
        private static PersonsDatabase pdRef;   // reference to person database
        private static HeadEssay heRef; // reference to a head essay object
        // entry point
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles(); 
            Application.SetCompatibleTextRenderingDefault(false);
            PersonsDatabase pd = new PersonsDatabase(PD_DEBUGPATH,heRef);   // create important objects
            LoginSystem ls = new LoginSystem(pd);
            HeadEssay headEssay = new HeadEssay(pd, ls);
            pdRef = pd; // setup references
            heRef = headEssay;
            pd.ChangeParent(heRef);
            pd.Initalize(PD_FILENAME);  //Initalize the person database
            pd.FullRebuildInterestTable();  // rebuild tables
            headEssay.ShowLoginForm();  // show the login form 
            Application.Run();  // run the application 
        }
        // save the database and exit the program
        public static void Exit(ISystem.ExitStatus t_exitStatus)
        {
            pdRef.Save(PD_FILENAME);    // save the database file 
            switch(t_exitStatus)    // exit 
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
