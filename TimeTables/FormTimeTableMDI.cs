using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.Runtime;
using System.Security;
using System.Threading;

namespace TimeTables
{
    public partial class FormTimeTableMDI : Form
    {
        Basic_HTB_Info htbs;
        public FormTimeTableMDI()
        {
            InitializeComponent();
            htbs = Basic_HTB_Info.GetInstance();
            htbs.log("½Ð¡yÀÉ®×¡z:¡y¸ü¤J¤W¦¸¦sÀÉ ¡z¡C");
            MsgBox msb = new MsgBox(Basic_HTB_Info.GetInstance().logToArray());
            msb.MdiParent = this;
            msb.Show();

            MainMenu fmainMenu = new MainMenu();
            MenuItem initdata_msys = fmainMenu.MenuItems.Add("A.ªì©l¤Æ¸ê®Æ");
   
            initdata_msys.MenuItems.Add(new MenuItem("Teacher.dat", (sender, e) =>
            {
                Process notePad = new Process();
                notePad.StartInfo.FileName = "notepad.exe";
                String path_ = Basic_HTB_Info.baseFilePath + @"\BasicInfo\Teacher.dat";
                notePad.StartInfo.Arguments = path_;
                notePad.Start();
            }));
            initdata_msys.MenuItems.Add(new MenuItem("Subject.dat", (sender, e) =>
            {
                Process notePad = new Process();
                notePad.StartInfo.FileName = "notepad.exe";
                String path_ = Basic_HTB_Info.baseFilePath + @"\BasicInfo\Subject.dat";
                notePad.StartInfo.Arguments = path_;
                notePad.Start();
            }));
            initdata_msys.MenuItems.Add(new MenuItem("Class.dat", (sender, e) =>
            {
                Process notePad = new Process();
                notePad.StartInfo.FileName = "notepad.exe";
                String path_ = Basic_HTB_Info.baseFilePath + @"\BasicInfo\Class.dat";
                notePad.StartInfo.Arguments = path_;
                notePad.Start();
            }));

            initdata_msys.MenuItems.Add(new MenuItem("ClassMaster.dat", (sender, e) =>
            {
                Process notePad = new Process();
                notePad.StartInfo.FileName = "notepad.exe";
                String path_ = Basic_HTB_Info.baseFilePath + @"\BasicInfo\ClassMaster.dat";
                notePad.StartInfo.Arguments = path_;
                notePad.Start();
            }));
            initdata_msys.MenuItems.Add("¥H¤W¥\¯à»Ý­n­«±Òµ{¦¡");
            initdata_msys.MenuItems.Add("-");


            initdata_msys.MenuItems.Add(new MenuItem("A4.½Òµ{¤À°tªí®æ_¶×¥X¡y³Ì¤p¥ô½Ò³æ¦ì Def Unit->file¡zCourse Assig Grid", (sender, e) =>
            {
                FormCourseAssignGrid fcg = new FormCourseAssignGrid(new C_FormCourseAssignGridForSubjectUnit());
                fcg.ShowDialog();
            }));
            initdata_msys.MenuItems.Add(new MenuItem("A5.±Ð®v¥ð®§ªí_Teacher_Week_Take_Rest_Grid", (sender, e) =>
            {
                FormCourseAssignGrid fcg = new FormCourseAssignGrid(new C_FormCourseAssignGridForTeacherRest());
                fcg.ShowDialog();
            }));

            initdata_msys.MenuItems.Add("-");
            initdata_msys.MenuItems.Add(new MenuItem("A6.¦Û°Ê¤À°t½Òªíµ¦²¤", (sender, e) =>
            {
                FormAssgControl fac = new FormAssgControl();
                fac.ShowDialog();
            }));
            
            


            MenuItem msys = fmainMenu.MenuItems.Add("B.ÀÉ®×");


            msys.MenuItems.Add(new MenuItem("B1.¸ü¤J¤W½Ò³æ¦ìªí_Load Lesson Unit", this.menuLoadLessonUnitItem_click));
            msys.MenuItems.Add(new MenuItem("B2.¸ü¤J±Ð®v¥ð®§ªíLoad Teacher Rest TimeTable", this.menuLoadTeacherRestimeTableItem_click));
            msys.MenuItems.Add("-");
            msys.MenuItems.Add(new MenuItem("B3.ÀËµø¤À°t½Òµ{¤jªíAssig Subject Grid TimeTable", this.menuAssigSubjectGrid));
            msys.MenuItems.Add("-");
            msys.MenuItems.Add(new MenuItem("B4.¸ü¤J¤W¦¸¦sÀÉ Load old TimeTable", this.menuLoadOldTimeTableItem_click));
            msys.MenuItems.Add("-");
            msys.MenuItems.Add(new MenuItem("B5.¦sÀÉ",(sender, e)=>{  SaveTiemTableGridOut();        } ));
            

            MenuItem subj_With = fmainMenu.MenuItems.Add("C ¤À°t¦Cªí");
            MenuItem subj_With_Class = subj_With.MenuItems.Add("C1.«ö¯Z");
            MenuItem subj_With_Teacher = subj_With.MenuItems.Add("C2.«ö±Ð®v");

            msys = fmainMenu.MenuItems.Add("D.¤À°t¯Zªí");
            
            for (int i = 1; i <= htbs.htbIDClass.Count; i++)
            {
                msys.MenuItems.Add(new MenuItem(string.Format("{0}.{1}", i, htbs.htbIDClass[i]), this.menuClassNoItem_click));
                subj_With_Class.MenuItems.Add(new MenuItem(string.Format("{0}.{1} Subject_List", i, htbs.htbIDClass[i]), this.menuFormClassNoTimeTableDoubleClick));
            }
            msys.MenuItems.Add("All", this.menuAllClassNoItem_Click);
            msys = fmainMenu.MenuItems.Add("E.±Ð®vªí");
            
            msys.MenuItems.Add(new MenuItem("±Ð®v", (sender, e) =>{
                TeachernoInput ti = new TeachernoInput("Teacher Name:");
                if (ti.ShowDialog() == DialogResult.OK )
                {
                    String[] stra = ti.out_res.Split(',');
                    foreach (String s in stra)
                    {
                        if (Basic_HTB_Info.GetInstance().htbTeacherID.ContainsKey(s))
                        {
                            int akey = int.Parse(Basic_HTB_Info.GetInstance().htbTeacherID[s].ToString());
                            
                            FormClassNoTimeTableDoubleClick_Action aTT = new FormClassNoTimeTableDoubleClick_Action(""+akey+"."+s, new C_FormClassNoTimeTableDoubleClick_Teacher(akey));
                            aTT.MdiParent = this;
                            aTT.updateList();
                            aTT.button_ADD.Enabled = false;
                            aTT.Show();
                        }
                    }
                }
            }));
            for (int i = 1; i <= htbs.htbIDTeacher.Count; i++)
            {
                msys.MenuItems.Add(new MenuItem(string.Format("{0}.{1}", i, htbs.htbIDTeacher[i]), this.menuTeacherNoItem_click));
                subj_With_Teacher.MenuItems.Add(new MenuItem(string.Format("{0}.{1} Subject_List", i, htbs.htbIDTeacher[i]), this.menuFormClassNoTimeTableDoubleClick_Teacher));
            }
            /*
                TeachernoInput ti = new TeachernoInput("Teacher No.");
                String[] stra=ti.out_res.Split(',');
                foreach (String s in stra)
                {
                    if(Basic_HTB_Info.GetInstance().htbTeacherID.ContainsKey(s))
                    {
                        int akey = int.Parse(Basic_HTB_Info.GetInstance().htbTeacherID[s].ToString());
                        FormClassNoTimeTableDoubleClick_Action aTT = new FormClassNoTimeTableDoubleClick_Action(mi.Text, new C_FormClassNoTimeTableDoubleClick_Teacher(akey));
                        aTT.MdiParent = this;
                        aTT.updateList();
                        aTT.button_ADD.Enabled = false;
                        aTT.Show();
                    }
                }
            
            }));*/
            msys = fmainMenu.MenuItems.Add("F.³øªí");
            msys.MenuItems.Add("F1.½Òµ{¦w±ÆReport ClassSubjectTeacherCrosorTable", mnuReport_Cross_Table);
            msys.MenuItems.Add("F2.½Òµ{©P¸`¦w±ÆReport ClassSubjectWeekLessonCrosorTable", mnuReport_Cross_Week_Table);
            msys.MenuItems.Add("F3.±Ð®v½Òµ{¦w±ÆReport Teacher TimeTable",mnuReport_Teacher_TimeTable);
            msys.MenuItems.Add("F4.¯Z¯Å½Òµ{¦w±ÆReport Class TimeTable",mnuReport_Class_TimeTable);
            msys.MenuItems.Add("F5.©P/¸`/(¯Z¯Å¬Y½Òµ{)Report Class TimeTable", mnuReport_WeekDay_Course_TimeTable);

            msys.MenuItems.Add("-");
            msys.MenuItems.Add("F6.¤W¶Ç³ø¦Ü250¦øªA¾¹", mnuReport_UploadTo250);

            
            
            msys = fmainMenu.MenuItems.Add("G.µ¡¤f");
            msys.MenuItems.Add("G1 ¦P¨BDATA", mnuSynFormData_click);
            msys.MenuItems.Add("-");
            msys.MenuItems.Add(new MenuItem("±Æ¦C¹Ï¼Ð", this.mnuIcons_click));
            msys.MenuItems.Add(new MenuItem("¼h¼hÅ|Å|", this.mnuCascade_click));
            msys.MenuItems.Add(new MenuItem("¤ô¥­¾Q¥­", this.mnuTileHorizontal_click));
            msys.MenuItems.Add(new MenuItem("««ª½¾Q¥­", this.mnuTileVertical_click));
            msys.MenuItems.Add(new MenuItem("Ãö³¬©Ò¦³¤lµ¡¤f", this.CloseAllSubForm_click));
            msys.MenuItems.Add("-");
            msys.MenuItems.Add(new MenuItem("¦rÅé¤@", (sender, e) => { adjustFont(16f); }));
            msys.MenuItems.Add(new MenuItem("¦rÅé¤G", (sender, e) => { adjustFont(12f); }));
            msys.MenuItems.Add(new MenuItem("¦rÅé¤T", (sender, e) => { adjustFont(8f); }));
            msys.MenuItems.Add(new MenuItem("¦rÅé¥|", (sender, e) => { adjustFont(7f); }));
            msys.MenuItems.Add(new MenuItem("¦rÅé¤j¤p", this.mnuFontNum_click));

            msys = fmainMenu.MenuItems.Add("H.¸ê°T");
            msys.MenuItems.Add("Åª§Ú", mnuReadMe);
            msys.MenuItems.Add("ÀËµøLast Errors", mnuLastError);
            msys.MenuItems.Add("²M°£Clear Erros List", mnuClrError);
            msys.MenuItems.Add("-");
            msys.MenuItems.Add(new MenuItem("­«¸m¦r¨åªí Clear HTBS", this.menuClearHtbs));

            this.Menu = fmainMenu;
            this.FormClosing += (sender, e) => { SaveTiemTableGridOut(); };
        }
        

