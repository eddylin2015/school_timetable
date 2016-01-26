using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace TimeTables
{
    public class GridActControls
    {
    }
    public  interface iRefreshGridContent
    {
        void RefreshContent();
        void SetFontSize(System.Drawing.Font f);
    }
    
    public interface iGridAction
    {
        int GetGridCellSUID(int weekno, int lessno);
        bool AddSubjectToTimeTable(int unitkey, int weekno, int lessonno);
        string[] GridCellItemStringArr(int a_SUID);
        string GridCellItemString(int a_SUID);
        bool ExchangeSubject(string stbName, string dtbName);
        void View_Color_DVG_For_FreeCell(System.Windows.Forms.TableLayoutPanel tlp, int a_suid);
        void View_Color_DVG_For_ExchangeCell(System.Windows.Forms.TableLayoutPanel tlp, int s_suid, int weekno, int lessonno);
        // void GridCelltxtInput_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e);
        // void mWheel_click(Object sender, EventArgs e);
    }
    public interface iListAction
    {
        List<string> ListStr();
        void UpdateSUIDKeyListItems();
    }
    public class C_ClassNoTimeTable: iListAction,iGridAction
    {
        protected int key;             //
        protected List<int> SUID_keys; //³B²zSUID ¦Cªí

        public C_ClassNoTimeTable(int akey)
        {
            key = akey;
            UpdateSUIDKeyListItems();
        }
        public C_ClassNoTimeTable(List<int> a_SUID_keys)
        {
            SUID_keys = a_SUID_keys;
        }
        public virtual List<String> ListStr()
        {
            return null;
        }
        public virtual void UpdateSUIDKeyListItems()
        {
            return ;
        }
        public virtual int GetGridCellSUID(int weekno, int lessno)
        {
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            foreach (int asuid in SUID_keys)
                for (int c = 0; c < htbs.TimeTableGrid.GetLength(0); c++)
                {
                    if (asuid == htbs.TimeTableGrid[c, weekno, lessno]) return htbs.TimeTableGrid[c, weekno, lessno];
                }
            return 0;
        }
        public virtual bool ExchangeSubject(string stbName, string dtbName)
        {
            return false;
        }
        public virtual string[] GridCellItemStringArr(int a_SUID)
        {
            return null;
        }
        public virtual string GridCellItemString(int a_SUID)
        {
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            if (htbs.htbIDLessionUnit.ContainsKey(a_SUID) && a_SUID>0)
                return htbs.htbIDLessionUnit[a_SUID].ToString();
            return null;
        }
        public virtual bool AddSubjectToTimeTable(int unitkey, int weekno, int lessonno)
        {
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            return htbs.AddSubjectToTimeTable(unitkey, weekno, lessonno);
        }

        public void View_Color_DVG_For_FreeCell(System.Windows.Forms.TableLayoutPanel tlp, int a_suid)
        {
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            for (int i = 0; i < htbs.TimeTableGrid.GetLength(1); i++)
            {
                for (int j = 0; j < htbs.TimeTableGrid.GetLength(2); j++)
                {

                    System.Windows.Forms.TextBox tb = (System.Windows.Forms.TextBox)tlp.Controls[Basic_HTB_Info.WeekLesson2TextBoxName(i,j)];
                    if (htbs.Validate_SubjectUnitForLessonCell(a_suid, i, j))
                    {
                        tb.BackColor = System.Drawing.Color.Green;
                        tb.Enabled = true;
                    }
                    else
                    {
                        string c = GridCellItemString(GetGridCellSUID(i, j));
                        if (c == null)
                        {
                            tb.BackColor =  System.Drawing.Color.Gray;
                            tb.Enabled = false;
                        }
                        else
                        {
                            tb.BackColor =  System.Drawing.Color.Red;
                            tb.Text = c;
                        }
                    }
                }
            }
            return;
        }
        public void View_Color_DVG_For_ExchangeCell(System.Windows.Forms.TableLayoutPanel tlp, int s_suid,int weekno,int lessonno )
        {
            if (s_suid == 0) return;
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            int[] classes = htbs.unitkeyToClassnos(s_suid);
            if (classes.Length != 1 || classes[0]==0) return;
            
            int s_classno = classes[0];

            htbs.TimeTableGrid[s_classno-1,weekno, lessonno] = 0;

            for (int i = 0; i < htbs.TimeTableGrid.GetLength(1); i++)
            {
                for (int j = 0; j < htbs.TimeTableGrid.GetLength(2); j++)
                {

                    if (i == weekno && j == lessonno) continue;
                    int d_suid=GetGridCellSUID(i, j);

                    System.Windows.Forms.TextBox tb = (System.Windows.Forms.TextBox)tlp.Controls[Basic_HTB_Info.WeekLesson2TextBoxName(i,j)];
                    if (d_suid == 0 && htbs.Validate_SubjectUnitForLessonCell(s_suid, i, j))
                    {
                        tb.BackColor = System.Drawing.Color.AliceBlue;
                        continue;
                    }
                    else
                    {
                        tb.BackColor = System.Drawing.Color.White;
                    }
                    if (d_suid == 0) continue; 

                    int d_weekno = i;
                    int d_lessonno = j;
                    int[] d_classes = htbs.unitkeyToClassnos(d_suid);
                    if (d_classes.Length != 1) continue;
                    int d_classno = d_classes[0];
                    if (d_classno == 0 || d_classno > 29) System.Windows.Forms.MessageBox.Show(d_suid.ToString());
                    htbs.TimeTableGrid[d_classno-1,d_weekno,d_lessonno]=0;
                    
                    if (htbs.Validate_SubjectUnitForLessonCell(s_suid, d_weekno, d_lessonno) 
                        && htbs.Validate_SubjectUnitForLessonCell(d_suid, weekno, lessonno))
                    {
                        tb.BackColor = System.Drawing.Color.AliceBlue;
                    }
                    else 
                    {
                         tb.BackColor = System.Drawing.Color.White;
                    }

                    htbs.TimeTableGrid[d_classno-1, d_weekno, d_lessonno] = d_suid;
                }
            }
            label01:htbs.TimeTableGrid[s_classno-1,weekno, lessonno] = s_suid;
            return;
        }
    }

    public class C_ClassNoTimeTable_for_Class : C_ClassNoTimeTable
    {
        public C_ClassNoTimeTable_for_Class(int key)
            : base(key)
        {
        }
        public override void UpdateSUIDKeyListItems()
        {
            if (SUID_keys == null) SUID_keys = new List<int>();
             SUID_keys.Clear();
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            foreach (DictionaryEntry de in htbs.htbIDLessionUnit)
            {
                int asuid = int.Parse(de.Key.ToString());
                int[] asuid_class = htbs.unitkeyToClassnos(asuid);
                foreach (int asuid_a_class in asuid_class)
                {
                    if (asuid_a_class == key)
                    {
                        SUID_keys.Add(asuid);
                        break;
                    }
                }
            }
        }
        public override bool ExchangeSubject(string stbName, string dtbName)
        {

            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            int tmpint0 = int.Parse(stbName.Substring(7));
            int weekno0 = (tmpint0 - 1) % Basic_HTB_Info.grid_panel_cols;
            int lessno0 = (tmpint0 - 1) / Basic_HTB_Info.grid_panel_cols;
            int tmpint1 = int.Parse(dtbName.Substring(7));
            int weekno1 = (tmpint1 - 1) % Basic_HTB_Info.grid_panel_cols;
            int lessno1 = (tmpint1 - 1) / Basic_HTB_Info.grid_panel_cols;
            int s_suid = this.GetGridCellSUID(weekno0, lessno0);
            int d_suid = this.GetGridCellSUID(weekno1, lessno1);
            if (s_suid == d_suid) return false;

            if (s_suid <= 0 || !htbs.RemoveSubjectFromTimeTable(s_suid))             goto LabelF;
            if (!htbs.RemoveSubjectFromTimeTable(d_suid)) goto LabelF;

            if (s_suid != d_suid &&(d_suid==0 || htbs.Validate_SubjectUnitForLessonCell(d_suid, weekno0, lessno0))
                &&
               ( s_suid==0 ||htbs.Validate_SubjectUnitForLessonCell(s_suid, weekno1, lessno1)))
            {

                htbs.AddSubjectToTimeTable(s_suid, weekno1, lessno1);
                htbs.AddSubjectToTimeTable(d_suid, weekno0, lessno0);
                return true;

            }
            LabelF:
            htbs.AddSubjectToTimeTable(d_suid, weekno1, lessno1);
            htbs.AddSubjectToTimeTable(s_suid, weekno0, lessno0);
            return false;

        }
        public override string[] GridCellItemStringArr(int a_SUID)
        {
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();

            string[] sr = htbs.htbIDLessionUnit[a_SUID].ToString().Split('(');
            string[] TextAr = new string[2];
            if (a_SUID % 10 == 1)
            { TextAr[0] = sr[0]; }
            else if (htbs.unitkey2LessonType(a_SUID) == 2)
            {
                TextAr[0] = (a_SUID % 10).ToString() + sr[0];
            }
            else
            {
                TextAr[0] = sr[0];
            }
            if (sr.Length > 1) TextAr[1] = sr[1].Substring(8);
            return TextAr;
        }
    }
    public class C_ClassNoTimeTable_for_Teacher : C_ClassNoTimeTable
    {
        public C_ClassNoTimeTable_for_Teacher(int key)
            : base(key)
        {
        }

        public override void UpdateSUIDKeyListItems()
        {
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            SUID_keys = new List<int>();
            foreach (DictionaryEntry de in htbs.htbIDLessionUnit)
            {
                int asuid = int.Parse(de.Key.ToString());
                int[] asuid_teachers = htbs.unitkeyToTeachernos(asuid);
                foreach (int asuid_a_teacher in asuid_teachers)
                {
                    if (asuid_a_teacher == key)
                    {
                        SUID_keys.Add(asuid);
                        break;
                    }
                }
            }
        }
        public override string[] GridCellItemStringArr(int a_SUID)
        {
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            string[] sr = htbs.htbIDLessionUnit[a_SUID].ToString().Split('(');
            string[] TextAr = new string[2];

            if (a_SUID % 10 == 1)
            { TextAr[0] = sr[0]; }
            else
                TextAr[0] = (a_SUID % 10).ToString() + sr[0];

            if (sr.Length > 1) TextAr[1] = sr[1].Substring(8) + sr[1].Substring(4, 4);
            return TextAr;
        }
    }

    public class C_FormClassNoTimeTableDoubleClick_PreAssigSubj : C_ClassNoTimeTable
    {
        public C_FormClassNoTimeTableDoubleClick_PreAssigSubj(List<int> a_suid_keys)
            : base(a_suid_keys)
        {
        }
        public override List<string> ListStr()
        {
            List<string> resList = new List<string>();
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
                foreach (int sk in SUID_keys)
                {
                        bool flag = true;
                        for (int k = 0; k < htbs.TimeTableGrid.GetLength(0); k++)
                            for (int i = 0; i < htbs.TimeTableGrid.GetLength(1); i++)
                                for (int j = 0; j < htbs.TimeTableGrid.GetLength(2); j++)
                                    if (sk == htbs.TimeTableGrid[k, i, j]) flag = false;

                        if (flag)
                        {
                            resList.Add(string.Format("{0}:{1}",sk, htbs.htbIDLessionUnit[sk]));
                        }
                    }
            return resList;
        }
    }

    public class C_FormClassNoTimeTableDoubleClick_Class : C_ClassNoTimeTable
    {
        public C_FormClassNoTimeTableDoubleClick_Class(int akey)
            : base(akey)
        {
        }
        public override void UpdateSUIDKeyListItems()
        {
            SUID_keys = new List<int>();
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            foreach (DictionaryEntry item in htbs.htbIDLessionUnit)
            {
                int a_suid = int.Parse(item.Key.ToString());
                int[] suid_class = htbs.unitkeyToClassnos(a_suid);
                foreach (int classno in suid_class)
                {
                    if (classno == key)
                    {
                        SUID_keys.Add(a_suid);
                    }
                }
            }
        }
        public override List<string> ListStr()
        {
            List<string> resList = new List<string>();

            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();

            foreach (int a_suid in SUID_keys)
            {
                bool flag = true;
                for (int i = 0; i < htbs.TimeTableGrid.GetLength(1); i++)
                    for (int j = 0; j < htbs.TimeTableGrid.GetLength(2); j++)
                        if (a_suid == htbs.TimeTableGrid[key - 1, i, j]) flag = false;
                if (flag) resList.Add(string.Format("{0}:{1}", a_suid, htbs.htbIDLessionUnit[a_suid]));
            }
            resList.Sort();
            return resList;
        }
    }
    public class C_FormClassNoTimeTableDoubleClick_Teacher : C_ClassNoTimeTable
    {
        public C_FormClassNoTimeTableDoubleClick_Teacher(int akey)
            : base(akey)
        {
        }

        public override void UpdateSUIDKeyListItems()
        {
            SUID_keys = new List<int>();
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            foreach (DictionaryEntry item in htbs.htbIDLessionUnit)
            {
                int a_suid = int.Parse(item.Key.ToString());
                int[] suid_teachers = htbs.unitkeyToTeachernos(a_suid);
                foreach (int t_id in suid_teachers)
                {
                    if (t_id == key)
                    {
                        SUID_keys.Add(a_suid);
                    }
                }
            }
        }
        public override List<string> ListStr()
        {
            List<string> resList = new List<string>();
            Basic_HTB_Info htbs = Basic_HTB_Info.GetInstance();
            foreach (int a_suid in SUID_keys)
            {
                bool flag = true;
                for (int h = 0; h < htbs.TimeTableGrid.GetLength(0); h++)
                    for (int i = 0; i < htbs.TimeTableGrid.GetLength(1); i++)
                        for (int j = 0; j < htbs.TimeTableGrid.GetLength(2); j++)
                            if (a_suid == htbs.TimeTableGrid[h, i, j]) flag = false;
                if (flag) resList.Add(string.Format("{0}:{1}", a_suid, htbs.htbIDLessionUnit[a_suid]));
            }
            resList.Sort();
            return resList;
        }
    }
}
