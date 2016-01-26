using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
namespace TimeTables
{
    public partial class FormCourseAssignGrid : Form,iRefreshGridContent
    {
        private Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
        private C_FormCourseAssignGrid contr = null;
        public FormCourseAssignGrid(C_FormCourseAssignGrid c_FormCAG)
            : base()
        {
            InitializeComponent();
            contr = c_FormCAG;
            contr.InitializeComponent(this.dGV);
        }
        private FormCourseAssignGrid()
        {

        }

        private void tslSaveFile_Click(object sender, EventArgs e)
        {
            contr.Save_File(this.dGV);
        }

        private void tslLoadFile_Click(object sender, EventArgs e)
        {
            contr.Load_File(this.dGV);
        }

        private void dGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           string res= contr.dGVCell_EditItem_Dialog(e.RowIndex, e.ColumnIndex, this.dGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
           contr.RefreshGridContent(this.dGV);
           if (res != null)
           {
               this.dGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = res;
               
           }
        }

        private void tslDefineMinUnit_Click(object sender, EventArgs e)
        {
                contr.Export_File( this.dGV);
        }
        public void RefreshContent()
        {
            contr.RefreshGridContent(this.dGV);
        }
        public void SetFontSize(Font f)
        {
        }
    }

    public class C_FormCourseAssignGrid
    {

        public static int[] CellTextToLessonCnt(string CellText)
        {
            int[] resInta = new int[2];
            string[] sb = CellText.Split(',');
            resInta[0] = int.Parse(sb[0]);
            resInta[1] = int.Parse(sb[1][0].ToString());
            return resInta;
        }
        protected void savefile(string FileName,DataGridView dGV)
        {
            StreamWriter sw = new StreamWriter(FileName, false);
            int cols = dGV.Columns.Count;
            int rows = dGV.Rows.Count;
            for (int i = 0; i < rows; i++)
                for (int j = 1; j < cols; j++)
                {
                    sw.Write("{0}:{1}\n", j + i * 100, dGV.Rows[i].Cells[j].Value);
                }
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
        protected void loadfile(String FileName,DataGridView dGV)
        {
            StreamReader sr = new StreamReader(FileName);
            string input = null;
            while ((input = sr.ReadLine()) != null)
            {
                string[] ar = input.Split(':');
                if (ar.Length > 1)
                {
                    if (ar[1].Equals("")) continue;
                    int intx = int.Parse(ar[0]);
                    int cols = intx % 100;
                    int rows = intx / 100;
                    dGV.Rows[rows].Cells[cols].Value = ar[1];
                }
            }
            sr.Close();
            sr.Dispose();
        }
        public virtual string dGVCell_EditItem_Dialog(int rowindex, int columnindex, Object preValue)
        {
            return null;
        }
        public virtual void Export_File(DataGridView dGV)
        {
            
        }
        public virtual void Load_File(DataGridView dGV)
        {
        }
        public virtual void Save_File(DataGridView dGV)
        {
        }

        public virtual void InitializeComponent(DataGridView dGV)
        {
        }
        public virtual void RefreshGridContent(DataGridView dGV)
        {
        }
        public virtual string CellShowFormatString()
        {
            return null;
        }
    }
    public class C_FormCourseAssignGridForSubjectUnit : C_FormCourseAssignGrid
    {
        FormSubjectTeacherInput fstinput = new FormSubjectTeacherInput();
        public override void InitializeComponent(DataGridView dGV)
        {
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            for (int i = 1; i <= htbs.htbIDClass.Count; i++)
            {
                DataGridViewTextBoxColumn tc = new DataGridViewTextBoxColumn();
                tc.HeaderText = string.Format("{0}\n{1}", i, htbs.htbIDClass[i]);
                tc.Name = string.Format("{0}{1}", i, htbs.htbIDClass[i]);
                dGV.Columns.Add(tc);
            }
            dGV.Rows.Add(htbs.htbIDSubject.Count + 1);
            dGV.Rows[0].Cells["btnSubjectClassno"].Value = "班任";
            for (int i = 1; i <= htbs.htbIDSubject.Count; i++)
            {
                dGV.Rows[i].Cells["btnSubjectClassno"].Value = string.Format("{0}\n{1}", i, htbs.htbIDSubject[i]);
            }
            
        }

