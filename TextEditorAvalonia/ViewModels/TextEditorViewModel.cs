﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditorAvalonia.ViewModels
{
    public class TextEditorViewModel : ViewModelBase
    {
        public string CurrentOpenedFilePath { set; get; } = "";
    }
}
