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
        private GroupBox groupBox3_;
        private GroupBox gbDescription_;
        private GroupBox gbHeader_;
        private ListBox lbRecipients_;
        private TextBox txtBox1_;
        private TextBox textBox2_;
        private Button button3_;
        private Button button4_;
        private Button button1_;
        private Button button2_;
        private Button btnExit_;
        // other data members
        private UserDialogue ud_; // a reference to a user dialogue object
        public InvitationInspectorDialogue(in UserDialogue t_ud)
        {   // initalize all data members
            ud_ = t_ud;
            InitializeComponent();  // initalize form elements
        }
        // setup all form fields and then show this form
        public void Show(in Invitation t_next)
        {
            gbHeader_.Text = t_next.title;  // set the header to t_next's title
            rtxtDescription_.Text = t_next.body;    // set rtxtDescription_ to show the invitation body
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
            this.lbRecipients_ = new System.Windows.Forms.ListBox();
            this.groupBox3_ = new System.Windows.Forms.GroupBox();
            this.txtBox1_ = new System.Windows.Forms.TextBox();
            this.button3_ = new System.Windows.Forms.Button();
            this.button4_ = new System.Windows.Forms.Button();
            this.gbDescription_ = new System.Windows.Forms.GroupBox();
            this.textBox2_ = new System.Windows.Forms.TextBox();
            this.rtxtDescription_ = new System.Windows.Forms.RichTextBox();
            this.button1_ = new System.Windows.Forms.Button();
            this.button2_ = new System.Windows.Forms.Button();
            this.btnExit_ = new System.Windows.Forms.Button();
            this.gbHeader_.SuspendLayout();
            this.groupBox3_.SuspendLayout();
            this.gbDescription_.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbHeader_
            // 
            this.gbHeader_.Controls.Add(this.btnExit_);
            this.gbHeader_.Controls.Add(this.groupBox3_);
            this.gbHeader_.Controls.Add(this.gbDescription_);
            this.gbHeader_.Location = new System.Drawing.Point(12, 12);
            this.gbHeader_.Name = "gbHeader_";
            this.gbHeader_.Size = new System.Drawing.Size(260, 344);
            this.gbHeader_.TabIndex = 0;
            this.gbHeader_.TabStop = false;
            this.gbHeader_.Text = "groupBox1";
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
            // groupBox3_
            // 
            this.groupBox3_.Controls.Add(this.lbRecipients_);
            this.groupBox3_.Controls.Add(this.txtBox1_);
            this.groupBox3_.Controls.Add(this.button3_);
            this.groupBox3_.Controls.Add(this.button4_);
            this.groupBox3_.Location = new System.Drawing.Point(13, 150);
            this.groupBox3_.Name = "groupBox3_";
            this.groupBox3_.Size = new System.Drawing.Size(234, 153);
            this.groupBox3_.TabIndex = 10;
            this.groupBox3_.TabStop = false;
            this.groupBox3_.Text = "Recipents";
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
            // gbDescription_
            // 
            this.gbDescription_.Controls.Add(this.textBox2_);
            this.gbDescription_.Controls.Add(this.rtxtDescription_);
            this.gbDescription_.Controls.Add(this.button1_);
            this.gbDescription_.Controls.Add(this.button2_);
            this.gbDescription_.Location = new System.Drawing.Point(13, 19);
            this.gbDescription_.Name = "gbDescription_";
            this.gbDescription_.Size = new System.Drawing.Size(234, 125);
            this.gbDescription_.TabIndex = 9;
            this.gbDescription_.TabStop = false;
            this.gbDescription_.Text = "Description";
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
            this.rtxtDescription_.ReadOnly = true;
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
            // btnExit_
            // 
            this.btnExit_.Location = new System.Drawing.Point(179, 309);
            this.btnExit_.Name = "btnExit_";
            this.btnExit_.Size = new System.Drawing.Size(75, 23);
            this.btnExit_.TabIndex = 1;
            this.btnExit_.Text = "Exit";
            this.btnExit_.UseVisualStyleBackColor = true;
            this.btnExit_.Click += new System.EventHandler(this.btnExit__Click);
            // 
            // InvitationInspectorDialogue
            // 
            this.ClientSize = new System.Drawing.Size(284, 364);
            this.Controls.Add(this.gbHeader_);
            this.Name = "InvitationInspectorDialogue";
            this.Text = "Invitation Inspector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InvitationInspectorDialogue_FormClosing);
            this.gbHeader_.ResumeLayout(false);
            this.groupBox3_.ResumeLayout(false);
            this.groupBox3_.PerformLayout();
            this.gbDescription_.ResumeLayout(false);
            this.gbDescription_.PerformLayout();
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
    }
}
