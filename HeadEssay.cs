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
        private UserDialogue ud_;

        public HeadEssay(PersonsDatabase t_pd, LoginSystem t_ls)
        {
            pd_ = t_pd;
            ls_ = t_ls;
            //ud_ = new UserDialogue(this);
            //ld_ = new LoginDialogue(this, ref ls_, ref ld_);
            //rd_ = new RegisterDialogue(this, ref t_pd);
        }

        Action NextAction;

        private ExitStatus exitStatus_;
        public void CloseHeadEssay()
        {
            Program.Exit(exitStatus_);
        }

        public void ShowLoginForm()
        {
            using (ld_ = new LoginDialogue(this, ref ls_, ref ld_))
            {
                ld_.ShowDialog();
            }
            NextAction();
        }
        
        public void ShowRegForm()
        {
            using (rd_ = new RegisterDialogue(this, ref pd_))
            {
                rd_.ShowDialog();
            }
            NextAction();
        }

        public void ShowUserForm()
        {
            bool udStatus;
            using (ud_ = new UserDialogue(this, ref pd_))
            {
                udStatus = ud_.AssignUser(ld_.Nextlogin);
            }
            if(!udStatus)
                ShowLoginForm();
            NextAction();
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
                            NextAction = ShowUserForm;
                            return;
                        case ExitStatus.userClosed:
                            exitStatus_ = ExitStatus.userClosed;
                            NextAction = CloseHeadEssay;
                            return;
                        case ExitStatus.noError:
                            NextAction = ShowUserForm;
                            return;
                    }
                    throw new ArgumentException();
                case RegisterDialogue b:
                    switch(t_data)
                    {
                        case RegisterDialogue.ExitStatus.success:
                            NextAction = ShowLoginForm;
                            return;
                        case RegisterDialogue.ExitStatus.canceled:
                            NextAction = ShowLoginForm;
                            return;
                    }
                    throw new ArgumentException();
                case UserDialogue c:
                    switch(t_data)
                    {
                        case UserDialogue.ExitStatus.logout:
                            NextAction = ShowLoginForm;
                            return;
                    }
                    throw new ArgumentException();
            }
            throw new ArgumentException();
        }
    }
}
