using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YandexCellInfoWF.Models.Settings;

namespace YandexCellInfoWF
{
    public static class FormChanger
    {
        private static Form1 form;
        public static void LoadForm(Form1 formToLoad)
        {
            form = formToLoad;
        }

        public static bool IsFormLoaded()
        {
            return form != null;
        }

        public static void ChangeInputElementsState()
        {
            if (IsFormLoaded())
                form.ChangeTextStatus();
        }
        public static void ResetInterface()
        {
            if (IsFormLoaded())
                form.ResetInterface();
        }
    }
}
