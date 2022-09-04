using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YandexCellInfoWF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, System.EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://yandex.ru/dev/locator/keys/get/");
        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, System.EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, System.EventArgs e)
        {

        }

        private void toolStripStatusLabel2_Click(object sender, System.EventArgs e)
        {

        }

        private void sectorsTextBox_TextChanged(object sender, System.EventArgs e)
        {

        }

        private async void StartButton_Click(object sender, System.EventArgs e)
        {
            ChangeTextStatus();

            if (allSearchRadioButton.Checked)
            {
                if (StartButton.Text == "Остановить сканирование")
                {
                    Workers.ManyInfoWorker.CancelTask();
                    StartButton.Text = "Начать сканирование";
                }
                else
                {
                    StartButton.Text = "Остановить сканирование";
                    await Workers.ManyInfoWorker.SearchEnbs(ConsoleTextBox, progressBar1, currentEnbLabel, TokenTextBox.Text, MccTextBox.Text, MncTextBox.Text, enbsTextBox.Text, LacTextBox.Text, sectorsTextBox.Text);
                    ResetInterface();
                }
            }
            else if (detaliedSearchRadioButton.Checked)
                if (StartButton.Text == "Остановить сканирование")
                {
                    Workers.DetailedInfoWorker.CancelTask();
                    StartButton.Text = "Начать сканирование";
                }
                else
                {
                    StartButton.Text = "Остановить сканирование";
                    await Workers.DetailedInfoWorker.SearchEnbs(ConsoleTextBox, progressBar1, currentEnbLabel, TokenTextBox.Text, MccTextBox.Text, MncTextBox.Text, enbsTextBox.Text, LacTextBox.Text, detectLacCheckBox.Checked);
                    ResetInterface();
                }

        }

        private void allSearchRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            detectLacCheckBox.Enabled = !detectLacCheckBox.Enabled;
            detailedSearchDescrLabel.Enabled = !detailedSearchDescrLabel.Enabled;
            StartButton.Enabled = true;
        }

        private void detaliedSearchRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            sectorsTextBox.Enabled = !sectorsTextBox.Enabled;
            sectorsLabel.Enabled = !sectorsLabel.Enabled;
            allSearrchDescrLabel.Enabled = !allSearrchDescrLabel.Enabled;
            StartButton.Enabled = true;
        }

        private void progressBar1_Click(object sender, System.EventArgs e)
        {

        }

        private void progressLabel_Click(object sender, System.EventArgs e)
        {

        }

        private void ConsoleTextBox_TextChanged(object sender, System.EventArgs e)
        {

        }

        public void ChangeTextStatus()
        {
            progressBar1.Value = 0;
            TokenTextBox.Enabled = !TokenTextBox.Enabled;
            MccTextBox.Enabled = !MccTextBox.Enabled;
            MncTextBox.Enabled = !MncTextBox.Enabled;
            LacTextBox.Enabled = !LacTextBox.Enabled;
            enbsTextBox.Enabled = !enbsTextBox.Enabled;
            allSearchRadioButton.Enabled = !allSearchRadioButton.Enabled;
            detaliedSearchRadioButton.Enabled = !detaliedSearchRadioButton.Enabled;
            sendDataCheckBox.Enabled = !sendDataCheckBox.Enabled;

            if (detaliedSearchRadioButton.Checked)
                detectLacCheckBox.Enabled = !detectLacCheckBox.Enabled;
            else if (allSearchRadioButton.Checked)
                sectorsTextBox.Enabled = !sectorsTextBox.Enabled;
        }

        public void ResetInterface()
        {
            MccTextBox.Enabled = true;
            MncTextBox.Enabled = true;
            LacTextBox.Enabled = true;
            enbsTextBox.Enabled = true;
            allSearchRadioButton.Enabled = true;
            detaliedSearchRadioButton.Enabled = true;
            sendDataCheckBox.Enabled = true;
            TokenTextBox.Enabled = true;
            StartButton.Text = "Начать сканирование";
            if (detaliedSearchRadioButton.Checked)
                detectLacCheckBox.Enabled = detectLacCheckBox.Enabled;
            else if (allSearchRadioButton.Checked)
                sectorsTextBox.Enabled = sectorsTextBox.Enabled;
        }

        private void saveManyFilesCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {

        }
    }
}
