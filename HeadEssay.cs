using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS12_Project_1
{   // HEAD ESSAY
    // an object that manages dialogue
    class HeadEssay : ISystem
    {   // DATA MEMBERS 
        private ExitStatus exitStatus_; // exit status
        private RegisterDialogue rd_;   // reference to the register dialogue; provides an graphical interface for signing up to the Head Essay
        private PersonsDatabase pd_;    // reference to person database for querying persons
        private Action NextAction;  // points to the next action to execute
        private LoginDialogue ld_;  // reference to the login dialogue; provides an graphical interface for logging in to the Head Essay
        private UserDialogue ud_;   // reference to the user dialogue; provides an graphical interface for viewing profile information, friends, and invites
        private LoginSystem ls_;    // reference to the login system; provides an interface for login dialogue to test login credentials.
        // CONSTRUCTOR
        // params:
        //  PersonsDatabase t_pd;   ref to person database
        //  LoginSystem t_ls;   ref to login system
        public HeadEssay(PersonsDatabase t_pd, LoginSystem t_ls)
        {   // initialize all critical data members
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
            using (ld_ = new LoginDialogue(this, ref ls_, ref ld_)) // TODO: make this more efficient; no need to dispose this
            {   // show the login dialogue
                ld_.ShowDialog();
            }
            NextAction();   // execute the next action
        }
        // show signup form
        public void ShowRegForm()
        {
            using (rd_ = new RegisterDialogue(this, ref pd_))   
            {   // show the register dialogue
                rd_.ShowDialog();
            }
            NextAction();   // execute the next action
        }
        // display the user form
        public void ShowUserForm()
        {
            bool udStatus;  // holds the exit status of the user dialogue
            using (ud_ = new UserDialogue(this, ref pd_))
            {   // show the register dialogue
                udStatus = ud_.AssignUser(ld_.Nextlogin);
            }
            if(!udStatus)
                ShowLoginForm();    // show the login form if the sign up request failed
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
                    switch(t_data)  // do stuff based on t_data
                    {
                        case LoginDialogue.Signals.signupRequest:   // sign up request
                            NextAction = ShowRegForm;   // set the next action to show the register form
                            return;
                        case LoginDialogue.Signals.loginSuccess:    // login request accepted
                            NextAction = ShowUserForm;  // set the next action to show the user form
                            return;
                        case ExitStatus.userClosed:
                            exitStatus_ = ExitStatus.userClosed;    // user closed login form
                            NextAction = CloseHeadEssay;    // set the next action to exit the program
                            return;
                        case ExitStatus.noError:    // the login form gave no error so its probably save to show the user form
                            NextAction = ShowUserForm;  // set the next action to show the user form
                            return;
                    }
                    throw new ArgumentException();  // used for debugging; throw an error alerting the programmer (me) that they messed up
                case RegisterDialogue b:    // caller was a register dialogue
                    switch(t_data)  // do stuff based on t_data
                    {
                        case RegisterDialogue.ExitStatus.success:   // sign up was successful
                            NextAction = ShowLoginForm; // set the next action to show the login form
                            return;
                        case RegisterDialogue.ExitStatus.canceled:  // sign up failed
                            NextAction = ShowLoginForm; // set the next action to show the login form
                            return;
                    }
                    throw new ArgumentException(); // used for debugging; throw an error alerting the programmer (me) that they messed up
                case UserDialogue c:    // caller was a UserDialogue
                    switch(t_data)  // do stuff based on t_data
                    {
                        case UserDialogue.ExitStatus.logout:    // user requested to log off
                            NextAction = ShowLoginForm; // set the next action to show the login form
                            return;
                    }
                    throw new ArgumentException(); // used for debugging; throw an error alerting the programmer (me) that they messed up
            }
            throw new ArgumentException(); // used for debugging; throw an error alerting the programmer (me) that they messed up
        }
    }
}
