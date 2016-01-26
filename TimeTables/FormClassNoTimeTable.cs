using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TimeTables
{
    public partial class FormClassNoTimeTable : Form, iRefreshGridContent
    {
        private FormClassNoTimeTableDoubleClick_Action dact;
        private C_ClassNoTimeTable dact_dc;
        public int classno;
        public C_ClassNoTimeTable contr = null;
        private Form mdi_p = null;
        public FormClassNoTimeTable(string formtxt, C_ClassNoTimeTable c, C_ClassNoTimeTable adact,Form a_mdi_p)
            : base()
        {
            InitializeComponent();

            foreach (Object tb in this.tableLayoutPanel1.Controls)
            {
                if (tb is TextBox)
                {
                    TextBox aTextBox = (TextBox)tb;
                    aTextBox.ReadOnly = true;
                    aTextBox.MouseWheel += mWheel_click;
                    aTextBox.AllowDrop = true;
                    aTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.TextBox_DragDrop);
                    aTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.TextBox_DragEnter);
                    aTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextBox_MouseDown);
                    aTextBox.KeyPress += txtInput_KeyPress;
                }
            }
            this.Text = formtxt;
            classno= int.Parse(this.Text.Split('.')[0]);
            contr = c;
            dact_dc = adact;
            dact = new FormClassNoTimeTableDoubleClick_Action(formtxt+"Subject List",dact_dc);
        }
        private void txtInput_KeyPress(object sender,System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == 'D' || e.KeyChar == 'd')
            {
                TextBox tb = (TextBox)sender;
                int[] tempint = Basic_HTB_Info.TextBoxName2WeekLesson(tb);
                int weekno = tempint[0];
                int lessno = tempint[1];
                if (contr.GetGridCellSUID(weekno, lessno) > 0)
                {
                    ;
                    if (MessageBox.Show("delete " + contr.GetGridCellSUID(weekno, lessno).ToString() + contr.GridCellItemString(contr.GetGridCellSUID(weekno, lessno)), "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (Basic_HTB_Info.GetInstance().RemoveSubjectFromTimeTable(contr.GetGridCellSUID(weekno, lessno)))
                        {
                            RefreshContent();
                            MessageBox.Show("Deleted Success!");
                        }
                    }
                }
            }
            else if (e.KeyChar == 'T' || e.KeyChar == 't')
            {
                TextBox tb = (TextBox)sender;
                int[] tempint = Basic_HTB_Info.TextBoxName2WeekLesson(tb);
                int weekno = tempint[0];
                int lessno = tempint[1];
                if (contr.GetGridCellSUID(weekno, lessno) > 0)
                {
                    Basic_HTB_Info htb = Basic_HTB_Info.GetInstance();
                    int a_suid = contr.GetGridCellSUID(weekno, lessno);
                    int akey   =htb.unitkeyToTeachernos(a_suid)[0];

                    FormClassNoTimeTable aTT = new FormClassNoTimeTable(akey+"."+htb.htbIDTeacher[akey].ToString(), new C_ClassNoTimeTable_for_Teacher(akey), new C_FormClassNoTimeTableDoubleClick_Teacher(akey),mdi_p);
                    aTT.MdiParent = mdi_p;
                    aTT.RefreshContent();
                    aTT.Show();
                }
            }
            else if (e.KeyChar == 'C' || e.KeyChar == 'c')
            {
                TextBox tb = (TextBox)sender;
                int[] tempint = Basic_HTB_Info.TextBoxName2WeekLesson(tb);
                int weekno = tempint[0];
                int lessno = tempint[1];
                if (contr.GetGridCellSUID(weekno, lessno) > 0)
                {
                    Basic_HTB_Info htb = Basic_HTB_Info.GetInstance();
                    int a_suid = contr.GetGridCellSUID(weekno, lessno);
                    int akey = htb.unitkeyToClassnos(a_suid)[0];

                    FormClassNoTimeTable aTT = new FormClassNoTimeTable(akey + "." + htb.htbIDClass[akey].ToString(), new C_ClassNoTimeTable_for_Class(akey), new C_FormClassNoTimeTableDoubleClick_Class(akey), mdi_p);
                    aTT.MdiParent = mdi_p;
                    aTT.RefreshContent();
                    aTT.Show();
                }
            }
            else if (e.KeyChar == 'V' || e.KeyChar == 'v')
            {
                TextBox tb = (TextBox)sender;
                int[] tempint = Basic_HTB_Info.TextBoxName2WeekLesson(tb);
                int weekno = tempint[0];
                int lessno = tempint[1];
                Basic_HTB_Info htb = Basic_HTB_Info.GetInstance();
                int a_suid = contr.GetGridCellSUID(weekno, lessno);
                contr.View_Color_DVG_For_ExchangeCell(this.tableLayoutPanel1, a_suid, weekno, lessno);
            }
                /*
            else if ((!Char.IsControl(e.KeyChar)) && ((e.KeyChar < '0') || (e.KeyChar > '9')))
            {
             //   MessageBox.Show("1");
                e.Handled = true;
            }
            else
            {
             //   MessageBox.Show("2");
          */
         }
        private void mWheel_click(Object sender, EventArgs e)
        {
            if (dact.Visible) return;
            dact.updateList();
            dact.button_ADD.Visible = true;
            dact.MdiParent = null;
            if (dact.ShowDialog() == DialogResult.Yes)
            {
                TextBox tb = (TextBox)sender;
                int[] tempint = Basic_HTB_Info.TextBoxName2WeekLesson(tb);

                int weekno = tempint[0];
                int lessno = tempint[1];
                
              //  tb.Text = dact.resV;
                if (contr.AddSubjectToTimeTable(dact.resK, weekno, lessno))
                    RefreshContent();
            }

        }
        
        private void TextBox_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox txt = (TextBox)sender;
           // txt.SelectAll();
            txt.DoDragDrop(txt.Name, DragDropEffects.Copy);
        }

        private void TextBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void TextBox_DragDrop(object sender, DragEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            //txt.Text = (string)e.Data.GetData(DataFormats.Text);
            if (contr.ExchangeSubject((string)e.Data.GetData(DataFormats.Text), txt.Name)) RefreshContent();
        }

        public void RefreshContent()
        {
            String msg = "";
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            for(int i=0;i<htbs.TimeTableGrid.GetLength(1);i++)
                for (int j = 0; j < htbs.TimeTableGrid.GetLength(2); j++)
                {
                    TextBox tb = (TextBox) this.tableLayoutPanel1.Controls[ Basic_HTB_Info.WeekLesson2TextBoxName(i,j)];
                    msg += Basic_HTB_Info.WeekLesson2TextBoxName(i, j);
                    if (contr.GetGridCellSUID(i, j) > 0)
                    {
                        tb.Lines = contr.GridCellItemStringArr(contr.GetGridCellSUID(i, j));
                     }
                    else
                    {
                        try
                        {
                            tb.Text = "";
                        }
                        catch (Exception e1) { MessageBox.Show(e1.Message+msg); }
                    }
                }
            dact_dc.UpdateSUIDKeyListItems();
        }
        public void SetFontSize(Font f)
        {
            foreach (Object tb in this.tableLayoutPanel1.Controls)
            {
                if (tb is TextBox)
                {
                    TextBox aTextBox = (TextBox)tb;
                    aTextBox.Font = f;
                }
            }
        }


    }
  
}