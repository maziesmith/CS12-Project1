using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS12_Project_1
{
    class UserDialogue : Form
    {
        private User currentUser_;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem changeInfoToolStripMenuItem;
        private ToolStripMenuItem manageAccountToolStripMenuItem;
        private ToolStripMenuItem logoutToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem accountInfoToolStripMenuItem;
        private ToolStripMenuItem invitesToolStripMenuItem1;
        private ToolStripMenuItem searchToolStripMenuItem;
        private ToolStripMenuItem homepageToolStripMenuItem;
        private GroupBox gbMain;
        private Panel panel1;
        private Label lblTitleHomepage;
        private GroupBox groupBox1;
        private ListBox lbFriends;
        private Label lblNoFriends;
        private ToolStripMenuItem firendsToolStripMenuItem;
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.firendsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invitesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.homepageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbMain = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbFriends = new System.Windows.Forms.ListBox();
            this.lblTitleHomepage = new System.Windows.Forms.Label();
            this.lblNoFriends = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.gbMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(446, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accountInfoToolStripMenuItem,
            this.firendsToolStripMenuItem,
            this.invitesToolStripMenuItem1,
            this.searchToolStripMenuItem,
            this.homepageToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // accountInfoToolStripMenuItem
            // 
            this.accountInfoToolStripMenuItem.Name = "accountInfoToolStripMenuItem";
            this.accountInfoToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.accountInfoToolStripMenuItem.Text = "Account Info";
            this.accountInfoToolStripMenuItem.Click += new System.EventHandler(this.accountInfoToolStripMenuItem_Click);
            // 
            // firendsToolStripMenuItem
            // 
            this.firendsToolStripMenuItem.Name = "firendsToolStripMenuItem";
            this.firendsToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.firendsToolStripMenuItem.Text = "Friends";
            // 
            // invitesToolStripMenuItem1
            // 
            this.invitesToolStripMenuItem1.Name = "invitesToolStripMenuItem1";
            this.invitesToolStripMenuItem1.Size = new System.Drawing.Size(143, 22);
            this.invitesToolStripMenuItem1.Text = "Invites";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.searchToolStripMenuItem.Text = "Search";
            // 
            // homepageToolStripMenuItem
            // 
            this.homepageToolStripMenuItem.Name = "homepageToolStripMenuItem";
            this.homepageToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.homepageToolStripMenuItem.Text = "Homepage";
            this.homepageToolStripMenuItem.Click += new System.EventHandler(this.homepageToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeInfoToolStripMenuItem,
            this.manageAccountToolStripMenuItem,
            this.logoutToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // changeInfoToolStripMenuItem
            // 
            this.changeInfoToolStripMenuItem.Name = "changeInfoToolStripMenuItem";
            this.changeInfoToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.changeInfoToolStripMenuItem.Text = "Change Info";
            this.changeInfoToolStripMenuItem.Click += new System.EventHandler(this.changeInfoToolStripMenuItem_Click);
            // 
            // manageAccountToolStripMenuItem
            // 
            this.manageAccountToolStripMenuItem.Name = "manageAccountToolStripMenuItem";
            this.manageAccountToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.manageAccountToolStripMenuItem.Text = "Manage Account";
            this.manageAccountToolStripMenuItem.Click += new System.EventHandler(this.manageAccountToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // gbMain
            // 
            this.gbMain.Controls.Add(this.panel1);
            this.gbMain.Location = new System.Drawing.Point(12, 27);
            this.gbMain.Name = "gbMain";
            this.gbMain.Size = new System.Drawing.Size(422, 247);
            this.gbMain.TabIndex = 1;
            this.gbMain.TabStop = false;
            this.gbMain.Text = "gbMain";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.lblTitleHomepage);
            this.panel1.Location = new System.Drawing.Point(7, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(409, 221);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblNoFriends);
            this.groupBox1.Controls.Add(this.lbFriends);
            this.groupBox1.Location = new System.Drawing.Point(265, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(141, 215);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Friends";
            // 
            // lbFriends
            // 
            this.lbFriends.FormattingEnabled = true;
            this.lbFriends.Location = new System.Drawing.Point(6, 19);
            this.lbFriends.Name = "lbFriends";
            this.lbFriends.Size = new System.Drawing.Size(129, 186);
            this.lbFriends.TabIndex = 1;
            // 
            // lblTitleHomepage
            // 
            this.lblTitleHomepage.AutoSize = true;
            this.lblTitleHomepage.Location = new System.Drawing.Point(3, 0);
            this.lblTitleHomepage.Name = "lblTitleHomepage";
            this.lblTitleHomepage.Size = new System.Drawing.Size(89, 13);
            this.lblTitleHomepage.TabIndex = 0;
            this.lblTitleHomepage.Text = "lblTitleHomepage";
            // 
            // lblNoFriends
            // 
            this.lblNoFriends.AutoSize = true;
            this.lblNoFriends.Location = new System.Drawing.Point(39, 103);
            this.lblNoFriends.Name = "lblNoFriends";
            this.lblNoFriends.Size = new System.Drawing.Size(58, 13);
            this.lblNoFriends.TabIndex = 3;
            this.lblNoFriends.Text = "No Friends";
            // 
            // UserDialogue
            // 
            this.ClientSize = new System.Drawing.Size(446, 286);
            this.Controls.Add(this.gbMain);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "UserDialogue";
            this.Text = "User";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.gbMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private ISystem parent_;
        private PersonsDatabase pd_;
        public UserDialogue(ISystem t_parent, ref PersonsDatabase t_pd)
        {
            parent_ = t_parent;
            pd_ = t_pd;
            InitializeComponent();
            //ShowHomepage();
        }
        private void LoadUserContent()
        {
            Text = "USER: " + currentUser_.userinfo.UserName;
        }

        public List<Person> GenerateUniqueFriendsOfFriends()
        {
            const int START_SIZE = 10;
            //BloomFilter<Person> memo = new BloomFilter<Person>(START_SIZE);
            List<Person> unique = new List<Person>(pd_.UserCount);
            // logical memo table 
            bool[] memo = Enumerable.Repeat(false,pd_.UserCount).ToArray();
            void SumFriendCount(Person t_target)
            {
                foreach(Person p in t_target.friends_)
                {
                    if (memo[p.ID])
                        continue;
                    else
                    {
                        unique.Add(p);
                        memo[p.ID] = true;
                        SumFriendCount(p);
                    }
                }
            }
            memo[currentUser_.userinfo.ID] = true;
            SumFriendCount(currentUser_.userinfo);
            return unique;
        }

        public bool AssignUser(Person t_user)
        {
            if (t_user == null)
                return false;
            currentUser_ = new User(t_user);
            LoadUserContent();
            ShowHomepage();
            List<Person> baz = GenerateUniqueFriendsOfFriends();
            ShowDialog();
            return true;
        }
        public void DisposeOfUser()
        {
            currentUser_ = null;
            Hide();
        }

        public enum ExitStatus
        {
            logout,
        }

        private void HideAllPanels()
        {
        }

        private void ShowHomepage()
        {
            gbMain.Text = "Home";
            lblTitleHomepage.Text = string.Format("Welcome {0} to the HeadEssay!",currentUser_.userinfo.UserName);
            List<Person> friends;
            if ((friends = currentUser_.userinfo.GetFriends(0, 5)) == null)
            {
                lbFriends.Hide();
                lblNoFriends.Show();
            }
            else
            {
                lbFriends.Show();
                lblNoFriends.Hide();
                lbFriends.Items.Add(friends);
            }
            HideAllPanels();
        }

        private void changeInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void manageAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            parent_.Callback(ExitStatus.logout,this);
            Close();
        }

        private void pendingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void accountInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void homepageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowHomepage();
        }
    }
}
