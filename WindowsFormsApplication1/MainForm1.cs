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
using System.Resources;

namespace WindowsFormsApplication1
{
    public struct teststruct
    {
        public string login;
        public string password;
        public string host;
        public teststruct(string l, string p, string h)
        {
            login = l;
            password = p;
            host = h;
        }
    }
    public partial class MainForm1 : Form
    {
        Client client;
        ResourceManager LocRM;
        public MainForm1()
        {
            InitializeComponent();
        }
        public MainForm1(string login, string password, string host)
        {
            InitializeComponent();
            RLogin.RunWorkerAsync(new teststruct(login,password,host));
            LocRM = new ResourceManager("RedmineUpdateMain.RedmineClientMFStrings", GetType().Assembly);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            RUpdateIssue.RunWorkerAsync();
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

        private void RUpdateIssue_OnWork(object sender, DoWorkEventArgs e)
        {
            client.Total = client.GetUpdatedTotal();

        }

        private void RUpdateIssue_OnComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            listBox1.Items.Clear();
            for (int i = 0; i < client.Total.Count; i++)
            {
                listBox1.Items.Add(client.Total[i].Subject.ToString());
            }
            for (int i = 0; i < client.Cache.Count; i++)
            {
                if (client.Total[i].Id != client.Cache[i].Id)
                {
                    notifyIcon1.BalloonTipTitle = String.Format(LocRM.GetString("MFChangesinProj"), client.Total[i].Project.Name);
                    notifyIcon1.BalloonTipText = String.Format(LocRM.GetString("MFAddedIssue"), client.Total[i].Subject);
                    notifyIcon1.ShowBalloonTip(30);
                    continue;
                }
                if (client.Total[i].Description != client.Cache[i].Description)
                {
                    notifyIcon1.BalloonTipTitle = String.Format(LocRM.GetString("MFChangesinIssue"), client.Total[i].Subject);
                    notifyIcon1.BalloonTipText = String.Format(LocRM.GetString("MFCDisc"), client.Total[i].Description);
                    notifyIcon1.ShowBalloonTip(30);
                }
                if (client.Total[i].Subject != client.Cache[i].Subject)
                {
                    notifyIcon1.BalloonTipTitle = String.Format(LocRM.GetString("MFChangesinIssue"), client.Total[i].Subject);
                    notifyIcon1.BalloonTipText = String.Format(LocRM.GetString("MFCSubj"), client.Cache[i].Subject, client.Total[i].Subject);
                    notifyIcon1.ShowBalloonTip(30);
                }
                if (client.Total[i].Status.Name != client.Cache[i].Status.Name)
                {
                    notifyIcon1.BalloonTipTitle = String.Format(LocRM.GetString("MFChangesinIssue"), client.Total[i].Subject);
                    notifyIcon1.BalloonTipText = String.Format(LocRM.GetString("MFCStat"), client.Cache[i].Status.Name, client.Total[i].Status.Name);
                    notifyIcon1.ShowBalloonTip(30);
                }
                if (client.Total[i].Priority.Name != client.Cache[i].Priority.Name)
                {
                    notifyIcon1.BalloonTipTitle = String.Format(LocRM.GetString("MFChangesinIssue"), client.Total[i].Subject);
                    notifyIcon1.BalloonTipText = String.Format(LocRM.GetString("MFCPrior"), client.Cache[i].Priority.Name, client.Total[i].Priority.Name);
                    notifyIcon1.ShowBalloonTip(30);
                }
                if (client.Total[i].Tracker.Name != client.Cache[i].Tracker.Name)
                {
                    notifyIcon1.BalloonTipTitle = String.Format(LocRM.GetString("MFChangesinIssue"), client.Total[i].Subject);
                    notifyIcon1.BalloonTipText = String.Format(LocRM.GetString("MFCTrck"), client.Cache[i].Tracker.Name, client.Total[i].Tracker.Name);
                    notifyIcon1.ShowBalloonTip(30);
                }
                if (client.Total[i].DoneRatio.Value != client.Cache[i].DoneRatio.Value)
                {
                    notifyIcon1.BalloonTipTitle = String.Format(LocRM.GetString("MFChangesinIssue"), client.Total[i].Subject);
                    notifyIcon1.BalloonTipText = String.Format(LocRM.GetString("MFCRatio"), client.Cache[i].DoneRatio.Value, client.Total[i].DoneRatio.Value);
                    notifyIcon1.ShowBalloonTip(30);
                }
            }
            client.Cache = client.Total;
        }

        private void RLogin_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                teststruct t = (teststruct)e.Argument;
                client = new Client(t.login, t.password, t.host);
            }
            catch
            {
                e.Result = "InvalidLogin";
            }
        }

        private void RLogin_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (e.Result == "InvalidLogin")
            {
                MessageBox.Show(LocRM.GetString("MFConnError"));
                Application.Restart();
            }
            else
            {
                toolStripStatusLabel1.Text = String.Format(LocRM.GetString("MFLogInfo"), client.User.FirstName, client.User.LastName);
                RUpdateIssue.RunWorkerAsync();
                timer1.Enabled = true;
            }
        }
    }
}
