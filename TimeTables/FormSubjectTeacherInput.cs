using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TimeTables
{
    public partial class FormSubjectTeacherInput : Form
    {
        public FormSubjectTeacherInput()
        {
            InitializeComponent();
            Basic_HTB_Info bths = Basic_HTB_Info.GetInstance();
            for(int i=1;i<=bths.htbIDTeacher.Count;i++)
            comboBox1.Items.Add(bths.htbIDTeacher[i]);
        }
        public string res = null;

        private void button1_Click(object sender, EventArgs e)
        {
            System.Text.RegularExpressions.Regex reg=new System.Text.RegularExpressions.Regex(@"\d+");
            if (comboBox1.Text != "" && reg.IsMatch(textBox1.Text)&& reg.IsMatch(textBox2.Text))
            {
                res = string.Format("{0}({1},{2})", comboBox1.Text, textBox1.Text, textBox2.Text);
            }
            else
            {
                res = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"\d+");
            if (listBox1.SelectedIndex > -1 && reg.IsMatch(textBox1.Text) && reg.IsMatch(textBox2.Text))
            {
                res = string.Format("{0}({1},{2})",listBox1.Items[listBox1.SelectedIndex].ToString(),textBox1.Text,textBox2.Text);
            }
            else
                res = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Basic_HTB_Info.GetInstance().Load_VirtualSubject_MultiTeachers(); this.listBox1.Items.Clear();
            foreach(string s in Basic_HTB_Info.GetInstance().VirtualSubject_MultiTeachers)
            this.listBox1.Items.Add(s);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ListBoxForm lf = new ListBoxForm();
            Basic_HTB_Info bths = Basic_HTB_Info.GetInstance();
            int cnt = 0;
            foreach (string s in listBox1.Items)
            {
                String temp_s = s.Split('{')[0];
                if (!s.Equals("") && int.Parse(temp_s) > cnt) cnt = int.Parse(temp_s);
            }
            for (int i = 1; i <= bths.htbIDTeacher.Count; i++)
                lf.lb.Items.Add(bths.htbIDTeacher[i]);
            if(lf.ShowDialog()==DialogResult.OK)
            {
                string ts=null;
               foreach(string s in lf.lb.CheckedItems)
               {
                   if(ts ==null )
                   {
                       ts=s;
                   }else{
                       ts+=","+s;
                   }
               }
                cnt++;
                if(cnt<3) cnt=3;
                string temps=string.Format("\n{0}{{{1}}}",cnt,ts);
                bths.Append_TeacherRest_file(temps);
            }

            MessageBox.Show("append suc & please refresh list!");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TeachernoInput ti = new TeachernoInput("Teacher No.");
            int inta = 0;
            if (ti.ShowDialog() == DialogResult.OK&&int.TryParse(ti.out_res,out inta))
            {
                if (Basic_HTB_Info.GetInstance().htbIDTeacher.ContainsKey(inta))
                    comboBox1.Text = Basic_HTB_Info.GetInstance().htbIDTeacher[inta].ToString();

            }
        }
    }

}