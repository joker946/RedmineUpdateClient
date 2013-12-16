using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (LanguageBox.SelectedIndex)
            {
                case 0:
                    RedmineUpdateMain.Properties.Settings.Default.Language = "en";
                    break;
                case 1:
                    RedmineUpdateMain.Properties.Settings.Default.Language = "ru";
                    break;
            }
            RedmineUpdateMain.Properties.Settings.Default.Save();
            MessageBox.Show("You have to restart application to accept new changes");
        }
    }
}
