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
            htbs.log("請『檔案』:『載入上次存檔 』。");
            MsgBox msb = new MsgBox(Basic_HTB_Info.GetInstance().logToArray());
            msb.MdiParent = this;
            msb.Show();

            MainMenu fmainMenu = new MainMenu();
            MenuItem initdata_msys = fmainMenu.MenuItems.Add("A.初始化資料");
   
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
            initdata_msys.MenuItems.Add("以上功能需要重啟程式");
            initdata_msys.MenuItems.Add("-");


            initdata_msys.MenuItems.Add(new MenuItem("A4.課程分配表格_匯出『最小任課單位 Def Unit->file』Course Assig Grid", (sender, e) =>
            {
                FormCourseAssignGrid fcg = new FormCourseAssignGrid(new C_FormCourseAssignGridForSubjectUnit());
                fcg.ShowDialog();
            }));
            initdata_msys.MenuItems.Add(new MenuItem("A5.教師休息表_Teacher_Week_Take_Rest_Grid", (sender, e) =>
            {
                FormCourseAssignGrid fcg = new FormCourseAssignGrid(new C_FormCourseAssignGridForTeacherRest());
                fcg.ShowDialog();
            }));

            initdata_msys.MenuItems.Add("-");
            initdata_msys.MenuItems.Add(new MenuItem("A6.自動分配課表策略", (sender, e) =>
            {
                FormAssgControl fac = new FormAssgControl();
                fac.ShowDialog();
            }));
            
            


            MenuItem msys = fmainMenu.MenuItems.Add("B.檔案");


            msys.MenuItems.Add(new MenuItem("B1.載入上課單位表_Load Lesson Unit", this.menuLoadLessonUnitItem_click));
            msys.MenuItems.Add(new MenuItem("B2.載入教師休息表Load Teacher Rest TimeTable", this.menuLoadTeacherRestimeTableItem_click));
            msys.MenuItems.Add("-");
            msys.MenuItems.Add(new MenuItem("B3.檢視分配課程大表Assig Subject Grid TimeTable", this.menuAssigSubjectGrid));
            msys.MenuItems.Add("-");
            msys.MenuItems.Add(new MenuItem("B4.載入上次存檔 Load old TimeTable", this.menuLoadOldTimeTableItem_click));
            msys.MenuItems.Add("-");
            msys.MenuItems.Add(new MenuItem("B5.存檔",(sender, e)=>{  SaveTiemTableGridOut();        } ));
            

            MenuItem subj_With = fmainMenu.MenuItems.Add("C 分配列表");
            MenuItem subj_With_Class = subj_With.MenuItems.Add("C1.按班");
            MenuItem subj_With_Teacher = subj_With.MenuItems.Add("C2.按教師");

            msys = fmainMenu.MenuItems.Add("D.分配班表");
            
            for (int i = 1; i <= htbs.htbIDClass.Count; i++)
            {
                msys.MenuItems.Add(new MenuItem(string.Format("{0}.{1}", i, htbs.htbIDClass[i]), this.menuClassNoItem_click));
                subj_With_Class.MenuItems.Add(new MenuItem(string.Format("{0}.{1} Subject_List", i, htbs.htbIDClass[i]), this.menuFormClassNoTimeTableDoubleClick));
            }
            msys.MenuItems.Add("All", this.menuAllClassNoItem_Click);
            msys = fmainMenu.MenuItems.Add("E.教師表");
            
            msys.MenuItems.Add(new MenuItem("教師", (sender, e) =>{
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
           
            msys = fmainMenu.MenuItems.Add("F.報表");
            msys.MenuItems.Add("F1.教師課程安排Report Teacher TimeTable", mnuReport_Teacher_TimeTable);
            msys.MenuItems.Add("F2.班級課程安排Report Class TimeTable", mnuReport_Class_TimeTable);
            msys.MenuItems.Add("-");
            msys.MenuItems.Add("F3.課程安排Report ClassSubjectTeacherCrosorTable", mnuReport_Cross_Table);
            msys.MenuItems.Add("F4.課程周節安排Report ClassSubjectWeekLessonCrosorTable", mnuReport_Cross_Week_Table);
            msys.MenuItems.Add("-");
            msys.MenuItems.Add("F5.周/節/(班級某課程)Report Class TimeTable", mnuReport_WeekDay_Course_TimeTable);
            msys.MenuItems.Add("-");
            msys.MenuItems.Add("F6.上傳報至伺服器", mnuReport_UploadTo250);

            
            
            msys = fmainMenu.MenuItems.Add("G.窗口");
            msys.MenuItems.Add("G1 同步DATA", mnuSynFormData_click);
            msys.MenuItems.Add("-");
            msys.MenuItems.Add(new MenuItem("排列圖標", this.mnuIcons_click));
            msys.MenuItems.Add(new MenuItem("層層疊疊", this.mnuCascade_click));
            msys.MenuItems.Add(new MenuItem("水平鋪平", this.mnuTileHorizontal_click));
            msys.MenuItems.Add(new MenuItem("垂直鋪平", this.mnuTileVertical_click));
            msys.MenuItems.Add(new MenuItem("關閉所有子窗口", this.CloseAllSubForm_click));
            msys.MenuItems.Add("-");
            msys.MenuItems.Add(new MenuItem("字體一", (sender, e) => { adjustFont(16f); }));
            msys.MenuItems.Add(new MenuItem("字體二", (sender, e) => { adjustFont(12f); }));
            msys.MenuItems.Add(new MenuItem("字體三", (sender, e) => { adjustFont(8f); }));
            msys.MenuItems.Add(new MenuItem("字體四", (sender, e) => { adjustFont(7f); }));
            msys.MenuItems.Add(new MenuItem("字體大小", this.mnuFontNum_click));

            msys = fmainMenu.MenuItems.Add("H.資訊");
            msys.MenuItems.Add("讀我", mnuReadMe);
            msys.MenuItems.Add("檢視Last Errors", mnuLastError);
            msys.MenuItems.Add("清除Clear Erros List", mnuClrError);
            msys.MenuItems.Add("-");
            msys.MenuItems.Add(new MenuItem("重置字典表 Clear HTBS", this.menuClearHtbs));

            this.Menu = fmainMenu;
            this.FormClosing += (sender, e) => { SaveTiemTableGridOut(); };
        }
        

        private void adjustFont(float size)
        {
            Font f = new Font("微軟正黑體", size);
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
            MessageBox.Show("not implement!");
            /*
            Basic_HTB_Info.GetInstance().Export_Cross_Week_Table(Basic_HTB_Info.baseFilePath + @"\report_cross_week_table.htm");
            Basic_HTB_Info.GetInstance().Export_Cross_Table(Basic_HTB_Info.baseFilePath + @"\report_cross_table.htm");
            Basic_HTB_Info.GetInstance().Export_Teacher_TimeTable(Basic_HTB_Info.baseFilePath + @"\report_teacher_timetable.htm");
            Basic_HTB_Info.GetInstance().Export_Class_TimeTable(Basic_HTB_Info.baseFilePath + @"\report_class_table.htm");
            ProcessStartInfo pi = new ProcessStartInfo(Basic_HTB_Info.baseFilePath + @"\FTPUploadTimeTableReports.exe");
            pi.UseShellExecute = true;
            Process p = Process.Start(pi);
            p.WaitForExit();*/
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
            NumericUpDownBox ub = new NumericUpDownBox("輸入字型大小",160);
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
            if (LoadOldTimeTableFile_Cnt == 0 && MessageBox.Show("是否用新檔覆�舊臍仈imeTableGridOut.dat", "警告", MessageBoxButtons.YesNo) == DialogResult.No)
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