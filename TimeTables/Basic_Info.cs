using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.Common;

namespace TimeTables
{
    public class Basic_HTB_Info
    {
        /// <summary>
        /// 每日 9 節,節編號 0~8 
        /// </summary>
        public static int day_max_lession_no=8;
        public static int day_lessions = day_max_lession_no + 1;
        /// <summary>
        /// 每周五日
        /// </summary>
        public static int week_studydays = 5;
        /// <summary>
        /// 網格Cols
        /// </summary>
        public static int grid_panel_cols = 5;
        /// <summary>
        ///教師字典 
        /// </summary>
        public Hashtable htbTeacherID = new Hashtable();
        /// <summary>
        /// 教師字典
        /// </summary>
        public Hashtable htbIDTeacher = new Hashtable();
        /// <summary>
        ///教師字典,簡稱 
        /// </summary>
        public Hashtable htbIDTeacherShortName = new Hashtable();
        /// <summary>
        /// 班級字典 1, SG1A
        /// </summary>
        public Hashtable htbIDClass = new Hashtable();
        /// <summary>
        /// 班級字典 SG1A, 1 
        /// </summary>
        public Hashtable htbClassID = new Hashtable();
        /// <summary>
        /// 課程名 1,中文
        /// </summary>
        public Hashtable htbIDSubject = new Hashtable();
        /// <summary>
        /// 課程短名 1, 中
        /// </summary>
        public Hashtable htbIDSubjectShortName = new Hashtable();
        /// <summary>
        /// 課程名 中文,1
        /// </summary>
        public Hashtable htbSubjectID = new Hashtable();
        /// <summary>
        /// 課程分割小單元
        /// </summary>
        public Hashtable htbIDLessionUnit = new Hashtable();
        /// <summary>
        /// 多選課程單元多位教師
        /// </summary>
        private Hashtable htbIDLessionTeachers = new Hashtable();
        /// <summary>
        /// 多選課程單元多個班級
        /// </summary>
        private Hashtable htbIDLessionClasses = new Hashtable();
        /// <summary>
        /// 班主任
        /// </summary>
        public Hashtable htbIDClassMaster = new Hashtable();
        /// <summary>
        /// 教師周中修息表
        /// </summary>
        private Hashtable htbTeacherRestWeekLesson = new Hashtable();
        /// <summary>
        /// 每節時間描述,節間時間描述
        /// </summary>
        private Hashtable htbLessonTimeDesc = new Hashtable();
        /// <summary>
        /// 多選課程描述,用多排課時選擇列表
        /// </summary>
        public List<String> VirtualSubject_MultiTeachers = new List<string>();
        /// <summary>
        /// 班,周,節 排課列陣
        /// </summary>
        // public int[,,] TimeTableGrid=new  int[30, week_studydays , day_lessions];
        public int[, ,] TimeTableGrid = null;
        /// <summary>
        /// 唯讀 SUID
        /// </summary>
        private List<int> SUID_ReadOnly_List = new List<int>();
        /// <summary>
        /// 周 中文對應
        /// </summary>
        public int[,] ASSG_SUBJSET_PLACE_RESTRICT_ = new int[5, 6];
        private  string[] weekstr=new string[]{"一","二","三","四","五","六","日",};
        /// <summary>
        /// 節 中文對應
        /// </summary>
        private string[] NumToLessonstr = new string[] { "第一節", "第二節", "第三節", "第四節", "第五節", "第六節", "第七節", "第八節", "第九節" };
        /// <summary>
        /// 報錯訊息
        /// </summary>
        public List<String> LastErrors = new List<string>();

        private static Basic_HTB_Info instance = null;

        private static int instanceCNT=0;

        public int InstanceCount
        {
            get { return instanceCNT;}
        }
        /// <summary>
        /// 單一實體
        /// </summary>
        /// <returns>返來實體</returns>
        public static Basic_HTB_Info GetInstance()
        {
            if (instance == null)
            {
                instance = new Basic_HTB_Info();
                instanceCNT++;
            }
            return instance;
        }
        /// <summary>
        /// 系統目錄
        /// </summary>
        public static string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        /// <summary>
        /// 文件目錄
        /// </summary>
        public static string baseFilePath
        {
            get
            {
                return appPath.Substring(6);  
            }
        }
        /// <summary>
        /// 構造函數
        /// </summary>
        private Basic_HTB_Info()
        {
            string[] BasicInfoFileNames = new string[] { 
                @"\BasicInfo\Teacher.dat",@"\BasicInfo\Class.dat", @"\BasicInfo\Subject.dat" ,@"\BasicInfo\LessonTimeDesc.dat",@"\BasicInfo\ClassMaster.dat"
            };

            List<Hashtable> htbList = new List<Hashtable>();
            htbList.Add(htbIDTeacher);
            htbList.Add(htbTeacherID);
            htbList.Add(htbIDTeacherShortName);

            htbList.Add(htbIDClass);
            htbList.Add(htbClassID);
            htbList.Add(null);
         
            htbList.Add(htbIDSubject);
            htbList.Add(htbSubjectID);
            htbList.Add(htbIDSubjectShortName);

            htbList.Add(htbLessonTimeDesc);
            htbList.Add(null);
            htbList.Add(null);
            
            htbList.Add(htbIDClassMaster);
            htbList.Add(null);
            htbList.Add(null);
            
            for (int i = 0; i < BasicInfoFileNames.Length; i++)
            {
                log(BasicInfoFileNames[i]);
                StreamReader sr = new StreamReader(Basic_HTB_Info.baseFilePath +BasicInfoFileNames[i], Encoding.Default);
                string input = null;
                while ((input = sr.ReadLine()) != null)
                {
                    try
                    {
                        string[] strA = input.Split('.');
                        if (strA.Length > 1)
                        {
                            htbList[i * 3].Add(int.Parse(strA[0]), strA[1]);
                            if (htbList[i * 3 + 1] != null)
                                htbList[i * 3 + 1].Add(strA[1], int.Parse(strA[0]));
                            if (strA.Length > 2 && htbList[i * 3 + 2] != null)
                            {
                                htbList[i * 3 + 2].Add(int.Parse(strA[0]), strA[2]);
                            }
                        }
                    }
                    catch (Exception excep) { MessageBox.Show(excep.Message +"\n\n Please check dupli _key from "+ BasicInfoFileNames[i]); }
                }
                sr.Close();
                sr.Dispose();
            }
            if(TimeTableGrid ==null)
                TimeTableGrid = new int[htbIDClass.Count, week_studydays, day_lessions];
         //   Load_VirtualSubject_MultiTeachers();
            
            log(@"\BasicInfo\TimeTable_Def_LessonUnit.dat");
            try
            {
                Load_Lesson_Unit_Def_File(Basic_HTB_Info.baseFilePath + @"\BasicInfo\TimeTable_Def_LessonUnit.dat");
            }catch
            {
                log("error");
            }

            log(@"\BasicInfo\PreAssigLessonUnitTimeTable.dat");
            try
            {
                Load_TimeTable_File_To_Grid(Basic_HTB_Info.baseFilePath + @"\BasicInfo\PreAssigLessonUnitTimeTable.dat", null);
            }
            catch { log("error"); }
            log(@"\BasicInfo\TimeTable_Def_TeacherRestUnit.dat");
            try{
            load_TeacherRest_file( Basic_HTB_Info.baseFilePath + @"\BasicInfo\TimeTable_Def_TeacherRestUnit.dat");
            }catch{log("error");}
          }
        public List<String> _rt=new List<string>();
        
