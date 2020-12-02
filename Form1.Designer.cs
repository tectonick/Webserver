namespace Webserver
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.startButton = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitTrayButton = new System.Windows.Forms.ToolStripMenuItem();
            this.openTrayButton = new System.Windows.Forms.ToolStripMenuItem();
            this.webRootInput = new System.Windows.Forms.TextBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.rootPathLabel = new System.Windows.Forms.Label();
            this.trayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(232, 25);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start server";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.trayMenu;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // trayMenu
            // 
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openTrayButton,
            this.exitTrayButton});
            this.trayMenu.Name = "trayMenu";
            this.trayMenu.Size = new System.Drawing.Size(104, 48);
            this.trayMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.trayMenu_ItemClicked);
            this.trayMenu.Click += new System.EventHandler(this.trayMenu_Click);
            // 
            // exitTrayButton
            // 
            this.exitTrayButton.Name = "exitTrayButton";
            this.exitTrayButton.Size = new System.Drawing.Size(103, 22);
            this.exitTrayButton.Text = "Exit";
            // 
            // openTrayButton
            // 
            this.openTrayButton.Name = "openTrayButton";
            this.openTrayButton.Size = new System.Drawing.Size(103, 22);
            this.openTrayButton.Text = "Open";
            // 
            // webRootInput
            // 
            this.webRootInput.Location = new System.Drawing.Point(12, 25);
            this.webRootInput.Name = "webRootInput";
            this.webRootInput.Size = new System.Drawing.Size(214, 20);
            this.webRootInput.TabIndex = 2;
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(313, 25);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 0;
            this.stopButton.Text = "Stop server";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // rootPathLabel
            // 
            this.rootPathLabel.AutoSize = true;
            this.rootPathLabel.Location = new System.Drawing.Point(12, 9);
            this.rootPathLabel.Name = "rootPathLabel";
            this.rootPathLabel.Size = new System.Drawing.Size(51, 13);
            this.rootPathLabel.TabIndex = 3;
            this.rootPathLabel.Text = "Web root";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 450);
            this.Controls.Add(this.rootPathLabel);
            this.Controls.Add(this.webRootInput);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Simple web server";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.trayMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip trayMenu;
        private System.Windows.Forms.ToolStripMenuItem exitTrayButton;
        private System.Windows.Forms.ToolStripMenuItem openTrayButton;
        private System.Windows.Forms.TextBox webRootInput;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label rootPathLabel;
    }
}

