
namespace YandexCellInfoWF
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.TokenLabel = new System.Windows.Forms.Label();
            this.TokenTextBox = new System.Windows.Forms.TextBox();
            this.GetTokenLinkLabel = new System.Windows.Forms.LinkLabel();
            this.allSearchRadioButton = new System.Windows.Forms.RadioButton();
            this.detaliedSearchRadioButton = new System.Windows.Forms.RadioButton();
            this.searchModeGroupBox = new System.Windows.Forms.GroupBox();
            this.dontSaveFileCheckBox = new System.Windows.Forms.CheckBox();
            this.detailedSearchDescrLabel = new System.Windows.Forms.Label();
            this.allSearrchDescrLabel = new System.Windows.Forms.Label();
            this.detectLacCheckBox = new System.Windows.Forms.CheckBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.sectorsTextBox = new System.Windows.Forms.TextBox();
            this.sectorsLabel = new System.Windows.Forms.Label();
            this.sendDataCheckBox = new System.Windows.Forms.CheckBox();
            this.ConsoleGroupBox = new System.Windows.Forms.GroupBox();
            this.ConsoleTextBox = new System.Windows.Forms.TextBox();
            this.generalSettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.EnbDescrLinkLabel = new System.Windows.Forms.LinkLabel();
            this.enbsTextBox = new System.Windows.Forms.TextBox();
            this.LacTextBox = new System.Windows.Forms.TextBox();
            this.enbsLabel = new System.Windows.Forms.Label();
            this.MncTextBox = new System.Windows.Forms.TextBox();
            this.MccTextBox = new System.Windows.Forms.TextBox();
            this.MncLabel = new System.Windows.Forms.Label();
            this.LacLabel = new System.Windows.Forms.Label();
            this.MccLabel = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressLabel = new System.Windows.Forms.Label();
            this.currentEnbTextLabel = new System.Windows.Forms.Label();
            this.currentEnbLabel = new System.Windows.Forms.Label();
            this.fileToKmlButton = new System.Windows.Forms.Button();
            this.searchModeGroupBox.SuspendLayout();
            this.ConsoleGroupBox.SuspendLayout();
            this.generalSettingsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // TokenLabel
            // 
            this.TokenLabel.AutoSize = true;
            this.TokenLabel.Location = new System.Drawing.Point(11, 12);
            this.TokenLabel.Name = "TokenLabel";
            this.TokenLabel.Size = new System.Drawing.Size(53, 13);
            this.TokenLabel.TabIndex = 0;
            this.TokenLabel.Text = "1. Токен:\r\n";
            this.TokenLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // TokenTextBox
            // 
            this.TokenTextBox.AllowDrop = true;
            this.TokenTextBox.Location = new System.Drawing.Point(63, 10);
            this.TokenTextBox.Name = "TokenTextBox";
            this.TokenTextBox.Size = new System.Drawing.Size(395, 20);
            this.TokenTextBox.TabIndex = 1;
            this.TokenTextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // GetTokenLinkLabel
            // 
            this.GetTokenLinkLabel.AutoSize = true;
            this.GetTokenLinkLabel.Location = new System.Drawing.Point(459, 13);
            this.GetTokenLinkLabel.Name = "GetTokenLinkLabel";
            this.GetTokenLinkLabel.Size = new System.Drawing.Size(54, 13);
            this.GetTokenLinkLabel.TabIndex = 22;
            this.GetTokenLinkLabel.TabStop = true;
            this.GetTokenLinkLabel.Text = "Получить";
            this.GetTokenLinkLabel.UseWaitCursor = true;
            this.GetTokenLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // allSearchRadioButton
            // 
            this.allSearchRadioButton.AutoSize = true;
            this.allSearchRadioButton.Location = new System.Drawing.Point(10, 17);
            this.allSearchRadioButton.Name = "allSearchRadioButton";
            this.allSearchRadioButton.Size = new System.Drawing.Size(236, 17);
            this.allSearchRadioButton.TabIndex = 6;
            this.allSearchRadioButton.TabStop = true;
            this.allSearchRadioButton.Text = "Поиск всех БС по заданным параметрам\r\n";
            this.allSearchRadioButton.UseVisualStyleBackColor = true;
            this.allSearchRadioButton.CheckedChanged += new System.EventHandler(this.allSearchRadioButton_CheckedChanged);
            // 
            // detaliedSearchRadioButton
            // 
            this.detaliedSearchRadioButton.AutoSize = true;
            this.detaliedSearchRadioButton.Location = new System.Drawing.Point(262, 17);
            this.detaliedSearchRadioButton.Name = "detaliedSearchRadioButton";
            this.detaliedSearchRadioButton.Size = new System.Drawing.Size(174, 17);
            this.detaliedSearchRadioButton.TabIndex = 7;
            this.detaliedSearchRadioButton.TabStop = true;
            this.detaliedSearchRadioButton.Text = "Подробная информация о БС\r\n";
            this.detaliedSearchRadioButton.UseVisualStyleBackColor = true;
            this.detaliedSearchRadioButton.CheckedChanged += new System.EventHandler(this.detaliedSearchRadioButton_CheckedChanged);
            // 
            // searchModeGroupBox
            // 
            this.searchModeGroupBox.Controls.Add(this.fileToKmlButton);
            this.searchModeGroupBox.Controls.Add(this.dontSaveFileCheckBox);
            this.searchModeGroupBox.Controls.Add(this.detailedSearchDescrLabel);
            this.searchModeGroupBox.Controls.Add(this.allSearrchDescrLabel);
            this.searchModeGroupBox.Controls.Add(this.detectLacCheckBox);
            this.searchModeGroupBox.Controls.Add(this.StartButton);
            this.searchModeGroupBox.Controls.Add(this.sectorsTextBox);
            this.searchModeGroupBox.Controls.Add(this.detaliedSearchRadioButton);
            this.searchModeGroupBox.Controls.Add(this.sectorsLabel);
            this.searchModeGroupBox.Controls.Add(this.allSearchRadioButton);
            this.searchModeGroupBox.Location = new System.Drawing.Point(5, 119);
            this.searchModeGroupBox.Name = "searchModeGroupBox";
            this.searchModeGroupBox.Size = new System.Drawing.Size(508, 252);
            this.searchModeGroupBox.TabIndex = 15;
            this.searchModeGroupBox.TabStop = false;
            this.searchModeGroupBox.Text = "3. Режим поиска и подробные настройки";
            this.searchModeGroupBox.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // dontSaveFileCheckBox
            // 
            this.dontSaveFileCheckBox.AutoSize = true;
            this.dontSaveFileCheckBox.Location = new System.Drawing.Point(247, 220);
            this.dontSaveFileCheckBox.Name = "dontSaveFileCheckBox";
            this.dontSaveFileCheckBox.Size = new System.Drawing.Size(132, 17);
            this.dontSaveFileCheckBox.TabIndex = 27;
            this.dontSaveFileCheckBox.Text = "Не сохранять файлы";
            this.dontSaveFileCheckBox.UseVisualStyleBackColor = true;
            // 
            // detailedSearchDescrLabel
            // 
            this.detailedSearchDescrLabel.AutoSize = true;
            this.detailedSearchDescrLabel.Location = new System.Drawing.Point(259, 50);
            this.detailedSearchDescrLabel.Name = "detailedSearchDescrLabel";
            this.detailedSearchDescrLabel.Size = new System.Drawing.Size(239, 65);
            this.detailedSearchDescrLabel.TabIndex = 26;
            this.detailedSearchDescrLabel.Text = "Подробная информация о БС: существующие\r\nна ней сектора и информация о них: номер" +
    ",\r\nLAC, местоположение сектора.\r\n\r\nРезультат: список БС c секторами и LAC.";
            // 
            // allSearrchDescrLabel
            // 
            this.allSearrchDescrLabel.AutoSize = true;
            this.allSearrchDescrLabel.Location = new System.Drawing.Point(7, 50);
            this.allSearrchDescrLabel.Name = "allSearrchDescrLabel";
            this.allSearrchDescrLabel.Size = new System.Drawing.Size(198, 78);
            this.allSearrchDescrLabel.TabIndex = 25;
            this.allSearrchDescrLabel.Text = "Поиск всех БС среди указанных Enb,\r\nкоторые соответствуют требованиям\r\nпо LAC и н" +
    "омерам секторов.\r\n\r\nРезультат: номера БС, отвечающие\r\nтребованиям поиска.";
            // 
            // detectLacCheckBox
            // 
            this.detectLacCheckBox.AutoSize = true;
            this.detectLacCheckBox.Location = new System.Drawing.Point(262, 131);
            this.detectLacCheckBox.Name = "detectLacCheckBox";
            this.detectLacCheckBox.Size = new System.Drawing.Size(239, 17);
            this.detectLacCheckBox.TabIndex = 9;
            this.detectLacCheckBox.Text = "Определять LAC сектора (ниже скорость)";
            this.detectLacCheckBox.UseVisualStyleBackColor = true;
            this.detectLacCheckBox.CheckedChanged += new System.EventHandler(this.saveManyFilesCheckBox_CheckedChanged);
            // 
            // StartButton
            // 
            this.StartButton.Enabled = false;
            this.StartButton.Location = new System.Drawing.Point(385, 189);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(116, 53);
            this.StartButton.TabIndex = 10;
            this.StartButton.Text = "Начать сканирование";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // sectorsTextBox
            // 
            this.sectorsTextBox.AllowDrop = true;
            this.sectorsTextBox.Location = new System.Drawing.Point(69, 150);
            this.sectorsTextBox.Multiline = true;
            this.sectorsTextBox.Name = "sectorsTextBox";
            this.sectorsTextBox.Size = new System.Drawing.Size(146, 92);
            this.sectorsTextBox.TabIndex = 8;
            this.sectorsTextBox.TextChanged += new System.EventHandler(this.sectorsTextBox_TextChanged);
            // 
            // sectorsLabel
            // 
            this.sectorsLabel.AutoSize = true;
            this.sectorsLabel.Location = new System.Drawing.Point(9, 150);
            this.sectorsLabel.Name = "sectorsLabel";
            this.sectorsLabel.Size = new System.Drawing.Size(52, 13);
            this.sectorsLabel.TabIndex = 0;
            this.sectorsLabel.Text = "Сектора:";
            // 
            // sendDataCheckBox
            // 
            this.sendDataCheckBox.AutoSize = true;
            this.sendDataCheckBox.Checked = true;
            this.sendDataCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sendDataCheckBox.Location = new System.Drawing.Point(125, 362);
            this.sendDataCheckBox.Name = "sendDataCheckBox";
            this.sendDataCheckBox.Size = new System.Drawing.Size(266, 30);
            this.sendDataCheckBox.TabIndex = 24;
            this.sendDataCheckBox.Text = "Отправить результаты поиска разработчику\r\n(Помощь в составлении собственной базы " +
    "БС)\r\n";
            this.sendDataCheckBox.UseVisualStyleBackColor = true;
            this.sendDataCheckBox.Visible = false;
            // 
            // ConsoleGroupBox
            // 
            this.ConsoleGroupBox.Controls.Add(this.ConsoleTextBox);
            this.ConsoleGroupBox.Location = new System.Drawing.Point(516, 2);
            this.ConsoleGroupBox.Name = "ConsoleGroupBox";
            this.ConsoleGroupBox.Size = new System.Drawing.Size(203, 375);
            this.ConsoleGroupBox.TabIndex = 20;
            this.ConsoleGroupBox.TabStop = false;
            this.ConsoleGroupBox.Text = "Вывод";
            // 
            // ConsoleTextBox
            // 
            this.ConsoleTextBox.Location = new System.Drawing.Point(4, 14);
            this.ConsoleTextBox.Multiline = true;
            this.ConsoleTextBox.Name = "ConsoleTextBox";
            this.ConsoleTextBox.ReadOnly = true;
            this.ConsoleTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ConsoleTextBox.Size = new System.Drawing.Size(197, 355);
            this.ConsoleTextBox.TabIndex = 21;
            this.ConsoleTextBox.TextChanged += new System.EventHandler(this.ConsoleTextBox_TextChanged);
            // 
            // generalSettingsGroupBox
            // 
            this.generalSettingsGroupBox.Controls.Add(this.EnbDescrLinkLabel);
            this.generalSettingsGroupBox.Controls.Add(this.enbsTextBox);
            this.generalSettingsGroupBox.Controls.Add(this.LacTextBox);
            this.generalSettingsGroupBox.Controls.Add(this.enbsLabel);
            this.generalSettingsGroupBox.Controls.Add(this.MncTextBox);
            this.generalSettingsGroupBox.Controls.Add(this.MccTextBox);
            this.generalSettingsGroupBox.Controls.Add(this.MncLabel);
            this.generalSettingsGroupBox.Controls.Add(this.LacLabel);
            this.generalSettingsGroupBox.Controls.Add(this.MccLabel);
            this.generalSettingsGroupBox.Location = new System.Drawing.Point(5, 35);
            this.generalSettingsGroupBox.Name = "generalSettingsGroupBox";
            this.generalSettingsGroupBox.Size = new System.Drawing.Size(508, 78);
            this.generalSettingsGroupBox.TabIndex = 23;
            this.generalSettingsGroupBox.TabStop = false;
            this.generalSettingsGroupBox.Text = "2. Общие настройки";
            this.generalSettingsGroupBox.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // EnbDescrLinkLabel
            // 
            this.EnbDescrLinkLabel.AutoSize = true;
            this.EnbDescrLinkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.EnbDescrLinkLabel.Location = new System.Drawing.Point(4, 62);
            this.EnbDescrLinkLabel.Name = "EnbDescrLinkLabel";
            this.EnbDescrLinkLabel.Size = new System.Drawing.Size(98, 12);
            this.EnbDescrLinkLabel.TabIndex = 13;
            this.EnbDescrLinkLabel.TabStop = true;
            this.EnbDescrLinkLabel.Text = "Диапазон или номера";
            // 
            // enbsTextBox
            // 
            this.enbsTextBox.AccessibleDescription = "";
            this.enbsTextBox.AllowDrop = true;
            this.enbsTextBox.Location = new System.Drawing.Point(102, 46);
            this.enbsTextBox.Name = "enbsTextBox";
            this.enbsTextBox.Size = new System.Drawing.Size(400, 20);
            this.enbsTextBox.TabIndex = 5;
            // 
            // LacTextBox
            // 
            this.LacTextBox.AllowDrop = true;
            this.LacTextBox.Location = new System.Drawing.Point(194, 17);
            this.LacTextBox.Name = "LacTextBox";
            this.LacTextBox.Size = new System.Drawing.Size(308, 20);
            this.LacTextBox.TabIndex = 4;
            // 
            // enbsLabel
            // 
            this.enbsLabel.AutoSize = true;
            this.enbsLabel.Location = new System.Drawing.Point(7, 49);
            this.enbsLabel.Name = "enbsLabel";
            this.enbsLabel.Size = new System.Drawing.Size(89, 13);
            this.enbsLabel.TabIndex = 2;
            this.enbsLabel.Text = "Enb для поиска:";
            // 
            // MncTextBox
            // 
            this.MncTextBox.AllowDrop = true;
            this.MncTextBox.Location = new System.Drawing.Point(120, 17);
            this.MncTextBox.MaxLength = 3;
            this.MncTextBox.Name = "MncTextBox";
            this.MncTextBox.Size = new System.Drawing.Size(35, 20);
            this.MncTextBox.TabIndex = 3;
            // 
            // MccTextBox
            // 
            this.MccTextBox.AllowDrop = true;
            this.MccTextBox.Location = new System.Drawing.Point(42, 17);
            this.MccTextBox.MaxLength = 3;
            this.MccTextBox.Name = "MccTextBox";
            this.MccTextBox.Size = new System.Drawing.Size(35, 20);
            this.MccTextBox.TabIndex = 2;
            this.MccTextBox.Text = "250";
            // 
            // MncLabel
            // 
            this.MncLabel.AutoSize = true;
            this.MncLabel.Location = new System.Drawing.Point(83, 20);
            this.MncLabel.Name = "MncLabel";
            this.MncLabel.Size = new System.Drawing.Size(34, 13);
            this.MncLabel.TabIndex = 0;
            this.MncLabel.Text = "MNC:";
            // 
            // LacLabel
            // 
            this.LacLabel.AutoSize = true;
            this.LacLabel.Location = new System.Drawing.Point(161, 20);
            this.LacLabel.Name = "LacLabel";
            this.LacLabel.Size = new System.Drawing.Size(30, 13);
            this.LacLabel.TabIndex = 0;
            this.LacLabel.Text = "LAC:";
            // 
            // MccLabel
            // 
            this.MccLabel.AutoSize = true;
            this.MccLabel.Location = new System.Drawing.Point(6, 20);
            this.MccLabel.Name = "MccLabel";
            this.MccLabel.Size = new System.Drawing.Size(33, 13);
            this.MccLabel.TabIndex = 0;
            this.MccLabel.Text = "MCC:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 374);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(723, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(541, 378);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(176, 15);
            this.progressBar1.TabIndex = 19;
            this.progressBar1.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.progressLabel.Location = new System.Drawing.Point(487, 380);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(48, 12);
            this.progressLabel.TabIndex = 18;
            this.progressLabel.Text = "Прогресс:";
            this.progressLabel.Click += new System.EventHandler(this.progressLabel_Click);
            // 
            // currentEnbTextLabel
            // 
            this.currentEnbTextLabel.AutoSize = true;
            this.currentEnbTextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.currentEnbTextLabel.Location = new System.Drawing.Point(3, 380);
            this.currentEnbTextLabel.Name = "currentEnbTextLabel";
            this.currentEnbTextLabel.Size = new System.Drawing.Size(63, 12);
            this.currentEnbTextLabel.TabIndex = 16;
            this.currentEnbTextLabel.Text = "Текущий enb:";
            // 
            // currentEnbLabel
            // 
            this.currentEnbLabel.AutoSize = true;
            this.currentEnbLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.currentEnbLabel.Location = new System.Drawing.Point(72, 380);
            this.currentEnbLabel.Name = "currentEnbLabel";
            this.currentEnbLabel.Size = new System.Drawing.Size(10, 12);
            this.currentEnbLabel.TabIndex = 17;
            this.currentEnbLabel.Text = "0";
            // 
            // fileToKmlButton
            // 
            this.fileToKmlButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.fileToKmlButton.Location = new System.Drawing.Point(392, 154);
            this.fileToKmlButton.Name = "fileToKmlButton";
            this.fileToKmlButton.Size = new System.Drawing.Size(100, 20);
            this.fileToKmlButton.TabIndex = 28;
            this.fileToKmlButton.Text = "KML из файла...";
            this.fileToKmlButton.UseVisualStyleBackColor = true;
            this.fileToKmlButton.Click += new System.EventHandler(this.fileToKmlButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 396);
            this.Controls.Add(this.currentEnbLabel);
            this.Controls.Add(this.currentEnbTextLabel);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.sendDataCheckBox);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.generalSettingsGroupBox);
            this.Controls.Add(this.ConsoleGroupBox);
            this.Controls.Add(this.searchModeGroupBox);
            this.Controls.Add(this.GetTokenLinkLabel);
            this.Controls.Add(this.TokenTextBox);
            this.Controls.Add(this.TokenLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "YandexCellInfo by Bastel2020 (v1.1)";
            this.searchModeGroupBox.ResumeLayout(false);
            this.searchModeGroupBox.PerformLayout();
            this.ConsoleGroupBox.ResumeLayout(false);
            this.ConsoleGroupBox.PerformLayout();
            this.generalSettingsGroupBox.ResumeLayout(false);
            this.generalSettingsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TokenLabel;
        private System.Windows.Forms.TextBox TokenTextBox;
        private System.Windows.Forms.LinkLabel GetTokenLinkLabel;
        private System.Windows.Forms.RadioButton allSearchRadioButton;
        private System.Windows.Forms.RadioButton detaliedSearchRadioButton;
        private System.Windows.Forms.GroupBox searchModeGroupBox;
        private System.Windows.Forms.GroupBox ConsoleGroupBox;
        private System.Windows.Forms.TextBox ConsoleTextBox;
        private System.Windows.Forms.GroupBox generalSettingsGroupBox;
        private System.Windows.Forms.TextBox MccTextBox;
        private System.Windows.Forms.Label MccLabel;
        private System.Windows.Forms.TextBox MncTextBox;
        private System.Windows.Forms.Label MncLabel;
        private System.Windows.Forms.Label LacLabel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox LacTextBox;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox sectorsTextBox;
        private System.Windows.Forms.Label sectorsLabel;
        private System.Windows.Forms.TextBox enbsTextBox;
        private System.Windows.Forms.Label enbsLabel;
        private System.Windows.Forms.CheckBox detectLacCheckBox;
        private System.Windows.Forms.LinkLabel EnbDescrLinkLabel;
        private System.Windows.Forms.Label detailedSearchDescrLabel;
        private System.Windows.Forms.Label allSearrchDescrLabel;
        private System.Windows.Forms.CheckBox sendDataCheckBox;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.Label currentEnbTextLabel;
        private System.Windows.Forms.Label currentEnbLabel;
        private System.Windows.Forms.CheckBox dontSaveFileCheckBox;
        private System.Windows.Forms.Button fileToKmlButton;
    }
}

