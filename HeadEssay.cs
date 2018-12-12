using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS12_Project_1
{
    class HeadEssay : ISystem
    {
        private PersonsDatabase pd_;    // ref to database
        private LoginSystem ls_;        // ref to login system
        private Action NextAction;  // points to the next action to execute

        private LoginDialogue ld_;
        private RegisterDialogue rd_;
        private UserDialogue ud_;
        
        private ExitStatus exitStatus_; // exit status

        // CONSTRUCTOR
        // params:
        //  PersonsDatabase t_pd;   ref to person database
        //  LoginSystem t_ls;   ref to login system
        public HeadEssay(PersonsDatabase t_pd, LoginSystem t_ls)
        {
            pd_ = t_pd;
            ls_ = t_ls;
        }

        // exit the program
        public void CloseHeadEssay()
        {
            Program.Exit(exitStatus_);
        }

        // show login form
        public void ShowLoginForm()
        {
            using (ld_ = new LoginDialogue(this, ref ls_, ref ld_))
            {
                ld_.ShowDialog();
            }
            NextAction();   // execute the next action
        }
        
        // show signup form
        public void ShowRegForm()
        {
            using (rd_ = new RegisterDialogue(this, ref pd_))
            {
                rd_.ShowDialog();
            }
            NextAction();   // execute the next action
        }

        // display the user form
        public void ShowUserForm()
        {
            bool udStatus;  // exit status
            using (ud_ = new UserDialogue(this, ref pd_))
            {
                udStatus = ud_.AssignUser(ld_.Nextlogin);
            }
            if(!udStatus)
                ShowLoginForm();    // show the login form if the signup request failed
            NextAction();   // execute the next action
        }

        // signal handler; recives signals from related objects
        // params:
        //  object t_data;  data passed from the caller
        //  object t_sender;    the object of the caller itself
        public override void Callback(object t_data, object t_sender)
        {
            switch(t_sender)    // check the type of the sender
            {
                case LoginDialogue a:   // caller was a login system
                    switch(t_data)
                    {
                        case LoginDialogue.Signals.signupRequest:   // sign up request
                            NextAction = ShowRegForm;
                            return;
                        case LoginDialogue.Signals.loginSuccess:    // login request accepted
                            NextAction = ShowUserForm;
                            return;
                        case ExitStatus.userClosed:
                            exitStatus_ = ExitStatus.userClosed;    // user closed login form
                            NextAction = CloseHeadEssay;
                            return;
                        case ExitStatus.noError:
                            NextAction = ShowUserForm;
                            return;
                    }
                    throw new ArgumentException();
                case RegisterDialogue b:    // caller was a register dialogue
                    switch(t_data)
                    {
                        case RegisterDialogue.ExitStatus.success:   // signup was successful
                            NextAction = ShowLoginForm;
                            return;
                        case RegisterDialogue.ExitStatus.canceled:  // signup failed
                            NextAction = ShowLoginForm;
                            return;
                    }
                    throw new ArgumentException();
                case UserDialogue c:    // caller was a UserDialogue
                    switch(t_data)
                    {
                        case UserDialogue.ExitStatus.logout:    // user requested to log off
                            NextAction = ShowLoginForm;
                            return;
                    }
                    throw new ArgumentException();
            }
            throw new ArgumentException();
        }
    }
}
