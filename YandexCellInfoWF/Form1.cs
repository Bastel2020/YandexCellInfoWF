using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using YandexCellInfoWF.Models;
using YandexCellInfoWF.Services;

namespace YandexCellInfoWF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Service.SettingsLoaderService.ChangeInterface(TokenTextBox, MccTextBox, MncTextBox, LacTextBox, enbsTextBox, sectorsTextBox);
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
                    await Workers.ManyInfoWorker.SearchEnbs(ConsoleTextBox, progressBar1, currentEnbLabel, TokenTextBox.Text, MccTextBox.Text, MncTextBox.Text, enbsTextBox.Text, LacTextBox.Text, sectorsTextBox.Text, dontSaveFileCheckBox);
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
                    await Workers.DetailedInfoWorker.SearchEnbs(ConsoleTextBox, progressBar1, currentEnbLabel, TokenTextBox.Text, MccTextBox.Text, MncTextBox.Text, enbsTextBox.Text, LacTextBox.Text, detectLacCheckBox.Checked, dontSaveFileCheckBox);
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
                detectLacCheckBox.Enabled = true;
            else if (allSearchRadioButton.Checked)
                sectorsTextBox.Enabled = true;

            progressBar1.Value = 0;
        }

        private void saveManyFilesCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {

        }

        private void fileToKmlButton_Click(object sender, System.EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            var result = dialog.ShowDialog();
            if (result != DialogResult.OK)           
                return;

            try
            {
                var serializerSettings = new JsonSerializerSettings();
                serializerSettings.MissingMemberHandling = MissingMemberHandling.Error;
                var files = dialog.FileNames
                    .Select(file => System.IO.File.ReadAllText(file))
                    .Select(str =>
                    {
                        try { return (object)JsonConvert.DeserializeObject<List<BaseItemInfo>>(str, serializerSettings); }
                        catch
                        {
                            try
                            {
                                return (object)JsonConvert.DeserializeObject<List<EnbFullInfo>>(str);
                            }
                            catch
                            {
                                var _json = JObject.Parse(str);
                                var sampledataJson = JArray.Parse(_json["Enbs"].ToString());
                                return (object)JsonConvert.DeserializeObject<List<EnbFullInfo>>(sampledataJson.ToString());
                            }
                        }
                    })
                    .Select(obj =>
                    {
                        if (obj is List<EnbFullInfo>)
                            return KmlService.GetKML((List<EnbFullInfo>)obj);
                        else
                            return KmlService.GetKML((List<BaseItemInfo>)obj);
                    });
                var counter = 0;
                foreach(var file in files)
                {
                    System.IO.File.WriteAllText(Environment.CurrentDirectory + "\\" + $"{dialog.SafeFileNames[counter]} map.kml", file);
                    counter++;
                }
                MessageBox.Show($"Успешно обработано {counter} файл(ов).");
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Произошла ошибка! Проверьте, что на вход переданы файлы DeailedInfo или AllInfo. \r\nОшибка: {ex.Message}.", "Ошибка при обработке файлов");
            }
        }
    }
}
