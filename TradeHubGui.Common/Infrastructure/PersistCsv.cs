using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeHubGui.Common.Infrastructure
{
    /// <summary>
    /// Saves data in a CSV file
    /// </summary>
    public static class PersistCsv
    {
        /// <summary>
        /// Saves incoming data in csv file
        /// </summary>
        /// <param name="folderPath">The folder in which to save the file</param>
        /// <param name="dataList">Data to be saved</param>
        /// <param name="strategyDescription">Brief Strategy Description</param>
        public static void SaveData(string folderPath, IReadOnlyList<string> dataList, string strategyDescription)
        {
            // Create file path
            string path = folderPath + "\\" + strategyDescription + "-" + DateTime.Now.ToString("yyMMddHmsfff") + ".csv";

            // Write data
            File.WriteAllLines(path, dataList);
        }
    }
}
