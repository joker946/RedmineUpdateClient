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
            for (int i = 0; i < Client.Instance.TotalProject.Count; i++)
            {
                ProjectListBox.Items.Add(Client.Instance.TotalProject[i].Name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
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
        }
    }
}
