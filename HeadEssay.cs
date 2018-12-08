using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS12_Project_1
{
    class HeadEssay : ISystem
    {
        private PersonsDatabase pd_;
        private LoginSystem ls_;

        private LoginDialogue ld_;
        private RegisterDialogue rd_;

        public HeadEssay(PersonsDatabase t_pd, LoginSystem t_ls)
        {
            pd_ = t_pd;
            ls_ = t_ls;
            ld_ = new LoginDialogue(this, ref ls_, ref ld_);
            rd_ = new RegisterDialogue(this, ref t_pd);
        }

        Action NextAction;

        private ExitStatus exitStatus_;
        public void CloseHeadEssay()
        {
            Program.Exit(exitStatus_);
        }

        public void ShowLoginForm()
        {
            rd_.Hide();
            ld_.Reload();
            ld_.ShowDialog();
            NextAction();
        }
        
        public void ShowRegForm()
        {
            ld_.Hide();
            rd_.Reload();
            rd_.ShowDialog();
            NextAction();
        }

        public void ShowUserForm()
        {


        }

        // signal handler impl
        public override void Callback(object t_data, object t_sender)
        {
            switch(t_sender)
            {
                case LoginDialogue a:
                    switch(t_data)
                    {
                        case LoginDialogue.Signals.signupRequest:
                            NextAction = ShowRegForm;
                            return;
                        case LoginDialogue.Signals.loginSuccess:
                            //NextAction
                            return;
                        case ExitStatus.userClosed:
                            exitStatus_ = ExitStatus.userClosed;
                            NextAction = CloseHeadEssay;
                            return;
                    }
                    throw new ArgumentException();
                case RegisterDialogue b:
                    switch(t_data)
                    {
                        case ISystem.ExitStatus.userClosed:
                            NextAction = ShowLoginForm;
                            return;
                    }
                    throw new ArgumentException();
            }
            throw new ArgumentException();
        }
    }
}
