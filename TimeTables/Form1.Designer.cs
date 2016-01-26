namespace TimeTables
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_MrsCourse_Export_file_in = new System.Windows.Forms.Button();
            this.btnCourseAssignGrid = new System.Windows.Forms.Button();
            this.btnManual_AllocationMode = new System.Windows.Forms.Button();
            this.btnTeacherWeekResetLessonno = new System.Windows.Forms.Button();
            this.Rtb = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btn_MrsCourse_Export_file_in
            // 
            this.btn_MrsCourse_Export_file_in.Location = new System.Drawing.Point(12, 6);
            this.btn_MrsCourse_Export_file_in.Name = "btn_MrsCourse_Export_file_in";
            this.btn_MrsCourse_Export_file_in.Size = new System.Drawing.Size(366, 23);
            this.btn_MrsCourse_Export_file_in.TabIndex = 0;
            this.btn_MrsCourse_Export_file_in.Text = "測試分配策略";
            this.btn_MrsCourse_Export_file_in.UseVisualStyleBackColor = true;
            this.btn_MrsCourse_Export_file_in.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCourseAssignGrid
            // 
            this.btnCourseAssignGrid.Location = new System.Drawing.Point(12, 63);
            this.btnCourseAssignGrid.Name = "btnCourseAssignGrid";
            this.btnCourseAssignGrid.Size = new System.Drawing.Size(366, 26);
            this.btnCourseAssignGrid.TabIndex = 1;
            this.btnCourseAssignGrid.Text = "課程分配表格_初始定義 Course Assig Grid";
            this.btnCourseAssignGrid.UseVisualStyleBackColor = true;
            this.btnCourseAssignGrid.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnManual_AllocationMode
            // 
            this.btnManual_AllocationMode.Location = new System.Drawing.Point(12, 124);
            this.btnManual_AllocationMode.Name = "btnManual_AllocationMode";
            this.btnManual_AllocationMode.Size = new System.Drawing.Size(366, 23);
            this.btnManual_AllocationMode.TabIndex = 2;
            this.btnManual_AllocationMode.Text = "主功能 Manual_Allocation";
            this.btnManual_AllocationMode.UseVisualStyleBackColor = true;
            this.btnManual_AllocationMode.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnTeacherWeekResetLessonno
            // 
            this.btnTeacherWeekResetLessonno.Location = new System.Drawing.Point(12, 95);
            this.btnTeacherWeekResetLessonno.Name = "btnTeacherWeekResetLessonno";
            this.btnTeacherWeekResetLessonno.Size = new System.Drawing.Size(366, 23);
            this.btnTeacherWeekResetLessonno.TabIndex = 3;
            this.btnTeacherWeekResetLessonno.Text = "教師休息表_Teacher_Week_Take_Rest_Grid";
            this.btnTeacherWeekResetLessonno.UseVisualStyleBackColor = true;
            this.btnTeacherWeekResetLessonno.Click += new System.EventHandler(this.btnTeacherWeekResetLessonno_Click);
            // 
            // Rtb
            // 
            this.Rtb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Rtb.Location = new System.Drawing.Point(12, 153);
            this.Rtb.Name = "Rtb";
            this.Rtb.Size = new System.Drawing.Size(366, 217);
            this.Rtb.TabIndex = 4;
            this.Rtb.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 369);
            this.Controls.Add(this.Rtb);
            this.Controls.Add(this.btnTeacherWeekResetLessonno);
            this.Controls.Add(this.btnManual_AllocationMode);
            this.Controls.Add(this.btnCourseAssignGrid);
            this.Controls.Add(this.btn_MrsCourse_Export_file_in);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_MrsCourse_Export_file_in;
        private System.Windows.Forms.Button btnCourseAssignGrid;
        private System.Windows.Forms.Button btnManual_AllocationMode;
        private System.Windows.Forms.Button btnTeacherWeekResetLessonno;
        private System.Windows.Forms.RichTextBox Rtb;
    }
}

