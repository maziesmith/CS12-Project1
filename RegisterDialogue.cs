using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace CS12_Project_1
{
    //REGISTER DIALOGUE
    // a form that allows users to create person objects
    class RegisterDialogue : Form
    {   // DATA MEMBERS  
        // form elements
        private System.ComponentModel.IContainer components_;
        private PersonsDatabase pd_;
        private MaskedTextBox txtInfo2_;
        private TextBox txtInfo3_;
        private TextBox txtInfo1_;
        private Button btnCancel_;
        private Button btn1_;
        private Panel pnl2_;
        private Label lblHeader_;
        private Label lblInfo1_;
        private Label lblInfo3_;
        private Label lblInfo2_;
        // other data members
        private System.Windows.Forms.Timer tmrClose_;   // timer for signaling this form to close
        private DialogStates dialogState_ = DialogStates.auth;  // determines if the user's input will be used as authentication or profile configuration
        private ISystem parent_;    // the parent object to send signals to 
        private object exitStatus_ = ExitStatus.canceled;   // the status that this object will signal to the parent opon closing 
        private string nextUserName_ = null;    // user submitted info
        private string nextPassword_ = null;
        private string nextFirstName_ = null;
        private string nextLastName_ = null;
        private string nextCity_ = null;
        private string lastUserName_ = null;
        public enum Errors
        {
            badPassword,
            badUsername,
            shortUsername,
            databaseError,
            noCity,
            noFirstName,
            noLastName
        };
        public enum ExitStatus  // the exit statuses to be sent to the parent
        {
            success,
            canceled
        };
        private enum DialogStates   // determines if the user's input will be used as authentication or profile configuration
        {
            auth,
            config
        };
        // CONSTRUCTOR
        public RegisterDialogue(ISystem t_parent, ref PersonsDatabase t_pd)
        {   // initialize data members
            exitStatus_ = ExitStatus.canceled;
            pd_ = t_pd;
            parent_ = t_parent;
            InitializeComponent();  // setup the form
            ShowAuthSetup();    // show auth setup
        }
        // called when this form should be displayed again
        public void Reload()
        {   // reset the exit status
            exitStatus_ = null;
        }
        //  initialize the form
        private void InitializeComponent()
        {
            this.components_ = new System.ComponentModel.Container();
            this.btn1_ = new System.Windows.Forms.Button();
            this.pnl2_ = new System.Windows.Forms.Panel();
            this.btnCancel_ = new System.Windows.Forms.Button();
            this.lblInfo1_ = new System.Windows.Forms.Label();
            this.txtInfo1_ = new System.Windows.Forms.TextBox();
            this.lblInfo3_ = new System.Windows.Forms.Label();
            this.lblInfo2_ = new System.Windows.Forms.Label();
            this.txtInfo3_ = new System.Windows.Forms.TextBox();
            this.txtInfo2_ = new System.Windows.Forms.MaskedTextBox();
            this.lblHeader_ = new System.Windows.Forms.Label();
            this.tmrClose_ = new System.Windows.Forms.Timer(this.components_);
            this.pnl2_.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.btn1_.Location = new System.Drawing.Point(219, 89);
            this.btn1_.Name = "button1";
            this.btn1_.Size = new System.Drawing.Size(75, 23);
            this.btn1_.TabIndex = 10;
            this.btn1_.Text = "Submit";
            this.btn1_.UseVisualStyleBackColor = true;
            this.btn1_.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.pnl2_.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl2_.Controls.Add(this.btnCancel_);
            this.pnl2_.Controls.Add(this.lblInfo1_);
            this.pnl2_.Controls.Add(this.btn1_);
            this.pnl2_.Controls.Add(this.txtInfo1_);
            this.pnl2_.Controls.Add(this.lblInfo3_);
            this.pnl2_.Controls.Add(this.lblInfo2_);
            this.pnl2_.Controls.Add(this.txtInfo3_);
            this.pnl2_.Controls.Add(this.txtInfo2_);
            this.pnl2_.Location = new System.Drawing.Point(12, 12);
            this.pnl2_.Name = "panel2";
            this.pnl2_.Size = new System.Drawing.Size(310, 124);
            this.pnl2_.TabIndex = 12;
            // 
            // btnCancel
            // 
            this.btnCancel_.Location = new System.Drawing.Point(138, 89);
            this.btnCancel_.Name = "btnCancel";
            this.btnCancel_.Size = new System.Drawing.Size(75, 23);
            this.btnCancel_.TabIndex = 11;
            this.btnCancel_.Text = "Cancel";
            this.btnCancel_.UseVisualStyleBackColor = true;
            this.btnCancel_.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblInfo1
            // 
            this.lblInfo1_.AutoSize = true;
            this.lblInfo1_.Location = new System.Drawing.Point(9, 14);
            this.lblInfo1_.Name = "lblInfo1";
            this.lblInfo1_.Size = new System.Drawing.Size(49, 13);
            this.lblInfo1_.TabIndex = 6;
            this.lblInfo1_.Text = "subtitle 1";
            // 
            // txtInfo1
            // 
            this.txtInfo1_.Location = new System.Drawing.Point(104, 11);
            this.txtInfo1_.Name = "txtInfo1";
            this.txtInfo1_.Size = new System.Drawing.Size(190, 20);
            this.txtInfo1_.TabIndex = 0;
            // 
            // lblInfo3
            // 
            this.lblInfo3_.AutoSize = true;
            this.lblInfo3_.Location = new System.Drawing.Point(11, 66);
            this.lblInfo3_.Name = "lblInfo3";
            this.lblInfo3_.Size = new System.Drawing.Size(46, 13);
            this.lblInfo3_.TabIndex = 8;
            this.lblInfo3_.Text = "subtitle3";
            // 
            // lblInfo2
            // 
            this.lblInfo2_.AutoSize = true;
            this.lblInfo2_.Location = new System.Drawing.Point(9, 40);
            this.lblInfo2_.Name = "lblInfo2";
            this.lblInfo2_.Size = new System.Drawing.Size(49, 13);
            this.lblInfo2_.TabIndex = 9;
            this.lblInfo2_.Text = "subtitle 2";
            // 
            // txtInfo3
            // 
            this.txtInfo3_.Location = new System.Drawing.Point(104, 63);
            this.txtInfo3_.Name = "txtInfo3";
            this.txtInfo3_.Size = new System.Drawing.Size(190, 20);
            this.txtInfo3_.TabIndex = 2;
            // 
            // txtInfo2
            // 
            this.txtInfo2_.Location = new System.Drawing.Point(104, 37);
            this.txtInfo2_.Name = "txtInfo2";
            this.txtInfo2_.Size = new System.Drawing.Size(190, 20);
            this.txtInfo2_.TabIndex = 1;
            // 
            // lblHeader
            // 
            this.lblHeader_.AutoSize = true;
            this.lblHeader_.Location = new System.Drawing.Point(21, 5);
            this.lblHeader_.Name = "lblHeader";
            this.lblHeader_.Size = new System.Drawing.Size(23, 13);
            this.lblHeader_.TabIndex = 11;
            this.lblHeader_.Text = "title";
            // 
            // tmrClose
            // 
            this.tmrClose_.Interval = 2000;
            this.tmrClose_.Tick += new System.EventHandler(this.tmrClose_Tick);
            // 
            // RegisterDialogue
            // 
            this.ClientSize = new System.Drawing.Size(334, 158);
            this.Controls.Add(this.lblHeader_);
            this.Controls.Add(this.pnl2_);
            this.Name = "RegisterDialogue";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RegisterDialogue_FormClosing);
            this.pnl2_.ResumeLayout(false);
            this.pnl2_.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        // the the confirm password field
        private bool TestPasswordFields() { return txtInfo2_.Text != txtInfo3_.Text; }
        // show auth setup
        private void ShowAuthSetup()
        {   // reset form elements
            dialogState_ = DialogStates.auth;
            btnCancel_.Hide();
            lblHeader_.ForeColor = System.Drawing.Color.Black;
            lblHeader_.Text = "Sign Up";
            lblInfo1_.Text = "Username";
            lblInfo2_.Text = "Password";
            lblInfo3_.Text = "Confirm Password";
            ClearAllFields();   // clear user input fields
            txtInfo2_.UseSystemPasswordChar = true;
            txtInfo3_.UseSystemPasswordChar = true;
        }
        private void ShowConfigSetup()
        {   // reset form elements
            dialogState_ = DialogStates.config;
            btnCancel_.Show();
            lblHeader_.ForeColor = System.Drawing.Color.Black;
            lblHeader_.Text = "User Config";
            lblInfo1_.Text = "First Name";
            lblInfo2_.Text = "Last Name";
            lblInfo3_.Text = "City";
            ClearAllFields();   // clear user input fields
            txtInfo2_.UseSystemPasswordChar = false;
            txtInfo3_.UseSystemPasswordChar = false;
        }
        // reset everything
        public void Init()
        {
            InvalidateAllInput();
            ShowAuthSetup();
        }
        // Invalidate all user inputed fields.
        private void InvalidateAllInput()
        {
            nextUserName_ = null;
            nextPassword_ = null;
            nextFirstName_ = null;
            nextLastName_ = null;
            nextCity_ = null;
        }
        // check for problems in password fields
        private bool HandlePasswords()
        {
            if (TestPasswordFields())   // test the retype password field
            {
                Error(Errors.badPassword);  // show error message
                return true;    // fail
            }
            Password.ValidatePasswordStatus test;   // test password format
            if ((test = Password.ValidatePasswordFormat(txtInfo2_.Text)) != Password.ValidatePasswordStatus.noError)
            {
                Error(test);    // show error message
                return true;    // fail
            }
            return false;   // success
        }
        // Handle auth setup
        private void HandleAuth()
        {
            const int MIN_USER_NAME_LEN = 1;
            const int MAX_USER_NAME_LEN = 100;
            if (lastUserName_ == txtInfo1_.Text
            && lastUserName_ != null)
            {   // use cached username
                if (HandlePasswords())
                    return;
                goto success;   // will i lose marks for using unconditional jumps?
            }
            // re-cache username
            lastUserName_ = null;
            if (string.IsNullOrWhiteSpace(txtInfo1_.Text)   // null check
            || txtInfo1_.Text.Length < MIN_USER_NAME_LEN    // length check
            || txtInfo1_.Text.Length > MAX_USER_NAME_LEN)
            {   // invalid username
                Error(Errors.shortUsername);    // show error message
                return;     // fail
            }
            if(pd_.BloomFilterSearch(txtInfo1_.Text) != null)   // test if person is already in database
            {   // username has been taken
                Error(Errors.badUsername);  // show error message
                return;     // fail
            }
            // username has not been taken
            lastUserName_ = txtInfo1_.Text; // set cache to user input
            if(HandlePasswords())   // bad passwords
                return; // fail
        success:;
            nextUserName_ = txtInfo1_.Text; // set username to user input
            nextPassword_ = txtInfo2_.Text; // set password to user input
            ShowConfigSetup();  // show the config setup 
            return; // success  
        }
        // clear user input fields
        private void ClearAllFields()
        {
            txtInfo1_.Clear();
            txtInfo2_.Clear();
            txtInfo3_.Clear();
        }
        // Error handler
        public void Error(object t_errno)
        {
            lblHeader_.ForeColor = System.Drawing.Color.Red;    // set the header colour to red
            switch(t_errno) // check t_errno
            {
                case Errors a:
                    switch (t_errno)    // pick an error message and then display it
                    {
                        case Errors.badPassword:
                            //MessageBox.Show("Passwords do not match.");
                            lblHeader_.Text = "Passwords do not match.";
                            return;
                        case Errors.badUsername:
                            //MessageBox.Show("Username already taken.");
                            lblHeader_.Text = "Username already taken.";
                            return;
                        case Errors.shortUsername:
                            //MessageBox.Show("Username is too short.");
                            lblHeader_.Text = "Username is too short.";
                            return;
                        case Errors.databaseError:
                            //MessageBox.Show("Database error.");
                            lblHeader_.Text = "Database error.";
                            return;
                        case Errors.noCity:
                            //MessageBox.Show("No city provided");
                            lblHeader_.Text = "No city provided";
                            return;
                        case Errors.noFirstName:
                            //MessageBox.Show("No first name provided");
                            lblHeader_.Text = "No first name provided";
                            return;
                        case Errors.noLastName:
                            //MessageBox.Show("No last name provided");
                            lblHeader_.Text = "No last name provided";
                            return;
                    }
                    return; // success
                case Password.ValidatePasswordStatus b:
                    switch(t_errno) // bad password errors
                    {
                        case Password.ValidatePasswordStatus.badRepChars:   // repeating chars
                            lblHeader_.Text = "Password contains repeating characters.";
                            return;
                        case Password.ValidatePasswordStatus.badPasswdLen:  // small password
                            lblHeader_.Text = "Password is too short.";
                            return;
                    }
                    return;
            }
            throw new Exception();
        }
        // accept user input for setting up the person object config
        private void HandleConfig()
        {
            if (!pd_.AddPerson(txtInfo1_.Text,  // try to add a person using the info provided by the user
                txtInfo2_.Text,
                txtInfo3_.Text,
                nextUserName_,
                nextPassword_))
            {
                return; // fail
            }
            // success
            exitStatus_ = ExitStatus.success;
            lblHeader_.ForeColor = System.Drawing.Color.Green;
            lblHeader_.Text = "Welcome to the HeadEssay!";  // welcome the new user
            tmrClose_.Start();
        }
        // when btn1_ is clicked
        private void button1_Click(object sender, EventArgs e)
        {
            switch(dialogState_)    // handle different inputs based on the context 
            {
                case DialogStates.config:
                    HandleConfig(); // handle profile config
                    return;
                case DialogStates.auth:
                    HandleAuth();   // handle auth config
                    return;
            }
        }
        // go back to the first step
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ShowAuthSetup();
        }
        // exit handler
        private void RegisterDialogue_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = true;    // stop the form from being deleted
            parent_.Callback(exitStatus_,this); // signal the parent that the user wants to close this form
        }
        // dispose of the timer object
        private void tmrClose_Tick(object sender, EventArgs e)
        {
            tmrClose_.Dispose();
            Close();    // exit
        }
    }
}
