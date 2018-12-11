using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace CS12_Project_1
{
    class RegisterDialogue : Form
    {
        /// <summary>
        /// A utility struct used for managing user data.
        /// Contains an internal state that determines what content will be exposed.
        /// The motovation for making this is to avoid excessive string copying. 
        /// </summary>
        internal struct Userinfo
        {
            public bool state;  // the state of the object
            private readonly string[] info_;    // custom message
            public string Info
            {   // contains 
                get
                {   // return a string based on an internal state.
                    // logic 1 = default message
                    // logic 0 = custom message
                    return info_[state ? 1 : 0];
                }
                set
                {   //set custom message and update internal state to logic 0
                    info_[0] = value;
                    state = false;
                }
            }
            /// <summary>
            /// A constructor that initialize all fields.
            /// </summary>
            /// <param name="t_defaultInfo"></param>
            public Userinfo(string t_defaultInfo)
            {   // initialize all fields
                info_ = new string[2];
                info_[1] = t_defaultInfo;
                state = false;
            }
        }
        private string nextUserName = null;
        private string nextPassword = null;
        private Userinfo nextFirstName = new Userinfo("no first name submitted");
        private Userinfo nextLastName = new Userinfo("no last name submitted");
        private Userinfo nextCity = new Userinfo("no city submitted");
        private PersonsDatabase pd_;
        private Button button1;
        private Panel panel2;
        private Label lblInfo1;
        private Label lblInfo3;
        private Label lblInfo2;
        private TextBox txtInfo3;
        private MaskedTextBox txtInfo2;
        private TextBox txtInfo1;
        private Label lblHeader;
        private Button btnCancel;
        private ISystem parent_;
        private string lastUserName_ = null;
        private DialogStates dialogState_ = DialogStates.auth;
        private System.Windows.Forms.Timer tmrClose;
        private System.ComponentModel.IContainer components;
        private object exitStatus = ExitStatus.canceled;

        private enum DialogStates
        {
            auth,
            config
        }
        public RegisterDialogue(ISystem t_parent, ref PersonsDatabase t_pd)
        {
            exitStatus = ExitStatus.canceled;
            pd_ = t_pd;
            parent_ = t_parent;
            InitializeComponent();
            ShowAuthSetup();
            //ShowDialog();
        }
        public void Reload()
        {
            exitStatus = null;
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblInfo1 = new System.Windows.Forms.Label();
            this.txtInfo1 = new System.Windows.Forms.TextBox();
            this.lblInfo3 = new System.Windows.Forms.Label();
            this.lblInfo2 = new System.Windows.Forms.Label();
            this.txtInfo3 = new System.Windows.Forms.TextBox();
            this.txtInfo2 = new System.Windows.Forms.MaskedTextBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.tmrClose = new System.Windows.Forms.Timer(this.components);
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(219, 89);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.lblInfo1);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.txtInfo1);
            this.panel2.Controls.Add(this.lblInfo3);
            this.panel2.Controls.Add(this.lblInfo2);
            this.panel2.Controls.Add(this.txtInfo3);
            this.panel2.Controls.Add(this.txtInfo2);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(310, 124);
            this.panel2.TabIndex = 12;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(138, 89);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblInfo1
            // 
            this.lblInfo1.AutoSize = true;
            this.lblInfo1.Location = new System.Drawing.Point(9, 14);
            this.lblInfo1.Name = "lblInfo1";
            this.lblInfo1.Size = new System.Drawing.Size(49, 13);
            this.lblInfo1.TabIndex = 6;
            this.lblInfo1.Text = "subtitle 1";
            // 
            // txtInfo1
            // 
            this.txtInfo1.Location = new System.Drawing.Point(104, 11);
            this.txtInfo1.Name = "txtInfo1";
            this.txtInfo1.Size = new System.Drawing.Size(190, 20);
            this.txtInfo1.TabIndex = 0;
            // 
            // lblInfo3
            // 
            this.lblInfo3.AutoSize = true;
            this.lblInfo3.Location = new System.Drawing.Point(11, 66);
            this.lblInfo3.Name = "lblInfo3";
            this.lblInfo3.Size = new System.Drawing.Size(46, 13);
            this.lblInfo3.TabIndex = 8;
            this.lblInfo3.Text = "subtitle3";
            // 
            // lblInfo2
            // 
            this.lblInfo2.AutoSize = true;
            this.lblInfo2.Location = new System.Drawing.Point(9, 40);
            this.lblInfo2.Name = "lblInfo2";
            this.lblInfo2.Size = new System.Drawing.Size(49, 13);
            this.lblInfo2.TabIndex = 9;
            this.lblInfo2.Text = "subtitle 2";
            // 
            // txtInfo3
            // 
            this.txtInfo3.Location = new System.Drawing.Point(104, 63);
            this.txtInfo3.Name = "txtInfo3";
            this.txtInfo3.Size = new System.Drawing.Size(190, 20);
            this.txtInfo3.TabIndex = 2;
            // 
            // txtInfo2
            // 
            this.txtInfo2.Location = new System.Drawing.Point(104, 37);
            this.txtInfo2.Name = "txtInfo2";
            this.txtInfo2.Size = new System.Drawing.Size(190, 20);
            this.txtInfo2.TabIndex = 1;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Location = new System.Drawing.Point(21, 5);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(23, 13);
            this.lblHeader.TabIndex = 11;
            this.lblHeader.Text = "title";
            // 
            // tmrClose
            // 
            this.tmrClose.Interval = 2000;
            this.tmrClose.Tick += new System.EventHandler(this.tmrClose_Tick);
            // 
            // RegisterDialogue
            // 
            this.ClientSize = new System.Drawing.Size(334, 158);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.panel2);
            this.Name = "RegisterDialogue";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RegisterDialogue_FormClosing);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private bool TestPasswordFields() { return txtInfo2.Text != txtInfo3.Text; }
        private void ShowAuthSetup()
        {
            dialogState_ = DialogStates.auth;
            btnCancel.Hide();
            lblHeader.ForeColor = System.Drawing.Color.Black;
            lblHeader.Text = "Sign Up";
            lblInfo1.Text = "Username";
            lblInfo2.Text = "Password";
            lblInfo3.Text = "Confirm Password";
            txtInfo1.Text = "";
            txtInfo2.Text = "";
            txtInfo3.Text = "";
            txtInfo2.UseSystemPasswordChar = true;
            txtInfo3.UseSystemPasswordChar = true;
        }
        private void ShowConfigSetup()
        {
            dialogState_ = DialogStates.config;
            btnCancel.Show();
            lblHeader.ForeColor = System.Drawing.Color.Black;
            lblHeader.Text = "User Config";
            lblInfo1.Text = "First Name";
            lblInfo2.Text = "Last Name";
            lblInfo3.Text = "City";
            txtInfo1.Text = "";
            txtInfo2.Text = "";
            txtInfo3.Text = "";
            txtInfo2.UseSystemPasswordChar = false;
            txtInfo3.UseSystemPasswordChar = false;
        }
        /// <summary>
        /// Invalidate all user inputed fields.
        /// </summary>
        /// <returns></returns>
        private bool InvalidateAllInput()
        {
            nextUserName = null;
            nextPassword = null;
            nextFirstName.state = false;
            nextLastName.state = false;
            nextCity.state = false;
            return false;
        }
        private bool HandlePasswords()
        {
            if (TestPasswordFields())
            {
                Error(Errno.badPassword);
                return true;
            }
            Password.ValidatePasswordStatus test;
            if ((test = Password.ValidatePasswordFormat(txtInfo2.Text)) != Password.ValidatePasswordStatus.noError)
            {
                Error(test);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Handle auth setup
        /// </summary>
        /// <returns></returns>
        private void HandleAuth()
        {
            const int minUserNameLen = 1;
            const int maxUserNameLen = 100;
            if (lastUserName_ == txtInfo1.Text
            && lastUserName_ != null)
            {   // use cached username
                if (HandlePasswords())
                    return;
                goto success;   // will i lose marks for using unconditional jumps?
            }
            // re-cache username
            lastUserName_ = null;
            if (string.IsNullOrWhiteSpace(txtInfo1.Text)
            || txtInfo1.Text.Length < minUserNameLen
            || txtInfo1.Text.Length > maxUserNameLen)
            {
                Error(Errno.shortUsername);
                return;   // invalid username
            }
            if(pd_.BloomFilterSearch(txtInfo1.Text) != null)
            {
                Error(Errno.badUsername);
                return; // username has been taken
            }
            // username has not been taken
            lastUserName_ = txtInfo1.Text;
            if(HandlePasswords())
                return;
        success:;
            nextUserName = txtInfo1.Text;
            nextPassword = txtInfo2.Text;
            ShowConfigSetup();
            return;
        }
        /// <summary>
        /// set the state of all Userinfo objects to logic 1.
        /// </summary>
        private void ZeroAllUserinfo()
        {
            nextFirstName.state = true;
            nextLastName.state = true;
            nextCity.state = true;
        }
        enum Errno
        {
            badPassword,
            badUsername,
            shortUsername,
            databaseError
        }
        /// <summary>
        /// Error handler
        /// </summary>
        /// <param name="t_errno"></param>
        private void Error(object t_errno)
        {
            lblHeader.ForeColor = System.Drawing.Color.Red;
            switch(t_errno)
            {
                //if (t_errno.GetType() == typeof(Errno))
                case Errno a:
                    switch (t_errno)
                    {
                        case Errno.badPassword:
                            //MessageBox.Show("Passwords do not match.");
                            lblHeader.Text = "Passwords do not match.";
                            return;
                        case Errno.badUsername:
                            //MessageBox.Show("Username already taken.");
                            lblHeader.Text = "Username already taken.";
                            return;
                        case Errno.shortUsername:
                            //MessageBox.Show("Username is too short.");
                            lblHeader.Text = "Username is too short.";
                            return;
                        case Errno.databaseError:
                            //MessageBox.Show("Database error.");
                            lblHeader.Text = "Database error.";
                            return;
                    }
                    return;
                case Password.ValidatePasswordStatus b:
                //else if (t_errno.GetType() == typeof(Password.ValidatePasswordStatus))
                    switch(t_errno)
                    {
                        case Password.ValidatePasswordStatus.badRepChars:
                            lblHeader.Text = "Password contains repeating characters.";
                            return;
                        case Password.ValidatePasswordStatus.badPasswdLen:
                            lblHeader.Text = "Password is too short.";
                            return;
                    }
                    return;
            }
            throw new Exception();
        }
        public enum ExitStatus
        {
            success,
            canceled
        }
        private void HandleConfig()
        {
            ZeroAllUserinfo();
            if(!string.IsNullOrWhiteSpace(txtInfo1.Text))
                nextFirstName.Info = txtInfo1.Text;
            if(!string.IsNullOrWhiteSpace(txtInfo2.Text))
                nextLastName.Info = txtInfo2.Text;
            if(!string.IsNullOrWhiteSpace(txtInfo3.Text))
                nextCity.Info = txtInfo3.Text;
            if (!pd_.AddPerson(nextFirstName.Info, nextLastName.Info, nextCity.Info, nextUserName, nextPassword))
            {
                Error(Errno.databaseError);
                return;
            }
            // success
            exitStatus = ExitStatus.success;
            lblHeader.ForeColor = System.Drawing.Color.Green;
            lblHeader.Text = "Welcome to the HeadEssay!";
            tmrClose.Start();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            switch(dialogState_)
            {
                case DialogStates.config:
                    HandleConfig();
                    return;
                case DialogStates.auth:
                    HandleAuth();
                    return;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ZeroAllUserinfo();
            ShowAuthSetup();
        }
        // exit handler
        private void RegisterDialogue_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (e.CloseReason != CloseReason.UserClosing)
            //    return;
            //parent_.Callback(ISystem.ExitStatus.userClosed,this);
            parent_.Callback(exitStatus,this);
        }

        private void tmrClose_Tick(object sender, EventArgs e)
        {
            tmrClose.Dispose();
            Close();
        }
    }
}
