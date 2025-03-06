namespace VMDebugger
{
    partial class frmRegisters
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            lstRegisters = new System.Windows.Forms.ListView();
            colRegister = new System.Windows.Forms.ColumnHeader();
            colValue = new System.Windows.Forms.ColumnHeader();
            btnStep = new System.Windows.Forms.Button();
            tmrUpdate = new System.Windows.Forms.Timer(components);
            lstInfo = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            btnStart = new System.Windows.Forms.Button();
            btnDebug = new System.Windows.Forms.Button();
            lstStack = new System.Windows.Forms.ListBox();
            btnResume = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // lstRegisters
            // 
            lstRegisters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { colRegister, colValue });
            lstRegisters.HideSelection = false;
            lstRegisters.Location = new System.Drawing.Point(14, 14);
            lstRegisters.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            lstRegisters.Name = "lstRegisters";
            lstRegisters.Size = new System.Drawing.Size(300, 659);
            lstRegisters.TabIndex = 0;
            lstRegisters.UseCompatibleStateImageBehavior = false;
            lstRegisters.View = System.Windows.Forms.View.Details;
            // 
            // colRegister
            // 
            colRegister.Name = "colRegister";
            colRegister.Text = "Register";
            colRegister.Width = 120;
            // 
            // colValue
            // 
            colValue.Name = "colValue";
            colValue.Text = "Value";
            colValue.Width = 100;
            // 
            // btnStep
            // 
            btnStep.Location = new System.Drawing.Point(479, 687);
            btnStep.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnStep.Name = "btnStep";
            btnStep.Size = new System.Drawing.Size(140, 38);
            btnStep.TabIndex = 1;
            btnStep.Text = "Step (F10)";
            btnStep.UseVisualStyleBackColor = true;
            btnStep.Click += btnStep_Click;
            // 
            // tmrUpdate
            // 
            tmrUpdate.Enabled = true;
            tmrUpdate.Tick += tmrUpdate_Tick;
            // 
            // lstInfo
            // 
            lstInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2 });
            lstInfo.HideSelection = false;
            lstInfo.Location = new System.Drawing.Point(322, 14);
            lstInfo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            lstInfo.Name = "lstInfo";
            lstInfo.Size = new System.Drawing.Size(300, 153);
            lstInfo.TabIndex = 2;
            lstInfo.UseCompatibleStateImageBehavior = false;
            lstInfo.View = System.Windows.Forms.View.Details;
            lstInfo.SelectedIndexChanged += lstInfo_SelectedIndexChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Name = "columnHeader1";
            columnHeader1.Text = "Field";
            columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            columnHeader2.Name = "columnHeader2";
            columnHeader2.Text = "Value";
            columnHeader2.Width = 100;
            // 
            // btnStart
            // 
            btnStart.Location = new System.Drawing.Point(12, 684);
            btnStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnStart.Name = "btnStart";
            btnStart.Size = new System.Drawing.Size(115, 44);
            btnStart.TabIndex = 3;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnDebug
            // 
            btnDebug.Location = new System.Drawing.Point(134, 683);
            btnDebug.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnDebug.Name = "btnDebug";
            btnDebug.Size = new System.Drawing.Size(115, 44);
            btnDebug.TabIndex = 4;
            btnDebug.Text = "Start (Debug)";
            btnDebug.UseVisualStyleBackColor = true;
            btnDebug.Click += btnDebug_Click;
            // 
            // lstStack
            // 
            lstStack.FormattingEnabled = true;
            lstStack.ItemHeight = 15;
            lstStack.Location = new System.Drawing.Point(321, 174);
            lstStack.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            lstStack.Name = "lstStack";
            lstStack.Size = new System.Drawing.Size(300, 499);
            lstStack.TabIndex = 5;
            // 
            // btnResume
            // 
            btnResume.Location = new System.Drawing.Point(357, 687);
            btnResume.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnResume.Name = "btnResume";
            btnResume.Size = new System.Drawing.Size(115, 38);
            btnResume.TabIndex = 6;
            btnResume.Text = "Resume";
            btnResume.UseVisualStyleBackColor = true;
            btnResume.Click += btnResume_Click;
            // 
            // frmRegisters
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(632, 741);
            Controls.Add(btnResume);
            Controls.Add(lstStack);
            Controls.Add(btnDebug);
            Controls.Add(btnStart);
            Controls.Add(lstInfo);
            Controls.Add(btnStep);
            Controls.Add(lstRegisters);
            DoubleBuffered = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "frmRegisters";
            Text = "Registers";
            FormClosing += frmRegisters_FormClosing;
            Load += frmRegisters_Load;
            Shown += frmRegisters_Shown;
            KeyDown += frmRegisters_KeyDown;
            KeyPress += frmRegisters_KeyPress;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListView lstRegisters;
        private System.Windows.Forms.ColumnHeader colRegister;
        private System.Windows.Forms.ColumnHeader colValue;
        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.ListView lstInfo;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnDebug;
        private System.Windows.Forms.ListBox lstStack;
        private System.Windows.Forms.Button btnResume;
    }
}

