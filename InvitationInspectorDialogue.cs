using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS12_Project_1
{   // INVITATION INSPECTOR DIALOGUE
    // shows info about an invitation object
    public class InvitationInspectorDialogue : Form
    {   // DATA MEMBERS
        // form elements
        private RichTextBox rtxtDescription_;
        private GroupBox gbDescription_;
        private GroupBox gb3_;
        private GroupBox gbHeader_;
        private GroupBox gb1_;
        private ListBox lbRecipients_;
        private TextBox txtBox1_;
        private TextBox txtBox2_;
        private Button btnDelete_;
        private Button btn1_;
        private Button btn2_;
        private Button btn3_;
        private Button btn4_;
        private Button btnExit_;
        private Label lblDateHeader_;
        private Label lblDate_;
        // other data members
        private Person.InvitationContext context_;   // governs how invitations will be deleted
        private Invitation invitation_; // refers to the invitation that was called with Show()
        private UserDialogue ud_; // a reference to a user dialogue object
        // months
        private readonly string[] months = { "January","February","March","April","May","June","July","August","September","October","November","December" };
        // CONSTRUCTOR
        //  params:
        //  in UserDialogue t_ud; a reference to a UserDialogue object
        public InvitationInspectorDialogue(in UserDialogue t_ud)
        {   // initalize all data members
            ud_ = t_ud;
            InitializeComponent();  // initalize form elements
        }
        // setup all form fields and then show this form
        public void Show(in Invitation t_next, Person.InvitationContext t_context)
        {
            context_ = t_context;
            invitation_ = t_next;
            gbHeader_.Text = t_next.title;  // set the header to t_next's title
            rtxtDescription_.Text = t_next.body;    // set rtxtDescription_ to show the invitation body
            lblDate_.Text = string.Format("{0} {1}", t_next.timestamp.Item2, months[t_next.timestamp.Item1 - 1]);
            lbRecipients_.Items.Clear();    // clear lbRecipients_
            if (t_next.GetRecipients != null)
                foreach (Person x in t_next.GetRecipients)  // show all recipients
                    lbRecipients_.Items.Add(x.UserName);
            Show(); // show the form
            Focus(); // bring this form info focus
        }
        private void btnExit__Click(object sender, EventArgs e)
        {   // hide this form
            Hide();
        }
        private void lbRecipients__DoubleClick(object sender, EventArgs e)
        {   // show info about a person object
            if (lbRecipients_.SelectedIndex == -1)
                return; // ensure that the user has selected an item in the list
            ud_.LoadFriendInspectorDialogue(lbRecipients_.Items[lbRecipients_.SelectedIndex] as string);    // show info about the selected person
        }
        // initalize the form
        private void InitializeComponent()
        {
            this.gbHeader_ = new System.Windows.Forms.GroupBox();
            this.btnExit_ = new System.Windows.Forms.Button();
            this.gb3_ = new System.Windows.Forms.GroupBox();
            this.lbRecipients_ = new System.Windows.Forms.ListBox();
            this.txtBox1_ = new System.Windows.Forms.TextBox();
            this.btn1_ = new System.Windows.Forms.Button();
            this.btn2_ = new System.Windows.Forms.Button();
            this.gbDescription_ = new System.Windows.Forms.GroupBox();
            this.txtBox2_ = new System.Windows.Forms.TextBox();
            this.rtxtDescription_ = new System.Windows.Forms.RichTextBox();
            this.btn3_ = new System.Windows.Forms.Button();
            this.btn4_ = new System.Windows.Forms.Button();
            this.btnDelete_ = new System.Windows.Forms.Button();
            this.gb1_ = new System.Windows.Forms.GroupBox();
            this.lblDateHeader_ = new System.Windows.Forms.Label();
            this.lblDate_ = new System.Windows.Forms.Label();
            this.gbHeader_.SuspendLayout();
            this.gb3_.SuspendLayout();
            this.gbDescription_.SuspendLayout();
            this.gb1_.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbHeader_
            // 
            this.gbHeader_.Controls.Add(this.gb1_);
            this.gbHeader_.Controls.Add(this.btnDelete_);
            this.gbHeader_.Controls.Add(this.btnExit_);
            this.gbHeader_.Controls.Add(this.gb3_);
            this.gbHeader_.Controls.Add(this.gbDescription_);
            this.gbHeader_.Location = new System.Drawing.Point(12, 12);
            this.gbHeader_.Name = "gbHeader_";
            this.gbHeader_.Size = new System.Drawing.Size(260, 410);
            this.gbHeader_.TabIndex = 0;
            this.gbHeader_.TabStop = false;
            this.gbHeader_.Text = "groupBox1";
            // 
            // btnExit_
            // 
            this.btnExit_.Location = new System.Drawing.Point(179, 371);
            this.btnExit_.Name = "btnExit_";
            this.btnExit_.Size = new System.Drawing.Size(75, 23);
            this.btnExit_.TabIndex = 1;
            this.btnExit_.Text = "Exit";
            this.btnExit_.UseVisualStyleBackColor = true;
            this.btnExit_.Click += new System.EventHandler(this.btnExit__Click);
            // 
            // groupBox3_
            // 
            this.gb3_.Controls.Add(this.lbRecipients_);
            this.gb3_.Controls.Add(this.txtBox1_);
            this.gb3_.Controls.Add(this.btn1_);
            this.gb3_.Controls.Add(this.btn2_);
            this.gb3_.Location = new System.Drawing.Point(13, 212);
            this.gb3_.Name = "groupBox3_";
            this.gb3_.Size = new System.Drawing.Size(234, 153);
            this.gb3_.TabIndex = 10;
            this.gb3_.TabStop = false;
            this.gb3_.Text = "Recipents";
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
            // txtBox1_
            // 
            this.txtBox1_.Location = new System.Drawing.Point(40, 207);
            this.txtBox1_.Name = "txtBox1_";
            this.txtBox1_.Size = new System.Drawing.Size(100, 20);
            this.txtBox1_.TabIndex = 3;
            // 
            // button3_
            // 
            this.btn1_.Location = new System.Drawing.Point(97, 207);
            this.btn1_.Name = "button3_";
            this.btn1_.Size = new System.Drawing.Size(75, 23);
            this.btn1_.TabIndex = 1;
            this.btn1_.Text = "Cancel";
            this.btn1_.UseVisualStyleBackColor = true;
            // 
            // button4_
            // 
            this.btn2_.Location = new System.Drawing.Point(178, 207);
            this.btn2_.Name = "button4_";
            this.btn2_.Size = new System.Drawing.Size(75, 23);
            this.btn2_.TabIndex = 0;
            this.btn2_.Text = "Submit";
            this.btn2_.UseVisualStyleBackColor = true;
            // 
            // gbDescription_
            // 
            this.gbDescription_.Controls.Add(this.txtBox2_);
            this.gbDescription_.Controls.Add(this.rtxtDescription_);
            this.gbDescription_.Controls.Add(this.btn3_);
            this.gbDescription_.Controls.Add(this.btn4_);
            this.gbDescription_.Location = new System.Drawing.Point(13, 81);
            this.gbDescription_.Name = "gbDescription_";
            this.gbDescription_.Size = new System.Drawing.Size(234, 125);
            this.gbDescription_.TabIndex = 9;
            this.gbDescription_.TabStop = false;
            this.gbDescription_.Text = "Description";
            // 
            // textBox2_
            // 
            this.txtBox2_.Location = new System.Drawing.Point(40, 207);
            this.txtBox2_.Name = "textBox2_";
            this.txtBox2_.Size = new System.Drawing.Size(100, 20);
            this.txtBox2_.TabIndex = 3;
            // 
            // rtxtDescription_
            // 
            this.rtxtDescription_.Location = new System.Drawing.Point(6, 19);
            this.rtxtDescription_.Name = "rtxtDescription_";
            this.rtxtDescription_.ReadOnly = true;
            this.rtxtDescription_.Size = new System.Drawing.Size(222, 96);
            this.rtxtDescription_.TabIndex = 6;
            this.rtxtDescription_.Text = "";
            // 
            // button1_
            // 
            this.btn3_.Location = new System.Drawing.Point(97, 207);
            this.btn3_.Name = "button1_";
            this.btn3_.Size = new System.Drawing.Size(75, 23);
            this.btn3_.TabIndex = 1;
            this.btn3_.Text = "Cancel";
            this.btn3_.UseVisualStyleBackColor = true;
            // 
            // button2_
            // 
            this.btn4_.Location = new System.Drawing.Point(178, 207);
            this.btn4_.Name = "button2_";
            this.btn4_.Size = new System.Drawing.Size(75, 23);
            this.btn4_.TabIndex = 0;
            this.btn4_.Text = "Submit";
            this.btn4_.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete_.Location = new System.Drawing.Point(98, 371);
            this.btnDelete_.Name = "btnDelete";
            this.btnDelete_.Size = new System.Drawing.Size(75, 23);
            this.btnDelete_.TabIndex = 11;
            this.btnDelete_.Text = "Delete";
            this.btnDelete_.UseVisualStyleBackColor = true;
            this.btnDelete_.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox1
            // 
            this.gb1_.Controls.Add(this.lblDate_);
            this.gb1_.Controls.Add(this.lblDateHeader_);
            this.gb1_.Location = new System.Drawing.Point(13, 19);
            this.gb1_.Name = "groupBox1";
            this.gb1_.Size = new System.Drawing.Size(234, 56);
            this.gb1_.TabIndex = 12;
            this.gb1_.TabStop = false;
            this.gb1_.Text = "Info";
            // 
            // lblDateHeader
            // 
            this.lblDateHeader_.AutoSize = true;
            this.lblDateHeader_.Location = new System.Drawing.Point(6, 23);
            this.lblDateHeader_.Name = "lblDateHeader";
            this.lblDateHeader_.Size = new System.Drawing.Size(61, 13);
            this.lblDateHeader_.TabIndex = 13;
            this.lblDateHeader_.Text = "Created On";
            // 
            // lblDate
            // 
            this.lblDate_.AutoSize = true;
            this.lblDate_.Location = new System.Drawing.Point(73, 23);
            this.lblDate_.Name = "lblDate";
            this.lblDate_.Size = new System.Drawing.Size(40, 13);
            this.lblDate_.TabIndex = 14;
            this.lblDate_.Text = "lblDate";
            // 
            // InvitationInspectorDialogue
            // 
            this.ClientSize = new System.Drawing.Size(284, 434);
            this.Controls.Add(this.gbHeader_);
            this.Name = "InvitationInspectorDialogue";
            this.Text = "Invitation Inspector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InvitationInspectorDialogue_FormClosing);
            this.gbHeader_.ResumeLayout(false);
            this.gb3_.ResumeLayout(false);
            this.gb3_.PerformLayout();
            this.gbDescription_.ResumeLayout(false);
            this.gbDescription_.PerformLayout();
            this.gb1_.ResumeLayout(false);
            this.gb1_.PerformLayout();
            this.ResumeLayout(false);

        }
        // EVENTS
        // called when the form is closing
        private void InvitationInspectorDialogue_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
            {   // ignore the close event
                e.Cancel = true;    
                Hide(); // hide the form
            }
        }
        // called when btnDelete_ is pressed 
        private void btnDelete_Click(object sender, EventArgs e)
        {   // try to delete an invite
            if(ud_.CurrentUser.DeleteInvitation(invitation_, context_))
            {   // success
                ud_.UpdatePendingInvitations(); // update form elements
                ud_.UpdateSentInvitations();
                Hide(); // hide the form
            }   // fail
        }
    }
}
