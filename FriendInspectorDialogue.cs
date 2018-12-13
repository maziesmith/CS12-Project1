using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS12_Project_1
{
    public class FriendInspectorDialogue : Form
    {
        private Button btnExit;
        private GroupBox groupBox3;
        private ListBox lbFriends;
        private GroupBox groupBox2;
        private Label lblCity;
        private Label lblName;
        private Label label1;
        private Label label2;
        private GroupBox groupBox1;
        private ListBox lbInterests;
        private GroupBox gbHeader;
        private UserDialogue ud_;
        public FriendInspectorDialogue(in UserDialogue t_ud)
        {
            ud_ = t_ud;
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            this.gbHeader = new System.Windows.Forms.GroupBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbFriends = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbInterests = new System.Windows.Forms.ListBox();
            this.gbHeader.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbHeader
            // 
            this.gbHeader.Controls.Add(this.btnExit);
            this.gbHeader.Controls.Add(this.groupBox3);
            this.gbHeader.Controls.Add(this.groupBox2);
            this.gbHeader.Controls.Add(this.groupBox1);
            this.gbHeader.Location = new System.Drawing.Point(12, 12);
            this.gbHeader.Name = "gbHeader";
            this.gbHeader.Size = new System.Drawing.Size(260, 393);
            this.gbHeader.TabIndex = 0;
            this.gbHeader.TabStop = false;
            this.gbHeader.Text = "<Name>";
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(179, 358);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lbFriends);
            this.groupBox3.Location = new System.Drawing.Point(6, 219);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(248, 133);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Friends";
            // 
            // lbFriends
            // 
            this.lbFriends.FormattingEnabled = true;
            this.lbFriends.Location = new System.Drawing.Point(6, 19);
            this.lbFriends.Name = "lbFriends";
            this.lbFriends.Size = new System.Drawing.Size(236, 108);
            this.lbFriends.TabIndex = 1;
            this.lbFriends.DoubleClick += new System.EventHandler(this.lbFriends_DoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblCity);
            this.groupBox2.Controls.Add(this.lblName);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(6, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(248, 55);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Info";
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(47, 29);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(24, 13);
            this.lblCity.TabIndex = 5;
            this.lblCity.Text = "City";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(47, 16);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 4;
            this.lblName.Text = "Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "City";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbInterests);
            this.groupBox1.Location = new System.Drawing.Point(6, 80);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 133);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Interests";
            // 
            // lbInterests
            // 
            this.lbInterests.FormattingEnabled = true;
            this.lbInterests.Location = new System.Drawing.Point(6, 19);
            this.lbInterests.Name = "lbInterests";
            this.lbInterests.Size = new System.Drawing.Size(236, 108);
            this.lbInterests.TabIndex = 0;
            // 
            // FriendInspectorDialogue
            // 
            this.ClientSize = new System.Drawing.Size(285, 413);
            this.Controls.Add(this.gbHeader);
            this.Name = "FriendInspectorDialogue";
            this.Text = "Friend Inspector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FriendInspectorDialogue_FormClosing);
            this.gbHeader.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        public void Show(in Person t_next)
        {
            gbHeader.Text = t_next.UserName;
            lblName.Text = string.Format("{0} {1}", t_next.FirstName, t_next.LastName);
            lblCity.Text = t_next.City;
            lbFriends.Items.Clear();
            lbInterests.Items.Clear();
            t_next.friends_.ForEach(x => lbFriends.Items.Add(x.UserName));
            t_next.interests_.ForEach(x => lbInterests.Items.Add(x));
            Show();
            Focus();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Hide();
        }
        private void FriendInspectorDialogue_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
        private void lbFriends_DoubleClick(object sender, EventArgs e)
        {
            if (lbFriends.SelectedIndex == -1)
                return;
            ud_.LoadFriendInspectorDialogue(lbFriends.Items[lbFriends.SelectedIndex] as string);
        }
    }
}