        private void adjustFont(float size)
        {
            Font f = new Font("·L³n¥¿¶ÂÅé", size);
            for (int i = MdiChildren.Length - 1; i > -1; i--)
            {
                if (this.MdiChildren[i] is iRefreshGridContent)
                {
                    iRefreshGridContent form = (iRefreshGridContent)this.MdiChildren[i];
                    form.SetFontSize(f);
                }
            }
        }
        private void menuAssigSubjectGrid(Object sender, EventArgs e)
        {
            FormCourseAssignGrid fcg = new FormCourseAssignGrid(new C_FormCourseAssignGridForPreAssigSubjectTeacher());
            fcg.MdiParent = this;
            fcg.RefreshContent();
            fcg.Show();
        }

        private void mnuLastError(Object sender, EventArgs e)
        {
            MsgBox msb = new MsgBox(Basic_HTB_Info.GetInstance().LastErrors.ToArray());
            msb.MdiParent = this;
            msb.Show();
        }
        private void mnuClrError(Object sender, EventArgs e)
        {
            Basic_HTB_Info.GetInstance().LastErrors.Clear();
        }
        [DllImport("shell32.dll")]
        public static extern int ShellExecute(IntPtr hwnd, StringBuilder lpszOp, StringBuilder lpszFile, StringBuilder lpszParams, StringBuilder lpszDir, int FsShowCmd);


