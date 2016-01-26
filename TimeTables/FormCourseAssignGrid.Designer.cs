namespace TimeTables
{
    partial class FormCourseAssignGrid
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dGV = new System.Windows.Forms.DataGridView();
            this.btnSubjectClassno = new System.Windows.Forms.DataGridViewButtonColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tslLoadFile = new System.Windows.Forms.ToolStripLabel();
            this.tslSaveFile = new System.Windows.Forms.ToolStripLabel();
            this.tslDefineMinUnit = new System.Windows.Forms.ToolStripLabel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dGV)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dGV
            // 
            this.dGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btnSubjectClassno});
            this.dGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV.Location = new System.Drawing.Point(3, 12);
            this.dGV.Name = "dGV";
            this.dGV.RowTemplate.Height = 24;
            this.dGV.Size = new System.Drawing.Size(691, 624);
            this.dGV.TabIndex = 0;
            this.dGV.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGV_CellDoubleClick);
            this.dGV.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGV_CellClick);
            // 
            // btnSubjectClassno
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSubjectClassno.DefaultCellStyle = dataGridViewCellStyle1;
            this.btnSubjectClassno.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubjectClassno.Frozen = true;
            this.btnSubjectClassno.HeaderText = "Subject\\Classno";
            this.btnSubjectClassno.Name = "btnSubjectClassno";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslLoadFile,
            this.tslSaveFile,
            this.tslDefineMinUnit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(697, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tslLoadFile
            // 
            this.tslLoadFile.Name = "tslLoadFile";
            this.tslLoadFile.Size = new System.Drawing.Size(46, 22);
            this.tslLoadFile.Text = "LoadFile";
            this.tslLoadFile.Click += new System.EventHandler(this.tslLoadFile_Click);
            // 
            // tslSaveFile
            // 
            this.tslSaveFile.Name = "tslSaveFile";
            this.tslSaveFile.Size = new System.Drawing.Size(44, 22);
            this.tslSaveFile.Text = "SaveFile";
            this.tslSaveFile.Click += new System.EventHandler(this.tslSaveFile_Click);
            // 
            // tslDefineMinUnit
            // 
            this.tslDefineMinUnit.Name = "tslDefineMinUnit";
            this.tslDefineMinUnit.Size = new System.Drawing.Size(66, 22);
            this.tslDefineMinUnit.Text = "Def Unit>file";
            this.tslDefineMinUnit.Click += new System.EventHandler(this.tslDefineMinUnit_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.dGV, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1.408451F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 98.59155F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(697, 639);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // FormCourseAssignGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 664);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FormCourseAssignGrid";
            this.Text = "FormCourseAssignGrid";
            ((System.ComponentModel.ISupportInitialize)(this.dGV)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dGV;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel tslLoadFile;
        private System.Windows.Forms.ToolStripLabel tslSaveFile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripLabel tslDefineMinUnit;
        private System.Windows.Forms.DataGridViewButtonColumn btnSubjectClassno;
    }
}