using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS12_Project_1
{
    class LoginDialogue : Form
    {
        // DATA MEMBERS
        private ISystem parent_;    //
        private LoginSystem ls_;    //
        private Person nextLogin_;  //
        private Label lblInfoAuth2_;    //
        private TextBox txtAuthUser_;   //
        private TextBox txtAuthPass_;   //
        private Button btnAuthSubmit_;  //
        private Button btnAuthReg_; //
        private Panel panel1;   //
        private Label lblHeader;    //
        private Label lblInfoAuth1_;    //
        private void InitializeComponent()
        {
            this.lblInfoAuth1_ = new System.Windows.Forms.Label();
            this.lblInfoAuth2_ = new System.Windows.Forms.Label();
            this.txtAuthUser_ = new System.Windows.Forms.TextBox();
            this.txtAuthPass_ = new System.Windows.Forms.TextBox();
            this.btnAuthSubmit_ = new System.Windows.Forms.Button();
            this.btnAuthReg_ = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
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
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblInfoAuth1_);
            this.panel1.Controls.Add(this.txtAuthUser_);
            this.panel1.Controls.Add(this.btnAuthSubmit_);
            this.panel1.Controls.Add(this.btnAuthReg_);
            this.panel1.Controls.Add(this.txtAuthPass_);
            this.panel1.Controls.Add(this.lblInfoAuth2_);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(243, 98);
            this.panel1.TabIndex = 6;
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
            this.Controls.Add(this.panel1);
            this.Name = "LoginDialogue";
            this.Text = " ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginDialogue_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private object exitStatus;
        public enum Signals
        {
            signupRequest,
            loginSuccess
        }
        public LoginDialogue(ISystem t_parent, ref LoginSystem t_ls, ref LoginDialogue t_ld)
        {
            t_ld = this;
            ls_ = t_ls;
            parent_ = t_parent;
            InitializeComponent();
            Reload();
        }
        public void Reload()
        {
            lblHeader.ForeColor = System.Drawing.Color.Black;
            lblHeader.Text = "Sign In";
            exitStatus = null;
        }
        private enum Errno
        {
            emptyField,
            badAuth
        }
        private void Error(Errno t_errno)
        {
            lblHeader.ForeColor = System.Drawing.Color.Red;
            switch(t_errno)
            {
                case Errno.badAuth:
                    lblHeader.Text = "Incorrect username or password.";
                    return;
                case Errno.emptyField:
                    lblHeader.Text = "All fields must be filled.";
                    return;
            }
        }
        public Person Nextlogin
        {
            get { return nextLogin_; }
        }
        private void btnAuthSubmit__Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtAuthUser_.Text)
            || string.IsNullOrEmpty(txtAuthPass_.Text))
            {
                Error(Errno.emptyField);
                return;
            }
            if((nextLogin_ = ls_.Login(txtAuthUser_.Text, txtAuthPass_.Text)) == null)
            {
                Error(Errno.badAuth);
                return;
            }
            parent_.Callback(Signals.loginSuccess,this);
            exitStatus = ISystem.ExitStatus.noError;
            Close();
        }
        private void btnAuthReg__Click(object sender, EventArgs e)
        {
            exitStatus = Signals.signupRequest;
            Close();
        }
        private void LoginDialogue_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(exitStatus == null)
                parent_.Callback(ISystem.ExitStatus.userClosed,this);
            else
                parent_.Callback(exitStatus,this);
        }
    }
}
