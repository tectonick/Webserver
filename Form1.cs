using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Webserver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            stopButton.Enabled = true;
            startButton.Enabled = false;

            Server server = new Server(System.Net.IPAddress.Parse("127.0.0.1"),8080,webRootInput.Text);
            server.Start();
        }

        private void trayMenu_Click(object sender, EventArgs e)
        {
            
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
        }

        private void chooseFolderButton_Click(object sender, EventArgs e)
        {
            webRootBrowserDialog.ShowDialog();
            webRootInput.Text = webRootBrowserDialog.SelectedPath;
        }
    }
}
