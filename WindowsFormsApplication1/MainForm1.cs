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
        int countofissues;
        int selectedItem;
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
            countofissues = 0;
            selectedItem = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
            IssueListBox.Items.Clear();
            ProjectListBox.Items.Clear();
            for (int i = client.TotalProject.Count-1; i >= 0; i--)
            {
                ProjectListBox.Items.Add(client.TotalProject[i].Name);
            }
            for (int i = 0; i < client.Project[0].Count; i++)
            {
                IssueListBox.Items.Add(client.Project[0][i].Subject.ToString());
            }
            if (countofissues == 0)
            {
                IssueListBox.SetSelected(selectedItem, true);
                countofissues = client.TotalIssue.Count;
            }
            else if (countofissues < client.TotalIssue.Count){
                int dif = client.TotalIssue.Count - countofissues;
                IssueListBox.SetSelected(selectedItem + dif, true);
                countofissues = client.TotalIssue.Count;
            }
            else if (countofissues == client.TotalIssue.Count)
            {
                IssueListBox.SetSelected(selectedItem, true);
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
            selectedItem = IssueListBox.SelectedIndex;
        }

        private void ProjectListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            IssueListBox.Items.Clear();         
            for (int i = 0; i < client.Project[ProjectListBox.SelectedIndex].Count; i++)
            {
                IssueListBox.Items.Add(client.Project[ProjectListBox.SelectedIndex][i].Subject);
            }
        }
    }
}
