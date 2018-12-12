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
        // DATA MEMBERS
        // FORM OBJECTS
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
        private Label lblFriendsWithSameInterestNoFriends;
        private ListBox lbFriendsWithSameInterest;
        private ToolStripMenuItem firendsToolStripMenuItem;
        // OTHER DATA MEMBERS
        private ISystem parent_;    // the parent object to send signals to
        private User currentUser_;  // the current logged in user 
        private PersonsDatabase pd_;    // a reference to the person database 
        public enum ExitStatus  // a list of posible exit statuses
        {
            logout,
        }
        private object exitStatus_ = ExitStatus.logout; // set the default exit status
        private List<Person>    // friends lists
            friendsOfFriendsList = null,
            friendsOfFriendsWithSameInterest = null,
            peopleSameCityAndInterest = null,
            peopleInSameCity = null;
        private GroupBox groupBox3;
        private Label lblPeopleInYourArea;
        private GroupBox groupBox4;
        private Label lblPeopleInYourAreaWithSimilarInterests;
        private ListBox lbPeopleInYourAreaWithSimilarInterests;
        private GroupBox groupBox5;
        private Label label1;
        private ListBox lbReceivedInvitations;
        private GroupBox groupBox6;
        private Label label2;
        private ListBox lbSentInvitations;
        private ListBox lbPeopleInYourArea;

        // CONSTRUCTOR
        // params:
        //  ISystem t_parent;   the parent object ( HeadEssay object )
        //  ref PersonsDatabase t_pd;   a reference to the current  working person database
        public UserDialogue(ISystem t_parent, ref PersonsDatabase t_pd)
        {
            parent_ = t_parent;
            pd_ = t_pd;
            InitializeComponent();
        }
        // METHOD MEMBERS
        // sets up form objects to display the currently logged in user's info 
        private void LoadUserContent()
        {
            Text = "USER: " + currentUser_.userinfo.UserName;
            lblTitleHomepage.Text = string.Format("Welcome {0} {1} to the HeadEssay!",currentUser_.userinfo.FirstName,currentUser_.userinfo.LastName);
            if (currentUser_.userinfo.friends_.Count > 0)
            {
                currentUser_.userinfo.friends_.ForEach(x => lbFriendsList.Items.Add(x.UserName));
                lbFriendsList.Show();
                lblFriendsListNoFriends.Hide();
            }
            else
            {
                lbFriendsList.Hide();
                lblFriendsListNoFriends.Show();
            }
            friendsOfFriendsList = GenerateUniqueFriendsOfFriends();
            friendsOfFriendsWithSameInterest = GenerateListOfFriendsOfFriendsWithInterest(friendsOfFriendsList);
            peopleInSameCity = GenerateListOfPersonsInTheSameCity();
            peopleSameCityAndInterest = GenerateFriendsOfFriendsThatShareTheSameCityAndInterest();
            if(friendsOfFriendsList.Count > 0)
            {
                friendsOfFriendsList.ForEach(x => lbFriendsOfFriendsList.Items.Add(x.UserName));
                lbFriendsOfFriendsList.Show();
                lblFriendsOfFriendsListNoFriends.Hide();
            }
            else
            {
                lbFriendsOfFriendsList.Hide();
               lblFriendsOfFriendsListNoFriends.Show();
            }
            if(friendsOfFriendsWithSameInterest.Count > 0)
            {
                friendsOfFriendsWithSameInterest.ForEach(x => lbFriendsWithSameInterest.Items.Add(x.UserName));
                lbFriendsWithSameInterest.Show();
                lblFriendsWithSameInterestNoFriends.Hide();
            }
            else
            {
                lbFriendsWithSameInterest.Hide();
                lblFriendsWithSameInterestNoFriends.Show();
            }
            if(peopleInSameCity.Count > 0)
            {
                peopleInSameCity.ForEach(x => lbPeopleInYourArea.Items.Add(x.UserName));
                lbPeopleInYourArea.Show();
                lblPeopleInYourArea.Hide();
            }
            else
            {
                lbPeopleInYourArea.Hide();
                lblPeopleInYourArea.Show();
            }
            if(peopleSameCityAndInterest.Count > 0)
            {
                peopleSameCityAndInterest.ForEach(x => lbPeopleInYourAreaWithSimilarInterests.Items.Add(x.UserName));
                lbPeopleInYourAreaWithSimilarInterests.Show();
                lblPeopleInYourAreaWithSimilarInterests.Hide();
            }
            else
            {
                lbPeopleInYourAreaWithSimilarInterests.Hide();
                lblPeopleInYourAreaWithSimilarInterests.Show();
            }
        }
        // Generates a set of friends; include friends of friends
        public List<Person> GenerateUniqueFriendsOfFriends()
        {   
            List<Person> unique = new List<Person>(pd_.UserCount);  // person list
            bool[] memo = Enumerable.Repeat(false,pd_.UserCount).ToArray(); // logical memo table
            memo[currentUser_.userinfo.ID] = true;  // add the current logged in user to the memo table
            Action<Person> MakeList = null;
            (MakeList = (Person t_target) =>
            {
                foreach (Person p in t_target.friends_)
                {
                    if (memo[p.ID]) // check if p has been DFSed already; skip p if true
                        continue;
                    else
                    {
                        unique.Add(p);  // add p to the return list
                        memo[p.ID] = true;  // tag p with true indicating that p has been DFSed
                        MakeList(p);    // recurse over p's friends list
                    }
                }
            })(currentUser_.userinfo);  // execute DFS, where the root node is the logged in user.
            return unique;  // return the return list
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
        private List<Person> GenerateFriendsOfFriendsThatShareTheSameCityAndInterest()
        {
            const int SIZE_LIMIT = 10;
            var resultList = new List<Person>(SIZE_LIMIT); // person list
            bool[] memo = Enumerable.Repeat(false,pd_.UserCount).ToArray(); // logical memo table 
            ulong[] reservedStaticIDs = GetFriendsStaticIDs();
            // initialize the city and interest tables
            pd_.ZeroOutCityTable();
            pd_.ZeroOutInterestTable();
            Func<Person, bool> Filter = x =>   // a function used to test Person p against a filter
            {
                if (x.staticID == currentUser_.userinfo.staticID)
                    return false;
                if (!pd_.CityTable[x.City])
                    return false;
                foreach(string s in x.interests_)
                    if (pd_.InterestTable[s])
                        return true;
                return false;
            };
            // map logic 1 to the city and interests tables
            // to indicate what strings (city & interests) will
            // pass the filter when DFSing
            pd_.CityTable[currentUser_.userinfo.City] = true;
            currentUser_.userinfo.interests_.ForEach( (Action<string>)(x => 
            {
                pd_.InterestTable[x] = true;
            }));
            // DFS over all friends in each friends list
            Action<Person> MakeList = null; 
            (MakeList = (Person t_target) =>
            {
                foreach (var p in t_target.friends_)    // search over all friends in t_target's friends list.
                {
                    if (resultList.Count == SIZE_LIMIT)  // exit if size limit has been reached
                        return;
                    if (memo[p.ID]) // check if p has been DFSed already; skip p if true
                        continue;
                    memo[p.ID] = true;  // tag p with true indicating that p has been DFSed
                    //if(!reservedStaticIDs.Contains(p.staticID)  // apply the search filters (city & interests)
                    //&& pd_.)
                    if(Filter(p))
                        resultList.Add(p);  // add p to the return list
                    MakeList(p);    // recurse over p's friends list
                }
            })(currentUser_.userinfo);  // execute DFS, where the root node is the logged in user.
            if (resultList.Count == SIZE_LIMIT)
                return resultList;  // return if size limit has been reached
            foreach(Person p in pd_.Data)
            {   // do a linear search against everyone if size limit has not been reached
                if (resultList.Count == SIZE_LIMIT) // break if size limit has been reached
                    return resultList;  // return the return list 
                if (memo[p.ID]) // skip p  if p is in the list already
                    continue;
                if (Filter(p))  // test p against the filter; add p to the list if it passes the filter
                    resultList.Add(p);
            }
            return resultList;  // return the return list 
        }
        // generate a list of up to 10 persons that are in the same city
        private List<Person> GenerateListOfPersonsInTheSameCity()
        {
            const int OUTPUT_SIZE = 10;
            var resultList = new List<Person>(OUTPUT_SIZE);
            pd_.ZeroOutCityTable();
            pd_.CityTable[currentUser_.userinfo.City] = true;
            Func<Person, bool> Filter = x => 
            {
                if (x.staticID == currentUser_.userinfo.staticID)
                    return false;
                if(!pd_.CityTable[x.City])
                    return false;
                if (currentUser_.userinfo.friends_.Contains(x))
                    return false;
                return true;
            };
            foreach(Person p in pd_.Data)
                if (resultList.Count == OUTPUT_SIZE)
                    return resultList;
                else if(Filter(p))
                    resultList.Add(p);
            return resultList;
        }
        // gets the static IDs of all friends in the logged in user
        private ulong[] GetFriendsStaticIDs()
        {   // aggregate all static IDs; return as an array of unsigned longs ( quad words c: )
            return currentUser_.userinfo.friends_.Select(x => x.staticID).ToArray();     
        }
        // load the next logged in user; display the form.
        public bool AssignUser(Person t_user)
        {
            if (t_user == null) // if the user is null; exit with error
                return false;
            currentUser_ = new User(t_user);    // set the current user
            LoadUserContent();  // load the user's content
            ShowHomepage(); // load the homepage
            ShowDialog();   // display the form
            return true;    // exit with no errors
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
            //List<Person> friends;
            HideAllPanels();
        }

        // FORM METHODS & EVENT HANDLERS 
        private void changeInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void manageAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {   // logout and close the form
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
        {   // show homepage 
            ShowHomepage();
        }

        private void lbFriends_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void UserDialogue_FormClosing(object sender, FormClosingEventArgs e)
        {   // signal the network that the user closed the form
            parent_.Callback(exitStatus_,this);
        }
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblPeopleInYourAreaWithSimilarInterests = new System.Windows.Forms.Label();
            this.lbPeopleInYourAreaWithSimilarInterests = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblPeopleInYourArea = new System.Windows.Forms.Label();
            this.lbPeopleInYourArea = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblFriendsWithSameInterestNoFriends = new System.Windows.Forms.Label();
            this.lbFriendsWithSameInterest = new System.Windows.Forms.ListBox();
            this.gbFriends = new System.Windows.Forms.GroupBox();
            this.lblFriendsListNoFriends = new System.Windows.Forms.Label();
            this.lbFriendsList = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblFriendsOfFriendsListNoFriends = new System.Windows.Forms.Label();
            this.lbFriendsOfFriendsList = new System.Windows.Forms.ListBox();
            this.lblTitleHomepage = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbReceivedInvitations = new System.Windows.Forms.ListBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbSentInvitations = new System.Windows.Forms.ListBox();
            this.menuStrip1.SuspendLayout();
            this.gbMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbFriends.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(901, 24);
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
            this.accountInfoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.accountInfoToolStripMenuItem.Text = "Account Info";
            this.accountInfoToolStripMenuItem.Click += new System.EventHandler(this.accountInfoToolStripMenuItem_Click);
            // 
            // firendsToolStripMenuItem
            // 
            this.firendsToolStripMenuItem.Name = "firendsToolStripMenuItem";
            this.firendsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.firendsToolStripMenuItem.Text = "Friends";
            // 
            // invitesToolStripMenuItem1
            // 
            this.invitesToolStripMenuItem1.Name = "invitesToolStripMenuItem1";
            this.invitesToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.invitesToolStripMenuItem1.Text = "Invites";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.searchToolStripMenuItem.Text = "Search";
            // 
            // homepageToolStripMenuItem
            // 
            this.homepageToolStripMenuItem.Name = "homepageToolStripMenuItem";
            this.homepageToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
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
            this.changeInfoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.changeInfoToolStripMenuItem.Text = "Change Info";
            this.changeInfoToolStripMenuItem.Click += new System.EventHandler(this.changeInfoToolStripMenuItem_Click);
            // 
            // manageAccountToolStripMenuItem
            // 
            this.manageAccountToolStripMenuItem.Name = "manageAccountToolStripMenuItem";
            this.manageAccountToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.manageAccountToolStripMenuItem.Text = "Manage Account";
            this.manageAccountToolStripMenuItem.Click += new System.EventHandler(this.manageAccountToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // gbMain
            // 
            this.gbMain.Controls.Add(this.panel1);
            this.gbMain.Location = new System.Drawing.Point(12, 27);
            this.gbMain.Name = "gbMain";
            this.gbMain.Size = new System.Drawing.Size(877, 555);
            this.gbMain.TabIndex = 1;
            this.gbMain.TabStop = false;
            this.gbMain.Text = "gbMain";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox6);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.gbFriends);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.lblTitleHomepage);
            this.panel1.Location = new System.Drawing.Point(7, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(870, 527);
            this.panel1.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblPeopleInYourAreaWithSimilarInterests);
            this.groupBox4.Controls.Add(this.lbPeopleInYourAreaWithSimilarInterests);
            this.groupBox4.Location = new System.Drawing.Point(223, 270);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(211, 242);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "People in your Area with Similar Interests";
            // 
            // lblPeopleInYourAreaWithSimilarInterests
            // 
            this.lblPeopleInYourAreaWithSimilarInterests.AutoSize = true;
            this.lblPeopleInYourAreaWithSimilarInterests.Location = new System.Drawing.Point(72, 122);
            this.lblPeopleInYourAreaWithSimilarInterests.Name = "lblPeopleInYourAreaWithSimilarInterests";
            this.lblPeopleInYourAreaWithSimilarInterests.Size = new System.Drawing.Size(57, 13);
            this.lblPeopleInYourAreaWithSimilarInterests.TabIndex = 3;
            this.lblPeopleInYourAreaWithSimilarInterests.Text = "No People";
            // 
            // lbPeopleInYourAreaWithSimilarInterests
            // 
            this.lbPeopleInYourAreaWithSimilarInterests.FormattingEnabled = true;
            this.lbPeopleInYourAreaWithSimilarInterests.Location = new System.Drawing.Point(6, 19);
            this.lbPeopleInYourAreaWithSimilarInterests.Name = "lbPeopleInYourAreaWithSimilarInterests";
            this.lbPeopleInYourAreaWithSimilarInterests.Size = new System.Drawing.Size(199, 212);
            this.lbPeopleInYourAreaWithSimilarInterests.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblPeopleInYourArea);
            this.groupBox3.Controls.Add(this.lbPeopleInYourArea);
            this.groupBox3.Location = new System.Drawing.Point(6, 270);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(211, 242);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Potential Friends in your Area";
            // 
            // lblPeopleInYourArea
            // 
            this.lblPeopleInYourArea.AutoSize = true;
            this.lblPeopleInYourArea.Location = new System.Drawing.Point(72, 122);
            this.lblPeopleInYourArea.Name = "lblPeopleInYourArea";
            this.lblPeopleInYourArea.Size = new System.Drawing.Size(57, 13);
            this.lblPeopleInYourArea.TabIndex = 3;
            this.lblPeopleInYourArea.Text = "No People";
            // 
            // lbPeopleInYourArea
            // 
            this.lbPeopleInYourArea.FormattingEnabled = true;
            this.lbPeopleInYourArea.Location = new System.Drawing.Point(6, 19);
            this.lbPeopleInYourArea.Name = "lbPeopleInYourArea";
            this.lbPeopleInYourArea.Size = new System.Drawing.Size(199, 212);
            this.lbPeopleInYourArea.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblFriendsWithSameInterestNoFriends);
            this.groupBox1.Controls.Add(this.lbFriendsWithSameInterest);
            this.groupBox1.Location = new System.Drawing.Point(6, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(211, 242);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Friends With Same Interest";
            // 
            // lblFriendsWithSameInterestNoFriends
            // 
            this.lblFriendsWithSameInterestNoFriends.AutoSize = true;
            this.lblFriendsWithSameInterestNoFriends.Location = new System.Drawing.Point(72, 122);
            this.lblFriendsWithSameInterestNoFriends.Name = "lblFriendsWithSameInterestNoFriends";
            this.lblFriendsWithSameInterestNoFriends.Size = new System.Drawing.Size(58, 13);
            this.lblFriendsWithSameInterestNoFriends.TabIndex = 3;
            this.lblFriendsWithSameInterestNoFriends.Text = "No Friends";
            // 
            // lbFriendsWithSameInterest
            // 
            this.lbFriendsWithSameInterest.FormattingEnabled = true;
            this.lbFriendsWithSameInterest.Location = new System.Drawing.Point(6, 19);
            this.lbFriendsWithSameInterest.Name = "lbFriendsWithSameInterest";
            this.lbFriendsWithSameInterest.Size = new System.Drawing.Size(199, 212);
            this.lbFriendsWithSameInterest.TabIndex = 1;
            // 
            // gbFriends
            // 
            this.gbFriends.Controls.Add(this.lblFriendsListNoFriends);
            this.gbFriends.Controls.Add(this.lbFriendsList);
            this.gbFriends.Location = new System.Drawing.Point(657, 22);
            this.gbFriends.Name = "gbFriends";
            this.gbFriends.Size = new System.Drawing.Size(200, 490);
            this.gbFriends.TabIndex = 5;
            this.gbFriends.TabStop = false;
            this.gbFriends.Text = "Friends List";
            // 
            // lblFriendsListNoFriends
            // 
            this.lblFriendsListNoFriends.AutoSize = true;
            this.lblFriendsListNoFriends.Location = new System.Drawing.Point(73, 238);
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
            this.lbFriendsList.Size = new System.Drawing.Size(188, 459);
            this.lbFriendsList.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblFriendsOfFriendsListNoFriends);
            this.groupBox2.Controls.Add(this.lbFriendsOfFriendsList);
            this.groupBox2.Location = new System.Drawing.Point(223, 22);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(211, 242);
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
            this.lbFriendsOfFriendsList.Size = new System.Drawing.Size(199, 212);
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
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.lbReceivedInvitations);
            this.groupBox5.Location = new System.Drawing.Point(440, 270);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(211, 242);
            this.groupBox5.TabIndex = 9;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Received Invitations";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(72, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "No Invitations";
            // 
            // lbReceivedInvitations
            // 
            this.lbReceivedInvitations.FormattingEnabled = true;
            this.lbReceivedInvitations.Location = new System.Drawing.Point(6, 19);
            this.lbReceivedInvitations.Name = "lbReceivedInvitations";
            this.lbReceivedInvitations.Size = new System.Drawing.Size(199, 212);
            this.lbReceivedInvitations.TabIndex = 1;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.lbSentInvitations);
            this.groupBox6.Location = new System.Drawing.Point(440, 22);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(211, 242);
            this.groupBox6.TabIndex = 8;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Sent Invitations";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(72, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "No Invitations";
            // 
            // lbSentInvitations
            // 
            this.lbSentInvitations.FormattingEnabled = true;
            this.lbSentInvitations.Location = new System.Drawing.Point(6, 19);
            this.lbSentInvitations.Name = "lbSentInvitations";
            this.lbSentInvitations.Size = new System.Drawing.Size(199, 212);
            this.lbSentInvitations.TabIndex = 1;
            // 
            // UserDialogue
            // 
            this.ClientSize = new System.Drawing.Size(901, 591);
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
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbFriends.ResumeLayout(false);
            this.gbFriends.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
