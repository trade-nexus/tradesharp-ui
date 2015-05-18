using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Common.Constants
{
    public static class DirectoryPath
    {
        // ReSharper disable InconsistentNaming
        public static string ROOT_DIRECTORY_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\TradeHub\\";
        //public static string ROOT_DIRECTORY_PATH = Path.GetFullPath(@"~\..\..\");
        // ReSharper enable InconsistentNaming
    }
}
