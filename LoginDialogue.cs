using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS12_Project_1
{   // provides a user interface for loggin into the Head Essay
    class LoginDialogue : Form
    {   // DATA MEMBERS
        // form elements
        private TextBox txtAuthUser_; 
        private TextBox txtAuthPass_; 
        private Button btnAuthSubmit_;
        private Button btnAuthReg_;
        private Panel pnl1_;
        private Label lblHeader;
        private Label lblInfoAuth2_;  
        private Label lblInfoAuth1_;
        // other data members
        private object exitStatus;  // exit status of the login form
        private ISystem parent_;    // parent object for sending signals to
        private LoginSystem ls_;    // reference to a login system object
        private Person nextLogin_;  // the next person object that will be logged in
        public enum Signals 
        {   // signals that can be sent to the parent
            signupRequest,
            loginSuccess
        }
        private enum Errors  
        {   // error codes
            emptyField,
            badAuth
        }
        // CONSTRUCTOR
        // params:
        //  ISystem t_parent;   parent object to send signals to 
        //  ref LoginSystem t_ls;   a reference to a login system
        public LoginDialogue(ISystem t_parent, ref LoginSystem t_ls)
        {   // initialize all data members 
            ls_ = t_ls;
            parent_ = t_parent;
            InitializeComponent();
            Reload();
        }
        // METHOD MEMBERS
        // reload the dialog elements
        public void Reload()
        {
            lblHeader.ForeColor = System.Drawing.Color.Black;   // set the ForeColor to black
            lblHeader.Text = "Sign In"; // set the header text to "Sign In"
            exitStatus = null;  // set the exit status to null
        }
        // clear all input fields
        public void ClearFields()
        {
            txtAuthPass_.Clear();
            txtAuthUser_.Clear();
        }
        private void Error(Errors t_errno)
        {
            lblHeader.ForeColor = System.Drawing.Color.Red; // set the header forecolour to red
            switch(t_errno) // change the header text based on the error code
            {
                case Errors.badAuth:     // the user entered incorrect credentials
                    lblHeader.Text = "Incorrect username or password.";
                    return;
                case Errors.emptyField:  // the user did not fill in all fields
                    lblHeader.Text = "All fields must be filled.";
                    return;
            }
        }
        // get the logged in user
        public Person Nextlogin
        {
            get { return nextLogin_; }
        }
        // EVENTS
        // when btnAuthSubmit_ is pressed
        private void btnAuthSubmit__Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtAuthUser_.Text)
            || string.IsNullOrEmpty(txtAuthPass_.Text))
            {   // if any of the fields are empty, alert the user that they messed up
                Error(Errors.emptyField);
                return;
            }
            if((nextLogin_ = ls_.Login(txtAuthUser_.Text, txtAuthPass_.Text)) == null)
            {   // if the login credentials are incorrect, alert the user that they messed up 
                Error(Errors.badAuth);
                return;
            }
            parent_.Callback(Signals.loginSuccess,this);    // signal to the parent that the user logged in correctly
            exitStatus = ISystem.ExitStatus.noError;    // set the exit status to no error
        }
        // when btnAuthReg_ is pressed
        private void btnAuthReg__Click(object sender, EventArgs e)
        {
            parent_.Callback(Signals.signupRequest,this);   // send sign up request to parent
        }
        // when this form is closing 
        private void LoginDialogue_FormClosing(object sender, FormClosingEventArgs e)
        {   // if the exit status is null, tell the parent that the user closed the form
            if(e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                if (exitStatus == null)
                    parent_.Callback(ISystem.ExitStatus.userClosed, this);
                else    // tell the parent that this form is closing
                    parent_.Callback(exitStatus, this);
            }
        }
        // initialize the form 
        private void InitializeComponent()
        {
            this.lblInfoAuth1_ = new System.Windows.Forms.Label();
            this.lblInfoAuth2_ = new System.Windows.Forms.Label();
            this.txtAuthUser_ = new System.Windows.Forms.TextBox();
            this.txtAuthPass_ = new System.Windows.Forms.TextBox();
            this.btnAuthSubmit_ = new System.Windows.Forms.Button();
            this.btnAuthReg_ = new System.Windows.Forms.Button();
            this.pnl1_ = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.pnl1_.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblInfoAuth1_
            // 
            this.lblInfoAuth1_.AutoSize = true;
            this.lblInfoAuth1_.Location = new System.Drawing.Point(10, 14);
            this.lblInfoAuth1_.Name = "lblInfoAuth1_";
            this.lblInfoAuth1_.Size = new System.Drawing.Size(29, 13);
            this.lblInfoAuth1_.TabIndex = 0;
            this.lblInfoAuth1_.Text = "User";
            // 
            // lblInfoAuth2_
            // 
            this.lblInfoAuth2_.AutoSize = true;
            this.lblInfoAuth2_.Location = new System.Drawing.Point(10, 40);
            this.lblInfoAuth2_.Name = "lblInfoAuth2_";
            this.lblInfoAuth2_.Size = new System.Drawing.Size(30, 13);
            this.lblInfoAuth2_.TabIndex = 1;
            this.lblInfoAuth2_.Text = "Pass";
            // 
            // txtAuthUser_
            // 
            this.txtAuthUser_.Location = new System.Drawing.Point(43, 11);
            this.txtAuthUser_.Name = "txtAuthUser_";
            this.txtAuthUser_.Size = new System.Drawing.Size(184, 20);
            this.txtAuthUser_.TabIndex = 2;
            // 
            // txtAuthPass_
            // 
            this.txtAuthPass_.Location = new System.Drawing.Point(43, 37);
            this.txtAuthPass_.Name = "txtAuthPass_";
            this.txtAuthPass_.Size = new System.Drawing.Size(184, 20);
            this.txtAuthPass_.TabIndex = 3;
            this.txtAuthPass_.UseSystemPasswordChar = true;
            // 
            // btnAuthSubmit_
            // 
            this.btnAuthSubmit_.Location = new System.Drawing.Point(171, 63);
            this.btnAuthSubmit_.Name = "btnAuthSubmit_";
            this.btnAuthSubmit_.Size = new System.Drawing.Size(56, 23);
            this.btnAuthSubmit_.TabIndex = 4;
            this.btnAuthSubmit_.Text = "Submit";
            this.btnAuthSubmit_.UseVisualStyleBackColor = true;
            this.btnAuthSubmit_.Click += new System.EventHandler(this.btnAuthSubmit__Click);
            // 
            // btnAuthReg_
            // 
            this.btnAuthReg_.Location = new System.Drawing.Point(109, 63);
            this.btnAuthReg_.Name = "btnAuthReg_";
            this.btnAuthReg_.Size = new System.Drawing.Size(56, 23);
            this.btnAuthReg_.TabIndex = 5;
            this.btnAuthReg_.Text = "Register";
            this.btnAuthReg_.UseVisualStyleBackColor = true;
            this.btnAuthReg_.Click += new System.EventHandler(this.btnAuthReg__Click);
            // 
            // panel1
            // 
            this.pnl1_.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl1_.Controls.Add(this.lblInfoAuth1_);
            this.pnl1_.Controls.Add(this.txtAuthUser_);
            this.pnl1_.Controls.Add(this.btnAuthSubmit_);
            this.pnl1_.Controls.Add(this.btnAuthReg_);
            this.pnl1_.Controls.Add(this.txtAuthPass_);
            this.pnl1_.Controls.Add(this.lblInfoAuth2_);
            this.pnl1_.Location = new System.Drawing.Point(12, 12);
            this.pnl1_.Name = "panel1";
            this.pnl1_.Size = new System.Drawing.Size(243, 98);
            this.pnl1_.TabIndex = 6;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Location = new System.Drawing.Point(23, 5);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(35, 13);
            this.lblHeader.TabIndex = 7;
            this.lblHeader.Text = "label1";
            // 
            // LoginDialogue
            // 
            this.ClientSize = new System.Drawing.Size(268, 126);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.pnl1_);
            this.Name = "LoginDialogue";
            this.Text = " ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginDialogue_FormClosing);
            this.pnl1_.ResumeLayout(false);
            this.pnl1_.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
