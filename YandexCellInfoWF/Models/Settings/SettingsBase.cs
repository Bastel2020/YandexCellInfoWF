﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexCellInfoWF.Models.Settings
{
    public class SettingsBase
    {
        public DefaultFields DefaultFields;
        public PreloadSectors[] PreloadSectors;
        public int? RequsetsLimit;
    }
}
