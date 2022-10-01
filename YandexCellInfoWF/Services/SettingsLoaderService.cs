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
        public static SettingsBase LoadSettings()
        {
            try
            {
                var file = System.IO.File.ReadAllText(Environment.CurrentDirectory + "\\settings.config");
                return JObject.Parse(file).ToObject<SettingsBase>();
            }
            catch
            {
                return null;
            }
        }

        public static bool ChangeInterface(TextBox tokenBox, TextBox mccBox, TextBox mncBox, TextBox lacsBox, TextBox enbsBox, TextBox sectorsBox)
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
    }
}
