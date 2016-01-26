using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;

namespace TimeTables
{
    public partial class FormAssgControl : Form
    {
        public FormAssgControl()
        {
            InitializeComponent();
            //MessageBox.Show(Basic_HTB_Info.baseFilePath);
            SQLite_ESData sqe = new SQLite_ESData(Basic_HTB_Info.baseFilePath, "data");
            DbDataReader dr =sqe.Reader("Select * from data_tbl;");
            while (dr.Read())
            {
                textBox1.Text = dr.GetString(0);
                textBox2.Text = dr.GetString(1);
                textBox3.Text = dr.GetString(2);

            }
            dr.Close();
            dr.Dispose();
            MessageBox.Show(sqe.ViewTableTxt("data_tbl"));
            sqe.Close_Conn();
            textBox1.TextChanged += new EventHandler(textBox1_TextChanged);
            textBox2.TextChanged += new EventHandler(textBox2_TextChanged);
            textBox3.TextChanged += new EventHandler(textBox3_TextChanged);
        }

        void textBox3_TextChanged(object sender, EventArgs e)
        {
            SQLite_ESData.GetInst.Open_Conn();
            SQLite_ESData.GetInst.Exec(String.Format("update data_tbl set MAIN_SUBJ_IDs='{0}';", textBox3.Text));
            SQLite_ESData.GetInst.Close_Conn();

        }
        void textBox2_TextChanged(object sender, EventArgs e)
        {
            SQLite_ESData.GetInst.Open_Conn();
            SQLite_ESData.GetInst.Exec(String.Format("update data_tbl set ASSG_SUBJ_PLACE_SET='{0}';",textBox2.Text));
            SQLite_ESData.GetInst.Close_Conn();
            
        }

        void textBox1_TextChanged(object sender, EventArgs e)
        {
            SQLite_ESData.GetInst.Open_Conn();
            SQLite_ESData.GetInst.Exec(String.Format("update data_tbl set ASSG_SUBJ_IDs='{0}';", textBox1.Text));
            SQLite_ESData.GetInst.Close_Conn();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            htbs.LastErrors.Clear();
            List<int> mainsubjids = new List<int>();
            String[] stra = textBox3.Text.Split(',');
                foreach (String s in stra)
                {
                    int into = 0;
                    if (int.TryParse(s, out into))
                    {
                        mainsubjids.Add(into);
                    }
                }
            htbs.Sequence_ByClassMaster_Assig(mainsubjids);
            if (htbs.LastErrors.Count > 0)
            {
                MsgBox msgbox = new MsgBox(htbs.LastErrors.ToArray());
                msgbox.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<int> int_list = new List<int>();
            string[] s_arr=textBox1.Text.Split(',');
            foreach (string s_n in s_arr)
            {
                int outint;
                if (int.TryParse(s_n, out outint))
                {
                    int_list.Add(outint);
                }
            }
            if (int_list.Count == 0) return;

            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            htbs.LastErrors.Clear();
            htbs.Sequence_By_SubjectIDList(int_list.ToArray());
            if (htbs.LastErrors.Count > 0)
            {
                MsgBox msgbox = new MsgBox(htbs.LastErrors.ToArray());
                msgbox.ShowDialog();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            htbs.LastErrors.Clear();
            htbs.Sequence_By123_Assig(100);
            if (htbs.LastErrors.Count > 0)
            {
                MsgBox msgbox = new MsgBox(htbs.LastErrors.ToArray());
                msgbox.ShowDialog();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            String[] stra = textBox2.Text.Split(';');
            for (int i=0; i<stra.Length;i++)
            {
                String s=stra[i];
                if (s.Length > 3)
                {
                    Basic_HTB_Info.GetInstance().ASSG_SUBJSET_PLACE_RESTRICT_[i, 0] = int.Parse(s[s.Length - 1].ToString());
                    String[] strb = s.Substring(1, s.Length - 3).Split(',');
                    for (int j = 0; j < strb.Length; j++)
                    {
                        Basic_HTB_Info.GetInstance().ASSG_SUBJSET_PLACE_RESTRICT_[i, j+1] = int.Parse(strb[j]);
                    }

                }

            }

        }
    }
}