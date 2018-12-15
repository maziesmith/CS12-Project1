using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS12_Project_1
{   // HEAD ESSAY
    // an object that manages dialogue
    // HeadEssay overrides methods in ISystem
    class HeadEssay : ISystem
    {   // DATA MEMBERS 
        private ExitStatus exitStatus_; // exit status
        private RegisterDialogue rd_;   // reference to the register dialogue; provides an graphical interface for signing up to the Head Essay
        private PersonsDatabase pd_;    // reference to person database for querying persons
        private LoginDialogue ld_;  // reference to the login dialogue; provides an graphical interface for logging in to the Head Essay
        private UserDialogue ud_;   // reference to the user dialogue; provides an graphical interface for viewing profile information, friends, and invites
        private LoginSystem ls_;    // reference to the login system; provides an interface for login dialogue to test login credentials.
        // CONSTRUCTOR
        // params:
        //  PersonsDatabase t_pd;   ref to person database
        //  LoginSystem t_ls;   ref to login system
        public HeadEssay(in PersonsDatabase t_pd, in LoginSystem t_ls)
        {   // initialize all critical data members
            pd_ = t_pd;
            ls_ = t_ls;
            // make a bunch of new dialogue forms
            ld_ = new LoginDialogue(this, ref ls_);
            ud_ = new UserDialogue(this, ref pd_);
            rd_ = new RegisterDialogue(this, ref pd_);
        }
        // exit the program
        public void CloseHeadEssay()
        {
            Program.Exit(exitStatus_);
        }
        // show login form
        public void ShowLoginForm()
        {
            ld_.Reload();   // init
            ld_.ClearFields();  // clear all fields
            ld_.Show(); // show the form 
        }
        // show signup form
        public void ShowRegForm()
        {
            rd_.Init(); // init
            rd_.Show(); // show the form
        }
        // display the user form
        public void ShowUserForm()
        {
            if(!ud_.AssignUser(ld_.Nextlogin))
            {
                ShowLoginForm();    // show the login form
                return; // fail
            }
            ud_.Show(); // success
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
                            ld_.Hide(); // hide the login form
                            ShowRegForm();   // set the next action to show the register form
                            return;
                        case LoginDialogue.Signals.loginSuccess:    // login request accepted
                            rd_.Hide(); // hide the register form
                            ld_.Hide(); // hide the login form
                            ShowUserForm();  // set the next action to show the user form
                            return;
                        case ExitStatus.userClosed:
                            exitStatus_ = ExitStatus.userClosed;    // user closed login form
                            CloseHeadEssay();    // set the next action to exit the program
                            return;
                        case ExitStatus.noError:    // the login form gave no error so its probably save to show the user form
                            rd_.Hide(); // hide the register form
                            ld_.Hide(); // hide the login form
                            ShowUserForm();  // set the next action to show the user form
                            return;
                    }
                    throw new ArgumentException();  // used for debugging; throw an error alerting the programmer (me) that they messed up
                case RegisterDialogue b:    // caller was a register dialogue
                    switch(t_data)  // do stuff based on t_data
                    {
                        case RegisterDialogue.ExitStatus.success:   // sign up was successful
                            rd_.Hide(); // hide the register form
                            ShowLoginForm(); // set the next action to show the login form
                            return;
                        case RegisterDialogue.ExitStatus.canceled:  // sign up failed
                            rd_.Hide(); // hide the register form
                            ShowLoginForm(); // set the next action to show the login form
                            return;
                    }
                    throw new ArgumentException(); // used for debugging; throw an error alerting the programmer (me) that they messed up
                case UserDialogue c:    // caller was a UserDialogue
                    switch(t_data)  // do stuff based on t_data
                    {
                        case UserDialogue.ExitStatus.logout:    // user requested to log off
                            rd_.Hide(); // hide the register form
                            ShowLoginForm(); // set the next action to show the login form
                            return;
                    }
                    throw new ArgumentException(); // used for debugging; throw an error alerting the programmer (me) that they messed up
                case PersonsDatabase d:
                    switch(t_data)
                    {
                        case Person.PersonFactory.Errors.nullCity:
                            rd_.Error(RegisterDialogue.Errors.noCity);
                            return;
                        case Person.PersonFactory.Errors.nullFirstName:
                            rd_.Error(RegisterDialogue.Errors.noFirstName);
                            return;
                        case Person.PersonFactory.Errors.nullLastName:
                            rd_.Error(RegisterDialogue.Errors.noLastName);
                            return;
                        case Person.PersonFactory.Errors.nullPassword:
                            rd_.Error(RegisterDialogue.Errors.noLastName);
                            return;
                        case Person.PersonFactory.Errors.nullUserName:
                            rd_.Error(RegisterDialogue.Errors.badUsername);
                            return;
                        case Password.ValidatePasswordStatus.badPasswdLen:
                            rd_.Error(RegisterDialogue.Errors.badPassword);
                            return;
                        case Password.ValidatePasswordStatus.badRepChars:
                            rd_.Error(RegisterDialogue.Errors.badPassword);
                            return;
                        case Password.ValidatePasswordStatus.noError:
                            return;
                    }
                    throw new ArgumentException(); // used for debugging; throw an error alerting the programmer (me) that they messed up
            }
            throw new ArgumentException(); // used for debugging; throw an error alerting the programmer (me) that they messed up
        }
    }
}