        public override void Load_File(DataGridView dGV)
        {
        //    base.Load_File(dGV);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = Basic_HTB_Info.baseFilePath + @"\timetable_data.dat";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                loadfile(ofd.FileName,dGV);
                for (int i = 0; i < Basic_HTB_Info.GetInstance().htbIDClassMaster.Count; i++)
                    dGV.Rows[0].Cells[i + 1].Value = Basic_HTB_Info.GetInstance().htbIDClassMaster[i+1];
            }
        }
        public override void Save_File(DataGridView dGV)
        {
            //base.Save_File(dGV);
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = Basic_HTB_Info.baseFilePath + @"\timetable_data.dat";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                savefile(sfd.FileName, dGV);
            }
        }


        public override void Export_File(DataGridView dGV)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.FileName = Basic_HTB_Info.baseFilePath + @"\BasicInfo\TimeTable_Def_LessonUnit.dat";
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                {

                    Hashtable IDOpenCourese = new Hashtable();
                    Hashtable OpenCoureseClasses = new Hashtable();
                    
                    StreamWriter sr = new StreamWriter(SFD.FileName);
                    int cols = dGV.Columns.Count;
                    int rows = dGV.Rows.Count;
                    for (int i = 1; i < rows; i++)
                        for (int j = 1; j < cols; j++)
                        {
                            if (dGV.Rows[i].Cells[j].Value != null && dGV.Rows[i].Cells[j].Value.ToString().Length > 1)
                            {

                                String celltxt = dGV.Rows[i].Cells[j].Value.ToString();
                                if (!celltxt.Contains("("))
                                {
                                    MessageBox.Show(celltxt + "\n error (1,0)");
                                    continue;
                                }
                                celltxt += "(1,0)";
                                string[] sa = celltxt.Split('(');
                                int scnt = 0;
                                int tcnt = 0;
                                try
                                {
                                    int[] inta = CellTextToLessonCnt(sa[1]);
                                    scnt = inta[0];
                                    tcnt = inta[1];//XXXX(6,1) 6節,有一次連堂
                                }
                                catch (Exception except)
                                {
                                    MessageBox.Show(except.Message +"\n"+ celltxt+"\n"+sa[1]);
                                    continue;
                                }

                                String num_str = null;
                                int LessonTypeforVCID = 0;
                                {
                                    char c = dGV.Rows[i].Cells[j].Value.ToString()[0];
                                    char c1 = dGV.Rows[i].Cells[j].Value.ToString()[1];
                                    if (c >= '0' && c <= '9' && c1 >= '0' && c1 <= '9')
                                    {
                                        num_str = "" + c + c1;
                                    }
                                    else if (c >= '0' && c <= '9')
                                    {
                                        num_str = "" + c;
                                    }
                                    if (num_str != null) LessonTypeforVCID = int.Parse(num_str);
                                }//VCID XX{teacher1,teacher2}

                                
                                
                                try
                                {
                                    if (LessonTypeforVCID == 10)
                                    {
                                        int debug_i = 1;
                                    }
                                    if (LessonTypeforVCID > 2)//if (c > '2' && c <= '9')
                                    {
                                        if (!IDOpenCourese.Contains(Basic_HTB_Info.GenSUID(i, 0, 0, scnt, LessonTypeforVCID)))
                                        {
                                            string CourseDescriptionn = dGV.Rows[i].Cells[j].Value.ToString() + htbs.htbIDSubject[i];
                                            int startposi = dGV.Rows[i].Cells[j].Value.ToString().IndexOf('{');
                                            int endposi = dGV.Rows[i].Cells[j].Value.ToString().IndexOf('}');
                                            string temps = dGV.Rows[i].Cells[j].Value.ToString().Substring(startposi + 1, endposi - startposi - 1);
                                            string[] teachers = temps.Split(',');
                                            string teacherIDs = "";
                                            foreach (string s in teachers)
                                            {
                                                teacherIDs += htbs.htbTeacherID[s] + ",";
                                            }
                                            IDOpenCourese.Add(Basic_HTB_Info.GenSUID(i, 0, 0, scnt, LessonTypeforVCID), CourseDescriptionn + "#" + teacherIDs.Remove(teacherIDs.Length - 1));
                                            OpenCoureseClasses.Add(Basic_HTB_Info.GenSUID(i, 0, 0, scnt, LessonTypeforVCID), j.ToString());
                                        }
                                        else
                                        {
                                            OpenCoureseClasses[Basic_HTB_Info.GenSUID(i, 0, 0, scnt, LessonTypeforVCID)] = OpenCoureseClasses[Basic_HTB_Info.GenSUID(i, 0, 0, scnt, LessonTypeforVCID)].ToString() + "," + j;
                                        }
                                    }
                                    else
                                    {
                                        string CourseDescriptionn = dGV.Rows[i].Cells[j].Value.ToString() + htbs.htbIDClass[j] + htbs.htbIDSubject[i];
                                        int ia = 0;
                                        if (tcnt == 0)
                                        {
                                            for (ia = 1; ia <= scnt - tcnt; ia++)
                                            {
                                                sr.Write("{0}:{1}\n", Basic_HTB_Info.GenSUID(i, j, int.Parse(htbs.htbTeacherID[sa[0]].ToString()), ia, 1), CourseDescriptionn);
                                            }
                                        }
                                        else if (tcnt > 0)
                                        {
                                            for (ia = 1; ia < scnt - tcnt; ia++)
                                            {
                                                sr.Write("{0}:{1}\n", Basic_HTB_Info.GenSUID(i, j, int.Parse(htbs.htbTeacherID[sa[0]].ToString()), ia, 1), CourseDescriptionn);
                                            }

                                            for (int ib = 0; ib < tcnt; ib++)
                                            {
                                                sr.Write("{0}:{1}\n", Basic_HTB_Info.GenSUID(i, j, int.Parse(htbs.htbTeacherID[sa[0]].ToString()), ia, 2), CourseDescriptionn);
                                                ia++;
                                            }
                                        }
                                    }
                                }
                                catch (Exception except)
                                {
                                    MessageBox.Show(dGV.Rows[i].Cells[j].Value.ToString() + except.Message);

                                }
                            }
                        }
                        
                    foreach (DictionaryEntry de in IDOpenCourese)
                    {
                        int akey = int.Parse(de.Key.ToString());
                        int cnt =htbs.unitkey2cnt( akey);
                        for (int i = 1; i < cnt + 1; i++)
                        {
                            sr.WriteLine("{0}:{1}@{2}", htbs.unitkey_repace_cnt( akey ,cnt, i), de.Value, OpenCoureseClasses[de.Key]);
                        }
                    }
                    sr.Flush();
                    sr.Close();
                    sr.Dispose();
                }
  /*            htbs.htbIDLessionUnit.Clear();
                htbs.htbIDLessionClasses.Clear();
                htbs.htbIDLessionTeachers.Clear();
   * */
                htbs.Load_Lesson_Unit_Def_File(SFD.FileName);
            }
        }
        public override string dGVCell_EditItem_Dialog(int rowindex, int columnindex, Object preValue)
        {
            //return base.dGVCell_EditItem_Dialog(rowindex, columnindex, preValue);
            string res = null;
            if (rowindex > 0 && columnindex > 0)
            {
                DialogResult dr = fstinput.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    res= fstinput.res;
                }
                else if (dr == DialogResult.Yes)
                {
                    res= fstinput.res;
                }
            }
            return res;
        }
    }
    public class C_FormCourseAssignGridForPreAssigSubjectTeacher : C_FormCourseAssignGrid
    {
        public override void InitializeComponent(DataGridView dGV)
        {
            dGV.ReadOnly = true;
            string Error_Str = "";
            try
            {
                Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
                
                for (int i = 1; i <= htbs.htbIDClass.Count; i++)
                {
                    DataGridViewTextBoxColumn tc = new DataGridViewTextBoxColumn();
                    tc.HeaderText = string.Format("{0}.{1}", i, htbs.htbIDClass[i]);
                    tc.Name = string.Format("{0}{1}", i, htbs.htbIDClass[i]);
                    dGV.Columns.Add(tc);
                    Error_Str += tc.Name;
                }

                dGV.Rows.Add(htbs.htbIDSubject.Count + 1);
                dGV.Rows[0].Cells["btnSubjectClassno"].Value = "班任";
                for (int i = 1; i <= htbs.htbIDSubject.Count; i++)
                {
                    dGV.Rows[i].Cells["btnSubjectClassno"].Value = string.Format("{0}.{1}", i, htbs.htbIDSubject[i]);
                    Error_Str += dGV.Rows[i].Cells["btnSubjectClassno"].Value;
                }
                
            }catch(Exception except)
            {
                MessageBox.Show(Error_Str + except.Message);
            }
        }
        public override void RefreshGridContent(DataGridView dGV)
        {
            string[] weekstr=new string[]{"一","二","三","四","五","SA","SU",};
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();

            for (int i = 1;i <= htbs.htbIDSubject.Count; i++)
            {
                 for (int j = 1; j <= htbs.htbIDClass.Count; j++)    
                {
                    int sbjid =int.Parse(dGV.Rows[i].Cells[0].Value.ToString().Split('.')[0]);
                    int classno = int.Parse(dGV.Columns[j].HeaderText.Split('.')[0]);
                    int tmpsbjunit = Basic_HTB_Info.GenSUID(sbjid, classno, 0, 0, 0);
                    dGV.Rows[i].Cells[j].Value="";
                        for (int l = 0; l < htbs.TimeTableGrid.GetLength(1); l++)
                            for (int m = 0; m < htbs.TimeTableGrid.GetLength(2); m++)
                            {
                                try
                                {
                                    if (htbs.unitkey2sbjidclassno(tmpsbjunit) == htbs.unitkey2sbjidclassno(htbs.TimeTableGrid[j - 1, l, m]))
                                    {
                                        dGV.Rows[i].Cells[j].Value += string.Format("{0}{1}", weekstr[l], m + 1);
                                        if (dGV.Columns[j].Width < dGV.Rows[i].Cells[j].Value.ToString().Length * 10)
                                            dGV.Columns[j].Width = dGV.Rows[i].Cells[j].Value.ToString().Length * 10;
                                    }
                                    else if (htbs.unitkey2sbjid( htbs.TimeTableGrid[j - 1, l, m])== sbjid && htbs.unitkey2classno( htbs.TimeTableGrid[j - 1, l, m] ) == 0)
                                    {
                                        int[] suid_class = htbs.unitkeyToClassnos(htbs.TimeTableGrid[j - 1, l, m]);
                                        foreach (int a_tmp_class in suid_class)
                                        {
                                            if (a_tmp_class == classno)
                                            {
                                                dGV.Rows[i].Cells[j].Value += string.Format("{0}{1}", weekstr[l], m + 1);
                                                if (dGV.Columns[j].Width < dGV.Rows[i].Cells[j].Value.ToString().Length * 10)
                                                    dGV.Columns[j].Width = dGV.Rows[i].Cells[j].Value.ToString().Length * 10;
                                            }
                                        }
                                    }
                                }catch(Exception except){
                                    String Message_str = string.Format("sbjid={0}-classno={1}-TimeTableGrid={2}\n", sbjid, classno, htbs.TimeTableGrid[j - 1, l, m]);
                                    MessageBox.Show(Message_str+except.Message);
                                }
                            }
                }
            }
        }
        public override void Load_File(DataGridView dGV)
        {
        }
        public override void Save_File(DataGridView dGV)
        {
        }
        public override void Export_File(DataGridView dGV)
        {
        }
        public override string dGVCell_EditItem_Dialog(int rowindex, int columnindex, Object preValue)
        {
            List<int> preValueList=new List<int>();
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            int sbjid = rowindex;
            int classno = columnindex;
                int subjid =Basic_HTB_Info.GenSUID(sbjid, classno, 0, 0, 0);
                foreach (DictionaryEntry de in htbs.htbIDLessionUnit)
                {
                    int akey = int.Parse(de.Key.ToString());
                    if ( htbs.unitkey2LessonType(akey) <3 && htbs.unitkey2sbjidclassno( subjid) == htbs.unitkey2sbjidclassno( akey))
                    {
                       // preValue += string.Format("{0}:;", akey);
                        preValueList.Add(akey);
                    }
                    else if (htbs.unitkey2LessonType( akey) > 2 && htbs.unitkey2sbjidclassno(subjid) == htbs.unitkey2sbjidclassno(akey))
                    {
                        foreach (int a_class_no in htbs.unitkeyToClassnos(akey))
                        {
                            if (a_class_no == classno)
                            {
                                //       preValue += string.Format("{0}:;", akey);
                                preValueList.Add(akey);
                            }
                        }
                    }
                }
            FormClassNoTimeTableDoubleClick_Action dact = new FormClassNoTimeTableDoubleClick_Action("1.abc", new C_FormClassNoTimeTableDoubleClick_PreAssigSubj( preValueList));
            dact.updateList();
            dact.button_ADD.Visible = false;
            dact.MdiParent = null;
            if (dact.ShowDialog() == DialogResult.Yes) {}
            return null;
        }
    }
    public class C_FormCourseAssignGridForTeacherRest : C_FormCourseAssignGrid
    {
        public override void InitializeComponent(DataGridView dGV)
        {
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            string[] week=new string[]{"一","二","三","四","五"};
            for (int i = 0; i < week.Length; i++)
            {
                DataGridViewTextBoxColumn tc = new DataGridViewTextBoxColumn();
                tc.HeaderText = string.Format("{0}.{1}",i+1, week[i]);
                tc.Name = string.Format("{0}", week[i]);
                dGV.Columns.Add(tc);
            }
            dGV.Rows.Add(htbs.htbIDTeacher.Count + 1);
            dGV.Rows[0].Cells["btnSubjectClassno"].Value = "班任";
            for (int i = 1; i <= htbs.htbIDTeacher.Count; i++)
            {
                dGV.Rows[i].Cells["btnSubjectClassno"].Value = string.Format("{0}.{1}", i, htbs.htbIDTeacher[i]);
            }
        }
        public override void Load_File(DataGridView dGV)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = Basic_HTB_Info.baseFilePath + @"\timetable_TeacherRestdata.dat";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                loadfile(ofd.FileName, dGV);
            }
        }
        public override void Save_File(DataGridView dGV)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = Basic_HTB_Info.baseFilePath + @"\timetable_TeacherRestdata.dat";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                savefile(sfd.FileName, dGV);
            }
        }
        public override void Export_File(DataGridView dGV)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.FileName = Basic_HTB_Info.baseFilePath + @"\BasicInfo\TimeTable_Def_TeacherRestUnit.dat";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                Hashtable IDOpenCourese = new Hashtable();
                Hashtable OpenCoureseClasses = new Hashtable();
                Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
                StreamWriter sr = new StreamWriter(SFD.FileName);
                int cols = dGV.Columns.Count;
                int rows = dGV.Rows.Count;
                for (int i = 1; i < rows; i++)
                    for (int j = 1; j < cols; j++)
                    {
                        if (dGV.Rows[i].Cells[j].Value == null) continue;
                        if (dGV.Rows[i].Cells[j].Value.ToString().Length > 0)
                        {
                            int weekno = j - 1;
                            string[] tsa = dGV.Rows[i].Cells[0].Value.ToString().Split('.');
                            int teacherno = int.Parse(tsa[0]);
                            string[] sa = dGV.Rows[i].Cells[j].Value.ToString().Split(',');
                            foreach (string s in sa)
                            {
                                sr.WriteLine("{0}:{1}", weekno * 10 + int.Parse(s)-1, teacherno);
                            }
                        }
                    }
                sr.Flush();
                sr.Close();
                sr.Dispose();
            }
        }       
    }


}