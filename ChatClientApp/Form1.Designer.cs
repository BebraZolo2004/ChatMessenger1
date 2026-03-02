namespace ChatClientApp
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
            this.txtChatHistory = new System.Windows.Forms.TextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.lstUsers = new System.Windows.Forms.ListBox();
            this.btnFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtChatHistory
            // 
            this.txtChatHistory.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtChatHistory.Location = new System.Drawing.Point(0, 0);
            this.txtChatHistory.Multiline = true;
            this.txtChatHistory.Name = "txtChatHistory";
            this.txtChatHistory.ReadOnly = true;
            this.txtChatHistory.Size = new System.Drawing.Size(800, 368);
            this.txtChatHistory.TabIndex = 2;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(12, 418);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(636, 20);
            this.txtMessage.TabIndex = 4;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(679, 415);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(109, 23);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "Отправить";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // lstUsers
            // 
            this.lstUsers.FormattingEnabled = true;
            this.lstUsers.Location = new System.Drawing.Point(12, 374);
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.Size = new System.Drawing.Size(636, 30);
            this.lstUsers.TabIndex = 6;
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(679, 386);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(109, 23);
            this.btnFile.TabIndex = 7;
            this.btnFile.Text = "Прикрепить файл";
            this.btnFile.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 460);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.lstUsers);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.txtChatHistory);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtChatHistory;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ListBox lstUsers;
        private System.Windows.Forms.Button btnFile;
    }
}

