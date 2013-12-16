using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Resources;
using System.Reflection;

namespace WindowsFormsApplication1
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            switch (RedmineUpdateMain.Properties.Settings.Default.Language){
                case "en":
                    System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en");
                    break;
                case "ru":
                    System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");
                    break;
            }
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm1 form = new MainForm1(textBox1.Text, textBox2.Text, textBox3.Text);
            form.Show();
            this.Hide();
        }
        private void LoginForm_Load(object sender, EventArgs e)
        {
            ResourceManager LocRM = new ResourceManager("RedmineUpdateMain.RedmineClientStrings", GetType().Assembly);
            label1.Text = LocRM.GetString("LoginLabel");
            label2.Text = LocRM.GetString("PasswordLabel");
            label3.Text = LocRM.GetString("HostLabel");
            checkBox1.Text = LocRM.GetString("EditHostBox");
            TitleLabel.Text = LocRM.GetString("TitleLabel");
            button1.Text = LocRM.GetString("LoginButton");
            /*Assembly ass = Assembly.GetExecutingAssembly();
            String[] a = ass.GetManifestResourceNames();
            for (int i = 0; i < a.Count<String>(); i++)
            {
                listBox1.Items.Add(a[i].ToString());
            }*/ //Важный код на проверку пространства имен для resources
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox3.Enabled = true;
            }
            else
            {
                textBox3.Enabled = false;
            }
        }
    }
}
