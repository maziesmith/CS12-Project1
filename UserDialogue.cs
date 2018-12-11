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
        private GroupBox groupBox2;
        private Label lblFriendsOfFriendsListNoFriends;
        private ListBox lbFriendsOfFriendsList;
        private GroupBox gbFriends;
        private ListBox lbFriendsList;
        private Label lblFriendsListNoFriends;
        private GroupBox groupBox1;
        private Label lbFriendsWithSameInterestNoFriends;
        private ListBox lbFriendsWithSameInterest;
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
            this.gbFriends = new System.Windows.Forms.GroupBox();
            this.lblFriendsListNoFriends = new System.Windows.Forms.Label();
            this.lbFriendsList = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblFriendsOfFriendsListNoFriends = new System.Windows.Forms.Label();
            this.lbFriendsOfFriendsList = new System.Windows.Forms.ListBox();
            this.lblTitleHomepage = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbFriendsWithSameInterestNoFriends = new System.Windows.Forms.Label();
            this.lbFriendsWithSameInterest = new System.Windows.Forms.ListBox();
            this.menuStrip1.SuspendLayout();
            this.gbMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbFriends.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.menuStrip1.Size = new System.Drawing.Size(756, 24);
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
            this.gbMain.Size = new System.Drawing.Size(732, 619);
            this.gbMain.TabIndex = 1;
            this.gbMain.TabStop = false;
            this.gbMain.Text = "gbMain";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.gbFriends);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.lblTitleHomepage);
            this.panel1.Location = new System.Drawing.Point(7, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(719, 593);
            this.panel1.TabIndex = 0;
            // 
            // gbFriends
            // 
            this.gbFriends.Controls.Add(this.lblFriendsListNoFriends);
            this.gbFriends.Controls.Add(this.lbFriendsList);
            this.gbFriends.Location = new System.Drawing.Point(418, 22);
            this.gbFriends.Name = "gbFriends";
            this.gbFriends.Size = new System.Drawing.Size(200, 242);
            this.gbFriends.TabIndex = 5;
            this.gbFriends.TabStop = false;
            this.gbFriends.Text = "Friends List";
            // 
            // lblFriendsListNoFriends
            // 
            this.lblFriendsListNoFriends.AutoSize = true;
            this.lblFriendsListNoFriends.Location = new System.Drawing.Point(72, 114);
            this.lblFriendsListNoFriends.Name = "lblFriendsListNoFriends";
            this.lblFriendsListNoFriends.Size = new System.Drawing.Size(58, 13);
            this.lblFriendsListNoFriends.TabIndex = 4;
            this.lblFriendsListNoFriends.Text = "No Friends";
            // 
            // lbFriendsList
            // 
            this.lbFriendsList.FormattingEnabled = true;
            this.lbFriendsList.Location = new System.Drawing.Point(6, 19);
            this.lbFriendsList.Name = "lbFriendsList";
            this.lbFriendsList.Size = new System.Drawing.Size(188, 212);
            this.lbFriendsList.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblFriendsOfFriendsListNoFriends);
            this.groupBox2.Controls.Add(this.lbFriendsOfFriendsList);
            this.groupBox2.Location = new System.Drawing.Point(212, 22);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 242);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Friends Of Friends";
            // 
            // lblFriendsOfFriendsListNoFriends
            // 
            this.lblFriendsOfFriendsListNoFriends.AutoSize = true;
            this.lblFriendsOfFriendsListNoFriends.Location = new System.Drawing.Point(72, 122);
            this.lblFriendsOfFriendsListNoFriends.Name = "lblFriendsOfFriendsListNoFriends";
            this.lblFriendsOfFriendsListNoFriends.Size = new System.Drawing.Size(58, 13);
            this.lblFriendsOfFriendsListNoFriends.TabIndex = 3;
            this.lblFriendsOfFriendsListNoFriends.Text = "No Friends";
            // 
            // lbFriendsOfFriendsList
            // 
            this.lbFriendsOfFriendsList.FormattingEnabled = true;
            this.lbFriendsOfFriendsList.Location = new System.Drawing.Point(6, 19);
            this.lbFriendsOfFriendsList.Name = "lbFriendsOfFriendsList";
            this.lbFriendsOfFriendsList.Size = new System.Drawing.Size(188, 212);
            this.lbFriendsOfFriendsList.TabIndex = 1;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbFriendsWithSameInterestNoFriends);
            this.groupBox1.Controls.Add(this.lbFriendsWithSameInterest);
            this.groupBox1.Location = new System.Drawing.Point(6, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 242);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Friends With Same Interest";
            // 
            // lbFriendsWithSameInterestNoFriends
            // 
            this.lbFriendsWithSameInterestNoFriends.AutoSize = true;
            this.lbFriendsWithSameInterestNoFriends.Location = new System.Drawing.Point(72, 122);
            this.lbFriendsWithSameInterestNoFriends.Name = "lbFriendsWithSameInterestNoFriends";
            this.lbFriendsWithSameInterestNoFriends.Size = new System.Drawing.Size(58, 13);
            this.lbFriendsWithSameInterestNoFriends.TabIndex = 3;
            this.lbFriendsWithSameInterestNoFriends.Text = "No Friends";
            // 
            // lbFriendsWithSameInterest
            // 
            this.lbFriendsWithSameInterest.FormattingEnabled = true;
            this.lbFriendsWithSameInterest.Location = new System.Drawing.Point(6, 19);
            this.lbFriendsWithSameInterest.Name = "lbFriendsWithSameInterest";
            this.lbFriendsWithSameInterest.Size = new System.Drawing.Size(188, 212);
            this.lbFriendsWithSameInterest.TabIndex = 1;
            // 
            // UserDialogue
            // 
            this.ClientSize = new System.Drawing.Size(756, 658);
            this.Controls.Add(this.gbMain);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "UserDialogue";
            this.Text = "User";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserDialogue_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.gbMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbFriends.ResumeLayout(false);
            this.gbFriends.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        }

        public enum ExitStatus
        {
            logout,
        }

        private object exitStatus_ = ExitStatus.logout;

        private List<Person> friendsOfFriendsList = null, friendsOfFriendsWithSameInterest = null;

        private void LoadUserContent()
        {
            Text = "USER: " + currentUser_.userinfo.UserName;
            int i = -1;
            foreach (Person p in currentUser_.userinfo.friends_)
            {
                if ((i++) == 5)
                        break;
                lbFriendsList.Items.Add(currentUser_.userinfo.friends_[i].UserName);
            }
            if (currentUser_.userinfo.friends_.Count < 1)
            {
                lbFriendsList.Hide();
                lbFriendsOfFriendsList.Hide();
                lblFriendsListNoFriends.Show();
                lblFriendsOfFriendsListNoFriends.Show();
                lblFriendsOfFriendsListNoFriends.Show();
            }
            else
            {
                lblFriendsListNoFriends.Hide();
                if ((friendsOfFriendsList = GenerateUniqueFriendsOfFriends()) == null)
                {
                    lbFriendsOfFriendsList.Hide();
                    lblFriendsOfFriendsListNoFriends.Show();
                }
                else
                {
                    if((friendsOfFriendsWithSameInterest = GenerateListOfFriendsOfFriendsWithInterest(friendsOfFriendsList)) == null)
                    {
                        lbFriendsWithSameInterest.Hide();
                        lbFriendsWithSameInterestNoFriends.Show();
                    }
                    else
                    {
                        lbFriendsWithSameInterest.Show();
                        lbFriendsWithSameInterestNoFriends.Hide();
                        friendsOfFriendsWithSameInterest.ForEach(x => lbFriendsWithSameInterest.Items.Add(x.UserName));
                    }
                    lbFriendsOfFriendsList.Show();
                    lblFriendsOfFriendsListNoFriends.Hide();
                    friendsOfFriendsList.ForEach(p => lbFriendsOfFriendsList.Items.Add(p.UserName));
                }
                GenerateFriendsOfFriendsThatShareTheSameCityAndInterest(new List<Person>());
            }
        }

        public List<Person> GenerateUniqueFriendsOfFriends()
        {
            // person list
            List<Person> unique = new List<Person>(pd_.UserCount);
            // logical memo table 
            bool[] memo = Enumerable.Repeat(false,pd_.UserCount).ToArray();
            void MakeList(Person t_target)
            {
                foreach(Person p in t_target.friends_)
                {
                    if (memo[p.ID]) // skip all friends in the memo table
                        continue;
                    else
                    {
                        unique.Add(p); // add person to list
                        memo[p.ID] = true; // update memo table
                        MakeList(p);    // recurse for all friends
                    }
                }
            }
            memo[currentUser_.userinfo.ID] = true;  // add the current logged in user to the memo table
            MakeList(currentUser_.userinfo);    // make the list
            return unique;
        }

        private List<Person> GenerateListOfFriendsOfFriendsWithInterest(List<Person> t_set)
        {
            var sameInterest = new List<string>(pd_.UserCount);
            Dictionary<string,List<Person>> testSet = t_set
                .Select(x => x.interests_)
                .SelectMany(y => y)
                .Distinct()
                .ToDictionary( z => z, z => new List<Person>(t_set.Count) );
            t_set.ForEach(
            p =>
            {
                p.interests_.ForEach(i =>
                {
                    testSet[i].Add(p);
                });
            });
            int max = 0;
            List<Person> output = null;
            foreach(List<Person> p in testSet.Values)
            {
                if (p.Count > max)
                {
                    output = p;
                    max = p.Count;
                }
            }
            if (output.Count < t_set.Count)
                return null;
            return output;
        }

        // a friends of friends that are not in current user's friends list 
        // and that share the same intrest and city
        private List<Person> GenerateFriendsOfFriendsThatShareTheSameCityAndInterest(List<Person> t_test)
        {
            // person list
            List<Person> resultList = new List<Person>(pd_.UserCount);
            // logical memo table 
            bool[] memo = Enumerable.Repeat(false,pd_.UserCount).ToArray();
            ulong[] reservedStaticIDs = currentUser_.userinfo.friends_.Select(x => x.staticID).ToArray();
            pd_.cityTable.Values.Select( x => x = false );
            pd_.cityTable[currentUser_.userinfo.City] = true;
            Action<Person> MakeList = null; 
            (MakeList = (Person t_target) =>
            {
                foreach (var p in t_target.friends_)
                {
                    if (memo[p.ID])
                        continue;
                    memo[p.ID] = true;
                    if(!reservedStaticIDs.Contains(p.staticID)
                    && pd_.cityTable[p.City])
                        resultList.Add(p);
                    MakeList(p);
                }
            })(currentUser_.userinfo);
            return resultList;
        }

        public bool AssignUser(Person t_user)
        {
            if (t_user == null)
                return false;
            currentUser_ = new User(t_user);

            LoadUserContent();
            ShowHomepage();
            lbFriendsOfFriendsList.Items.Add("");
            ShowDialog();
            return true;
        }

        public void DisposeOfUser()
        {
            currentUser_ = null;
            Hide();
        }


        private void HideAllPanels()
        {
        }

        private void ShowHomepage()
        {
            gbMain.Text = "Home";
            lblTitleHomepage.Text = string.Format("Welcome {0} to the HeadEssay!",currentUser_.userinfo.UserName);
            //List<Person> friends;
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

        private void lbFriends_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void UserDialogue_FormClosing(object sender, FormClosingEventArgs e)
        {
            parent_.Callback(exitStatus_,this);
        }
    }
}
