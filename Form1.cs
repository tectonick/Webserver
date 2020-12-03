using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Webserver
{
    public partial class Form1 : Form
    {
        Thread serverThread;
        public Form1()
        {
            InitializeComponent();
        }

        void Log(string data)
        {
            this.Invoke(new MethodInvoker(delegate ()
            {
                logBox.Text += data+"\r\n\r\n";

            }));            
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            Server server = new Server(System.Net.IPAddress.Parse("127.0.0.1"),8080,webRootInput.Text);
            server.PHPFile = pathToPHPInput.Text;
            server.Log = this.Log;
            ThreadStart threadStart = new ThreadStart(server.Start);
            serverThread = new Thread(threadStart);
            serverThread.Start();

            stopButton.Enabled = true;
            startButton.Enabled = false;
        }

        private void trayMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var item = e.ClickedItem;


            switch (item.Text)
            {
                case "Open":
                    Show();
                    WindowState = FormWindowState.Normal;
                    break;
                case "Exit":
                    Application.Exit();
                    break;
                default:
                    break;
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = true;
            stopButton.Enabled = false;
            serverThread.Abort();
        }

        private void chooseFolderButton_Click(object sender, EventArgs e)
        {
            webRootBrowserDialog.ShowDialog();
            webRootInput.Text = webRootBrowserDialog.SelectedPath;
        }

        private void choosePHPFolderButton_Click(object sender, EventArgs e)
        {
            PHPBrowserDialog.ShowDialog();
            pathToPHPInput.Text = PHPBrowserDialog.SelectedPath;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIcon1.Visible = false;
        }
    }
}
