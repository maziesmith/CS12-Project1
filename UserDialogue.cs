using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app1
{
    class UserDialogue : Form
    {
        private User currentUser_;
        private Label lblFullname_;
        private Label lblUsername_;
        private void InitializeComponent()
        {
            this.lblFullname_ = new System.Windows.Forms.Label();
            this.lblUsername_ = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblFullname_
            // 
            this.lblFullname_.AutoSize = true;
            this.lblFullname_.Location = new System.Drawing.Point(12, 9);
            this.lblFullname_.Name = "lblFullname_";
            this.lblFullname_.Size = new System.Drawing.Size(46, 13);
            this.lblFullname_.TabIndex = 3;
            this.lblFullname_.Text = "fullname";
            // 
            // lblUsername_
            // 
            this.lblUsername_.AutoSize = true;
            this.lblUsername_.Location = new System.Drawing.Point(12, 32);
            this.lblUsername_.Name = "lblUsername_";
            this.lblUsername_.Size = new System.Drawing.Size(53, 13);
            this.lblUsername_.TabIndex = 4;
            this.lblUsername_.Text = "username";
            // 
            // UserDialogue
            // 
            this.ClientSize = new System.Drawing.Size(287, 225);
            this.Controls.Add(this.lblUsername_);
            this.Controls.Add(this.lblFullname_);
            this.Name = "UserDialogue";
            this.Text = "User";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        public UserDialogue()
        {
            InitializeComponent();
            Hide();
        }
        public bool AssignUser(User t_user)
        {
            if (t_user == null)
                return false;
            currentUser_ = t_user;
            ShowDialog();
            return true;
        }
        public void DisposeOfUser()
        {
            currentUser_ = null;
            Hide();
        }
    }
}
