using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Odbc;
using System.IO;
using System.Collections;

namespace TimeTables
{
    struct TimeTableElm_struct
    {
        string teacher;
        string coursename;
        public TimeTableElm_struct(string ateacher,string acoursename) {
            teacher = ateacher;
            coursename = acoursename;
        }
    }
    public partial class Form1 : Form
    {
       
        Hashtable htbTeacherID = new Hashtable();
        Hashtable htbIDTeacher = new Hashtable();
        Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
      //  private static readonly string cnst5_1DriverConnStrFormat = "Driver={MySQL ODBC 5.1 Driver};Server=192.168.101.250;Database=eschool;UID=root;PWD=winmbc;OPTION=3";
        public Form1()
        {
            InitializeComponent();
            foreach (String s in htbs._rt)
            {
                this.Rtb.AppendText(s+"\n");
            }
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            FormAssgControl fac = new FormAssgControl();
            fac.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            FormCourseAssignGrid fcg = new FormCourseAssignGrid(new C_FormCourseAssignGridForSubjectUnit() );
            fcg.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            FormTimeTableMDI fMDI = new FormTimeTableMDI();
            fMDI.ShowDialog();
        }

        private void btnTeacherWeekResetLessonno_Click(object sender, EventArgs e)
        {
            FormCourseAssignGrid fcg = new FormCourseAssignGrid(new C_FormCourseAssignGridForTeacherRest());
            fcg.ShowDialog();
        }



        private void btnClassTakeBreak_Click(object sender, EventArgs e)
        {

        }
    }
}