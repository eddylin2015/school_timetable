using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace TimeTables
{
    public partial class FormClassNoTimeTableDoubleClick_Action : Form, iRefreshGridContent
    {
        public FormClassNoTimeTableDoubleClick_Action(string txt, C_ClassNoTimeTable aContr)
            : base()
        {
            InitializeComponent();
            contr = aContr;
            this.Text = txt;
            a_key = int.Parse((txt.Split('.')[0]));
            foreach (Object tb in this.tableLayoutPanel1.Controls)
            {
                if (tb is TextBox)
                {
                    TextBox aTextBox = (TextBox)tb;
                    aTextBox.ReadOnly = true;
                    aTextBox.MouseClick += gridmouseClick;
                    aTextBox.KeyPress += txtInput_KeyPress;
                    //aTextBox.MouseWheel += mWheel_click;
                    //       aTextBox.AllowDrop = true;
                    //       aTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.TextBox_DragDrop);
                    //       aTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.TextBox_DragEnter);
                 }
            }
        }
        public int a_key;
        public int resK;
        public string resV;
        private C_ClassNoTimeTable contr;
        public void updateList()
        {
            this.listBox1.Items.Clear();
            foreach (string s in contr.ListStr())
            {
                this.listBox1.Items.Add(s);
            }
            Basic_HTB_Info htbs=Basic_HTB_Info.GetInstance();
            for (int i = 0; i < htbs.TimeTableGrid.GetLength(1); i++)
            {
                for (int j = 0; j < htbs.TimeTableGrid.GetLength(2); j++)
                {
                    TextBox tb = (TextBox)this.tableLayoutPanel1.Controls[  Basic_HTB_Info.WeekLesson2TextBoxName(i,j) ];
                    string c =contr.GridCellItemString(    contr.GetGridCellSUID(i,j));
                    if (c == null)
                    {
                        tb.BackColor = Color.Gray;
                        tb.Enabled = false;
                    }
                    else
                    {
                        tb.BackColor = Color.Red;
                        tb.Text = c;
                    }
                }
            }

        }
        public void RefreshContent()
        {
            contr.UpdateSUIDKeyListItems();
            updateList();
        }
        public void SetFontSize(System.Drawing.Font f) { }
        private void gridmouseClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
            Basic_HTB_Info htbs=Basic_HTB_Info.GetInstance();
            int resK=0;
            string[] sr = this.listBox1.Items[listBox1.SelectedIndex].ToString().Split(':');
            if (sr.Length > 1)
            {
                resK = int.Parse(sr[0]);
            }
                            TextBox tb = (TextBox)sender;

                //int tmpint=int.Parse(tb.Name.Substring(7));
                int[] int0 = Basic_HTB_Info.TextBoxName2WeekLesson(tb);
                int weekno = int0[0];
                int lessno = int0[1];
                if (contr.AddSubjectToTimeTable(resK, weekno, lessno)) updateList();
            //htbs.AddSubjectToTimeTable(resK,weekno,lessno);
            
        }
        private void txtInput_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == 'D' || e.KeyChar == 'd')
            {
                TextBox tb = (TextBox)sender;
                int[] tempint = Basic_HTB_Info.TextBoxName2WeekLesson(tb);
                int weekno = tempint[0];
                int lessno = tempint[1];
                if (contr.GetGridCellSUID(weekno, lessno) > 0)
                {
                    if (MessageBox.Show("delete " + contr.GetGridCellSUID(weekno, lessno).ToString(), "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Basic_HTB_Info.GetInstance().RemoveSubjectFromTimeTable(contr.GetGridCellSUID(weekno, lessno));
                        this.RefreshContent();
                    }
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string[] sr = this.listBox1.Items[listBox1.SelectedIndex].ToString().Split(':');
                resK = int.Parse(sr[0]);
                resV = sr[1];
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;
               string[] sr = this.listBox1.Items[listBox1.SelectedIndex].ToString().Split(':');
            if (sr.Length > 1)
            {
                resK = int.Parse(sr[0]);
                resV = sr[1];
                this.DialogResult = DialogResult.Yes;
            }
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {

            if (listBox1.SelectedIndex == -1) return;
            string[] sr = this.listBox1.Items[listBox1.SelectedIndex].ToString().Split(':');
            if (sr.Length > 1)
            {
                int a_suid = int.Parse(sr[0]);
                resV = sr[1];
                contr.View_Color_DVG_For_FreeCell(this.tableLayoutPanel1, a_suid);
      
            }
        }
    }
 
}