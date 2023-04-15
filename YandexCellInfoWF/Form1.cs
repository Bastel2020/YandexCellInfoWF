using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using YandexCellInfoWF.Models;
using YandexCellInfoWF.Service;
using YandexCellInfoWF.Services;

namespace YandexCellInfoWF
{
    public partial class Form1 : Form
    {
        private static DateTime dragndropAntispamTimestamp = DateTime.Now;
        public Form1()
        {
            InitializeComponent();
            var settings = SettingsLoaderService.LoadDefaultFileSettings();
            SettingsLoaderService.LoadSettings(settings, TokenTextBox, MccTextBox, MncTextBox, LacTextBox, enbsTextBox, sectorsTextBox);
            SettingsLoaderService.LoadTodayRequsets(RequsetsTodayCounter);

            FormClosing += Form1_FormClosing;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SettingsLoaderService.SaveTodayRequsets(int.Parse(RequsetsTodayCounter.Text));
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
                    var abortScan = false;
                    if (int.Parse(RequsetsTodayCounter.Text) >= SettingsLoaderService.GetRequsetsLimit())
                    {
                        var result = MessageBox.Show("Превышен лимит запросов! Закругляемся?", "Яндексу ты не нравишься", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                            abortScan = true;
                    }
                    if (!abortScan)
                        await Workers.ManyInfoWorker.SearchEnbs(ConsoleTextBox, progressBar1, currentEnbLabel, TotalFoundCounter, RequsetsTodayCounter, TokenTextBox.Text, MccTextBox.Text, MncTextBox.Text, enbsTextBox.Text, LacTextBox.Text, sectorsTextBox.Text, dontSaveFileCheckBox);
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
                    var abortScan = false;
                    if (int.Parse(RequsetsTodayCounter.Text) >= SettingsLoaderService.GetRequsetsLimit())
                    {
                        var result = MessageBox.Show("Превышен лимит запросов! Закругляемся?", "Яндексу ты не нравишься", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                            abortScan = true;
                    }
                    if (!abortScan)
                        await Workers.DetailedInfoWorker.SearchEnbs(ConsoleTextBox, progressBar1, currentEnbLabel, TotalFoundCounter, RequsetsTodayCounter, TokenTextBox.Text, MccTextBox.Text, MncTextBox.Text, enbsTextBox.Text, LacTextBox.Text, detectLacCheckBox.Checked, dontSaveFileCheckBox);
                    ResetInterface();
                }
            Service.SettingsLoaderService.SaveTodayRequsets(int.Parse(RequsetsTodayCounter.Text));
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
                        if (obj is List<EnbFullInfo> list)
                            return KmlService.GetKML(list);
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

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (DateTime.Now - dragndropAntispamTimestamp > TimeSpan.FromMilliseconds(200))
            {
                DragnDropMsgLabel.Enabled = true;
                DragnDropMsgLabel.Visible = true;
            }
        }

        private void Form1_DragLeave(object sender, EventArgs e)
        {
            DragnDropMsgLabel.Enabled = false;
            DragnDropMsgLabel.Visible = false;
            dragndropAntispamTimestamp = DateTime.Now;
        }

        private void DragnDropMsgLabel_DragLeave(object sender, DragEventArgs e)
        {
            DragnDropMsgLabel.Enabled = false;
            DragnDropMsgLabel.Visible = false;
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                List<string> files = new List<string>();
                try
                {
                    files.AddRange((string[])e.Data.GetData(DataFormats.FileDrop, false));
                    if (files.Count == 1)
                    {
                        var settings = JsonConvert.DeserializeObject<Models.Settings.SettingsBase>(File.ReadAllText(files[0]));
                        SettingsLoaderService.LoadSettings(settings, TokenTextBox, MccTextBox, MncTextBox, LacTextBox, enbsTextBox, sectorsTextBox);

                        if (!string.IsNullOrEmpty(ConsoleTextBox.Text))
                            ConsoleTextBox.AppendText($"\r\n");
                        ConsoleTextBox.AppendText($"[{DateTime.Now:T}] Загружен файл конфигурации ({files[0]})");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(ConsoleTextBox.Text))
                            ConsoleTextBox.AppendText($"\r\n");
                        ConsoleTextBox.AppendText($"[{DateTime.Now:T}] Не удалось загрузить файл конфигурации: передано несколько файлов");
                    }
                }
                catch(Exception ex)
                {
                    if (!string.IsNullOrEmpty(ConsoleTextBox.Text))
                        ConsoleTextBox.AppendText($"\r\n");
                    var filename = files.Count > 0 ? files[0] : "файл не загружен";
                    ConsoleTextBox.AppendText($"[{DateTime.Now:T}] Не удалось загрузить файл конфигурации ({filename})");
                }
            }
        }

        private void DragnDropMsgLabel_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            DragnDropMsgLabel.Enabled = false;
            DragnDropMsgLabel.Visible = false;
        }

        private void DragnDropMsgLabel_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            DragnDropMsgLabel.Enabled = false;
            DragnDropMsgLabel.Visible = false;
        }

        private void DragnDropMsgLabel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
                e.Effect = DragDropEffects.All;
        }
    }
}
