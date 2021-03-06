﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections;
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
    enum Redmine
    {
        SelectedIndexofProject,
        SelectedIndexofIssue
    }
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
        int countofissues;
        ResourceManager LocRM;
        Dictionary<Redmine, int> dict;
        bool FirstTimeLaunchProcess;
        int SelProj, SelIssue;
        public MainForm1()
        {
            InitializeComponent();
        }
        public MainForm1(string login, string password, string host)
        {
            InitializeComponent();
            RLogin.RunWorkerAsync(new teststruct(login,password,host));
            LocRM = new ResourceManager("RedmineUpdateMain.RedmineClientMFStrings", GetType().Assembly);
            countofissues = 0;
            FirstTimeLaunchProcess = true;
            SelProj = 0;
            SelIssue = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = LocRM.GetString("MFLoI");
            label2.Text = LocRM.GetString("MFRecents");
            label3.Text = LocRM.GetString("MFDoI");
            label4.Text = LocRM.GetString("MFProjects");
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!RUpdateIssue.IsBusy)
            {
                RUpdateIssue.RunWorkerAsync();
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
        void ShowIcon(String title, String text, String sender)
        {
            notifyIcon1.BalloonTipTitle = title;
            notifyIcon1.BalloonTipText = text;
            notifyIcon1.ShowBalloonTip(30);
            listBox2.Items.Add(String.Format("[{0}] {1}", sender, text));
        }
        private void RUpdateIssue_OnWork(object sender, DoWorkEventArgs e)
        {
            client.TotalIssue = client.GetTotalIssue();
            client.TotalProject = client.GetTotalProject();
            client.Project = client.GetProjects();
        }
        private void RUpdateIssue_OnComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            ProjectListBox.Enabled = false;
            IssueListBox.Enabled = false;
            if (!FirstTimeLaunchProcess)
            {
                SelProj = ProjectListBox.SelectedIndex;
                SelIssue = IssueListBox.SelectedIndex;
            }
            IssueListBox.Items.Clear();
            ProjectListBox.Items.Clear();
            for (int i = client.TotalProject.Count-1; i >= 0; i--)
            {
                ProjectListBox.Items.Add(client.TotalProject[i].Name);
            }
            for (int i = 0; i < client.Project[SelProj].Count; i++)
            {
                IssueListBox.Items.Add(client.Project[SelProj][i].Subject.ToString());
            }
            #region Checkers_for_Notify
            for (int i = 0; i < client.CacheIssue.Count; i++)
            {
                if (client.TotalIssue[i].Id != client.CacheIssue[i].Id)
                {
                    ShowIcon(String.Format(LocRM.GetString("MFChangesinProj"), client.TotalIssue[i].Project.Name),
                        String.Format(LocRM.GetString("MFAddedIssue"), client.TotalIssue[i].Subject), client.TotalIssue[i].Subject);
                    break;
                }
                if (client.TotalIssue[i].Description != client.CacheIssue[i].Description)
                {
                    ShowIcon(String.Format(LocRM.GetString("MFChangesinIssue"), client.TotalIssue[i].Subject),
                        String.Format(LocRM.GetString("MFCDisc"), client.TotalIssue[i].Description), client.TotalIssue[i].Subject);
                }
                if (client.TotalIssue[i].Subject != client.CacheIssue[i].Subject)
                {
                    ShowIcon(String.Format(LocRM.GetString("MFChangesinIssue"), client.TotalIssue[i].Subject),
                        String.Format(LocRM.GetString("MFCSubj"), client.CacheIssue[i].Subject, client.TotalIssue[i].Subject), client.TotalIssue[i].Subject);
                }
                if (client.TotalIssue[i].Status.Name != client.CacheIssue[i].Status.Name)
                {
                    ShowIcon(String.Format(LocRM.GetString("MFChangesinIssue"), client.TotalIssue[i].Subject),
                        String.Format(LocRM.GetString("MFCStat"), client.CacheIssue[i].Status.Name, client.TotalIssue[i].Status.Name), client.TotalIssue[i].Subject);
                }
                if (client.TotalIssue[i].Priority.Name != client.CacheIssue[i].Priority.Name)
                {
                    ShowIcon(String.Format(LocRM.GetString("MFChangesinIssue"), client.TotalIssue[i].Subject),
                        String.Format(LocRM.GetString("MFCPrior"), client.CacheIssue[i].Priority.Name, client.TotalIssue[i].Priority.Name), client.TotalIssue[i].Subject);
                }
                if (client.TotalIssue[i].Tracker.Name != client.CacheIssue[i].Tracker.Name)
                {
                    ShowIcon(String.Format(LocRM.GetString("MFChangesinIssue"), client.TotalIssue[i].Subject),
                        String.Format(LocRM.GetString("MFCTrck"), client.CacheIssue[i].Tracker.Name, client.TotalIssue[i].Tracker.Name), client.TotalIssue[i].Subject);
                }
                if (client.TotalIssue[i].DoneRatio.Value != client.CacheIssue[i].DoneRatio.Value)
                {
                    ShowIcon(String.Format(LocRM.GetString("MFChangesinIssue"), client.TotalIssue[i].Subject),
                        String.Format(LocRM.GetString("MFCRatio"), client.CacheIssue[i].DoneRatio.Value, client.TotalIssue[i].DoneRatio.Value), client.TotalIssue[i].Subject);
                }
            }
            #endregion
            client.CacheIssue = client.TotalIssue;
            ProjectListBox.Enabled = true;
            IssueListBox.Enabled = true;
            FirstTimeLaunchProcess = false;
            ProjectListBox.SelectedIndex = SelProj;
            IssueListBox.SelectedIndex = SelIssue;
        }
        #region Thread_on_start
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
        #endregion
        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            String target = "http://www.dkiredmine.bitnamiapp.com";
            System.Diagnostics.Process.Start(target);
        }

        private void IssueListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = client.TotalIssue[IssueListBox.SelectedIndex].Description.ToString();
        }

        private void ProjectListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            IssueListBox.Items.Clear();         
            for (int i = 0; i < client.Project[ProjectListBox.SelectedIndex].Count; i++)
            {
                IssueListBox.Items.Add(client.Project[ProjectListBox.SelectedIndex][i].Subject);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (object checkedItem in IssueListBox.CheckedItems)
            {
                for (int i = 0; i < client.Project[SelProj].Count; i++)
                {
                    if (checkedItem.ToString() == client.Project[SelProj][i].Subject.ToString())
                    {
                        client.Project[SelProj][i].Status.Id = 5;
                        client.Manager.UpdateObject(client.Project[SelProj][i].Id.ToString(), client.Project[SelProj][i]);
                    }
                }
            }
        }
    }
}
