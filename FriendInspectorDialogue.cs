using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS12_Project_1
{
    // FRIEND INSPECTOR DIALOGUE
    // shows info about a person object
    public class FriendInspectorDialogue : Form
    {   // DATA MEMBERS
        // form elements
        private GroupBox gb1_;
        private GroupBox gb2_;
        private GroupBox gb3_;
        private GroupBox gbHeader_;
        private ListBox lbFriends_;
        private ListBox lbInterests_;
        private Button btnExit_;
        private Label lblCity_;
        private Label lblName_;
        private Label lbl1_;
        private Label lbl2_;
        private Button btnAddFriend_;
        // other data members
        private FriendContext context_;
        private UserDialogue ud_;
        private Person source_;
        private enum FriendContext
        {
            newFriend,
            existingFriend
        };
        // CONSTRUCTOR
        // params:
        //  in UserDialogue t_ud; a reference to a UserDialogue object, 
        //  used for invoking this object on friends lists within this object.
        public FriendInspectorDialogue(in UserDialogue t_ud)
        {   // initialize data members
            ud_ = t_ud;
            InitializeComponent();
        }
        // show the RemoveFriend button
        private void ShowRemoveFriendBtn()
        {   // change the context 
            context_ = FriendContext.existingFriend;
            btnAddFriend_.Text = "Remove Friend";
        }
        // show the AddFriend button
        private void ShowAddFriendBtn()
        {   // change the context
            context_ = FriendContext.newFriend;
            btnAddFriend_.Text = "Add Friend";
        }
        // setup all form fields and then show this form 
        public void Show(in Person t_next)
        {   // do a null check
            if (t_next == null)
                return; // fail
            source_ = t_next;   // set source_ to refer to the t_next parameter
            if (t_next.StaticID == ud_.CurrentUser.StaticID // pick what context to use when adding / removing friends
            || ud_.CurrentUser.IsFriend(t_next))
                ShowRemoveFriendBtn();
            else
                ShowAddFriendBtn();
            gbHeader_.Text = t_next.UserName; // set the header to t_next's username.
            lblName_.Text = string.Format("{0} {1}", t_next.FirstName, t_next.LastName); // set the name field to t_next's first and last name
            lblCity_.Text = t_next.City; // set the city field to t_next's city
            lbFriends_.Items.Clear();    // clear all items in the friends list
            lbInterests_.Items.Clear();  // clear all items in the friends list
            t_next.Friends.ForEach(x => lbFriends_.Items.Add(x.UserName)); // fill the friends list with t_next's friends list
            t_next.Interests.ForEach(x => lbInterests_.Items.Add(x)); // fill the interests list with t_next's interests list
            Show(); // show the form
            Focus();    // bring this form info focus
        }
        // initialize form elements
        private void InitializeComponent()
        {
            this.gbHeader_ = new System.Windows.Forms.GroupBox();
            this.btnExit_ = new System.Windows.Forms.Button();
            this.gb3_ = new System.Windows.Forms.GroupBox();
            this.lbFriends_ = new System.Windows.Forms.ListBox();
            this.gb2_ = new System.Windows.Forms.GroupBox();
            this.lblCity_ = new System.Windows.Forms.Label();
            this.lblName_ = new System.Windows.Forms.Label();
            this.lbl1_ = new System.Windows.Forms.Label();
            this.lbl2_ = new System.Windows.Forms.Label();
            this.gb1_ = new System.Windows.Forms.GroupBox();
            this.lbInterests_ = new System.Windows.Forms.ListBox();
            this.btnAddFriend_ = new System.Windows.Forms.Button();
            this.gbHeader_.SuspendLayout();
            this.gb3_.SuspendLayout();
            this.gb2_.SuspendLayout();
            this.gb1_.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbHeader
            // 
            this.gbHeader_.Controls.Add(this.btnAddFriend_);
            this.gbHeader_.Controls.Add(this.btnExit_);
            this.gbHeader_.Controls.Add(this.gb3_);
            this.gbHeader_.Controls.Add(this.gb2_);
            this.gbHeader_.Controls.Add(this.gb1_);
            this.gbHeader_.Location = new System.Drawing.Point(12, 12);
            this.gbHeader_.Name = "gbHeader";
            this.gbHeader_.Size = new System.Drawing.Size(260, 393);
            this.gbHeader_.TabIndex = 0;
            this.gbHeader_.TabStop = false;
            this.gbHeader_.Text = "<Name>";
            // 
            // btnExit
            // 
            this.btnExit_.Location = new System.Drawing.Point(179, 358);
            this.btnExit_.Name = "btnExit";
            this.btnExit_.Size = new System.Drawing.Size(75, 23);
            this.btnExit_.TabIndex = 6;
            this.btnExit_.Text = "Exit";
            this.btnExit_.UseVisualStyleBackColor = true;
            this.btnExit_.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupBox3
            // 
            this.gb3_.Controls.Add(this.lbFriends_);
            this.gb3_.Location = new System.Drawing.Point(6, 219);
            this.gb3_.Name = "groupBox3";
            this.gb3_.Size = new System.Drawing.Size(248, 133);
            this.gb3_.TabIndex = 5;
            this.gb3_.TabStop = false;
            this.gb3_.Text = "Friends";
            // 
            // lbFriends
            // 
            this.lbFriends_.FormattingEnabled = true;
            this.lbFriends_.Location = new System.Drawing.Point(6, 19);
            this.lbFriends_.Name = "lbFriends";
            this.lbFriends_.Size = new System.Drawing.Size(236, 108);
            this.lbFriends_.TabIndex = 1;
            this.lbFriends_.DoubleClick += new System.EventHandler(this.lbFriends_DoubleClick);
            // 
            // groupBox2
            // 
            this.gb2_.Controls.Add(this.lblCity_);
            this.gb2_.Controls.Add(this.lblName_);
            this.gb2_.Controls.Add(this.lbl1_);
            this.gb2_.Controls.Add(this.lbl2_);
            this.gb2_.Location = new System.Drawing.Point(6, 19);
            this.gb2_.Name = "groupBox2";
            this.gb2_.Size = new System.Drawing.Size(248, 55);
            this.gb2_.TabIndex = 5;
            this.gb2_.TabStop = false;
            this.gb2_.Text = "Info";
            // 
            // lblCity
            // 
            this.lblCity_.AutoSize = true;
            this.lblCity_.Location = new System.Drawing.Point(47, 29);
            this.lblCity_.Name = "lblCity";
            this.lblCity_.Size = new System.Drawing.Size(24, 13);
            this.lblCity_.TabIndex = 5;
            this.lblCity_.Text = "City";
            // 
            // lblName
            // 
            this.lblName_.AutoSize = true;
            this.lblName_.Location = new System.Drawing.Point(47, 16);
            this.lblName_.Name = "lblName";
            this.lblName_.Size = new System.Drawing.Size(35, 13);
            this.lblName_.TabIndex = 4;
            this.lblName_.Text = "Name";
            // 
            // label1
            // 
            this.lbl1_.AutoSize = true;
            this.lbl1_.Location = new System.Drawing.Point(6, 29);
            this.lbl1_.Name = "label1";
            this.lbl1_.Size = new System.Drawing.Size(24, 13);
            this.lbl1_.TabIndex = 3;
            this.lbl1_.Text = "City";
            // 
            // label2
            // 
            this.lbl2_.AutoSize = true;
            this.lbl2_.Location = new System.Drawing.Point(6, 16);
            this.lbl2_.Name = "label2";
            this.lbl2_.Size = new System.Drawing.Size(35, 13);
            this.lbl2_.TabIndex = 2;
            this.lbl2_.Text = "Name";
            // 
            // groupBox1
            // 
            this.gb1_.Controls.Add(this.lbInterests_);
            this.gb1_.Location = new System.Drawing.Point(6, 80);
            this.gb1_.Name = "groupBox1";
            this.gb1_.Size = new System.Drawing.Size(248, 133);
            this.gb1_.TabIndex = 4;
            this.gb1_.TabStop = false;
            this.gb1_.Text = "Interests";
            // 
            // lbInterests
            // 
            this.lbInterests_.FormattingEnabled = true;
            this.lbInterests_.Location = new System.Drawing.Point(6, 19);
            this.lbInterests_.Name = "lbInterests";
            this.lbInterests_.Size = new System.Drawing.Size(236, 108);
            this.lbInterests_.TabIndex = 0;
            // 
            // btnAddFriend
            // 
            this.btnAddFriend_.Location = new System.Drawing.Point(98, 358);
            this.btnAddFriend_.Name = "btnAddFriend";
            this.btnAddFriend_.Size = new System.Drawing.Size(75, 23);
            this.btnAddFriend_.TabIndex = 7;
            this.btnAddFriend_.Text = "Add Friend";
            this.btnAddFriend_.UseVisualStyleBackColor = true;
            this.btnAddFriend_.Click += new System.EventHandler(this.btnAddFriend_Click);
            // 
            // FriendInspectorDialogue
            // 
            this.ClientSize = new System.Drawing.Size(285, 413);
            this.Controls.Add(this.gbHeader_);
            this.Name = "FriendInspectorDialogue";
            this.Text = "Friend Inspector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FriendInspectorDialogue_FormClosing);
            this.gbHeader_.ResumeLayout(false);
            this.gb3_.ResumeLayout(false);
            this.gb2_.ResumeLayout(false);
            this.gb2_.PerformLayout();
            this.gb1_.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        // FORM EVENTS
        // invoked when the exit button is pressed
        private void btnExit_Click(object sender, EventArgs e)
        {   // hide this form
            Hide();
        }
        // invoked when the form is about to be closed 
        private void FriendInspectorDialogue_FormClosing(object sender, FormClosingEventArgs e)
        {   // if the user wants to close the form using the X button, hide it instead
            if(e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // cancel the close request
                Hide(); // hide the form
            }
        }
        // invoked when lbFriends_ is double clicked
        private void lbFriends_DoubleClick(object sender, EventArgs e)
        {   // ensure that an element in lbFriends_ is selected
            if (lbFriends_.SelectedIndex == -1)
                return; // fail
            ud_.LoadFriendInspectorDialogue(lbFriends_.Items[lbFriends_.SelectedIndex] as string); // display info about the selected friend
        }
        // invoked when btnAddFriend is pressed
        private void btnAddFriend_Click(object sender, EventArgs e)
        {
            switch(context_)    // change actions based on context
            {
                case FriendContext.newFriend:   // add a new friend
                    if (ud_.CurrentUser.AddFriends(source_))
                    {   // update form elements
                        ShowRemoveFriendBtn();  
                        ud_.UpdateAllLists();
                    }
                    return;
                case FriendContext.existingFriend:  // remove a friend
                    if (ud_.CurrentUser.RemoveFriends(source_))
                    {   // update form elements
                        ShowAddFriendBtn();
                        ud_.UpdateAllLists();
                    }
                    return;
            }
        }
    }
}
