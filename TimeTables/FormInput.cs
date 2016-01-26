using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TimeTables
{
    public partial class FormInput : Form
    {
        public String out_res{
            get
            {
                return textBox1.Text;
            }
        }
        public FormInput(String le)
        {
            InitializeComponent();
            label1.Text = le;
            button1.Click += new EventHandler(button1_Click);
            textBox1.KeyDown += new KeyEventHandler(textBox1_KeyDown);
        }

        void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
//            throw new Exception("The method or operation is not implemented.");

            if (textBox1.Text != "" && e.KeyCode==Keys.Enter)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

        }

        void button1_Click(object sender, EventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");

            if (textBox1.Text != "")
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }


    }
    public class TeachernoInput : FormInput
    {
        public TeachernoInput(String prompt):base(prompt)
        {
            
        }

    }
}