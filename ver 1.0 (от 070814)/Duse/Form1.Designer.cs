namespace Duse
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSign = new System.Windows.Forms.Button();
            this.btnUnsign = new System.Windows.Forms.Button();
            this.btnChkSign = new System.Windows.Forms.Button();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.lb1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSign
            // 
            this.btnSign.Location = new System.Drawing.Point(12, 28);
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(116, 23);
            this.btnSign.TabIndex = 0;
            this.btnSign.Text = "Подписать";
            this.btnSign.UseVisualStyleBackColor = true;
            this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
            // 
            // btnUnsign
            // 
            this.btnUnsign.Location = new System.Drawing.Point(12, 58);
            this.btnUnsign.Name = "btnUnsign";
            this.btnUnsign.Size = new System.Drawing.Size(116, 23);
            this.btnUnsign.TabIndex = 1;
            this.btnUnsign.Text = "Снять подпись";
            this.btnUnsign.UseVisualStyleBackColor = true;
            this.btnUnsign.Click += new System.EventHandler(this.btnUnsign_Click);
            // 
            // btnChkSign
            // 
            this.btnChkSign.Location = new System.Drawing.Point(12, 87);
            this.btnChkSign.Name = "btnChkSign";
            this.btnChkSign.Size = new System.Drawing.Size(116, 23);
            this.btnChkSign.TabIndex = 2;
            this.btnChkSign.Text = "Проверить подпись";
            this.btnChkSign.UseVisualStyleBackColor = true;
            this.btnChkSign.Click += new System.EventHandler(this.btnChkSign_Click);
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(12, 116);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(116, 23);
            this.btnEncrypt.TabIndex = 3;
            this.btnEncrypt.Text = "Зашифровать";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Location = new System.Drawing.Point(12, 145);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(116, 23);
            this.btnDecrypt.TabIndex = 4;
            this.btnDecrypt.Text = "Расшифровать";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtLog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtLog.Location = new System.Drawing.Point(0, 181);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(612, 133);
            this.txtLog.TabIndex = 5;
            this.txtLog.TextChanged += new System.EventHandler(this.txtLog_TextChanged);
            // 
            // listView1
            // 
            this.listView1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.listView1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.listView1.Location = new System.Drawing.Point(134, 22);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(478, 152);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // lb1
            // 
            this.lb1.AutoSize = true;
            this.lb1.Location = new System.Drawing.Point(134, 6);
            this.lb1.Name = "lb1";
            this.lb1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lb1.Size = new System.Drawing.Size(0, 13);
            this.lb1.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 314);
            this.Controls.Add(this.lb1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.btnEncrypt);
            this.Controls.Add(this.btnChkSign);
            this.Controls.Add(this.btnUnsign);
            this.Controls.Add(this.btnSign);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSign;
        private System.Windows.Forms.Button btnUnsign;
        private System.Windows.Forms.Button btnChkSign;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label lb1;
    }
}

