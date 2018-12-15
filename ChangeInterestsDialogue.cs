using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS12_Project_1
{
    // CHANGE INTERESTS DIALOGUE
    // a tool, used for changing a person object's interests
    class ChangeInterestsDialogue : Form
    {   // DATA MEMBERS
        // form elements
        private GroupBox gbInterests_;
        private TextBox txtNextInterests_;
        private ListBox lbInterests_;
        private Button btnAdd_;
        private Button btnRemove_;
        private Button btnExit_;
        private Button btnSave_;
        // other data members
        private LinkedList<string> modInterests_;    // a list storing the users interests
        // a linked list is used here because the user is more likely to add and delete entries
        // both of these operations have better time complexity compared to List<T>
        private UserDialogue ud_;   // a reference to a user dialogue object
        // CONSTRUCTOR
        // params:
        //  in UserDialogue t_ud; the UserDialogue object to send messages to
        public ChangeInterestsDialogue(in UserDialogue t_ud)
        {   // initialize data members
            ud_ = t_ud;
            modInterests_ = new LinkedList<string>();    
            InitializeComponent();  // initialize the form 
        }
        // METHOD MEMBERS
        // set up every thing
        public void Init()
        {
            gbInterests_.Text = "Change Interests";  // reset the title
            txtNextInterests_.Clear();   // clear all fields and lists
            lbInterests_.Items.Clear();
            modInterests_.Clear();
            ud_.CurrentUser.Interests.ForEach( x => modInterests_.AddLast(x));   // fill the modInterests list
            ud_.CurrentUser.Interests.ForEach( x => lbInterests_.Items.Add(x));  // fill the lbInterests list
        }
        // initializes the form 
        private void InitializeComponent()
        {
            this.lbInterests_ = new System.Windows.Forms.ListBox();
            this.gbInterests_ = new System.Windows.Forms.GroupBox();
            this.btnRemove_ = new System.Windows.Forms.Button();
            this.btnAdd_ = new System.Windows.Forms.Button();
            this.txtNextInterests_ = new System.Windows.Forms.TextBox();
            this.btnExit_ = new System.Windows.Forms.Button();
            this.btnSave_ = new System.Windows.Forms.Button();
            this.gbInterests_.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbInterests
            // 
            this.lbInterests_.FormattingEnabled = true;
            this.lbInterests_.Location = new System.Drawing.Point(6, 19);
            this.lbInterests_.Name = "lbInterests";
            this.lbInterests_.Size = new System.Drawing.Size(309, 147);
            this.lbInterests_.TabIndex = 1;
            // 
            // gbInterests
            // 
            this.gbInterests_.Controls.Add(this.btnRemove_);
            this.gbInterests_.Controls.Add(this.btnAdd_);
            this.gbInterests_.Controls.Add(this.txtNextInterests_);
            this.gbInterests_.Controls.Add(this.lbInterests_);
            this.gbInterests_.Location = new System.Drawing.Point(12, 12);
            this.gbInterests_.Name = "gbInterests";
            this.gbInterests_.Size = new System.Drawing.Size(321, 206);
            this.gbInterests_.TabIndex = 2;
            this.gbInterests_.TabStop = false;
            this.gbInterests_.Text = "Interests";
            // 
            // btnRemove
            // 
            this.btnRemove_.Location = new System.Drawing.Point(271, 173);
            this.btnRemove_.Name = "btnRemove";
            this.btnRemove_.Size = new System.Drawing.Size(19, 23);
            this.btnRemove_.TabIndex = 4;
            this.btnRemove_.Text = "-";
            this.btnRemove_.UseVisualStyleBackColor = true;
            this.btnRemove_.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd_.Location = new System.Drawing.Point(296, 173);
            this.btnAdd_.Name = "btnAdd";
            this.btnAdd_.Size = new System.Drawing.Size(19, 23);
            this.btnAdd_.TabIndex = 3;
            this.btnAdd_.Text = "+";
            this.btnAdd_.UseVisualStyleBackColor = true;
            this.btnAdd_.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtNextInterests
            // 
            this.txtNextInterests_.Location = new System.Drawing.Point(6, 175);
            this.txtNextInterests_.Name = "txtNextInterests";
            this.txtNextInterests_.Size = new System.Drawing.Size(259, 20);
            this.txtNextInterests_.TabIndex = 2;
            // 
            // btnExit
            // 
            this.btnExit_.Location = new System.Drawing.Point(258, 224);
            this.btnExit_.Name = "btnExit";
            this.btnExit_.Size = new System.Drawing.Size(75, 23);
            this.btnExit_.TabIndex = 3;
            this.btnExit_.Text = "Exit";
            this.btnExit_.UseVisualStyleBackColor = true;
            this.btnExit_.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave_.Location = new System.Drawing.Point(177, 224);
            this.btnSave_.Name = "btnSave";
            this.btnSave_.Size = new System.Drawing.Size(75, 23);
            this.btnSave_.TabIndex = 4;
            this.btnSave_.Text = "Save";
            this.btnSave_.UseVisualStyleBackColor = true;
            this.btnSave_.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ChangeInterestsDialogue
            // 
            this.ClientSize = new System.Drawing.Size(345, 257);
            this.Controls.Add(this.btnSave_);
            this.Controls.Add(this.btnExit_);
            this.Controls.Add(this.gbInterests_);
            this.Name = "ChangeInterestsDialogue";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChangeInterestsDialogue_FormClosing);
            this.gbInterests_.ResumeLayout(false);
            this.gbInterests_.PerformLayout();
            this.ResumeLayout(false);

        }
        // EVENTS
        // when the add button is pressed
        private void btnAdd_Click(object sender, EventArgs e)
        {   // do a null and contains check
            if(!string.IsNullOrWhiteSpace(txtNextInterests_.Text)
            && !modInterests_.Contains(txtNextInterests_.Text))
            {   // add the next interest to lbInterests and modInterests
                modInterests_.AddLast(txtNextInterests_.Text);
                lbInterests_.Items.Add(txtNextInterests_.Text);
                return; // success
            }
            gbInterests_.Text = "Cannot add duplicate interests";    //fail; display the error message
        }
        // when the remove button is pressed
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if(lbInterests_.SelectedIndex == -1)    // check if any element in the list is selected
                return; // fail
            LinkedListNode<string> toBeDeleted = modInterests_.Find(lbInterests_.Items[lbInterests_.SelectedIndex] as string); // get the element that will be deleted
            if(toBeDeleted == null)    // do a null check
                return; // fail
            modInterests_.Remove(toBeDeleted);   // remove toBeDeleted from all lists
            lbInterests_.Items.RemoveAt(lbInterests_.SelectedIndex);  // success
        }
        // when the save button is pressed
        private void btnSave_Click(object sender, EventArgs e)
        {
            if(ud_.CurrentUser.SetInterests(modInterests_.Distinct().ToList()))  // push changes to the person object
            {
                gbInterests_.Text = "Saved Interests";   // change the header to indicate that the push was a success
                return; // success
            }
            gbInterests_.Text = "Could not save"; // change the header to indicate that the push was a failiur
        }
        // when the exit button is pressed
        private void btnExit_Click(object sender, EventArgs e)
        {   // hide the form
            Hide();
        }
        // when the form is about to close
        private void ChangeInterestsDialogue_FormClosing(object sender, FormClosingEventArgs e)
        {   // if the user closed this form, cancel thier request
            if(e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide(); // hide the from 
            }
        }
    }
}
