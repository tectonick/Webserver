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
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openTrayButton = new System.Windows.Forms.ToolStripMenuItem();
            this.exitTrayButton = new System.Windows.Forms.ToolStripMenuItem();
            this.webRootInput = new System.Windows.Forms.TextBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.rootPathLabel = new System.Windows.Forms.Label();
            this.webRootBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.chooseFolderButton = new System.Windows.Forms.Button();
            this.pathToPHPInput = new System.Windows.Forms.TextBox();
            this.choosePHPFolderButton = new System.Windows.Forms.Button();
            this.PHPBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.pathToPHPLabel = new System.Windows.Forms.Label();
            this.configurationLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.logBox = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.startTrayButton = new System.Windows.Forms.ToolStripMenuItem();
            this.stopTrayButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.portLabel = new System.Windows.Forms.Label();
            this.addressLabel = new System.Windows.Forms.Label();
            this.addressInput = new System.Windows.Forms.TextBox();
            this.portInput = new System.Windows.Forms.TextBox();
            this.trayMenu.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(12, 12);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(141, 55);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start server";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayMenu;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "Simple server (stopped)";
            this.trayIcon.Visible = true;
            // 
            // trayMenu
            // 
            this.trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openTrayButton,
            this.toolStripSeparator1,
            this.startTrayButton,
            this.stopTrayButton,
            this.toolStripSeparator2,
            this.exitTrayButton});
            this.trayMenu.Name = "trayMenu";
            this.trayMenu.Size = new System.Drawing.Size(104, 104);
            this.trayMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.trayMenu_ItemClicked);
            // 
            // openTrayButton
            // 
            this.openTrayButton.Name = "openTrayButton";
            this.openTrayButton.Size = new System.Drawing.Size(103, 22);
            this.openTrayButton.Text = "Open";
            // 
            // exitTrayButton
            // 
            this.exitTrayButton.Name = "exitTrayButton";
            this.exitTrayButton.Size = new System.Drawing.Size(103, 22);
            this.exitTrayButton.Text = "Exit";
            // 
            // webRootInput
            // 
            this.webRootInput.Location = new System.Drawing.Point(2, 85);
            this.webRootInput.Name = "webRootInput";
            this.webRootInput.Size = new System.Drawing.Size(252, 20);
            this.webRootInput.TabIndex = 2;
            this.webRootInput.Text = "C:\\Users\\korn9\\Desktop\\КСиС КП\\TestWebRoot";
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(161, 12);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(141, 55);
            this.stopButton.TabIndex = 0;
            this.stopButton.Text = "Stop server";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // rootPathLabel
            // 
            this.rootPathLabel.AutoSize = true;
            this.rootPathLabel.Location = new System.Drawing.Point(2, 69);
            this.rootPathLabel.Name = "rootPathLabel";
            this.rootPathLabel.Size = new System.Drawing.Size(51, 13);
            this.rootPathLabel.TabIndex = 3;
            this.rootPathLabel.Text = "Web root";
            // 
            // chooseFolderButton
            // 
            this.chooseFolderButton.Location = new System.Drawing.Point(260, 83);
            this.chooseFolderButton.Name = "chooseFolderButton";
            this.chooseFolderButton.Size = new System.Drawing.Size(27, 23);
            this.chooseFolderButton.TabIndex = 4;
            this.chooseFolderButton.Text = "...";
            this.chooseFolderButton.UseVisualStyleBackColor = true;
            this.chooseFolderButton.Click += new System.EventHandler(this.chooseFolderButton_Click);
            // 
            // pathToPHPInput
            // 
            this.pathToPHPInput.Location = new System.Drawing.Point(2, 123);
            this.pathToPHPInput.Name = "pathToPHPInput";
            this.pathToPHPInput.Size = new System.Drawing.Size(252, 20);
            this.pathToPHPInput.TabIndex = 5;
            this.pathToPHPInput.Text = "C:\\php\\php-cgi.exe";
            // 
            // choosePHPFolderButton
            // 
            this.choosePHPFolderButton.Location = new System.Drawing.Point(260, 121);
            this.choosePHPFolderButton.Name = "choosePHPFolderButton";
            this.choosePHPFolderButton.Size = new System.Drawing.Size(27, 23);
            this.choosePHPFolderButton.TabIndex = 6;
            this.choosePHPFolderButton.Text = "...";
            this.choosePHPFolderButton.UseVisualStyleBackColor = true;
            this.choosePHPFolderButton.Click += new System.EventHandler(this.choosePHPFolderButton_Click);
            // 
            // pathToPHPLabel
            // 
            this.pathToPHPLabel.AutoSize = true;
            this.pathToPHPLabel.Location = new System.Drawing.Point(2, 107);
            this.pathToPHPLabel.Name = "pathToPHPLabel";
            this.pathToPHPLabel.Size = new System.Drawing.Size(99, 13);
            this.pathToPHPLabel.TabIndex = 7;
            this.pathToPHPLabel.Text = "Path to php-cgi.exe";
            // 
            // configurationLabel
            // 
            this.configurationLabel.AutoSize = true;
            this.configurationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.configurationLabel.Location = new System.Drawing.Point(89, 4);
            this.configurationLabel.Name = "configurationLabel";
            this.configurationLabel.Size = new System.Drawing.Size(104, 20);
            this.configurationLabel.TabIndex = 9;
            this.configurationLabel.Text = "Configuration";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.portInput);
            this.panel1.Controls.Add(this.addressInput);
            this.panel1.Controls.Add(this.addressLabel);
            this.panel1.Controls.Add(this.portLabel);
            this.panel1.Controls.Add(this.chooseFolderButton);
            this.panel1.Controls.Add(this.configurationLabel);
            this.panel1.Controls.Add(this.webRootInput);
            this.panel1.Controls.Add(this.pathToPHPLabel);
            this.panel1.Controls.Add(this.rootPathLabel);
            this.panel1.Controls.Add(this.choosePHPFolderButton);
            this.panel1.Controls.Add(this.pathToPHPInput);
            this.panel1.Location = new System.Drawing.Point(12, 73);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(290, 149);
            this.panel1.TabIndex = 10;
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(309, 13);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(477, 209);
            this.logBox.TabIndex = 11;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // startTrayButton
            // 
            this.startTrayButton.Name = "startTrayButton";
            this.startTrayButton.Size = new System.Drawing.Size(103, 22);
            this.startTrayButton.Text = "Start";
            // 
            // stopTrayButton
            // 
            this.stopTrayButton.Enabled = false;
            this.stopTrayButton.Name = "stopTrayButton";
            this.stopTrayButton.Size = new System.Drawing.Size(103, 22);
            this.stopTrayButton.Text = "Stop";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(100, 6);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(100, 6);
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(228, 29);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(26, 13);
            this.portLabel.TabIndex = 10;
            this.portLabel.Text = "Port";
            // 
            // addressLabel
            // 
            this.addressLabel.AutoSize = true;
            this.addressLabel.Location = new System.Drawing.Point(2, 29);
            this.addressLabel.Name = "addressLabel";
            this.addressLabel.Size = new System.Drawing.Size(69, 13);
            this.addressLabel.TabIndex = 11;
            this.addressLabel.Text = "Host address";
            // 
            // addressInput
            // 
            this.addressInput.Location = new System.Drawing.Point(2, 45);
            this.addressInput.Name = "addressInput";
            this.addressInput.Size = new System.Drawing.Size(219, 20);
            this.addressInput.TabIndex = 12;
            this.addressInput.Text = "127.0.0.1";
            // 
            // portInput
            // 
            this.portInput.Location = new System.Drawing.Point(227, 45);
            this.portInput.Name = "portInput";
            this.portInput.Size = new System.Drawing.Size(60, 20);
            this.portInput.TabIndex = 13;
            this.portInput.Text = "8080";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 232);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Simple web server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.trayMenu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ContextMenuStrip trayMenu;
        private System.Windows.Forms.ToolStripMenuItem exitTrayButton;
        private System.Windows.Forms.ToolStripMenuItem openTrayButton;
        private System.Windows.Forms.TextBox webRootInput;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label rootPathLabel;
        private System.Windows.Forms.FolderBrowserDialog webRootBrowserDialog;
        private System.Windows.Forms.Button chooseFolderButton;
        private System.Windows.Forms.TextBox pathToPHPInput;
        private System.Windows.Forms.Button choosePHPFolderButton;
        private System.Windows.Forms.FolderBrowserDialog PHPBrowserDialog;
        private System.Windows.Forms.Label pathToPHPLabel;
        private System.Windows.Forms.Label configurationLabel;
        private System.Windows.Forms.Panel panel1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem startTrayButton;
        private System.Windows.Forms.ToolStripMenuItem stopTrayButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.TextBox portInput;
        private System.Windows.Forms.TextBox addressInput;
        private System.Windows.Forms.Label addressLabel;
        private System.Windows.Forms.Label portLabel;
    }
}

