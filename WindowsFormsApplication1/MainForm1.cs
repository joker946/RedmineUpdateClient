using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using System.Xml;
using System.Threading;
using System.Diagnostics;


namespace WindowsFormsApplication1
{
    public partial class MainForm1 : Form
    {
        Client client;
        public MainForm1()
        {
            InitializeComponent();
        }
        public MainForm1(string _login, string _password, string _host)
        {
            InitializeComponent();
            client = new Client(_login, _password, _host);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            try
            {
                for (int i = 0; i < client.Total.Count; i++)
                {
                        listBox1.Items.Add(client.Total[i].Subject.ToString());
                        
                }
                toolStripStatusLabel1.Text = "You are logged in as " + client.User.FirstName + ' ' + client.User.LastName;
            }
            catch
            {
                MessageBox.Show("Не удалось подключиться");
                Application.Exit();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            Thread t = new Thread(RedmineUpdate);
            t.Start();
        }
        void RedmineUpdate()
        {
            for (int i = 0; i < client.Cache.Count; i++)
            {
                if (client.Total[i].Description != client.Cache[i].Description)
                {
                    notifyIcon1.BalloonTipTitle = "New changes in " + client.Total[i].Project.Name;
                    notifyIcon1.BalloonTipText = "New changes in Description: " + client.Total[i].Description;
                    notifyIcon1.ShowBalloonTip(30);
                    client.Cache[i].Description = client.Total[i].Description;
                }
                if (client.Total[i].Subject != client.Cache[i].Subject)
                {
                    notifyIcon1.BalloonTipTitle = "New changes in " + client.Total[i].Project.Name;
                    notifyIcon1.BalloonTipText = "New changes in Subject: " + client.Total[i].Subject;
                    notifyIcon1.ShowBalloonTip(30);
                    client.Cache[i].Subject = client.Total[i].Subject;
                }
                if (client.Total[i].Status.Name != client.Cache[i].Status.Name)
                {
                    notifyIcon1.BalloonTipTitle = "New changes in Issue " + client.Total[i].Subject;
                    notifyIcon1.BalloonTipText = "Status has been changed from " + client.Cache[i].Status.Name + " to " + client.Total[i].Status.Name;
                    notifyIcon1.ShowBalloonTip(30);
                    client.Cache[i].Status.Name = client.Total[i].Status.Name;
                }
                if (client.Total[i].Priority.Name != client.Cache[i].Priority.Name)
                {
                    notifyIcon1.BalloonTipTitle = "New changes in Issue " + client.Total[i].Subject;
                    notifyIcon1.BalloonTipText = "Priority has been changed from " + client.Cache[i].Priority.Name + " to " + client.Total[i].Priority.Name;
                    notifyIcon1.ShowBalloonTip(30);
                    client.Cache[i].Priority.Name = client.Total[i].Priority.Name;
                }
            }
        }
        private void MainForm1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }


        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm s = new SettingsForm();
            s.Show();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }

        private void OnWork(object sender, DoWorkEventArgs e)
        {

        }

        private void OnComplete(object sender, RunWorkerCompletedEventArgs e)
        {

        }
    }
}
