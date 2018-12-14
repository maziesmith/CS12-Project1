using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS12_Project_1
{   // NEW INVITATION DIALOGUE
    // provides a user interface for creating new invites
    public class NewInvitationDialogue : Form
    {   // DATA MEMBERS
        // form elements
        private RichTextBox rtxtDescription_;
        private GroupBox gbHeader_;
        private GroupBox groupBox2_;
        private GroupBox groupBox3_;
        private ListBox lbRecipients_;
        private TextBox txtTitle_;
        private TextBox textBox2_;
        private TextBox txtAddRecipients_;
        private TextBox txtBox1_;
        private Button btnCancel_;
        private Button btnSubmit_;
        private Button button3_;
        private Button button4_;
        private Button btnRemoveRecipients_;
        private Button btnAddRecipients_;
        private Button button1_;
        private Button button2_;
        private Label lblTitle_;
        // other data members
        private LinkedList<Person> recipients_; // a linked list containing all the recipients
        private InvitationSystem is_;   // a reference to an invitation system to send invitations 
        private PersonsDatabase pd_;    // a reference to a person database to query person objects
        private UserDialogue ud_;   // a reference to a user dialogue object 
        private Person author_; // a reference to the author's person object
        private enum ErrorCodes
        {   // error codes
            NoTitle = 0,
            NoRecipents = 1,
            NoDescription = 2,
            RecipientNotFound = 3,
            RecipientInList = 4,
        }
        private readonly string[] errorMsg =
        {   // error messages
            "Title required.",
            "Recipents required.",
            "Description required.",
            "Could not add recipient, account not found.",
            "Recipent is already in list."
        };
        // CONSTRUCTOR
        // params:
        //  in UserDialogue t_ud;   reference to a user dialogue object
        //  in InvitationSystem t_is;   reference to an InvitationSystem object
        //  in PersonsDatabase t_pd;    reference to a person data base object
        // TODO: use the in keyword more
        public NewInvitationDialogue(
            in UserDialogue t_ud,
            in InvitationSystem t_is,
            in PersonsDatabase t_pd)
        {   // initialize all data members 
            is_ = t_is;
            pd_ = t_pd;
            ud_ = t_ud;
            recipients_ = new LinkedList<Person>(); // recipients_ to be a new LinkedList<recipients_>
            InitializeComponent();  // initialize form elements
        }
        // METHOD MEMBERS
        // initialize form elements
        private void InitializeComponent()
        {
            this.gbHeader_ = new System.Windows.Forms.GroupBox();
            this.groupBox3_ = new System.Windows.Forms.GroupBox();
            this.lbRecipients_ = new System.Windows.Forms.ListBox();
            this.btnRemoveRecipients_ = new System.Windows.Forms.Button();
            this.btnAddRecipients_ = new System.Windows.Forms.Button();
            this.txtAddRecipients_ = new System.Windows.Forms.TextBox();
            this.txtBox1_ = new System.Windows.Forms.TextBox();
            this.button3_ = new System.Windows.Forms.Button();
            this.button4_ = new System.Windows.Forms.Button();
            this.groupBox2_ = new System.Windows.Forms.GroupBox();
            this.textBox2_ = new System.Windows.Forms.TextBox();
            this.rtxtDescription_ = new System.Windows.Forms.RichTextBox();
            this.button1_ = new System.Windows.Forms.Button();
            this.button2_ = new System.Windows.Forms.Button();
            this.lblTitle_ = new System.Windows.Forms.Label();
            this.txtTitle_ = new System.Windows.Forms.TextBox();
            this.btnCancel_ = new System.Windows.Forms.Button();
            this.btnSubmit_ = new System.Windows.Forms.Button();
            this.gbHeader_.SuspendLayout();
            this.groupBox3_.SuspendLayout();
            this.groupBox2_.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbHeader_
            // 
            this.gbHeader_.Controls.Add(this.groupBox3_);
            this.gbHeader_.Controls.Add(this.groupBox2_);
            this.gbHeader_.Controls.Add(this.lblTitle_);
            this.gbHeader_.Controls.Add(this.txtTitle_);
            this.gbHeader_.Controls.Add(this.btnCancel_);
            this.gbHeader_.Controls.Add(this.btnSubmit_);
            this.gbHeader_.Location = new System.Drawing.Point(13, 13);
            this.gbHeader_.Name = "gbHeader_";
            this.gbHeader_.Size = new System.Drawing.Size(252, 393);
            this.gbHeader_.TabIndex = 0;
            this.gbHeader_.TabStop = false;
            this.gbHeader_.Text = "New Invitation";
            // 
            // groupBox3_
            // 
            this.groupBox3_.Controls.Add(this.lbRecipients_);
            this.groupBox3_.Controls.Add(this.btnRemoveRecipients_);
            this.groupBox3_.Controls.Add(this.btnAddRecipients_);
            this.groupBox3_.Controls.Add(this.txtAddRecipients_);
            this.groupBox3_.Controls.Add(this.txtBox1_);
            this.groupBox3_.Controls.Add(this.button3_);
            this.groupBox3_.Controls.Add(this.button4_);
            this.groupBox3_.Location = new System.Drawing.Point(9, 178);
            this.groupBox3_.Name = "groupBox3_";
            this.groupBox3_.Size = new System.Drawing.Size(234, 170);
            this.groupBox3_.TabIndex = 8;
            this.groupBox3_.TabStop = false;
            this.groupBox3_.Text = "Recipents";
            // 
            // lbRecipients_
            // 
            this.lbRecipients_.FormattingEnabled = true;
            this.lbRecipients_.Location = new System.Drawing.Point(6, 19);
            this.lbRecipients_.Name = "lbRecipients_";
            this.lbRecipients_.Size = new System.Drawing.Size(222, 121);
            this.lbRecipients_.TabIndex = 11;
            this.lbRecipients_.DoubleClick += new System.EventHandler(this.lbRecipients__DoubleClick);
            // 
            // btnRemoveRecipients_
            // 
            this.btnRemoveRecipients_.Location = new System.Drawing.Point(188, 142);
            this.btnRemoveRecipients_.Name = "btnRemoveRecipients_";
            this.btnRemoveRecipients_.Size = new System.Drawing.Size(17, 23);
            this.btnRemoveRecipients_.TabIndex = 10;
            this.btnRemoveRecipients_.Text = "-";
            this.btnRemoveRecipients_.UseVisualStyleBackColor = true;
            this.btnRemoveRecipients_.Click += new System.EventHandler(this.btnRemoveRecipent_Click);
            // 
            // btnAddRecipients_
            // 
            this.btnAddRecipients_.Location = new System.Drawing.Point(211, 142);
            this.btnAddRecipients_.Name = "btnAddRecipients_";
            this.btnAddRecipients_.Size = new System.Drawing.Size(17, 23);
            this.btnAddRecipients_.TabIndex = 9;
            this.btnAddRecipients_.Text = "+";
            this.btnAddRecipients_.UseVisualStyleBackColor = true;
            this.btnAddRecipients_.Click += new System.EventHandler(this.btnAddRecipent_Click);
            // 
            // txtAddRecipients_
            // 
            this.txtAddRecipients_.Location = new System.Drawing.Point(6, 144);
            this.txtAddRecipients_.Name = "txtAddRecipients_";
            this.txtAddRecipients_.Size = new System.Drawing.Size(176, 20);
            this.txtAddRecipients_.TabIndex = 9;
            // 
            // txtBox1_
            // 
            this.txtBox1_.Location = new System.Drawing.Point(40, 207);
            this.txtBox1_.Name = "txtBox1_";
            this.txtBox1_.Size = new System.Drawing.Size(100, 20);
            this.txtBox1_.TabIndex = 3;
            // 
            // button3_
            // 
            this.button3_.Location = new System.Drawing.Point(97, 207);
            this.button3_.Name = "button3_";
            this.button3_.Size = new System.Drawing.Size(75, 23);
            this.button3_.TabIndex = 1;
            this.button3_.Text = "Cancel";
            this.button3_.UseVisualStyleBackColor = true;
            // 
            // button4_
            // 
            this.button4_.Location = new System.Drawing.Point(178, 207);
            this.button4_.Name = "button4_";
            this.button4_.Size = new System.Drawing.Size(75, 23);
            this.button4_.TabIndex = 0;
            this.button4_.Text = "Submit";
            this.button4_.UseVisualStyleBackColor = true;
            // 
            // groupBox2_
            // 
            this.groupBox2_.Controls.Add(this.textBox2_);
            this.groupBox2_.Controls.Add(this.rtxtDescription_);
            this.groupBox2_.Controls.Add(this.button1_);
            this.groupBox2_.Controls.Add(this.button2_);
            this.groupBox2_.Location = new System.Drawing.Point(9, 47);
            this.groupBox2_.Name = "groupBox2_";
            this.groupBox2_.Size = new System.Drawing.Size(234, 125);
            this.groupBox2_.TabIndex = 7;
            this.groupBox2_.TabStop = false;
            this.groupBox2_.Text = "Description";
            // 
            // textBox2_
            // 
            this.textBox2_.Location = new System.Drawing.Point(40, 207);
            this.textBox2_.Name = "textBox2_";
            this.textBox2_.Size = new System.Drawing.Size(100, 20);
            this.textBox2_.TabIndex = 3;
            // 
            // rtxtDescription_
            // 
            this.rtxtDescription_.Location = new System.Drawing.Point(6, 19);
            this.rtxtDescription_.Name = "rtxtDescription_";
            this.rtxtDescription_.Size = new System.Drawing.Size(222, 96);
            this.rtxtDescription_.TabIndex = 6;
            this.rtxtDescription_.Text = "";
            // 
            // button1_
            // 
            this.button1_.Location = new System.Drawing.Point(97, 207);
            this.button1_.Name = "button1_";
            this.button1_.Size = new System.Drawing.Size(75, 23);
            this.button1_.TabIndex = 1;
            this.button1_.Text = "Cancel";
            this.button1_.UseVisualStyleBackColor = true;
            // 
            // button2_
            // 
            this.button2_.Location = new System.Drawing.Point(178, 207);
            this.button2_.Name = "button2_";
            this.button2_.Size = new System.Drawing.Size(75, 23);
            this.button2_.TabIndex = 0;
            this.button2_.Text = "Submit";
            this.button2_.UseVisualStyleBackColor = true;
            // 
            // lblTitle_
            // 
            this.lblTitle_.AutoSize = true;
            this.lblTitle_.Location = new System.Drawing.Point(6, 24);
            this.lblTitle_.Name = "lblTitle_";
            this.lblTitle_.Size = new System.Drawing.Size(27, 13);
            this.lblTitle_.TabIndex = 4;
            this.lblTitle_.Text = "Title";
            // 
            // txtTitle_
            // 
            this.txtTitle_.Location = new System.Drawing.Point(39, 21);
            this.txtTitle_.Name = "txtTitle_";
            this.txtTitle_.Size = new System.Drawing.Size(198, 20);
            this.txtTitle_.TabIndex = 1;
            // 
            // btnCancel_
            // 
            this.btnCancel_.Location = new System.Drawing.Point(87, 356);
            this.btnCancel_.Name = "btnCancel_";
            this.btnCancel_.Size = new System.Drawing.Size(75, 23);
            this.btnCancel_.TabIndex = 1;
            this.btnCancel_.Text = "Cancel";
            this.btnCancel_.UseVisualStyleBackColor = true;
            this.btnCancel_.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSubmit_
            // 
            this.btnSubmit_.Location = new System.Drawing.Point(168, 356);
            this.btnSubmit_.Name = "btnSubmit_";
            this.btnSubmit_.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit_.TabIndex = 0;
            this.btnSubmit_.Text = "Submit";
            this.btnSubmit_.UseVisualStyleBackColor = true;
            this.btnSubmit_.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // NewInvitationDialogue
            // 
            this.ClientSize = new System.Drawing.Size(279, 416);
            this.Controls.Add(this.gbHeader_);
            this.Name = "NewInvitationDialogue";
            this.Text = "Invitation Builder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewInvitationDialogue_FormClosing);
            this.gbHeader_.ResumeLayout(false);
            this.gbHeader_.PerformLayout();
            this.groupBox3_.ResumeLayout(false);
            this.groupBox3_.PerformLayout();
            this.groupBox2_.ResumeLayout(false);
            this.groupBox2_.PerformLayout();
            this.ResumeLayout(false);

        }
        // get or set the author object
        public Person Author
        {
            get { return author_; } // return the author's person object
            set { author_ = value ?? author_; } // setting author_ to null will be rejected
        }
        // sets gbHeader_ to the appropriate error message
        private void Error(ErrorCodes t_errno)
        {
            gbHeader_.Text = errorMsg[(int)t_errno];
        }
        // Initialize important form elements
        public void Initialize()
        {
            txtAddRecipients_.Text = "";    // delete all text in fields 
            rtxtDescription_.Text = "";     // 
            txtTitle_.Text = "";            // 
            recipients_.Clear();            // delete all list box entries
            lbRecipients_.Items.Clear();    //
        }
        // show the form
        public void Show(Person t_author)
        {
            author_ = t_author; // set the author
            Show(); // show this form
            Focus();    // bring this form to focus
        }
        // EVENTS
        // when the submit button is pressed
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtTitle_.Text))
            {   // test if txtTitle_ is empty then error
                Error(ErrorCodes.NoTitle);
                return;
            }
            if(string.IsNullOrWhiteSpace(rtxtDescription_.Text))
            {   // if rtxtDescription_ is empty then error
                Error(ErrorCodes.NoDescription);
                return;
            }
            if (lbRecipients_.Items.Count == 0)
            {   // if lbRecipients_.Items is empty then error
                Error(ErrorCodes.NoRecipents);
                return;
            }   // add the Invitation
            is_.AddInvitation(Invitation.InvitationFactory.MakeInvitation(author_, recipients_.ToArray(), txtTitle_.Text, rtxtDescription_.Text));
            ud_.UpdateSentInvitations();    // tell ud_ to update invitations
            ud_.UpdatePendingInvitations(); //
            Hide(); // hide the form
        }
        // when the cancel button is pressed
        private void btnCancel_Click(object sender, EventArgs e)
        {   // hide the form
            Hide();
        }
        // when the btnRemoveRecipent_ is pressed
        private void btnRemoveRecipent_Click(object sender, EventArgs e)
        {   // try to remove the selected recipient from the list
            if (lbRecipients_.SelectedIndex == -1)
                return; // return if no item is selected
            Person databaseCall = pd_.CachedLinearSearch(lbRecipients_.Items[lbRecipients_.SelectedIndex] as string);
            if (databaseCall == null)
                return; // return if the person does not exist
            LinkedListNode<Person> test = recipients_.Find(databaseCall);
            if (test == null)
                return; // return if person is not in the list
            recipients_.Remove(test);   // remove the node
            lbRecipients_.Items.RemoveAt(lbRecipients_.SelectedIndex);  // remove the label
        }
        // when the btnAddRecipent_ is pressed
        private void btnAddRecipent_Click(object sender, EventArgs e)
        {   // try to add a recipient
            Person databaseCall = pd_.BloomFilterSearch(txtAddRecipients_.Text);
            if (databaseCall == null)
            {   // return if the person is not in the database
                Error(ErrorCodes.RecipientNotFound);
                return;
            }
            if (recipients_.Any(x => x.staticID == databaseCall.staticID))
            {   // return if the person is already in the list 
                Error(ErrorCodes.RecipientInList);
                return;
            }
            txtAddRecipients_.Text = "";    // clear the txtAddRecipients_ field
            lbRecipients_.Items.Add(databaseCall.UserName); // add person to the list box
            recipients_.AddLast(databaseCall);  // add person to the list
        }
        // when the form is closing
        private void NewInvitationDialogue_FormClosing(object sender, FormClosingEventArgs e)
        {   // if the user tried to close the form
            if(e.CloseReason == CloseReason.UserClosing)
            {   // cancel the request
                e.Cancel = true;
                Hide(); // hide the form
            }
        }
        // when lbRecipients_ is double clicked, show info on the recipient
        private void lbRecipients__DoubleClick(object sender, EventArgs e)
        {   // return if no entry is selected
            if (lbRecipients_.SelectedIndex == -1)
                return; // request ud_ to set and open the dialogue
            ud_.LoadFriendInspectorDialogue(lbRecipients_.Items[lbRecipients_.SelectedIndex] as string);
        }
    }
}
