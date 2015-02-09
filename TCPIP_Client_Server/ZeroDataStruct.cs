using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;

namespace Server
{
    class ZeroDataStruct
    {
        #region Methods

        private ZeroDataStruct() { }
        public ZeroDataStruct(XmlReader r, string fileNameWithoutExtension) { ReadXml(r, fileNameWithoutExtension); }
        ///
        /// <summary> Read the XML file into memory </summary>
        ///
        public void ReadXml(XmlReader r, string fileNameWithourExtension)
        {
            //Skip the XML declaration
            r.MoveToContent();
            //Read the start tag of root element
            r.ReadStartElement(_elementName);
            r.MoveToContent();
            
            Observation elmt = new Observation();
            _datasource.Columns.Add(elmt.DateName, typeof(DateTime));
            _datasource.Columns.Add(fileNameWithourExtension, typeof(double));
            _datasource.PrimaryKey = new DataColumn[] {_datasource.Columns[elmt.DateName]};
            
            while (r.NodeType == XmlNodeType.Element)
            {
                //Check if Observation has value. If yes, add it to the list.
                if (elmt.ReadXML(r))
                {
                    _datasource.Rows.Add(elmt.Date, elmt.Value);
                    //_observations.Add(elmt.Date, elmt.Value);
                }
            }
            r.ReadEndElement();
        }

        #endregion Methods

        #region Fields
        DataTable _datasource = new DataTable();
        private const string _elementName = @"observations";
        //
        //Properties
        //
        public DataTable Datasource { get { return _datasource; } private set { } }
        #endregion Fields

        #region Class Observation

        private class Observation
        {
            #region Methods

            public Observation() { }
            ///
            /// <summary> Read the XML element into memory </summary> 
            ///
            /// <returns> True if element has value rather than ".", otherwise false </returns> 
            ///
            public bool ReadXML(XmlReader r)
            {
                if (r.Name == _elementName)
                {
                    if (r.MoveToAttribute(_dateName))
                        _date = r.ReadContentAsDateTime();
                    if (r.MoveToAttribute(_valueName))
                    {
                        string str = r.ReadContentAsString();
                        bool convet = double.TryParse(str, out _value);
                        r.ReadStartElement();
                        r.MoveToContent();
                        if (!convet)
                            return false;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

            #endregion Methods

            #region Fields
            private const string _elementName = @"observation";
            private const string _dateName = @"date";
            private const string _valueName = @"value";
            //Attribute of the XML element
            private DateTime _date;
            //Attribute of the XML element
            private double _value;
            //
            //Properties
            //
            public DateTime Date { get { return _date; } private set { } }
            public double Value { get { return _value; } private set { } }
            public string DateName { get { return _dateName; } private set { } }
            public string ValueName { get { return _valueName; } private set { } }
            #endregion Fields
        }

        #endregion Class Observation
    }
}
