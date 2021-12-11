
namespace RecipeManager
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.DownLoadStatistic = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.CountDayDownloadLabel = new System.Windows.Forms.Label();
            this.CountRecipeGetLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.DownLoadDayBufferToPLC = new System.Windows.Forms.Button();
            this.DownloadFromFileToBuffer = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // DownLoadStatistic
            // 
            this.DownLoadStatistic.Location = new System.Drawing.Point(679, 304);
            this.DownLoadStatistic.Name = "DownLoadStatistic";
            this.DownLoadStatistic.Size = new System.Drawing.Size(122, 101);
            this.DownLoadStatistic.TabIndex = 35;
            this.DownLoadStatistic.Text = "Считать статистику";
            this.DownLoadStatistic.UseVisualStyleBackColor = true;
            this.DownLoadStatistic.Click += new System.EventHandler(this.DownLoadStatistic_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.Location = new System.Drawing.Point(13, 147);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(208, 16);
            this.label11.TabIndex = 34;
            this.label11.Text = "----------------------------------------";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StatusLabel.Location = new System.Drawing.Point(238, 424);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(0, 16);
            this.StatusLabel.TabIndex = 33;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(227, 448);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(425, 46);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 32;
            // 
            // CountDayDownloadLabel
            // 
            this.CountDayDownloadLabel.AutoSize = true;
            this.CountDayDownloadLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CountDayDownloadLabel.Location = new System.Drawing.Point(86, 244);
            this.CountDayDownloadLabel.Name = "CountDayDownloadLabel";
            this.CountDayDownloadLabel.Size = new System.Drawing.Size(30, 31);
            this.CountDayDownloadLabel.TabIndex = 31;
            this.CountDayDownloadLabel.Text = "0";
            // 
            // CountRecipeGetLabel
            // 
            this.CountRecipeGetLabel.AutoSize = true;
            this.CountRecipeGetLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CountRecipeGetLabel.Location = new System.Drawing.Point(99, 83);
            this.CountRecipeGetLabel.Name = "CountRecipeGetLabel";
            this.CountRecipeGetLabel.Size = new System.Drawing.Size(30, 31);
            this.CountRecipeGetLabel.TabIndex = 30;
            this.CountRecipeGetLabel.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(13, 215);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 16);
            this.label2.TabIndex = 29;
            this.label2.Text = "Кол-во загруженно в ПЛК:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(-1, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 16);
            this.label1.TabIndex = 28;
            this.label1.Text = "Кол-во  считанных из файла:";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(226, 20);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(425, 385);
            this.richTextBox1.TabIndex = 27;
            this.richTextBox1.Text = "";
            // 
            // DownLoadDayBufferToPLC
            // 
            this.DownLoadDayBufferToPLC.Location = new System.Drawing.Point(679, 160);
            this.DownLoadDayBufferToPLC.Name = "DownLoadDayBufferToPLC";
            this.DownLoadDayBufferToPLC.Size = new System.Drawing.Size(122, 101);
            this.DownLoadDayBufferToPLC.TabIndex = 26;
            this.DownLoadDayBufferToPLC.Text = "Загрузить в ПЛК";
            this.DownLoadDayBufferToPLC.UseVisualStyleBackColor = true;
            this.DownLoadDayBufferToPLC.Click += new System.EventHandler(this.DownLoadDayBufferToPLC_Click);
            // 
            // DownloadFromFileToBuffer
            // 
            this.DownloadFromFileToBuffer.Location = new System.Drawing.Point(679, 19);
            this.DownloadFromFileToBuffer.Name = "DownloadFromFileToBuffer";
            this.DownloadFromFileToBuffer.Size = new System.Drawing.Size(122, 99);
            this.DownloadFromFileToBuffer.TabIndex = 25;
            this.DownloadFromFileToBuffer.Text = "Загрузить из файла";
            this.DownloadFromFileToBuffer.UseVisualStyleBackColor = true;
            this.DownloadFromFileToBuffer.Click += new System.EventHandler(this.DownloadFromFileToBuffer_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "RecipeManager";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(819, 510);
            this.Controls.Add(this.DownLoadStatistic);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.CountDayDownloadLabel);
            this.Controls.Add(this.CountRecipeGetLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.DownLoadDayBufferToPLC);
            this.Controls.Add(this.DownloadFromFileToBuffer);
            this.Name = "Form1";
            this.Text = "RecipeManager_130";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DownLoadStatistic;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label CountDayDownloadLabel;
        private System.Windows.Forms.Label CountRecipeGetLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button DownLoadDayBufferToPLC;
        private System.Windows.Forms.Button DownloadFromFileToBuffer;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}

