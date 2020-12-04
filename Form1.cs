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

                logBox.AppendText(data + "\r\n\r\n");

            }));            
        }

        private void startServer()
        {
            try
            {
                Server server = new Server(System.Net.IPAddress.Parse(addressInput.Text), Int32.Parse(portInput.Text), webRootInput.Text);
                server.PHPFile = pathToPHPInput.Text;
                server.Log = this.Log;
                server.OnStop += stopServer;

                ThreadStart threadStart = new ThreadStart(server.Start);
                serverThread = new Thread(threadStart);
                serverThread.Start();

                stopButton.Enabled = true;
                stopTrayButton.Enabled = true;
                startButton.Enabled = false;
                startTrayButton.Enabled = false;
                this.Text = $"Simple web server (Running on {addressInput.Text}:{portInput.Text})";
                trayIcon.Text= $"Simple web server (Running on {addressInput.Text}:{portInput.Text})";
            }
            catch (Exception)
            {
                Log("Cannot start server, maybe settings are incorrect");
            }           
        }



        private void stopServer()
        {
            this.Invoke(new MethodInvoker(delegate ()
            {
                startButton.Enabled = true;
                startTrayButton.Enabled = true;
                stopButton.Enabled = false;
                stopTrayButton.Enabled = false;
                this.Text = $"Simple web server (Stopped)";
                trayIcon.Text = $"Simple web server (Stopped)";
                if (serverThread.ThreadState==ThreadState.Running)
                {
                    serverThread.Abort();
                }                
            }));
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            startServer();
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
                case "Start":
                    startServer();
                    break;
                case "Stop":
                    stopServer();
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
            stopServer();
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
            trayIcon.Visible = false;
        }

        private void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }
    }
}