        private void mnuReport_Cross_Week_Table(Object sender, EventArgs e)
        {
            //Export_Cross_Week_Table
            Basic_HTB_Info.GetInstance().Export_Cross_Week_Table(Basic_HTB_Info.baseFilePath + @"\report_cross_week_table.htm");
            ShellExecute(IntPtr.Zero, new StringBuilder("Open"), new StringBuilder(Basic_HTB_Info.appPath + @"\report_cross_week_table.htm"), new StringBuilder(""), new StringBuilder(""), 1);
        }
        private void mnuReport_UploadTo250(Object sender, EventArgs e)
        {

            Basic_HTB_Info.GetInstance().Export_Cross_Week_Table(Basic_HTB_Info.baseFilePath + @"\report_cross_week_table.htm");
            Basic_HTB_Info.GetInstance().Export_Cross_Table(Basic_HTB_Info.baseFilePath + @"\report_cross_table.htm");
            Basic_HTB_Info.GetInstance().Export_Teacher_TimeTable(Basic_HTB_Info.baseFilePath + @"\report_teacher_timetable.htm");
            Basic_HTB_Info.GetInstance().Export_Class_TimeTable(Basic_HTB_Info.baseFilePath + @"\report_class_table.htm");
            ProcessStartInfo pi = new ProcessStartInfo(Basic_HTB_Info.baseFilePath + @"\FTPUploadTimeTableReports.exe");
            pi.UseShellExecute = true;
            Process p = Process.Start(pi);
            p.WaitForExit();
        }
        private void mnuReport_Cross_Table(Object sender, EventArgs e)
        {

            Basic_HTB_Info.GetInstance().Export_Cross_Table(Basic_HTB_Info.baseFilePath + @"\report_cross_table.htm");
            
            ShellExecute(IntPtr.Zero, new StringBuilder("Open"), new StringBuilder(Basic_HTB_Info.appPath+@"\report_cross_table.htm"), new StringBuilder(""), new StringBuilder(""), 1);
        }
        private void mnuReport_Teacher_TimeTable(Object sender, EventArgs e)
        {
            Basic_HTB_Info.GetInstance().Export_Teacher_TimeTable(Basic_HTB_Info.baseFilePath + @"\report_teacher_timetable.htm");
            ShellExecute(IntPtr.Zero, new StringBuilder("Open"), new StringBuilder(Basic_HTB_Info.appPath + @"\report_teacher_timetable.htm"), new StringBuilder(""), new StringBuilder(""), 1);
        }
        private void mnuReport_Class_TimeTable(Object sender, EventArgs e)
        {
            Basic_HTB_Info.GetInstance().Export_Class_TimeTable(Basic_HTB_Info.baseFilePath + @"\report_class_table.htm");
            ShellExecute(IntPtr.Zero, new StringBuilder("Open"), new StringBuilder(Basic_HTB_Info.appPath + @"\report_class_table.htm"), new StringBuilder(""), new StringBuilder(""), 1);
        }
        private void mnuReport_WeekDay_Course_TimeTable(Object sender, EventArgs e)
        {
            CourseIdInputBox ibx = new CourseIdInputBox();
            if (ibx.ShowDialog() == DialogResult.OK)
            {
                
                String url = @"\report_weekday_coruse.htm";
                MessageBox.Show(url+ibx.COURSE_ID);
                Basic_HTB_Info.GetInstance().Export_WeekDay_Course_TimeTable(Basic_HTB_Info.baseFilePath + url, ibx.COURSE_ID);
                ShellExecute(IntPtr.Zero, new StringBuilder("Open"), new StringBuilder(Basic_HTB_Info.appPath + url), new StringBuilder(""), new StringBuilder(""), 1);
            }
        }
        private void mnuReadMe(Object sender, EventArgs e)
        {
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            ShellExecute(IntPtr.Zero, new StringBuilder("Open"), new StringBuilder("notepad"), new StringBuilder("readme.txt" ), new StringBuilder(Basic_HTB_Info.appPath+@"\"), 1);
        }
        
        private void mnuFontNum_click(Object sender, EventArgs e)
        {
            NumericUpDownBox ub = new NumericUpDownBox("¿é¤J¦r«¬¤j¤p",160);
            if (ub.ShowDialog() == DialogResult.OK)
            {
                float fontsize = (float)ub.nud.Value / 10f;
                adjustFont(fontsize);
            }
        }

        private void menuLoadTeacherRestimeTableItem_click(Object sender, EventArgs e)
        {
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.FileName = Basic_HTB_Info.baseFilePath + @"\BasicInfo\TimeTable_Def_TeacherRestUnit.dat";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                htbs.load_TeacherRest_file(ofd.FileName);
            }
        }
        private void menuClearHtbs(Object sender, EventArgs e)
        {
            Basic_HTB_Info.GetInstance().ClearHTB();
        }
        private void menuLoadLessonUnitItem_click(Object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = Basic_HTB_Info.baseFilePath + @"\BasicInfo\TimeTable_Def_LessonUnit.dat";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
               Basic_HTB_Info.GetInstance().Load_Lesson_Unit_Def_File(ofd.FileName);
            }
        }
        private void menuLoadOldTimeTableItem_click(Object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = Basic_HTB_Info.baseFilePath + @"\TimeTableGridOut.dat";
            if (ofd.ShowDialog() == DialogResult.OK)
            {

                Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
                Cursor beforCursor=Cursor.Current;
                 
                Cursor.Current=Cursors.WaitCursor;
                htbs.Load_TimeTable_File_To_Grid(ofd.FileName, this);
                Cursor.Current = beforCursor;
                timer1.Start();
                LoadOldTimeTableFile_Cnt++;
            }
        }
        private int LoadOldTimeTableFile_Cnt = 0;
        private void SaveTiemTableGridOut()
        {
            {   String sfdFileName = Basic_HTB_Info.baseFilePath + @"\TimeTableGridOut_temp.dat" + DateTime.Now.ToString("yyyyMMddhhmmss");
            StreamWriter sw = new StreamWriter(sfdFileName, false);
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            for (int i = 0; i < htbs.TimeTableGrid.GetLength(0); i++)
                for (int j = 0; j < htbs.TimeTableGrid.GetLength(1); j++)
                    for (int k = 0; k < htbs.TimeTableGrid.GetLength(2); k++)
                        sw.WriteLine("{0}:{1}", i * 100 + j * 10 + k, htbs.TimeTableGrid[i, j, k]);
            sw.Flush();
            sw.Close();
            sw.Dispose();
          }
            if (LoadOldTimeTableFile_Cnt == 0 && MessageBox.Show("¬O§_¥Î·sÀÉÂÐŸÂÂÂÀÉTimeTableGridOut.dat", "Äµ§i", MessageBoxButtons.YesNo) == DialogResult.No)
            {

                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = Basic_HTB_Info.baseFilePath + @"\TimeTableGridOut.dat";
            FileInfo a = new FileInfo(sfd.FileName);
            if (a.Exists)
            {
                File.Copy(sfd.FileName, sfd.FileName + DateTime.Now.ToString("yyyyMMddhhmmss"));
            }
                
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName, false);
                Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
                for (int i = 0; i < htbs.TimeTableGrid.GetLength(0); i++)
                    for (int j = 0; j < htbs.TimeTableGrid.GetLength(1); j++)
                        for (int k = 0; k < htbs.TimeTableGrid.GetLength(2); k++)
                            sw.WriteLine("{0}:{1}", i * 100 + j * 10 + k, htbs.TimeTableGrid[i, j, k]);
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }

        }
       
