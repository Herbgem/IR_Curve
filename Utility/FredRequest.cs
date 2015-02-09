using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Xml;
using System.Windows.Forms;

namespace Utility
{
    public class FredRequest
    {
        #region Methods

        public FredRequest() { }

        /// <summary>
        /// Return a formatted FRED request string
        /// </summary>
        public string FormatRequest(string seriesID, DateTime observationstart, DateTime observationend, string api = _apiKey)
        {
            return string.Format(_fredRequestString, seriesID, observationstart.ToString("yyyy-MM-dd"), observationend.ToString("yyyy-MM-dd"), api);
        }
        /// <summary>
        /// Download IR data into file from FRED
        /// </summary>
        /// <param name="request"></param>
        /// <param name="filefullname"></param>
        /// <returns>
        /// Return true if no exception thrown, otherwise false
        /// </returns>
        public bool Download(string request, string filefullname)
        {
            try
            {
                if (File.Exists(filefullname))
                    File.Delete(filefullname);
                var req = (HttpWebRequest)WebRequest.Create(request);
                req.Proxy = null;
                req.Credentials = new NetworkCredential();
                req.Method = WebRequestMethods.Http.Get;
                using (var resp = (HttpWebResponse)req.GetResponse())
                using (Stream rs = resp.GetResponseStream())
                using (FileStream fs = File.Create(filefullname))
                {
                    //XmlDocument doc = new XmlDocument();
                    //doc.Load(rs);
                    rs.CopyTo(fs);
                }
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(@"(400)"))
                {
                    MessageBox.Show(string.Format(
                        "{0}\r\nCannot download file {1}",
                          ex.Message, Path.GetFileNameWithoutExtension(filefullname)));
                }
                else
                    MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        /// <summary>
        /// Download IR data from FRED and read it into MemoryStream
        /// </summary>
        /// <param name="request"></param>
        /// <param name="id"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public bool Download(string request, string id, out MemoryStream sr)
        {
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(request);
                req.Proxy = null;
                req.Credentials = new NetworkCredential();
                req.Method = WebRequestMethods.Http.Get;
                using(var resp = (HttpWebResponse)req.GetResponse())
                using(Stream s = resp.GetResponseStream())
                {
                    sr = new MemoryStream();
                    s.CopyTo(sr);
                }
                return true;
            }
            catch (Exception ex)
            {
                sr = null;
                if (ex.Message.Contains(@"(400)"))
                {
                    MessageBox.Show(string.Format(
                        "{0}\r\nCannot download file {1}",
                          ex.Message, id));
                }
                else
                    MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion Methods

        #region Fields
        private string _fredRequestString = @"http://api.stlouisfed.org/fred/series/observations?series_id={0}&observation_start={1}&observation_end={2}&api_key={3}";
        private const string _apiKey = @"0b10bfc1eb79d0c78efbc7d1b9908204";
        private string _seriesID;
        private string _observationStart;
        private string _observationEnd;
        private Dictionary<string, string> _treasuryIDs = new Dictionary<string, string>() {
                                               {"oneMO", "DGS1MO"}, {"threeMO", "DGS3MO"}, {"sixMO", "DGS6MO"},
                                               {"oneYR", "DGS1"}, {"twoYR", "DGS2"}, {"threeYR", "DGS3"},
                                               {"fiveYR", "DGS5"}, {"sevenYR", "DGS7"}, {"tenYR", "DGS10"},
                                               {"twentyYR", "DGS20"}, {"thirtyYR", "DGS30"}
                                               };
        private Dictionary<string, string> _liborIDs = new Dictionary<string, string>() {
                                               {"overNight", "USDONTD156N"}, {"oneWeek", "USD1WKD156N"},
                                               {"oneMO", "USD1MTD156N"}, {"twoMO", "USD2MTD156N"},
                                               {"threeMO", "USD3MTD156N"}, {"sixMO", "USD6MTD156N"},
                                               {"tweleveMO", "USD12MD156N"}
                                               };
        private Dictionary<string, string> _irsIDs = new Dictionary<string, string>() {
                                               {"oneYR", "DSWP1"}, {"twoYR", "DSWP2"},
                                               {"threeYR", "DSWP3"},{"fourYR", "DSWP4"}, 
                                               {"fiveYR", "DSWP5"}, {"sevenYR", "DSWP7"}, 
                                               {"tenYR", "DSWP10"}, {"thirtyYR", "DSWP30"}
                                               };

        public string FredRequestString { get { return _fredRequestString; } private set { } }
        public string SeriesId { get { return _seriesID; } set { _seriesID = value; } }
        public Dictionary<string, string> TreasureIDs { get { return _treasuryIDs; } private set { } }
        public Dictionary<string, string> LiborIDs { get { return _liborIDs; } private set { } }
        public Dictionary<string, string> IRSIDs { get { return _irsIDs; } private set { } }
        public string ObservationStart { get { return _observationStart; } set { _observationStart = value; } }
        public string ObservationEnd { get { return _observationEnd; } set { _observationEnd = value; } }

        #endregion Fields
    }
}
