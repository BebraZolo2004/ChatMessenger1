namespace ChatServerApp
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
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(25, 234);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(291, 20);
            this.txtPort.TabIndex = 20;
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(25, 77);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(291, 20);
            this.txtIP.TabIndex = 18;
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(25, 334);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(291, 23);
            this.btnStartServer.TabIndex = 21;
            this.btnStartServer.Text = "Подключить сервер";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 617);
            this.Controls.Add(this.btnStartServer);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIP);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Button btnStartServer;
    }
}