        private void menuTeacherNoItem_click(Object sender, EventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            bool flag = true;
            for (int i = MdiChildren.Length - 1; i > -1; i--)
            {
                if (this.MdiChildren[i].Text.Equals(mi.Text))
                {
                    if (this.MdiChildren[i] is FormClassNoTimeTable)
                    {
                        FormClassNoTimeTable aTT = (FormClassNoTimeTable)this.MdiChildren[i];
                        aTT.RefreshContent();
                        this.MdiChildren[i].WindowState = FormWindowState.Maximized;
                        flag = false;
                    }
                }
            }
            if (flag)
            {
                int akey=int.Parse(mi.Text.Split('.')[0]);
                FormClassNoTimeTable aTT = new FormClassNoTimeTable(mi.Text, new C_ClassNoTimeTable_for_Teacher(akey),new C_FormClassNoTimeTableDoubleClick_Teacher(akey),this);
                aTT.MdiParent = this;
                aTT.RefreshContent();
                aTT.Show();
            }
        }
        private void menuFormClassNoTimeTableDoubleClick(Object sender, EventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            bool flag = true;
            for (int i = MdiChildren.Length - 1; i > -1; i--)
            {
                if (this.MdiChildren[i].Text.Equals(mi.Text))
                {
                    if (this.MdiChildren[i] is FormClassNoTimeTableDoubleClick_Action)
                    {
                        FormClassNoTimeTableDoubleClick_Action aTT = (FormClassNoTimeTableDoubleClick_Action)this.MdiChildren[i];
                        int akey = int.Parse(mi.Text.Split('.')[0]);
                        aTT.updateList();
                        this.MdiChildren[i].WindowState = FormWindowState.Maximized;
                        flag = false;
                    }
                }
            }
            if (flag)
            {
                int akey = int.Parse(mi.Text.Split('.')[0]);
                FormClassNoTimeTableDoubleClick_Action aTT = new FormClassNoTimeTableDoubleClick_Action(mi.Text,new C_FormClassNoTimeTableDoubleClick_Class(akey));
                aTT.MdiParent = this;
                aTT.updateList();
                aTT.button_ADD.Enabled = false;
                aTT.Show();
            }
        }
        private void menuFormClassNoTimeTableDoubleClick_Teacher(Object sender, EventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            bool flag = true;
            for (int i = MdiChildren.Length - 1; i > -1; i--)
            {
                if (this.MdiChildren[i].Text.Equals(mi.Text))
                {
                    if (this.MdiChildren[i] is FormClassNoTimeTableDoubleClick_Action)
                    {
                        FormClassNoTimeTableDoubleClick_Action aTT = (FormClassNoTimeTableDoubleClick_Action)this.MdiChildren[i];
                        int akey = int.Parse(mi.Text.Split('.')[0]);
                        aTT.updateList();
                        this.MdiChildren[i].WindowState = FormWindowState.Maximized;
                        flag = false;
                    }
                }
            }
            if (flag)
            {
                int akey = int.Parse(mi.Text.Split('.')[0]);
                FormClassNoTimeTableDoubleClick_Action aTT = new FormClassNoTimeTableDoubleClick_Action(mi.Text, new C_FormClassNoTimeTableDoubleClick_Teacher(akey));
                aTT.MdiParent = this;
                aTT.updateList();
                aTT.button_ADD.Enabled = false;
                aTT.Show();
            }
        }
        private void menuAllClassNoItem_Click(Object sender, EventArgs e)
        {
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            for (int classid = 1; classid <= htbs.htbIDClass.Count; classid++)
            {
                string formText = string.Format("{0}.{1}", classid, htbs.htbIDClass[classid]);
                bool flag = true;
                for (int i = MdiChildren.Length - 1; i > -1; i--)
                {
                    if (this.MdiChildren[i].Text.Equals(formText))
                    {
                        if (this.MdiChildren[i] is FormClassNoTimeTable)
                        {
                            FormClassNoTimeTable aTT = (FormClassNoTimeTable)this.MdiChildren[i];
                            aTT.RefreshContent();
                            this.MdiChildren[i].WindowState = FormWindowState.Maximized;
                            flag = false;
                        }
                    }
                }
                if (flag)
                {
                    int akey = classid;
                    FormClassNoTimeTable aTT = new FormClassNoTimeTable(formText, new C_ClassNoTimeTable_for_Class(akey), new C_FormClassNoTimeTableDoubleClick_Class(akey),this);
                    aTT.MdiParent = this;
                    aTT.RefreshContent();
                    aTT.Show();
                }

            }
        }
        private void menuClassNoItem_click(Object sender, EventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            bool flag = true;
            for (int i = MdiChildren.Length - 1; i > -1; i--)
            {
                if (this.MdiChildren[i].Text.Equals(mi.Text))
                {
                    if(this.MdiChildren[i] is FormClassNoTimeTable)
                    {
                        FormClassNoTimeTable aTT = (FormClassNoTimeTable)this.MdiChildren[i];
                        aTT.RefreshContent();
                        this.MdiChildren[i].WindowState = FormWindowState.Maximized;
                        flag = false;
                    }
                }
            }
            if (flag)
            {
                int akey = int.Parse(mi.Text.Split('.')[0]) ;
                FormClassNoTimeTable aTT = new FormClassNoTimeTable(mi.Text, new C_ClassNoTimeTable_for_Class(akey), new C_FormClassNoTimeTableDoubleClick_Class(akey),this);
                aTT.MdiParent = this;
                aTT.RefreshContent();
                aTT.Show();
            }
        }
        private void mnuSynFormData_click(Object sender, EventArgs e)
        {
            for (int i = MdiChildren.Length - 1; i > -1; i--)
            {
                if (this.MdiChildren[i] is iRefreshGridContent)
                {
                        iRefreshGridContent aTT = (iRefreshGridContent)this.MdiChildren[i];
                        aTT.RefreshContent();
                }
            }
        }
        private void mnuIcons_click(Object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.ArrangeIcons);
        }
        private void mnuCascade_click(Object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }
        private void mnuTileHorizontal_click(Object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }
        private void mnuTileVertical_click(Object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }
        private void CloseAllSubForm_click(Object sender, EventArgs e)
        {
            for (int i = MdiChildren.Length - 1; i > -1; i--)
            {
                this.MdiChildren[i].Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string filename = "c:\\timetable" + string.Format("{0:yyyyMMddHHmmss}.txt", DateTime.Now);
            StreamWriter sw = new StreamWriter(filename, false);
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            for (int i = 0; i < htbs.TimeTableGrid.GetLength(0); i++)
                for (int j = 0; j < htbs.TimeTableGrid.GetLength(1); j++)
                    for (int k = 0; k < htbs.TimeTableGrid.GetLength(2); k++)
                        sw.WriteLine("{0}:{1}", i * 100 + j * 10 + k, htbs.TimeTableGrid[i, j, k]);
            sw.Flush();
            sw.Close();
            sw.Dispose();
            htbs.LastErrors.Add("backup "+filename);

        }
    }
    class CourseIdInputBox : Form
    {
        String courseid = "";
        TextBox tb0 = new TextBox();
        Button btn = new Button();
        FlowLayoutPanel flp = new FlowLayoutPanel();
        public CourseIdInputBox()
        {
            //tb0.Text = Form1.curr_year;
            tb0.TextChanged += new EventHandler(tb0_TextChanged);
            this.btn.DialogResult = DialogResult.OK;
            this.btn.Click += new EventHandler(btn_Click);
            Label lb1 = new Label();
            lb1.Text = "course id:";

            flp.Controls.Add(lb1);
            flp.Controls.Add(tb0);
            flp.Controls.Add(btn);
            this.Controls.Add(flp);
        }

        void tb0_TextChanged(object sender, EventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
           // Form1.curr_year = tb0.Text;
           // ESData.GetInst.Exec("update cfg set curr_year='" + tb0.Text + "'");
        }

        void btn_Click(object sender, EventArgs e)
        {
            courseid = tb0.Text;
            //throw new Exception("The method or operation is not implemented.");
        }

        public String COURSE_ID
        {
            get
            {
                return courseid;
            }
        }
    }
}