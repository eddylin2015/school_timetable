using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using System.IO;

namespace TimeTables
{
    public partial class FormAssgControl : Form
    {
        private String path = Basic_HTB_Info.baseFilePath + @"\FORMASSG_config.txt";
        public FormAssgControl()
        {
            InitializeComponent();

            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("1,2,3,4,5,6,7,8,10,11,12,13");
                    sw.WriteLine("{11,12,13}2");
                    sw.WriteLine("2,3,4,5,6,7,8");
                    sw.Flush();
                }
            }
            else
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    int cnt = 0;
                    while ((s = sr.ReadLine()) != null)
                    {
                        cnt++;
                        switch (cnt)
                        {
                            case 1:textBox1.Text=s;break;
                            case 2: textBox2.Text = s; break;
                            case 3: textBox3.Text = s; break;
                        }
                    }
                }
            }

            //MessageBox.Show(Basic_HTB_Info.baseFilePath);
            textBox1.TextChanged += new EventHandler(textBox1_TextChanged);
            textBox2.TextChanged += new EventHandler(textBox2_TextChanged);
            textBox3.TextChanged += new EventHandler(textBox3_TextChanged);
        }
        private void  edit_txtfile()
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(textBox1.Text);
                sw.WriteLine(textBox2.Text);
                sw.WriteLine(textBox3.Text);
            }
        }
        void textBox3_TextChanged(object sender, EventArgs e)
        {

            edit_txtfile();
        }
        void textBox2_TextChanged(object sender, EventArgs e)
        {

            edit_txtfile();
        }

        void textBox1_TextChanged(object sender, EventArgs e)
        {
            edit_txtfile();
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