using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS12_Project_1
{
    public class UserDialogue : Form
    {
        // DATA MEMBERS
        // FORM OBJECTS
        private ToolStripMenuItem changeAccountInfoToolStripMenuItem;
        private ToolStripMenuItem newInvitationToolStripMenuItem_;
        private ToolStripMenuItem logoutToolStripMenuItem1_;
        private MenuStrip menuStrip1_;
        private GroupBox gbMain_;
        private GroupBox gbFriendsOfFriends_;
        private GroupBox gbFriends_;
        private GroupBox groupBox1_;
        private GroupBox groupBox3_;
        private GroupBox groupBox4_;
        private GroupBox groupBox5_;
        private GroupBox groupBox6_;
        private ListBox lbFriendsOfFriendsList_;
        private ListBox lbFriendsList_;
        private ListBox lbFriendsWithSameInterest_;
        private ListBox lbPeopleInYourAreaWithSimilarInterests_;
        private ListBox lbReceivedInvitations_;
        private ListBox lbSentInvitations_;
        private ListBox lbPeopleInYourArea_;
        private Button btnViewMore;
        private Button btnViewAllFriends;
        private Label lblFriendsListNoFriends_;
        private Label lblTitleHomepage_;
        private Label lblFriendsOfFriendsListNoFriends_;
        private Label lblFriendsWithSameInterestNoFriends_;
        private Label lblPeopleInYourArea_;
        private Label lblPeopleInYourAreaWithSimilarInterests_;
        private Label lblNoReceivedInvitations_;
        private Label lblNoSentInvitations_;
        private Panel pnlMain_;
        // OTHER DATA MEMBERS
        private InvitationInspectorDialogue iid_;   // a bunch of user interfaces
        private ChangeInterestsDialogue cid_;
        private FriendInspectorDialogue fid_;
        private NewInvitationDialogue nid_;
        private InvitationSystem is_;
        private PersonsDatabase pd_;    // a reference to the person database 
        private List<Person>    // friends lists
            friendsOfFriendsList = null,
            friendsOfFriendsWithSameInterest = null,
            peopleSameCityAndInterest = null,
            peopleInSameCity = null;
        private int viewFriendsListLoopCount = 0;
        private ISystem parent_;    // the parent object to send signals to
        private Person currentUser_;  // the current logged in user 
        public enum ExitStatus  // a list of posible exit statuses
        {
            logout,
        }
        private object exitStatus_ = ExitStatus.logout; // set the default exit status
        // CONSTRUCTOR
        // params:
        //  ISystem t_parent;   the parent object ( HeadEssay object )
        //  ref PersonsDatabase t_pd;   a reference to the current  working person database
        public UserDialogue(ISystem t_parent, ref PersonsDatabase t_pd)
        {   // setup all data members
            pd_ = t_pd;
            parent_ = t_parent;
            iid_ = new InvitationInspectorDialogue(this);
            fid_ = new FriendInspectorDialogue(this);
            is_ = new InvitationSystem(this, pd_);
            nid_ = new NewInvitationDialogue(this, is_, pd_);
            cid_ = new ChangeInterestsDialogue(this);
            InitializeComponent();
        }
        // ensure that the user interfaces are deleted
        ~UserDialogue()
        {   // delete all forms
            iid_.Dispose();
            fid_.Dispose();
            nid_.Dispose();
            cid_.Dispose();
        }
        // METHOD MEMBERS
        // get the current user
        public Person CurrentUser
        {
            get => currentUser_;
        }
        // update the friends of friends list
        public void UpdateFriendsOfFriendsList()
        {   // make the list
            friendsOfFriendsList = GenerateUniqueFriendsOfFriends();
            if(friendsOfFriendsList != null // null check 
            && friendsOfFriendsList.Count > 0)
            {   // add friends to the list
                lbFriendsOfFriendsList_.Items.Clear();
                friendsOfFriendsList.ForEach(x => lbFriendsOfFriendsList_.Items.Add(x.UserName));
                lbFriendsOfFriendsList_.Show();
                lblFriendsOfFriendsListNoFriends_.Hide();
            }
            else
            {   // hide the list if there are no friends
                lbFriendsOfFriendsList_.Hide();
                lblFriendsOfFriendsListNoFriends_.Show();
            }
        }
        // update-friends-of-friends-with-same-interest list
        public void UpdateFriendsOfFriendsWithSameInterest()
        {   // make the list
            friendsOfFriendsWithSameInterest = GenerateListOfFriendsOfFriendsWithInterest(friendsOfFriendsList);
            if(friendsOfFriendsWithSameInterest != null // null check
            && friendsOfFriendsWithSameInterest.Count > 0)
            {   // add friends to the list
                lbFriendsWithSameInterest_.Items.Clear();
                friendsOfFriendsWithSameInterest.ForEach(x => lbFriendsWithSameInterest_.Items.Add(x.UserName));
                lbFriendsWithSameInterest_.Show();
                lblFriendsWithSameInterestNoFriends_.Hide();
            }
            else
            {   // hide the list if there are no friends
                lbFriendsWithSameInterest_.Hide();
                lblFriendsWithSameInterestNoFriends_.Show();
            }
        }
        // update people in the same city and have the same interests list
        public void UpdatePeopleSameCityAndInterest()
        {   // make the list
            peopleInSameCity = GenerateListOfPersonsInTheSameCity();
            if(peopleInSameCity != null // null check
            && peopleInSameCity.Count > 0)
            {   // add friends to the list
                lbPeopleInYourArea_.Items.Clear();
                peopleInSameCity.ForEach(x => lbPeopleInYourArea_.Items.Add(x.UserName));
                lbPeopleInYourArea_.Show();
                lblPeopleInYourArea_.Hide();
            }
            else
            {   // hide the list if there are no friends
                lbPeopleInYourArea_.Hide();
                lblPeopleInYourArea_.Show();
            }
        }
        // update people in the same city list
        public void UpdatePeopleInSameCity()
        {   // make the list
            peopleSameCityAndInterest = GenerateFriendsOfFriendsThatShareTheSameCityAndInterest();
            if(peopleSameCityAndInterest != null    // null check 
            && peopleSameCityAndInterest.Count > 0)
            {   // add  people to the list
                lbPeopleInYourAreaWithSimilarInterests_.Items.Clear();
                peopleSameCityAndInterest.ForEach(x => lbPeopleInYourAreaWithSimilarInterests_.Items.Add(x.UserName));
                lbPeopleInYourAreaWithSimilarInterests_.Show();
                lblPeopleInYourAreaWithSimilarInterests_.Hide();
            }
            else
            {   // hide if there are no people
                lbPeopleInYourAreaWithSimilarInterests_.Hide();
                lblPeopleInYourAreaWithSimilarInterests_.Show();
            }
        }
        // update pending invitations list
        public void UpdatePendingInvitations()
        {   // null check
            if(currentUser_.PendingInvitations != null
            && currentUser_.PendingInvitations.Count > 0)
            {   // add invitations to the list
                lbReceivedInvitations_.Items.Clear();
                currentUser_.PendingInvitations.ForEach(x => lbReceivedInvitations_.Items.Add(x.AuthorUsername));
                lbReceivedInvitations_.Show();
                lblNoReceivedInvitations_.Hide();
            }
            else
            {   // hide if there are no invitations
                lbReceivedInvitations_.Hide();
                lblNoReceivedInvitations_.Show();
            }
        }
        public void UpdateSentInvitations()
        {   // null check
            if(currentUser_.SentInvitations != null
            && currentUser_.SentInvitations.Count > 0)
            {   // add invitations to the list
                lbSentInvitations_.Items.Clear();
                currentUser_.SentInvitations.ForEach(x => lbSentInvitations_.Items.Add(x.AuthorUsername));
                lbSentInvitations_.Show();
                lblNoSentInvitations_.Hide();
            }
            else
            {   // hide if there are no invitations
                lbSentInvitations_.Hide();
                lblNoSentInvitations_.Show();
            }
        }
        // view friends in friends list, 5 friends at a time
        private void ViewFriendsList()
        {
            const int PREVIEW_AMT = 5;
            int limit = viewFriendsListLoopCount + PREVIEW_AMT; // set the loop limit
            for(; viewFriendsListLoopCount < currentUser_.Friends.Count; viewFriendsListLoopCount++)
            {
                if (viewFriendsListLoopCount == limit)  // test for the loop exit condition
                    return; // exit
                lbFriendsList_.Items.Add(currentUser_.Friends[viewFriendsListLoopCount].UserName);  // add item to list
                viewFriendsListLoopCount++; // inc the loop counter
            }
        }
        // view all friends
        private void ViewAllFriends()
        {   // clear lbFriendsList_
            lbFriendsList_.Items.Clear();
            currentUser_.Friends.ForEach(x => lbFriendsList_.Items.Add(x.UserName));    // populate lbFriendsList_ with all friends
        }
        // update friends list
        public void UpdateFriendsList()
        {   // do a null check
            if(currentUser_.Friends != null
            && currentUser_.Friends.Count > 0)
            {   // update the friends list
                lbFriendsList_.Items.Clear();
                viewFriendsListLoopCount = 0;
                ViewFriendsList();
                lbFriendsList_.Show();
                lblFriendsListNoFriends_.Hide();
            }
            else
            {   // hide  if there are no friends
                lbFriendsList_.Hide();
                lblFriendsListNoFriends_.Show();
            }
        }
        // sets up form objects to display the currently logged in user's info 
        private void LoadUserContent()
        {   // set the title
            Text = "USER: " + currentUser_.UserName;
            lblTitleHomepage_.Text = string.Format("Welcome {0} {1} to the HeadEssay!",currentUser_.FirstName,currentUser_.LastName);
            // welcom the user
            UpdateAllLists();   // update all lists
        }
        // update all lists
        public void UpdateAllLists()
        {   // clear all forms;
            UpdateFriendsList();
            UpdateFriendsOfFriendsList();
            UpdateFriendsOfFriendsWithSameInterest();
            UpdatePeopleSameCityAndInterest();
            UpdatePeopleInSameCity();
            UpdatePendingInvitations();
            UpdateSentInvitations();
        }
        // Generates a set of friends; include friends of friends
        public List<Person> GenerateUniqueFriendsOfFriends()
        {   
            List<Person> unique = new List<Person>(pd_.UserCount);  // person list
            bool[] memo = Enumerable.Repeat(false,pd_.UserCount).ToArray(); // logical memo table
            memo[currentUser_.ID] = true;  // add the current logged in user to the memo table
            Action<Person> MakeList = null;
            (MakeList = (Person t_target) =>
            {
                foreach (Person p in t_target.Friends)
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
            })(currentUser_);  // execute DFS, where the root node is the logged in user.
            return unique;  // return the return list
        }
        // Generates a  list of friends of friends with the same interest
        private List<Person> GenerateListOfFriendsOfFriendsWithInterest(List<Person> t_set)
        {   // a dictionary to map interests to a list of person objects
            Dictionary<string,List<Person>> testSet = t_set
                .Select(x => x.Interests)
                .SelectMany(y => y)
                .Distinct()
                .ToDictionary( z => z, z => new List<Person>(t_set.Count) );
            t_set.ForEach(
            p =>
            {
                p.Interests.ForEach(i =>
                {
                    testSet[i].Add(p);
                });
            }); // populate the dictionary
            int max = 0;    // max size of each list in the dictionary
            List<Person> output = null; // the ret value
            foreach(List<Person> p in testSet.Values)
            {
                if(p.Count > max)   // pick the list with the largest size
                {
                    output = p;
                    max = p.Count;
                }
            }
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
                if (x.StaticID == currentUser_.StaticID)    // test static ids
                    return false;   // fail
                if (!pd_.CityTable[x.City]) //  test using the city table
                    return false;   // fail
                foreach(string s in x.Interests)    // test  using interests table 
                    if (pd_.InterestTable[s])
                        return true;    // fail
                return false;   // success
            };
            // map logic 1 to the city and interests tables
            // to indicate what strings (city & interests) will
            // pass the filter when DFSing
            pd_.CityTable[currentUser_.City] = true;
            currentUser_.Interests.ForEach(x => 
            {
                pd_.InterestTable[x] = true;
            });
            // DFS over all friends in each friends list
            Action<Person> MakeList = null; 
            (MakeList = (Person t_target) =>
            {
                foreach (var p in t_target.Friends)    // search over all friends in t_target's friends list.
                {
                    if (resultList.Count == SIZE_LIMIT)  // exit if size limit has been reached
                        return;
                    if (memo[p.ID]) // check if p has been DFSed already; skip p if true
                        continue;
                    memo[p.ID] = true;  // tag p with true indicating that p has been DFSed
                    //if(!reservedStaticIDs.Contains(p.StaticID)  // apply the search filters (city & interests)
                    //&& pd_.)
                    if(Filter(p))
                        resultList.Add(p);  // add p to the return list
                    MakeList(p);    // recurse over p's friends list
                }
            })(currentUser_);  // execute DFS, where the root node is the logged in user.
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
            const int OUTPUT_SIZE = 10; // pre allocated size for list
            var resultList = new List<Person>(OUTPUT_SIZE);
            pd_.ZeroOutCityTable(); // zero the city table
            pd_.CityTable[currentUser_.City] = true;    // set the inital state of the dictionary
            Func<Person, bool> Filter = x => 
            {
                if (x.StaticID == currentUser_.StaticID) // test using static id
                    return false;   // fail
                if(!pd_.CityTable[x.City]) 
                    return false;   // fail
                if (currentUser_.Friends.Contains(x))  // test agains  friends 
                    return false;   // fail
                return true;    // success
            };
            foreach(Person p in pd_.Data)   // do a linear search to obtain leftwover elements
                if (resultList.Count == OUTPUT_SIZE)
                    return resultList;
                else if(Filter(p))
                    resultList.Add(p);
            return resultList;
        }
        // gets the static IDs of all friends in the logged in user
        private ulong[] GetFriendsStaticIDs()
        {   // aggregate all static IDs; return as an array of unsigned longs ( quad words c: )
            return currentUser_.Friends.Select(x => x.StaticID).ToArray();     
        }
        // load the next logged in user; display the form.
        public bool AssignUser(in Person t_user)
        {
            if (t_user == null) // if the user is null; exit with error
                return false;
            currentUser_ = t_user;    // set the current user
            is_.AddInvitation(pd_.Invitations.ToArray());
            LoadUserContent();  // load the user's content
            return true;    // exit with no errors
        }
        // hide all forms
        private void HideAllForms()
        {
            iid_.Hide();
            cid_.Hide();
            fid_.Hide();
            nid_.Hide();
        }
        // FORM METHODS & EVENT HANDLERS 
        // when the logout button is pressed
        private void logoutToolStripMenuItem1_Click(object sender, EventArgs e)
        {   // delete this form
            HideAllForms();
            Hide();
            parent_.Callback(ExitStatus.logout, this);
        }
        // when the newInvitation button is pressed
        private void newInvitationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nid_.Initialize();  // Initialize the New Invitation Tool
            nid_.Show(currentUser_);   // display the New Invitation Tool
        }
        // when a form object requests to show the Friend Inspector Dialogue
        public void LoadFriendInspectorDialogue(string t_username)  
        {
            Person next = pd_.BloomFilterSearch(t_username);    // look for person using t_username
            if (next == null)   // if the search failed, exit
                return;
            fid_.Show(next);    // show the Friend Inspector Dialogue
        }
        // when a form object requests to show the Invitation Inspector Dialogue
        public void LoadInvitationInspectorDialogue(Invitation t_next, Person.InvitationContext t_context)
        {
            if(t_next != null)  // if t_next not null, show the Invitation Inspector Dialogue
                iid_.Show(t_next, t_context);
        }
        // when lbFriendsList_ is double clicked
        private void lbFriendsList__DoubleClick(object sender, EventArgs e)
        {
            if (lbFriendsList_.SelectedIndex == -1)
                return;
            LoadFriendInspectorDialogue(lbFriendsList_.Items[lbFriendsList_.SelectedIndex] as string);
        }
        // when lbSentInvitations_ is double clicked
        private void lbSentInvitations__DoubleClick(object sender, EventArgs e)
        {
            if (lbSentInvitations_.SelectedIndex == -1)    // test if user made a selection
                return; // fail
            string invitationName = lbSentInvitations_.Items[lbSentInvitations_.SelectedIndex] as string;   // get the string from the list box
            if (invitationName == null) // null check
                return; // fail
            ulong authorStaticID = ((Func<ulong>)(() =>
            {
                Person test = pd_.BloomFilterSearch(invitationName);
                return test == null ? 0 : test.StaticID;
            }))();
            if (authorStaticID == 0)
                return; // fail
            Invitation invitation = currentUser_.SentInvitations.FirstOrDefault(x => x.AuthorStaticID == authorStaticID);
            LoadInvitationInspectorDialogue(invitation, Person.InvitationContext.sender);
        }
        // when lbFriendsOfFriendsList_ is double clicked
        private void lbFriendsOfFriendsList__DoubleClick(object sender, EventArgs e)
        {
            if (lbFriendsOfFriendsList_.SelectedIndex == -1)    // test if user made a selection
                return; // fail
            LoadFriendInspectorDialogue(lbFriendsOfFriendsList_.Items[lbFriendsOfFriendsList_.SelectedIndex] as string);    // load and show the friend inspector
        }
        // when lbFriendsWithSameInterest_ is double clicked
        private void lbFriendsWithSameInterest__DoubleClick(object sender, EventArgs e)
        {
            if (lbFriendsWithSameInterest_.SelectedIndex == -1)    // test if user made a selection
                return; // fail
            LoadFriendInspectorDialogue(lbFriendsWithSameInterest_.Items[lbFriendsWithSameInterest_.SelectedIndex] as string);    // load and show the friend inspector
        }
        // when lbPeopleInYourArea_ is double clicked
        private void lbPeopleInYourArea__DoubleClick(object sender, EventArgs e)
        {
            if (lbPeopleInYourArea_.SelectedIndex == -1)    // test if user made a selection
                return; // fail
            LoadFriendInspectorDialogue(lbPeopleInYourArea_.Items[lbPeopleInYourArea_.SelectedIndex] as string);    // load and show the friend inspector
        }
        // when lbPeopleInYourAreaWithSimilarInterests_ is double clicked
        private void lbPeopleInYourAreaWithSimilarInterests__DoubleClick(object sender, EventArgs e)
        {
            if (lbPeopleInYourAreaWithSimilarInterests_.SelectedIndex == -1)    // test if user made a selection
                return; // fail
            LoadFriendInspectorDialogue(lbPeopleInYourAreaWithSimilarInterests_.Items[lbPeopleInYourAreaWithSimilarInterests_.SelectedIndex] as string);    // load and show the friend inspector
        }
        // when changeAccountInfoToolStripMenuItem_ is clicked
        private void changeAccountInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {   // show changeAccountInfo
            cid_.Init();    // init  changeAccountInfo
            cid_.Show();    // show the form
        }
        // when btnViewMore is clicked
        private void btnViewMore_Click(object sender, EventArgs e)
        {   // view up to 5 more friends 
            ViewFriendsList();  
        }
        // when btnViewAllFriends_ is clicked
        private void btnViewAllFriends_Click(object sender, EventArgs e)
        {   // view all friends
            ViewAllFriends();
        }
        // when lbReceivedInvitations_ is double clicked
        private void lbReceivedInvitations__DoubleClick(object sender, EventArgs e)
        {
            if (lbReceivedInvitations_.SelectedIndex == -1)    // test if user made a selection
                return; // fail
            string invitationName = lbReceivedInvitations_.Items[lbReceivedInvitations_.SelectedIndex] as string;   // get the string from the list box
            if (invitationName == null) // null check
                return; // fail
            ulong authorStaticID = ((Func<ulong>)(() =>
            {
                Person test = pd_.BloomFilterSearch(invitationName);
                return test == null ? 0 : test.StaticID;
            }))();
            if (authorStaticID == 0)
                return; // fail
            Invitation invitation = currentUser_.PendingInvitations.FirstOrDefault(x => x.AuthorStaticID == authorStaticID);
            LoadInvitationInspectorDialogue(invitation, Person.InvitationContext.recipient);
        }
        // when this form is about to close 
        private void UserDialogue_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;    // cancel the close request
                Hide(); // hide the form
            }
            parent_.Callback(exitStatus_,this); // signal the network that the user closed the form
        }
        // initialize the form
        private void InitializeComponent()
        {
            this.menuStrip1_ = new System.Windows.Forms.MenuStrip();
            this.newInvitationToolStripMenuItem_ = new System.Windows.Forms.ToolStripMenuItem();
            this.changeAccountInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem1_ = new System.Windows.Forms.ToolStripMenuItem();
            this.gbMain_ = new System.Windows.Forms.GroupBox();
            this.pnlMain_ = new System.Windows.Forms.Panel();
            this.groupBox5_ = new System.Windows.Forms.GroupBox();
            this.lblNoReceivedInvitations_ = new System.Windows.Forms.Label();
            this.lbReceivedInvitations_ = new System.Windows.Forms.ListBox();
            this.groupBox4_ = new System.Windows.Forms.GroupBox();
            this.lblPeopleInYourAreaWithSimilarInterests_ = new System.Windows.Forms.Label();
            this.lbPeopleInYourAreaWithSimilarInterests_ = new System.Windows.Forms.ListBox();
            this.groupBox6_ = new System.Windows.Forms.GroupBox();
            this.lblNoSentInvitations_ = new System.Windows.Forms.Label();
            this.lbSentInvitations_ = new System.Windows.Forms.ListBox();
            this.groupBox3_ = new System.Windows.Forms.GroupBox();
            this.lblPeopleInYourArea_ = new System.Windows.Forms.Label();
            this.lbPeopleInYourArea_ = new System.Windows.Forms.ListBox();
            this.groupBox1_ = new System.Windows.Forms.GroupBox();
            this.lblFriendsWithSameInterestNoFriends_ = new System.Windows.Forms.Label();
            this.lbFriendsWithSameInterest_ = new System.Windows.Forms.ListBox();
            this.gbFriends_ = new System.Windows.Forms.GroupBox();
            this.lblFriendsListNoFriends_ = new System.Windows.Forms.Label();
            this.lbFriendsList_ = new System.Windows.Forms.ListBox();
            this.gbFriendsOfFriends_ = new System.Windows.Forms.GroupBox();
            this.lblFriendsOfFriendsListNoFriends_ = new System.Windows.Forms.Label();
            this.lbFriendsOfFriendsList_ = new System.Windows.Forms.ListBox();
            this.lblTitleHomepage_ = new System.Windows.Forms.Label();
            this.btnViewMore = new System.Windows.Forms.Button();
            this.btnViewAllFriends = new System.Windows.Forms.Button();
            this.menuStrip1_.SuspendLayout();
            this.gbMain_.SuspendLayout();
            this.pnlMain_.SuspendLayout();
            this.groupBox5_.SuspendLayout();
            this.groupBox4_.SuspendLayout();
            this.groupBox6_.SuspendLayout();
            this.groupBox3_.SuspendLayout();
            this.groupBox1_.SuspendLayout();
            this.gbFriends_.SuspendLayout();
            this.gbFriendsOfFriends_.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1_
            // 
            this.menuStrip1_.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newInvitationToolStripMenuItem_,
            this.changeAccountInfoToolStripMenuItem,
            this.logoutToolStripMenuItem1_});
            this.menuStrip1_.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1_.Name = "menuStrip1_";
            this.menuStrip1_.Size = new System.Drawing.Size(901, 24);
            this.menuStrip1_.TabIndex = 0;
            this.menuStrip1_.Text = "menuStrip1";
            // 
            // newInvitationToolStripMenuItem_
            // 
            this.newInvitationToolStripMenuItem_.Name = "newInvitationToolStripMenuItem_";
            this.newInvitationToolStripMenuItem_.Size = new System.Drawing.Size(96, 20);
            this.newInvitationToolStripMenuItem_.Text = "New Invitation";
            this.newInvitationToolStripMenuItem_.Click += new System.EventHandler(this.newInvitationToolStripMenuItem_Click);
            // 
            // changeAccountInfoToolStripMenuItem
            // 
            this.changeAccountInfoToolStripMenuItem.Name = "changeAccountInfoToolStripMenuItem";
            this.changeAccountInfoToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.changeAccountInfoToolStripMenuItem.Text = "Interests";
            this.changeAccountInfoToolStripMenuItem.Click += new System.EventHandler(this.changeAccountInfoToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem1_
            // 
            this.logoutToolStripMenuItem1_.Name = "logoutToolStripMenuItem1_";
            this.logoutToolStripMenuItem1_.Size = new System.Drawing.Size(57, 20);
            this.logoutToolStripMenuItem1_.Text = "Logout";
            this.logoutToolStripMenuItem1_.Click += new System.EventHandler(this.logoutToolStripMenuItem1_Click);
            // 
            // gbMain_
            // 
            this.gbMain_.Controls.Add(this.pnlMain_);
            this.gbMain_.Location = new System.Drawing.Point(12, 27);
            this.gbMain_.Name = "gbMain_";
            this.gbMain_.Size = new System.Drawing.Size(877, 551);
            this.gbMain_.TabIndex = 1;
            this.gbMain_.TabStop = false;
            this.gbMain_.Text = "HeadEssay";
            // 
            // pnlMain_
            // 
            this.pnlMain_.Controls.Add(this.groupBox5_);
            this.pnlMain_.Controls.Add(this.groupBox4_);
            this.pnlMain_.Controls.Add(this.groupBox6_);
            this.pnlMain_.Controls.Add(this.groupBox3_);
            this.pnlMain_.Controls.Add(this.groupBox1_);
            this.pnlMain_.Controls.Add(this.gbFriends_);
            this.pnlMain_.Controls.Add(this.gbFriendsOfFriends_);
            this.pnlMain_.Controls.Add(this.lblTitleHomepage_);
            this.pnlMain_.Location = new System.Drawing.Point(7, 20);
            this.pnlMain_.Name = "pnlMain_";
            this.pnlMain_.Size = new System.Drawing.Size(870, 527);
            this.pnlMain_.TabIndex = 0;
            // 
            // groupBox5_
            // 
            this.groupBox5_.Controls.Add(this.lblNoReceivedInvitations_);
            this.groupBox5_.Controls.Add(this.lbReceivedInvitations_);
            this.groupBox5_.Location = new System.Drawing.Point(440, 270);
            this.groupBox5_.Name = "groupBox5_";
            this.groupBox5_.Size = new System.Drawing.Size(211, 242);
            this.groupBox5_.TabIndex = 9;
            this.groupBox5_.TabStop = false;
            this.groupBox5_.Text = "Received Invitations";
            // 
            // lblNoReceivedInvitations_
            // 
            this.lblNoReceivedInvitations_.AutoSize = true;
            this.lblNoReceivedInvitations_.Location = new System.Drawing.Point(72, 122);
            this.lblNoReceivedInvitations_.Name = "lblNoReceivedInvitations_";
            this.lblNoReceivedInvitations_.Size = new System.Drawing.Size(72, 13);
            this.lblNoReceivedInvitations_.TabIndex = 3;
            this.lblNoReceivedInvitations_.Text = "No Invitations";
            // 
            // lbReceivedInvitations_
            // 
            this.lbReceivedInvitations_.FormattingEnabled = true;
            this.lbReceivedInvitations_.Location = new System.Drawing.Point(6, 19);
            this.lbReceivedInvitations_.Name = "lbReceivedInvitations_";
            this.lbReceivedInvitations_.Size = new System.Drawing.Size(199, 212);
            this.lbReceivedInvitations_.TabIndex = 1;
            this.lbReceivedInvitations_.DoubleClick += new System.EventHandler(this.lbReceivedInvitations__DoubleClick);
            // 
            // groupBox4_
            // 
            this.groupBox4_.Controls.Add(this.lblPeopleInYourAreaWithSimilarInterests_);
            this.groupBox4_.Controls.Add(this.lbPeopleInYourAreaWithSimilarInterests_);
            this.groupBox4_.Location = new System.Drawing.Point(223, 270);
            this.groupBox4_.Name = "groupBox4_";
            this.groupBox4_.Size = new System.Drawing.Size(211, 242);
            this.groupBox4_.TabIndex = 7;
            this.groupBox4_.TabStop = false;
            this.groupBox4_.Text = "People in your Area with Similar Interests";
            // 
            // lblPeopleInYourAreaWithSimilarInterests_
            // 
            this.lblPeopleInYourAreaWithSimilarInterests_.AutoSize = true;
            this.lblPeopleInYourAreaWithSimilarInterests_.Location = new System.Drawing.Point(72, 122);
            this.lblPeopleInYourAreaWithSimilarInterests_.Name = "lblPeopleInYourAreaWithSimilarInterests_";
            this.lblPeopleInYourAreaWithSimilarInterests_.Size = new System.Drawing.Size(57, 13);
            this.lblPeopleInYourAreaWithSimilarInterests_.TabIndex = 3;
            this.lblPeopleInYourAreaWithSimilarInterests_.Text = "No People";
            // 
            // lbPeopleInYourAreaWithSimilarInterests_
            // 
            this.lbPeopleInYourAreaWithSimilarInterests_.FormattingEnabled = true;
            this.lbPeopleInYourAreaWithSimilarInterests_.Location = new System.Drawing.Point(6, 19);
            this.lbPeopleInYourAreaWithSimilarInterests_.Name = "lbPeopleInYourAreaWithSimilarInterests_";
            this.lbPeopleInYourAreaWithSimilarInterests_.Size = new System.Drawing.Size(199, 212);
            this.lbPeopleInYourAreaWithSimilarInterests_.TabIndex = 1;
            this.lbPeopleInYourAreaWithSimilarInterests_.DoubleClick += new System.EventHandler(this.lbPeopleInYourAreaWithSimilarInterests__DoubleClick);
            // 
            // groupBox6_
            // 
            this.groupBox6_.Controls.Add(this.lblNoSentInvitations_);
            this.groupBox6_.Controls.Add(this.lbSentInvitations_);
            this.groupBox6_.Location = new System.Drawing.Point(440, 22);
            this.groupBox6_.Name = "groupBox6_";
            this.groupBox6_.Size = new System.Drawing.Size(211, 242);
            this.groupBox6_.TabIndex = 8;
            this.groupBox6_.TabStop = false;
            this.groupBox6_.Text = "Sent Invitations";
            // 
            // lblNoSentInvitations_
            // 
            this.lblNoSentInvitations_.AutoSize = true;
            this.lblNoSentInvitations_.Location = new System.Drawing.Point(72, 122);
            this.lblNoSentInvitations_.Name = "lblNoSentInvitations_";
            this.lblNoSentInvitations_.Size = new System.Drawing.Size(72, 13);
            this.lblNoSentInvitations_.TabIndex = 3;
            this.lblNoSentInvitations_.Text = "No Invitations";
            // 
            // lbSentInvitations_
            // 
            this.lbSentInvitations_.FormattingEnabled = true;
            this.lbSentInvitations_.Location = new System.Drawing.Point(6, 19);
            this.lbSentInvitations_.Name = "lbSentInvitations_";
            this.lbSentInvitations_.Size = new System.Drawing.Size(199, 212);
            this.lbSentInvitations_.TabIndex = 1;
            this.lbSentInvitations_.DoubleClick += new System.EventHandler(this.lbSentInvitations__DoubleClick);
            // 
            // groupBox3_
            // 
            this.groupBox3_.Controls.Add(this.lblPeopleInYourArea_);
            this.groupBox3_.Controls.Add(this.lbPeopleInYourArea_);
            this.groupBox3_.Location = new System.Drawing.Point(6, 270);
            this.groupBox3_.Name = "groupBox3_";
            this.groupBox3_.Size = new System.Drawing.Size(211, 242);
            this.groupBox3_.TabIndex = 6;
            this.groupBox3_.TabStop = false;
            this.groupBox3_.Text = "Potential Friends in your Area";
            // 
            // lblPeopleInYourArea_
            // 
            this.lblPeopleInYourArea_.AutoSize = true;
            this.lblPeopleInYourArea_.Location = new System.Drawing.Point(72, 122);
            this.lblPeopleInYourArea_.Name = "lblPeopleInYourArea_";
            this.lblPeopleInYourArea_.Size = new System.Drawing.Size(57, 13);
            this.lblPeopleInYourArea_.TabIndex = 3;
            this.lblPeopleInYourArea_.Text = "No People";
            // 
            // lbPeopleInYourArea_
            // 
            this.lbPeopleInYourArea_.FormattingEnabled = true;
            this.lbPeopleInYourArea_.Location = new System.Drawing.Point(6, 19);
            this.lbPeopleInYourArea_.Name = "lbPeopleInYourArea_";
            this.lbPeopleInYourArea_.Size = new System.Drawing.Size(199, 212);
            this.lbPeopleInYourArea_.TabIndex = 1;
            this.lbPeopleInYourArea_.DoubleClick += new System.EventHandler(this.lbPeopleInYourArea__DoubleClick);
            // 
            // groupBox1_
            // 
            this.groupBox1_.Controls.Add(this.lblFriendsWithSameInterestNoFriends_);
            this.groupBox1_.Controls.Add(this.lbFriendsWithSameInterest_);
            this.groupBox1_.Location = new System.Drawing.Point(6, 22);
            this.groupBox1_.Name = "groupBox1_";
            this.groupBox1_.Size = new System.Drawing.Size(211, 242);
            this.groupBox1_.TabIndex = 5;
            this.groupBox1_.TabStop = false;
            this.groupBox1_.Text = "Friends With Same Interest";
            // 
            // lblFriendsWithSameInterestNoFriends_
            // 
            this.lblFriendsWithSameInterestNoFriends_.AutoSize = true;
            this.lblFriendsWithSameInterestNoFriends_.Location = new System.Drawing.Point(72, 122);
            this.lblFriendsWithSameInterestNoFriends_.Name = "lblFriendsWithSameInterestNoFriends_";
            this.lblFriendsWithSameInterestNoFriends_.Size = new System.Drawing.Size(58, 13);
            this.lblFriendsWithSameInterestNoFriends_.TabIndex = 3;
            this.lblFriendsWithSameInterestNoFriends_.Text = "No Friends";
            // 
            // lbFriendsWithSameInterest_
            // 
            this.lbFriendsWithSameInterest_.FormattingEnabled = true;
            this.lbFriendsWithSameInterest_.Location = new System.Drawing.Point(6, 19);
            this.lbFriendsWithSameInterest_.Name = "lbFriendsWithSameInterest_";
            this.lbFriendsWithSameInterest_.Size = new System.Drawing.Size(199, 212);
            this.lbFriendsWithSameInterest_.TabIndex = 1;
            this.lbFriendsWithSameInterest_.DoubleClick += new System.EventHandler(this.lbFriendsWithSameInterest__DoubleClick);
            // 
            // gbFriends_
            // 
            this.gbFriends_.Controls.Add(this.btnViewAllFriends);
            this.gbFriends_.Controls.Add(this.btnViewMore);
            this.gbFriends_.Controls.Add(this.lblFriendsListNoFriends_);
            this.gbFriends_.Controls.Add(this.lbFriendsList_);
            this.gbFriends_.Location = new System.Drawing.Point(657, 22);
            this.gbFriends_.Name = "gbFriends_";
            this.gbFriends_.Size = new System.Drawing.Size(200, 490);
            this.gbFriends_.TabIndex = 5;
            this.gbFriends_.TabStop = false;
            this.gbFriends_.Text = "Friends List";
            // 
            // lblFriendsListNoFriends_
            // 
            this.lblFriendsListNoFriends_.AutoSize = true;
            this.lblFriendsListNoFriends_.Location = new System.Drawing.Point(73, 238);
            this.lblFriendsListNoFriends_.Name = "lblFriendsListNoFriends_";
            this.lblFriendsListNoFriends_.Size = new System.Drawing.Size(58, 13);
            this.lblFriendsListNoFriends_.TabIndex = 4;
            this.lblFriendsListNoFriends_.Text = "No Friends";
            // 
            // lbFriendsList_
            // 
            this.lbFriendsList_.FormattingEnabled = true;
            this.lbFriendsList_.Location = new System.Drawing.Point(6, 19);
            this.lbFriendsList_.Name = "lbFriendsList_";
            this.lbFriendsList_.Size = new System.Drawing.Size(188, 433);
            this.lbFriendsList_.TabIndex = 0;
            this.lbFriendsList_.DoubleClick += new System.EventHandler(this.lbFriendsList__DoubleClick);
            // 
            // gbFriendsOfFriends_
            // 
            this.gbFriendsOfFriends_.Controls.Add(this.lblFriendsOfFriendsListNoFriends_);
            this.gbFriendsOfFriends_.Controls.Add(this.lbFriendsOfFriendsList_);
            this.gbFriendsOfFriends_.Location = new System.Drawing.Point(223, 22);
            this.gbFriendsOfFriends_.Name = "gbFriendsOfFriends_";
            this.gbFriendsOfFriends_.Size = new System.Drawing.Size(211, 242);
            this.gbFriendsOfFriends_.TabIndex = 4;
            this.gbFriendsOfFriends_.TabStop = false;
            this.gbFriendsOfFriends_.Text = "Friends Of Friends";
            // 
            // lblFriendsOfFriendsListNoFriends_
            // 
            this.lblFriendsOfFriendsListNoFriends_.AutoSize = true;
            this.lblFriendsOfFriendsListNoFriends_.Location = new System.Drawing.Point(72, 122);
            this.lblFriendsOfFriendsListNoFriends_.Name = "lblFriendsOfFriendsListNoFriends_";
            this.lblFriendsOfFriendsListNoFriends_.Size = new System.Drawing.Size(58, 13);
            this.lblFriendsOfFriendsListNoFriends_.TabIndex = 3;
            this.lblFriendsOfFriendsListNoFriends_.Text = "No Friends";
            // 
            // lbFriendsOfFriendsList_
            // 
            this.lbFriendsOfFriendsList_.FormattingEnabled = true;
            this.lbFriendsOfFriendsList_.Location = new System.Drawing.Point(6, 19);
            this.lbFriendsOfFriendsList_.Name = "lbFriendsOfFriendsList_";
            this.lbFriendsOfFriendsList_.Size = new System.Drawing.Size(199, 212);
            this.lbFriendsOfFriendsList_.TabIndex = 1;
            this.lbFriendsOfFriendsList_.DoubleClick += new System.EventHandler(this.lbFriendsOfFriendsList__DoubleClick);
            // 
            // lblTitleHomepage_
            // 
            this.lblTitleHomepage_.AutoSize = true;
            this.lblTitleHomepage_.Location = new System.Drawing.Point(3, 0);
            this.lblTitleHomepage_.Name = "lblTitleHomepage_";
            this.lblTitleHomepage_.Size = new System.Drawing.Size(89, 13);
            this.lblTitleHomepage_.TabIndex = 0;
            this.lblTitleHomepage_.Text = "lblTitleHomepage";
            // 
            // btnViewMore
            // 
            this.btnViewMore.Location = new System.Drawing.Point(119, 456);
            this.btnViewMore.Name = "btnViewMore";
            this.btnViewMore.Size = new System.Drawing.Size(75, 23);
            this.btnViewMore.TabIndex = 5;
            this.btnViewMore.Text = "View More";
            this.btnViewMore.UseVisualStyleBackColor = true;
            this.btnViewMore.Click += new System.EventHandler(this.btnViewMore_Click);
            // 
            // btnViewAllFriends
            // 
            this.btnViewAllFriends.Location = new System.Drawing.Point(38, 456);
            this.btnViewAllFriends.Name = "btnViewAllFriends";
            this.btnViewAllFriends.Size = new System.Drawing.Size(75, 23);
            this.btnViewAllFriends.TabIndex = 6;
            this.btnViewAllFriends.Text = "View All";
            this.btnViewAllFriends.UseVisualStyleBackColor = true;
            this.btnViewAllFriends.Click += new System.EventHandler(this.btnViewAllFriends_Click);
            // 
            // UserDialogue
            // 
            this.ClientSize = new System.Drawing.Size(901, 585);
            this.Controls.Add(this.gbMain_);
            this.Controls.Add(this.menuStrip1_);
            this.MainMenuStrip = this.menuStrip1_;
            this.Name = "UserDialogue";
            this.Text = "User";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserDialogue_FormClosing);
            this.menuStrip1_.ResumeLayout(false);
            this.menuStrip1_.PerformLayout();
            this.gbMain_.ResumeLayout(false);
            this.pnlMain_.ResumeLayout(false);
            this.pnlMain_.PerformLayout();
            this.groupBox5_.ResumeLayout(false);
            this.groupBox5_.PerformLayout();
            this.groupBox4_.ResumeLayout(false);
            this.groupBox4_.PerformLayout();
            this.groupBox6_.ResumeLayout(false);
            this.groupBox6_.PerformLayout();
            this.groupBox3_.ResumeLayout(false);
            this.groupBox3_.PerformLayout();
            this.groupBox1_.ResumeLayout(false);
            this.groupBox1_.PerformLayout();
            this.gbFriends_.ResumeLayout(false);
            this.gbFriends_.PerformLayout();
            this.gbFriendsOfFriends_.ResumeLayout(false);
            this.gbFriendsOfFriends_.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
