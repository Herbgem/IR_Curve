using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;

namespace Server
{
    internal class ProgramSettings
    {
        #region Methods

        static ProgramSettings()
        {
            _programDir = Application.StartupPath;
            _iniFullName = string.Concat(_programDir, @"\ini.xml");
            _irDataDir = string.Concat(_programDir, @"\IRData");
            _errorLogDir = string.Concat(_programDir, @"\ErrorLogDir");
            _dataErrorFullName = string.Concat(_errorLogDir, @"\DateError.txt");
            _paths.Add("ProgramDir", _programDir);
            _paths.Add("IRDataDir", _irDataDir);
            _paths.Add("ErrorLogDir", _errorLogDir);
        }
        /// <summary>
        /// Initialize this static class and write the settings to log file.
        /// </summary>
        internal static void Initialize()
        {
            Directory.CreateDirectory(_irDataDir);
            Directory.CreateDirectory(_errorLogDir);
            WriteToLog();
        }
        /// <summary>
        /// Write settings to XML log file
        /// </summary>
        private static void WriteToLog()
        {
            XElement ini = new XElement("INI", new XAttribute("Dir", IniFullName),
                             from x in _paths
                             select
                               new XElement(x.Key, new XAttribute("Dir", x.Value)));
            ini.Save(IniFullName);
        }

        #endregion Methods

        #region Fields
        private static string _programDir;
        private static string _irDataDir;
        private static string _iniFullName;
        private static string _errorLogDir;
        private static string _dataErrorFullName;

        private static Dictionary<string, string> _paths = new Dictionary<string, string>();

        public static string ProgramDir { get { return _programDir; } private set { } }
        public static string IRDataDir { get { return _irDataDir; } private set { } }
        public static string IniFullName { get { return _iniFullName; } private set { } }
        public static string ErrorLogDir { get { return _errorLogDir; } private set { } }
        public static string DataErrorFullName { get { return _dataErrorFullName; } private set { } }

        #endregion

        #region struct Format

        public struct Format
        {
            private static string _xml = ".xml";

            public static string XML { get { return _xml; } private set { } }
        }

        #endregion struct Format
    }
}