        public void log(String txt)
        {
            if(_rt!=null)
                _rt.Add(txt);
        }
        public String[] logToArray()
        {
            return _rt.ToArray();
        }
            
        /// <summary>
        /// 清除課程單元字典
        /// </summary>
        public void ClearHTB()
        {
            this.htbIDLessionUnit.Clear();
            this.htbIDLessionClasses.Clear();
            this.htbIDLessionTeachers.Clear();
        }
        /// <summary>
        /// 增加一個課程單元入排課表
        /// </summary>
        /// <param name="a_SUID">課程單元編號</param>
        /// <param name="weekno">周</param>
        /// <param name="lessonno">節</param>
        /// <returns>True/false</returns>
        public  bool AddSubjectToTimeTable(int a_SUID, int weekno, int lessonno)
        {
            bool ValidateFlag = Validate_SubjectUnitForLessonCell(a_SUID, weekno, lessonno);
            if (!ValidateFlag) return false;
            int classno = Basic_HTB_Info.unitkeyToClassno(a_SUID);
            //判斷 1節,2節 或3-9 是多選科
            int ci = this.unitkey2LessonType(a_SUID);
            if (ci == 1 || ci == 2)
            {
                TimeTableGrid[classno - 1, weekno, lessonno] = a_SUID;
                if (ci == 2)
                    TimeTableGrid[classno - 1, weekno, lessonno + 1] = a_SUID;
            }
            else if (ci > 2)
            {
                int[] inta = unitkeyToClassnos(a_SUID);
                foreach (int i in inta)
                {
                    TimeTableGrid[i - 1, weekno, lessonno] = a_SUID;
                }
            }
            return true;
        }
        /// <summary>
        /// 移除一個課程單元出排課表
        /// </summary>
        /// <param name="a_SUID">課程單元編號</param>
        /// <returns>True/false</returns>
        public bool RemoveSubjectFromTimeTable(int a_SUID)
        {
            if (SUID_ReadOnly_List.Contains(a_SUID))
            {
                LastErrors.Add(String.Format("Error:{0}{1}為唯讀!", a_SUID, htbIDLessionUnit[a_SUID]));
                return false;
            }
            for(int i=0;i<TimeTableGrid.GetLength(0);i++)
                for(int j=0;j<TimeTableGrid.GetLength(1);j++)
                    for (int k = 0; k < TimeTableGrid.GetLength(2); k++)
                    {
                        if (a_SUID == TimeTableGrid[i, j, k])
                        {
                            TimeTableGrid[i, j, k] = 0;
                        }
                    }
            return true;
        }

        
        private static int unitkeyToClassno(int unitkey)
        {
            return unitkey / 100000 % 100;
        }
        private static int unitkeyToTeacherno(int unitkey)
        {
            return unitkey / 1000 % 100;
        }
        private static int unitkeyToSubjectID(int unitkey)
        {
            return unitkey / 10000000;
        }
        /// <summary>
        /// TextBox in TableLayoutPanel  TextBox1 TextBox2 TextBox3
        /// </summary>
        /// <param name="tb">TextBox</param>
        /// <returns>int[0]周{0~4} int[1]節{0~8}</returns>
        public static int[] TextBoxName2WeekLesson(System.Windows.Forms.TextBox tb)
        {
            int[] res = new int[2];
            int tmpint = int.Parse(tb.Name.Substring(7));
            res[0] = (tmpint - 1) % grid_panel_cols;//w
            res[1] = (tmpint - 1) / grid_panel_cols;//l
            return res;
        }
        public static String WeekLesson2TextBoxName(int grid_i,int grid_j)
        {
            return "TextBox" + (grid_j * grid_panel_cols + grid_i + 1);
        }
        /// <summary>
        /// 通過lesson Unit 計算出班級編號
        /// </summary>
        /// <param name="unitkey">Lesson Unit ID</param>
        /// <returns>出班級編號 int[]</returns>
        public int[] unitkeyToClassnos(int unitkey)
        {
            int[] inta = null;

            if (unitkey2LessonType(unitkey) > 2)
            {
                string s = htbIDLessionClasses[unitkey].ToString();
                string[] sa = s.Split(',');
                inta = new int[sa.Length];
                for (int i = 0; i < inta.Length; i++)
                {
                    try
                    {
                        inta[i] = int.Parse(sa[i]);
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
               inta = new int[1];
                inta[0] = unitkeyToClassno(unitkey);
            }
            return inta;
        }
        /// <summary>
        /// 通過lesson Unit 計算出教師編號
        /// </summary>
        /// <param name="unitkey">Lesson Unit ID</param>
        /// <returns>出教師編號 int[]</returns>
        public  int[] unitkeyToTeachernos(int unitkey)
        {
            int[] inta=null;
            if (unitkey2LessonType(unitkey) > 2)
            {
                string s = htbIDLessionTeachers[unitkey].ToString();
                string[] sa = s.Split(',');
                inta = new int[sa.Length];
                for (int i = 0; i < inta.Length; i++)
                {
                    try
                    {
                        inta[i] = int.Parse(sa[i]);
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                 inta = new int[1];
                inta[0] = unitkeyToTeacherno(unitkey);
            }
            return inta;
        }
        /// <summary>
        /// UNITKEY 對應 1 節 2節,或 VCID 3-99
        /// </summary>
        /// <param name="unitkey"></param>
        /// <returns></returns>
        public int unitkey2LessonType(int unitkey)
        {
            return unitkey % 100;
        }
        public int unitkey2sbjidclassno(int unitkey)
        {
            return unitkey / 100000;
        }
        public int unitkey2sbjid(int unitkey)
        {
            return unitkeyToSubjectID(unitkey);
        }
        public int unitkey2classno(int unitkey)
        {
            return unitkeyToClassno(unitkey);
        }
        public int unitkey2cnt(int unitkey)
        {
            return unitkey / 100 % 10;
        }
        public int unitkey_repace_cnt(int unitkey,int cnt,int i)
        {
            return unitkey - cnt * 100 + i * 100;
        }

        /// <summary>
        /// 生成一個SUID KEY
        /// </summary>
        /// <param name="subjid">課程編號</param>
        /// <param name="classno">班編號</param>
        /// <param name="teacherno">教師編號</param>
        /// <param name="cntid">分單元編號</param>
        /// <param name="LessonType">單元類別{1單節,2聯堂} >3是多選一</param>
        /// <returns>SUID KEY</returns>
        public static int GenSUID(int subjid, int classno, int teacherno, int cntid, int LessonType)
        {
            return subjid * 10000000 + classno * 100000 + teacherno * 1000 + cntid * 100 + LessonType;
        }
        /*
        private bool Validate_SUID_WEEKNO_LESSONNO(int SUID,int weekno,int lessonno)
        {
            //return ( ! this.htbIDLessionUnit.ContainsKey(SUID) ) && (weekno < 0 || weekno > 4) && (lessonno < 0 || lessonno >  6);   2014/06/23
            return ( ! this.htbIDLessionUnit.ContainsKey(SUID) ) && (weekno < 0 || weekno > (week_studydays-1)) && (lessonno < 0 || lessonno > (day_lessions-1) );
        }*/
        private bool Validate_LessonPlaceNoConflict(int SUID,int weekno,int lessonno)
        {
            if(!this.htbIDLessionUnit.ContainsKey(SUID)) return false;
            if (weekno < 0 || weekno > (week_studydays-1) ) return false;
            if (lessonno < 0 || lessonno > day_max_lession_no) return false;

            int[] classes = this.unitkeyToClassnos(SUID);

            foreach (int c in classes)
            {
                if (TimeTableGrid[c - 1, weekno, lessonno] > 0)
                {
                    return false;
                }
                if (unitkey2LessonType(SUID) == 2 && lessonno > (day_max_lession_no-1) )
                {
                    LastErrors.Add(String.Format("LessonPlaceNoConfilct_error:suid{0},week{1},lesson{2}", SUID, weekno, lessonno));
                    return false;
                }
                if (unitkey2LessonType(SUID) == 2 && TimeTableGrid[c - 1, weekno, lessonno + 1] > 0)
                {
                    LastErrors.Add(String.Format("LessonPlaceNoConfilct_error:suid{0},week{1},lesson{2}", SUID, weekno, lessonno));
                    return false; }
            }
            /////查課室限制{10,11,12}2;電腦課同一時間,使用2個電腦室
            int ASSG_SUBJSET_PLACE_RESTRICT_INDEX = -1;
            for (int i = 0; i < ASSG_SUBJSET_PLACE_RESTRICT_.GetLength(0); i++)
            {
                if (ASSG_SUBJSET_PLACE_RESTRICT_[i, 0] > 0)
                {
                    for (int j = 1; j< ASSG_SUBJSET_PLACE_RESTRICT_.GetLength(1); j++)
                    {
                        if (unitkey2sbjid(SUID) == ASSG_SUBJSET_PLACE_RESTRICT_[i, j]) { ASSG_SUBJSET_PLACE_RESTRICT_INDEX = i; }
                    }
                }
            }
            int[,] temp_set = new int[2, 2];
            temp_set[0, 0] = 2;
            temp_set[0, 1] = lessonno;
            temp_set[1, 0] = unitkey2LessonType(SUID);
            temp_set[1, 1] = lessonno + 1;
            if (ASSG_SUBJSET_PLACE_RESTRICT_INDEX > -1 && ASSG_SUBJSET_PLACE_RESTRICT_[ASSG_SUBJSET_PLACE_RESTRICT_INDEX, 0]>0)
            {
                for (int temp_int = 0; temp_int < temp_set.GetLength(0); temp_int++)
                {
                    int temp_lessonno = temp_set[temp_int, 1];
                    if (temp_set[temp_int, 0] == 2 && temp_lessonno < TimeTableGrid.GetLength(2) - 1)
                    {
                        int subjcnt = 0;
                        for (int i = 0; i < TimeTableGrid.GetLength(0); i++)
                        {
                            for (int j = 1; j < ASSG_SUBJSET_PLACE_RESTRICT_.GetLength(1); j++)
                            {
                                if (ASSG_SUBJSET_PLACE_RESTRICT_[ASSG_SUBJSET_PLACE_RESTRICT_INDEX, j] == 0 || TimeTableGrid[i, weekno, temp_lessonno] == 0) break;
                                if (unitkey2sbjid(TimeTableGrid[i, weekno, temp_lessonno]) == ASSG_SUBJSET_PLACE_RESTRICT_[ASSG_SUBJSET_PLACE_RESTRICT_INDEX, j])
                                {
                                    subjcnt++;
                                }
                            }
                        }
                        if (subjcnt > (ASSG_SUBJSET_PLACE_RESTRICT_[ASSG_SUBJSET_PLACE_RESTRICT_INDEX, 0] - 1))
                        {
                            LastErrors.Add(String.Format("LessonPlaceNoConfilct_ASSG_SUBJ_PLACE_RESTRICT_error:suid{0},week{1},lesson{2}", SUID, weekno, lessonno));
                            return false;
                        }
                        else
                        {
                            LastErrors.Add(String.Format("subjcnt {0},{1} LessonPlaceNoConfilct_ASSG_SUBJ_PLACE_RESTRICT_pass:suid{2},week{3},lesson{4}", subjcnt, ASSG_SUBJSET_PLACE_RESTRICT_[ASSG_SUBJSET_PLACE_RESTRICT_INDEX, 0], SUID, weekno, lessonno));
                        }

                    }
                }
            }
            //////////////////
            return true;
        }
        private bool Validate_LessonTeacherNoConflict(int SUID, int weekno, int lessonno)
        {
            if (!this.htbIDLessionUnit.ContainsKey(SUID)) return false;

            List<int> TeachOndutyList = new List<int>();
            for (int i = 0; i < TimeTableGrid.GetLength(0); i++)
            {
               if( TimeTableGrid[i,weekno,lessonno]>0)
               {
                   int[] tl = this.unitkeyToTeachernos(TimeTableGrid[i, weekno, lessonno]);
                   foreach (int tempint in tl)
                       if(!TeachOndutyList.Contains(tempint)) TeachOndutyList.Add(tempint);
               }

                if(unitkey2LessonType( SUID)  == 2 && lessonno < day_max_lession_no)
                {
                    if (TimeTableGrid[i, weekno, lessonno + 1] > 0)
                   {
                   int[] tl = this.unitkeyToTeachernos(TimeTableGrid[i, weekno, lessonno + 1]);
                   foreach (int tempint in tl)
                       if(!TeachOndutyList.Contains(tempint)) TeachOndutyList.Add(tempint);
                   }
                }
            }
            int[] SbjTeachers = this.unitkeyToTeachernos(SUID);
 
                foreach (int SbjTid in SbjTeachers)
                {
                    if (TeachOndutyList.Contains(SbjTid))
                    {
                        LastErrors.Add(String.Format("TeacherNoConflict_error:suid{0},week{1},lesson{2}", SUID, weekno, lessonno));
                        return false;
                    }
                }
            return true;
        }
        private bool Validate_LessonTeacherNoRest(int SUID, int weekno, int lessonno)
        {
  
            int[] SbjTeachers = this.unitkeyToTeachernos(SUID);
            if (!this.htbTeacherRestWeekLesson.ContainsKey(weekno * 10 + lessonno)) return true;
            String[] sa0 = this.htbTeacherRestWeekLesson[weekno * 10 + lessonno].ToString().Split(',');
            foreach (string s in sa0)
            {
                foreach (int SbjTid in SbjTeachers)
                {
                    if (int.Parse(s) == SbjTid) {
                       // LastErrors.Add(String.Format("No_Rest_error:suid{0},week{1},lesson{2}",SUID,weekno,lessonno));
                        return false; }
                }
            }
            if (unitkey2LessonType(SUID) == 2)
            {
                if (!this.htbTeacherRestWeekLesson.ContainsKey(weekno * 10 + lessonno+1)) return true;
                String[] sa1 = this.htbTeacherRestWeekLesson[weekno * 10 + lessonno + 1].ToString().Split(',');
                foreach (string s in sa1)
                {
                    foreach (int SbjTid in SbjTeachers)
                    {
                        if (int.Parse(s) == SbjTid)
                        {
                          //  LastErrors.Add(String.Format("No_Rest_error:suid{0},week{1},lesson{2}", SUID, weekno, lessonno));
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SUID"></param>
        /// <returns></returns>
        public bool Validate_LessonNotOnTable(int SUID)
        {
            for (int i = 0; i < TimeTableGrid.GetLength(0); i++)
                for (int j = 0; j < TimeTableGrid.GetLength(1); j++)
                    for (int k = 0; k < TimeTableGrid.GetLength(2); k++)
                    {
                        if (TimeTableGrid[i, j, k] == SUID)
                        {
                          //  LastErrors.Add(String.Format("Not_On_Table_Error:suid{0}", SUID));
                            return false;
                        }
                    }
            return true;
        }
        public bool Validate_SubjectUnitForLessonCell(int SUID, int weekno, int lessonno)
        {
            return Validate_LessonNotOnTable(SUID)&&Validate_LessonPlaceNoConflict(SUID, weekno, lessonno) && Validate_LessonTeacherNoConflict(SUID, weekno, lessonno) && Validate_LessonTeacherNoRest(SUID, weekno, lessonno);
        }
        public void Load_Lesson_Unit_Def_File(string FileName)
        {


            htbIDLessionUnit.Clear();
            htbIDLessionClasses.Clear();
            htbIDLessionTeachers.Clear();
            StreamReader sr = new StreamReader(FileName);
            string input = null;
            while ((input = sr.ReadLine()) != null)
            {
                string[] ar = input.Split(':');
                if (ar.Length > 1)
                {
                    int key = int.Parse(ar[0].ToString());
                    if (htbIDLessionUnit.ContainsKey(key)) continue;
                    htbIDLessionUnit.Add(key, ar[1]);


                    if (unitkey2LessonType( key ) > 2)
                    {
                        try
                        {
                            string[] ar0 = ar[1].Split('#');
                            string[] ar1 = ar0[1].Split('@');
                            htbIDLessionTeachers.Add(key, ar1[0]);
                            htbIDLessionClasses.Add(key, ar1[1]);
                        }
                        catch
                        {
                            MessageBox.Show(key.ToString() + input);
                        }
                        
                    }
                }
            }
            sr.Close();
            sr.Dispose();
        }

        public void Load_TimeTable_File_To_Grid(String FileName, System.Windows.Forms.Form mdiparent)
        {
            LastErrors.Clear();
            bool Flag = SUID_ReadOnly_List.Count == 0;
            if (Flag && MessageBox.Show("載入資是否唯讀(不能修改)?","問唯讀",MessageBoxButtons.YesNo) == DialogResult.No) { Flag = false; }
            StreamReader sr = new StreamReader(FileName);
            string input = null;
            while ((input = sr.ReadLine()) != null)
            {
                string[] ar = input.Split(':');
                if (ar.Length > 1)
                {
                    int key = int.Parse(ar[0]);
                    int a_suid = int.Parse(ar[1]);
                    int classno = key / 100;
                    int weekno = key / 10 % 10;
                    int lessonno = key % 10;
                    if (htbIDLessionUnit.ContainsKey(int.Parse(ar[1])) && Validate_SubjectUnitForLessonCell(a_suid, weekno, lessonno))
                    {
                        AddSubjectToTimeTable(a_suid, weekno, lessonno);  //htbs.TimeTableGrid[classno, weekno, lessonno] = a_suid; 
                        if (Flag) SUID_ReadOnly_List.Add(a_suid);
                    }
                    else if (TimeTableGrid[classno, weekno, lessonno] != a_suid)
                    {
                        LastErrors.Add(String.Format("ERROR:{0}:{1}-weekno{2},lessno{3},classno{4}", a_suid, htbIDLessionUnit[a_suid],weekno,lessonno,classno));
                    }
                }
            }
            if (LastErrors.Count > 0)
            {
                MsgBox mb = new MsgBox(LastErrors.ToArray());
                if (mdiparent != null)
                {
                    mb.MdiParent = mdiparent;
                    mb.Show();
                }
                else
                    mb.ShowDialog();
            }
            sr.Close();
            sr.Dispose();

        }
        public void load_TeacherRest_file(String FileName)
        {

                StreamReader sr = new StreamReader(FileName);
                string input = null;
                while ((input = sr.ReadLine()) != null)
                {
                   string[] ar = input.Split(':');
                   if (ar.Length > 1)
                   {
                       int key = int.Parse(ar[0].ToString());
                            if (this.htbTeacherRestWeekLesson.ContainsKey(key))
                            {
                                htbTeacherRestWeekLesson[key] += "," + ar[1];
                            }
                            else
                            {
                                htbTeacherRestWeekLesson.Add(key, ar[1]);
                            }
                        }
                    }
                    sr.Close();
                    sr.Dispose();
              
        }
        public void Load_VirtualSubject_MultiTeachers()
        {
            String FileName = Basic_HTB_Info.baseFilePath + @"\BasicInfo\VirtualSubject_MultiTeachers.dat";
            VirtualSubject_MultiTeachers.Clear();
            StreamReader sr = new StreamReader(FileName, Encoding.Default);
            string input = null;
            while ((input = sr.ReadLine()) != null)
            {
                VirtualSubject_MultiTeachers.Add(input);
            }
            sr.Close();
            sr.Dispose();
        }
        public void Append_TeacherRest_file(string s)
        {
            String FileName = Basic_HTB_Info.baseFilePath + @"\BasicInfo\VirtualSubject_MultiTeachers.dat";
            StreamWriter sw = new StreamWriter(FileName, true, Encoding.Default);
            sw.Write(s);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
        public void Export_Teacher_TimeTable(string FileName)
        {
            StreamWriter sw = new StreamWriter(FileName, false, Encoding.Default);
            List<int> key_s = new List<int>();
            foreach (DictionaryEntry de in htbIDTeacher)
            {
                int tid = int.Parse(de.Key.ToString());
                key_s.Add(tid);
            }
            key_s.Sort();

            foreach (int tid in key_s)
            {
                //int tid = int.Parse(de.Key.ToString());
                string tName = htbIDTeacher[tid].ToString();
                sw.WriteLine("<center>No.{0}_{1}上課時間表  日期{2}</center>", tid,tName, DateTime.Now.ToShortDateString());
                sw.Write("<center><Table border=1  valign=op|middle>");
                sw.Write("<col span=\"2\" style=\"width:4em\" />");
                sw.Write("<col span=\"5\" style=\"width:8em\" />");
                sw.Write("<TR ALIGN=center><TD colspan=2 ><td>一<TD >二<TD >三<td>四<td>五<td>六");
                for (int k = 0; k < TimeTableGrid.GetLength(2); k++)
                {
                    sw.Write("<TR ALIGN=center><td colspan=8>{1}<tr ALIGN=center><TD >{0}<TD >{2}", NumToLessonstr[k], htbLessonTimeDesc[k * 10 + 1], htbLessonTimeDesc[k * 10 + 2]);
                    for (int j = 0; j < TimeTableGrid.GetLength(1); j++)
                    {
                        string bgcolor = "white";
                        string cstr = "";
                        int SUID = 0;
                        for (int i = 0; i < TimeTableGrid.GetLength(0); i++)
                        {
                            foreach (int tsid in unitkeyToTeachernos(TimeTableGrid[i, j, k]))
                            {
                                if (tsid == tid)
                                {
                                    cstr = "";
                                    int cnt = 0;
                                    foreach (int csid in unitkeyToClassnos(TimeTableGrid[i, j, k]))
                                    {
                                        if (cnt++ > 0) cstr += " ";
                                        cstr += htbIDClass[csid];
                                    }
                                    SUID = TimeTableGrid[i, j, k];

                                    break;
                                }
                            }
                        }

                        //------------------------
                        if (this.htbTeacherRestWeekLesson.ContainsKey(j * 10 + k))
                        {
                            String[] sa0 = this.htbTeacherRestWeekLesson[j * 10 + k].ToString().Split(',');
                            foreach (string s in sa0)
                            {
                                if (tid == int.Parse(s))
                                {
                                    bgcolor = "Silver";
                                }
                            }
                        }
                        //--------------
                        sw.Write("<TD bgcolor={2}>{0}<br>{1}", htbIDSubjectShortName[unitkeyToSubjectID(SUID)], cstr,bgcolor);
                    }
                }
                sw.Write("</Table></center>");
                sw.WriteLine("<br style=\"page-break-before:always\">");
            }
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
        public void Export_WeekDay_Course_TimeTable(string FileName,string course_id_s)
        {
            String[] subject_ids = course_id_s.Split(',');
            MessageBox.Show(
                TimeTableGrid.GetLength(0) + "\n" +
                TimeTableGrid.GetLength(1) + "\n" +
            TimeTableGrid.GetLength(2)
                );
            StreamWriter sw = new StreamWriter(FileName, false, Encoding.Default);
          //  sw.WriteLine("<center>{0}上課時間表  日期{1}</center>", htbIDClass[i + 1], DateTime.Now.ToShortDateString());
            sw.Write("<center><Table border=1 ALIGN=center valign=op|middle>");
            sw.Write("<col span=\"2\" style=\"width:4em\" />");
            sw.Write("<col span=\"5\" style=\"width:8em\" />");
            sw.Write("<TR ALIGN=center><TD colspan=2 ><td>一<TD >二<TD >三<td>四<td>五<td>六");

            for (int i = 0; i < TimeTableGrid.GetLength(2); i++)
            {
                sw.Write("<TR ALIGN=center><td colspan=8>{1}<tr ALIGN=center><TD>{0}<TD>{2}", NumToLessonstr[i], htbLessonTimeDesc[i * 10 + 1], htbLessonTimeDesc[i * 10 + 2]);

                for (int j = 0; j < TimeTableGrid.GetLength(1); j++)
                {
                    sw.Write("<TD><table><tr>");
                    int cnt_suj = 0; 
                    for (int k = 0; k < TimeTableGrid.GetLength(0); k++)
                    {
                        string tstr = "";
                        int cnt = 0;
                        foreach (int tid in unitkeyToTeachernos(TimeTableGrid[k, j, i]))
                        {
                            if (cnt++ > 0) tstr += ",";
                            tstr += htbIDTeacherShortName[tid];

                        }
                        int subjectid_tmp = unitkeyToSubjectID(
                                         int.Parse(TimeTableGrid[k, j, i].ToString())
                                         );

                        bool flag = false;
                        foreach (String s in subject_ids)
                        {
                            if (s == subjectid_tmp.ToString()) {flag = true; break;}
                        }
                        if(course_id_s=="" || flag)
                        {
                            if (cnt_suj % 5 == 0) sw.Write("<tr>");
                            cnt_suj++;
                            
                            sw.Write("<TD><b>{0}{1}</b><br>{2}<br>{3}", subjectid_tmp, htbIDSubjectShortName[subjectid_tmp

                                         ], tstr, htbIDClass[unitkey2classno(TimeTableGrid[k,j,i])]);
                        }

                    }
                    sw.Write("</Table>");
                }
            }
            
            sw.Write("</Table></center>");
            sw.WriteLine("<br style=\"page-break-before:always\">");
            sw.Flush();
            sw.Close();
            sw.Dispose();

        }
        public void Export_Class_TimeTable(string FileName)
        {
            StreamWriter sw = new StreamWriter(FileName, false, Encoding.Default);
            for (int i = 0; i < TimeTableGrid.GetLength(0); i++)
            {
                sw.WriteLine("<center>{0}上課時間表  日期{1}</center>", htbIDClass[i + 1], DateTime.Now.ToShortDateString());
                sw.Write("<center><Table border=1 ALIGN=center valign=op|middle>");
                sw.Write("<col span=\"2\" style=\"width:4em\" />");
                sw.Write("<col span=\"5\" style=\"width:8em\" />");
                sw.Write("<TR ALIGN=center><TD colspan=2 ><td>一<TD >二<TD >三<td>四<td>五<td>六");
                for (int k = 0; k < TimeTableGrid.GetLength(2); k++)
                {
                    sw.Write("<TR ALIGN=center><td colspan=8>{1}<tr ALIGN=center><TD>{0}<TD>{2}", NumToLessonstr[k], htbLessonTimeDesc[k * 10 + 1], htbLessonTimeDesc[k * 10 + 2]);
                    for (int j = 0; j < TimeTableGrid.GetLength(1); j++)
                    {
                        string tstr = "";
                        int cnt = 0;
                        foreach (int tid in unitkeyToTeachernos(TimeTableGrid[i, j, k]))
                        {
                            if (cnt++ > 0) tstr += ",";
                            tstr += htbIDTeacherShortName[tid];

                        }
                        sw.Write("<TD><b>{0}</b><br>{1}", htbIDSubjectShortName[
                            unitkeyToSubjectID(
                                     int.Parse(TimeTableGrid[i, j, k].ToString())
                                     )
                                     ], tstr);
                    }
                }
                sw.Write("</Table></center>");
                sw.WriteLine("<br style=\"page-break-before:always\">");
            }
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
        public void Export_Cross_Week_Table(String FileName)
        {
            StreamWriter sw = new StreamWriter(FileName, false, Encoding.Default);
            sw.WriteLine("<center>課程周節安排  日期{0}</center>", DateTime.Now.ToShortDateString());
            sw.Write("<center><Table>");
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            sw.Write("<col style=\"width:6em\" />");
            sw.Write("<col span=\"30\" style=\"width:2em\" />");
            //            sw.Write("<col style=\"background-color: #6374AB; color: #ffffff\" /><col span=\"2\" style=\"background-color: #07B133; color: #ffffff;\" />");
            sw.Write("<tr><td>科目班別");
            for (int h = 1; h <= htbs.htbIDClass.Count; h++)
            {

                sw.Write("<td>{0} {1}</td>", htbIDClass[h].ToString().Substring(0, 2), htbIDClass[h].ToString().Substring(2, 2));
            }
            for (int h = 1; h <= htbIDSubject.Count; h++)
            {
                sw.Write("<tr><td>{0}</td>", htbIDSubjectShortName[h]);
                for (int i = 0; i < htbs.TimeTableGrid.GetLength(0); i++)
                {
                    int cnt = 0;
                    int suid = 0;
                    string weeklesson = "";
                    for (int j = 0; j < htbs.TimeTableGrid.GetLength(1); j++)
                        for (int k = 0; k < htbs.TimeTableGrid.GetLength(2); k++)
                            if (unitkeyToSubjectID(TimeTableGrid[i, j, k]) == h)
                            {
                                cnt++;
                                suid = TimeTableGrid[i, j, k];
                                weeklesson += string.Format("{0}<sup>{1}</sup>", weekstr[j], k + 1);

                            }
                    if (cnt > 0)
                    {
                        string tstr = "";
                        foreach (int tid in unitkeyToTeachernos(suid))
                        {
                            tstr += htbIDTeacher[tid] + ",";
                        }
                        sw.WriteLine("<td title=\"{1}\">{0}</td>", weeklesson, tstr);
                    }
                    else
                    {
                        sw.WriteLine("<td></td>");
                    }
                }
            }
            sw.WriteLine("</Table></center>");
            sw.Flush();
            sw.Close();
            sw.Dispose();

        }
        public void Export_Cross_Table(String FileName)
        {
            StreamWriter sw = new StreamWriter(FileName, false, Encoding.Default);
            sw.WriteLine("<center>課程安排  日期{0}</center>", DateTime.Now.ToShortDateString());
            sw.Write("<center><Table>");
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            sw.Write("<col style=\"width:6em\" />");
            sw.Write("<col span=\"30\" style=\"width:2em\" />");
            sw.Write("<tr><td>科目班別");
            for (int h = 1; h <= htbs.htbIDClass.Count; h++)
            {

                sw.Write("<td>{0} {1}</td>", htbIDClass[h].ToString().Substring(0, 2), htbIDClass[h].ToString().Substring(2, 2));
            }
            int[] classLessonSum = new int[htbs.htbIDClass.Count];

            for (int h = 1; h <= htbIDSubject.Count; h++)
            {
                sw.Write("<tr><td>{0}</td>", htbIDSubjectShortName[h]);
                for (int i = 0; i < htbs.TimeTableGrid.GetLength(0); i++)
                {
                    int cnt = 0;
                    int suid = 0;
                    string weeklesson = "";
                    for (int j = 0; j < htbs.TimeTableGrid.GetLength(1); j++)
                        for (int k = 0; k < htbs.TimeTableGrid.GetLength(2); k++)
                            if (unitkeyToSubjectID(TimeTableGrid[i, j, k]) == h)
                            {
                                cnt++;
                                suid = TimeTableGrid[i, j, k];
                                weeklesson += string.Format("{0}{1}", weekstr[j], k + 1);
                            }
                    if (cnt > 0)
                    {
                        string tstr = "";
                        foreach (int tid in unitkeyToTeachernos(suid))
                        {
                            tstr += htbIDTeacherShortName[tid] + ",";
                        }
                        classLessonSum[i]+=cnt;
                        sw.WriteLine("<td><sup title=\"{2}\" >{1}</a></sup>{0}</td>", tstr.Substring(0, tstr.Length - 1), cnt, weeklesson);
                    }
                    else
                    {
                        sw.WriteLine("<td></td>");
                    }
                }
            }
            sw.Write("<tr><td>{0}</td>","total");

            foreach (int ia in classLessonSum)
            {
                sw.Write("<td>{0}</td>", ia);
            }
            sw.WriteLine("</Table></center>");
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
        /// <summary>
        /// 按班級順序分配
        /// </summary>
        public void Sequence_ByClassMaster_Assig(List<int> mainsubjids)
        {
            foreach (DictionaryEntry de in htbIDClassMaster)
            {
                int classno = int.Parse(de.Key.ToString());
                int class_master = int.Parse(htbTeacherID[de.Value.ToString()].ToString());
                System.Diagnostics.Debug.WriteLine("{0}:{1}:{2}",classno,class_master,htbIDTeacher[class_master]);
                List<int> SUID_LIST = new List<int>();
                foreach (DictionaryEntry sbjde in htbIDLessionUnit)
                {
                    int suid = int.Parse(sbjde.Key.ToString());


                    if (unitkeyToClassno(suid) == classno && unitkeyToTeacherno(suid) == class_master && mainsubjids.Contains(unitkeyToSubjectID(suid)))
                    {
                        SUID_LIST.Add(suid);
                    }
                }
                foreach (int suid in SUID_LIST)
                {
                    for(int wno=0;wno<TimeTableGrid.GetLength(1);wno++)
                        if (Validate_SubjectUnitForLessonCell(suid, wno, 0))
                        {
                            AddSubjectToTimeTable(suid, wno,0);
                            goto Label1;
                        }
                    
                    for (int wno = 0; wno < TimeTableGrid.GetLength(1); wno++)
                        for (int lno = 1; lno < TimeTableGrid.GetLength(2); lno++)
                        {
                            if (Validate_SubjectUnitForLessonCell(suid, wno, lno))
                            {
                                AddSubjectToTimeTable(suid, wno, lno);
                                goto Label1;
                            }
                        }
                    Label1: ;
                }
            }
        }
        /// <summary>
        /// 順序分配
        /// </summary>
        /// <param name="id_range"></param>
        public void Sequence_By123_Assig(int id_range)
        {
            int cnt = 0;
            foreach (DictionaryEntry sbjde in htbIDLessionUnit)
            {
                bool flag = true;
                int suid = int.Parse(sbjde.Key.ToString());
                if (!Validate_LessonNotOnTable(suid)) continue;
                if (unitkeyToSubjectID(suid) < id_range)
                {
                    for (int lno = 0; lno < TimeTableGrid.GetLength(2); lno++)
                    for (int wno = 0; wno < TimeTableGrid.GetLength(1); wno++)
                        {
                            if (Validate_SubjectUnitForLessonCell(suid, wno, lno))
                            {
                                AddSubjectToTimeTable(suid, wno, lno);
                                flag = false;
                                goto Label1;
                            }
                        }
                }
            Label1:
                if (flag && Validate_LessonNotOnTable(suid))
                {
                    cnt++;
                    LastErrors.Add(string.Format("{0}:{1}",cnt,htbIDLessionUnit[suid].ToString()));
                }
                ;
            }
        }
        public void Sequence_By_SubjectIDList(int[] s_id_list)
        {
            List<int>[] suid_arr = new List<int>[s_id_list.Length];
            for (int i = 0; i < suid_arr.Length; i++) suid_arr[i] = new List<int>();
            foreach (DictionaryEntry sbjde in htbIDLessionUnit)
            {
                int suid = int.Parse(sbjde.Key.ToString());
                for (int i = 0; i < suid_arr.Length; i++)
                    if (unitkeyToSubjectID(suid) == s_id_list[i])
                    {
                        suid_arr[i].Add(suid);
                        break;
                    }
            }
            int cnt = 0;
            for (int i = 0; i < suid_arr.Length; i++)
                foreach(int a_suid in suid_arr[i])
                {
                    bool flag = true;
                    if (!Validate_LessonNotOnTable(a_suid)) continue;
                   for (int lno = 0; lno < TimeTableGrid.GetLength(2); lno++)
                        for (int wno = 0; wno < TimeTableGrid.GetLength(1); wno++)
                        {
                            if (Validate_SubjectUnitForLessonCell(a_suid, wno, lno))
                            {
                                AddSubjectToTimeTable(a_suid, wno, lno);
                                flag = false;
                                goto Label1;
                            }
                        }
                    Label1:
                        if (flag && Validate_LessonNotOnTable(a_suid))
                        {
                            cnt++;
                            LastErrors.Add(string.Format("{0}:{1}", cnt, htbIDLessionUnit[a_suid].ToString()));
                        }
                }
            }
        
    }
    /// <summary>
    /// 消息框(TextBox MultiLine)
    /// </summary>
    public class MsgBox : System.Windows.Forms.Form
    {
        private System.Windows.Forms.TextBox tb = new System.Windows.Forms.TextBox();
        /// <summary>
        /// MsgBox構造
        /// <example>
        /// MsgBox msg=new MsgBox(LastError.GetArray());
        /// MsgBox.ShowDialog();
        /// </example>
        /// </summary>
        /// <param name="a_msg">String[] 消息內容 </param>
        public MsgBox(string[] a_msg):base()
        {
            tb.Multiline = true;
            tb.Lines = a_msg;
            tb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            tb.Dock=System.Windows.Forms.DockStyle.Fill;
            this.Controls.Add(tb);
        }
    }
    public class NumericUpDownBox : System.Windows.Forms.Form
    {
        public System.Windows.Forms.NumericUpDown nud = new System.Windows.Forms.NumericUpDown();
        public System.Windows.Forms.Button OKbtn = new System.Windows.Forms.Button();
        private System.Windows.Forms.FlowLayoutPanel flp = new System.Windows.Forms.FlowLayoutPanel();
        public NumericUpDownBox(string TXT,int num )
        {
            this.Text = TXT;
            nud.Minimum = -1;
            nud.Maximum = 200;
            nud.Value = num;
            OKbtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            OKbtn.Text = "ok";
            flp.Controls.Add(nud);
            flp.Controls.Add(OKbtn);
            Controls.Add(flp);
        }
    }
    public class ListBoxForm : System.Windows.Forms.Form
    {
        public System.Windows.Forms.CheckedListBox lb = new System.Windows.Forms.CheckedListBox();
        public System.Windows.Forms.Button OKbtn = new System.Windows.Forms.Button();
        private System.Windows.Forms.TableLayoutPanel tblp = new System.Windows.Forms.TableLayoutPanel();

        public ListBoxForm()
        {
            OKbtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            OKbtn.Text = "ok";
            lb.Dock = System.Windows.Forms.DockStyle.Fill;
            tblp.ColumnCount = 2;
            tblp.RowCount = 1;
            tblp.Controls.Add(lb);
            tblp.Controls.Add(OKbtn);
            tblp.Dock = System.Windows.Forms.DockStyle.Fill;
            Controls.Add(tblp);
        }
    }
    public class SQLite_ESData 
    {
        public SQLite_ESData()
            : base()
        {
        }
        public SQLite_ESData(String path,String dbname)
            : base()
        {
            DBNAME = dbname + ".sqlite";
            _conn_txt = string.Format("Data Source=\"{0}\\{1}\"", path, DBNAME);
        }

        private static SQLite_ESData _instance = null;
        public static SQLite_ESData GetInst
        {
            get
            {
                if (_instance == null)
                {
                    //_instance = new SQLite_ESData();  //本地db
                    //_instance = new MySQL_ODBC_ESData();
                    _instance = new SQLite_ESData(); //250server
                }
                return _instance;
            }
        }
        private static String DBNAME = "data.sqlite";
        private static String _conn_txt = null;
        private static SQLiteConnection sqliteconn = null;

        public void Open_Conn()
        {
            if (GetConn().State == System.Data.ConnectionState.Closed)
            {
                GetConn().Open();
            }
        }
        public void Close_Conn()
        {
            GetConn().Close();
            GetConn().Dispose();
        }


        protected  string GetConn_Txt()
        {
            return _conn_txt;
        }
        protected  DbConnection GetConn()
        {
            if (sqliteconn == null)
            {
                sqliteconn = new SQLiteConnection(_conn_txt);
            }
            return sqliteconn;
        }
        public  DbDataReader Reader(string sql)
        {
            Open_Conn();
            SQLiteCommand cmd = new SQLiteCommand(sql, sqliteconn);
            return cmd.ExecuteReader();
        }
        public  DbDataReader Reader_ShowTables()
        {
            Open_Conn();
            SQLiteCommand cmd = new SQLiteCommand("select tbl_name from sqlite_master where type='table' order by tbl_name;", sqliteconn);
            return cmd.ExecuteReader();
        }
        public  int Exec(string sql)
        {
            return (new SQLiteCommand(sql, sqliteconn)).ExecuteNonQuery();
        }
        public  DbCommand GetCommand(string cmdsql)
        {
            return new SQLiteCommand(cmdsql, sqliteconn);
        }
        public  DbCommandBuilder GetDbCommandBuilder(DbDataAdapter da)
        {
            if (da is SQLiteDataAdapter)
            {
                return new SQLiteCommandBuilder((SQLiteDataAdapter)da);
            }
            else
            {
                throw new Exception("base.DBCommandBuilder!");
            }
        }
        public  DbDataAdapter GetAdapter()
        {
            return new SQLiteDataAdapter();
        }
        public  DbDataAdapter GetAdapter(DbCommand cmd)
        {
            if (cmd is SQLiteCommand)
            {
                return new SQLiteDataAdapter((SQLiteCommand)cmd);
            }
            else { throw new Exception("base.DBCommandBuilder!return base.GetAdapter(cmd)"); }
        }

        public  SQLiteCommand GetSQLiteCommand(string cmdsql, SQLiteParameter[] paras)
        {
            SQLiteCommand cmd = new SQLiteCommand(cmdsql, sqliteconn);
            if (paras != null)
                for (int i = 0; i < paras.Length; i++)
                {
                    cmd.Parameters.Add(paras[i]);
                }
            return cmd;
        }
        public  string ShowTablesSQL()
        {
            //return base.ShowTablesSQL();
            return "select tbl_name from sqlite_master where type='table' order by tbl_name;";
        }
        public String ViewTableTxt(String TableName)
        {
            MessageBox.Show(_conn_txt);
            Open_Conn();
            String txt = "TabeName:" + TableName + "\n";
            System.Data.Common.DbDataReader dr = Reader(String.Format("select * from {0};", TableName));
            for (int i = 0; i < dr.FieldCount; i++)
            {
                txt += dr.GetName(i) + "\t ";
            }
            txt += "\n";
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        try
                        {
                            txt += dr[i].ToString() + "\t ";
                        }
                        catch
                        {
                            MessageBox.Show(dr[0].ToString());
                        }
                    }
                    txt += "\n";
                }
            }
            dr.Close();
            dr.Dispose();
            return txt;
        }
    }
}
