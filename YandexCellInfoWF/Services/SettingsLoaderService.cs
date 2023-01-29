using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YandexCellInfoWF.Models.Settings;

namespace YandexCellInfoWF.Service
{
    public static class SettingsLoaderService
    {
        private static SettingsBase Settings { get; set; }
        private static RequsetsCountInfo ToadayRequsetsInfo { get; set; }

        public static SettingsBase LoadSettings()
        {
            try
            {
                var settingsFile = System.IO.File.ReadAllText(Environment.CurrentDirectory + "\\settings.config");
                Settings = JObject.Parse(settingsFile).ToObject<SettingsBase>();

                var todayRequsetsFile = System.IO.File.ReadAllText(Environment.CurrentDirectory + "\\todayRequsets.config");
                ToadayRequsetsInfo = JObject.Parse(todayRequsetsFile).ToObject<RequsetsCountInfo>();
                return Settings;
            }
            catch
            {
                ToadayRequsetsInfo = new RequsetsCountInfo();

                if (Settings != null)
                    return Settings;

                Settings = new SettingsBase();
                return null;
            }
        }

        public static int GetRequsetsLimit()
        {
            if (Settings == null || Settings.RequsetsLimit == null || Settings.RequsetsLimit < 1)
                return 1000;
            else
                return Settings.RequsetsLimit.Value;
        }

        public static int GetTodayRequsets()
        {
            if (ToadayRequsetsInfo == null || ToadayRequsetsInfo.RequsetsCount == null || ToadayRequsetsInfo.MoscowTimeZoneDate == null)
                return 0;

            if (ToadayRequsetsInfo.MoscowTimeZoneDate.Value.Date.Equals(DateTime.UtcNow.AddHours(3).Date))
                return ToadayRequsetsInfo.RequsetsCount.Value;
            
            ToadayRequsetsInfo.RequsetsCount = 0;
            ToadayRequsetsInfo.MoscowTimeZoneDate = DateTime.UtcNow.AddHours(3).Date;
            return 0;
        }

        public static bool SaveTodayRequsets(int todayRequsets)
        {
            try
            {
                if (ToadayRequsetsInfo == null)
                    ToadayRequsetsInfo = new RequsetsCountInfo();
                if (ToadayRequsetsInfo.RequsetsCount == null || ToadayRequsetsInfo.MoscowTimeZoneDate == null)
                {
                    ToadayRequsetsInfo.RequsetsCount = todayRequsets;
                    ToadayRequsetsInfo.MoscowTimeZoneDate = DateTime.UtcNow.AddHours(3).Date;
                }
                else
                    ToadayRequsetsInfo.RequsetsCount = todayRequsets;
                var toSave = JsonConvert.SerializeObject(ToadayRequsetsInfo, Formatting.Indented);
                System.IO.File.WriteAllText(Environment.CurrentDirectory + "\\todayRequsets.config", toSave);

                return true;
            }
            catch { return false; }
        }

        public static bool LoadSettings(TextBox tokenBox, TextBox mccBox, TextBox mncBox, TextBox lacsBox, TextBox enbsBox, TextBox sectorsBox)
        {
            var settings = LoadSettings();

            if (settings == null)
                return false;

            var editor = new Action<TextBox, string>((textBox, newText) =>
            {
                if (newText != null && newText.Length > 0)
                    textBox.Text = newText;
            });

            editor(tokenBox, settings.DefaultFields.Token);
            editor(mccBox, settings.DefaultFields.MCC.ToString());
            editor(mncBox, settings.DefaultFields.MNC.ToString());
            editor(lacsBox, settings.DefaultFields.LACs);
            editor(enbsBox, settings.DefaultFields.ENBs);
            editor(sectorsBox, settings.DefaultFields.Sectors);

            return true;
        }

        public static void LoadTodayRequsets(Label counter)
        {
            counter.Text = GetTodayRequsets().ToString();
        }
    }
}
