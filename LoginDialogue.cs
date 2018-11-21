using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app1
{
    class LoginDialogue : Form
    {
        private ISystem parent_;
        private LoginSystem ls_;
        private Person nextLogin_;
        private Label lblInfoAuth2_;
        private TextBox txtAuthUser_;
        private TextBox txtAuthPass_;
        private Button btnAuthSubmit_;
        private Button btnAuthReg_;
        private Label lblInfoAuth1_;
        private void InitializeComponent()
        {
            this.lblInfoAuth1_ = new System.Windows.Forms.Label();
            this.lblInfoAuth2_ = new System.Windows.Forms.Label();
            this.txtAuthUser_ = new System.Windows.Forms.TextBox();
            this.txtAuthPass_ = new System.Windows.Forms.TextBox();
            this.btnAuthSubmit_ = new System.Windows.Forms.Button();
            this.btnAuthReg_ = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblInfoAuth1_
            // 
            this.lblInfoAuth1_.AutoSize = true;
            this.lblInfoAuth1_.Location = new System.Drawing.Point(12, 9);
            this.lblInfoAuth1_.Name = "lblInfoAuth1_";
            this.lblInfoAuth1_.Size = new System.Drawing.Size(27, 13);
            this.lblInfoAuth1_.TabIndex = 0;
            this.lblInfoAuth1_.Text = "user";
            // 
            // lblInfoAuth2_
            // 
            this.lblInfoAuth2_.AutoSize = true;
            this.lblInfoAuth2_.Location = new System.Drawing.Point(10, 35);
            this.lblInfoAuth2_.Name = "lblInfoAuth2_";
            this.lblInfoAuth2_.Size = new System.Drawing.Size(29, 13);
            this.lblInfoAuth2_.TabIndex = 1;
            this.lblInfoAuth2_.Text = "pass";
            // 
            // txtAuthUser_
            // 
            this.txtAuthUser_.Location = new System.Drawing.Point(45, 6);
            this.txtAuthUser_.Name = "txtAuthUser_";
            this.txtAuthUser_.Size = new System.Drawing.Size(184, 20);
            this.txtAuthUser_.TabIndex = 2;
            // 
            // txtAuthPass_
            // 
            this.txtAuthPass_.Location = new System.Drawing.Point(45, 32);
            this.txtAuthPass_.Name = "txtAuthPass_";
            this.txtAuthPass_.Size = new System.Drawing.Size(184, 20);
            this.txtAuthPass_.TabIndex = 3;
            // 
            // btnAuthSubmit_
            // 
            this.btnAuthSubmit_.Location = new System.Drawing.Point(173, 58);
            this.btnAuthSubmit_.Name = "btnAuthSubmit_";
            this.btnAuthSubmit_.Size = new System.Drawing.Size(56, 23);
            this.btnAuthSubmit_.TabIndex = 4;
            this.btnAuthSubmit_.Text = "Submit";
            this.btnAuthSubmit_.UseVisualStyleBackColor = true;
            this.btnAuthSubmit_.Click += new System.EventHandler(this.btnAuthSubmit__Click);
            // 
            // btnAuthReg_
            // 
            this.btnAuthReg_.Location = new System.Drawing.Point(111, 58);
            this.btnAuthReg_.Name = "btnAuthReg_";
            this.btnAuthReg_.Size = new System.Drawing.Size(56, 23);
            this.btnAuthReg_.TabIndex = 5;
            this.btnAuthReg_.Text = "Register";
            this.btnAuthReg_.UseVisualStyleBackColor = true;
            this.btnAuthReg_.Click += new System.EventHandler(this.btnAuthReg__Click);
            // 
            // LoginDialogue
            // 
            this.ClientSize = new System.Drawing.Size(241, 98);
            this.Controls.Add(this.btnAuthReg_);
            this.Controls.Add(this.btnAuthSubmit_);
            this.Controls.Add(this.txtAuthPass_);
            this.Controls.Add(this.txtAuthUser_);
            this.Controls.Add(this.lblInfoAuth2_);
            this.Controls.Add(this.lblInfoAuth1_);
            this.Name = "LoginDialogue";
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public enum CallingCodes
        {
            LoginSuccess = 0
        }

        public LoginDialogue(ISystem t_parent, LoginSystem t_ls, ref LoginDialogue t_ld)
        {
            t_ld = this;
            ls_ = t_ls;
            parent_ = t_parent;
            InitializeComponent();
            ShowDialog();
        }

        private void btnAuthSubmit__Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtAuthUser_.Text)
            || string.IsNullOrEmpty(txtAuthPass_.Text))
                return;
            if((nextLogin_ = ls_.Login(txtAuthUser_.Text, txtAuthPass_.Text)) == null)
            {   // bad auth
                return;
            }
            parent_.Callback((uint)CallingCodes.LoginSuccess,nextLogin_,this);
            //parent_.Callback((uint)CallingCodes.LoginSuccess,nextLogin_,GetType());
        }

        private void btnAuthReg__Click(object sender, EventArgs e)
        {

        }
    }
}
