using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Utility;

namespace Server
{
    internal class ZeroCalculation
    {
        #region Methods
        private ZeroCalculation() { }
        public ZeroCalculation(DataTable source, IDictionary<string, string> lookup)
        {
            _datatable = source;
            _lookup = lookup;
        }

        public DataTable Run()
        {
            Bootstrapping();
            return _datatable;
        }

        /// <summary>
        /// Using CMT(Constant Maturity Rate) to infer zero coupon rate.
        /// </summary>
        /// 
        /// <remarks> 
        /// According to Dept of Treasury, CMT is taken directly from Treasury Yield Curve which is a par yield curve. 
        /// Bootstrapping method requires par yield datapoints for every six months and the time length difference between
        /// any Treasuries over one year long is either equal or greater than one year. Thus, no zero rate for more than
        /// a year can be inferred from this datasource by bootstraping. It is, however, possible to construct a complete
        /// zero curve set if the missed datapoint is inferred from cubic spline model. 
        /// </remarks> 
        /// 
        /// <see cref="http://www.treasury.gov/resource-center/faqs/Interest-Rates/Pages/faq.aspx"/>
        /// 
        public void Bootstrapping()
        {
            DataColumn primaryKeySrcTable = _datatable.PrimaryKey.First();
            _primaryKey = new DataColumn(primaryKeySrcTable.ColumnName, primaryKeySrcTable.DataType);
            _zeroTable.Columns.Add(_primaryKey);
            _zeroTable.PrimaryKey = new DataColumn[] { _primaryKey };

            foreach (string key in _lookup.Keys)
            {
                TimeRanges timeLength = (TimeRanges)Enum.Parse(typeof(TimeRanges), key);

                if ((int)timeLength < (int)TimeRanges.oneYR)
                    BootstrappingUnderOneYr(timeLength);
                else if ((int)timeLength == (int)TimeRanges.oneYR)
                    BootstrappingOverOneYr(timeLength);
            }
            _datatable.Merge(_zeroTable, false, MissingSchemaAction.Add);
        }
        /// <summary>
        /// Add to the _datatable with a new column which contains zero rate for underlying timelength
        /// </summary>
        /// <param name="timeLength"></param>
        private void BootstrappingUnderOneYr(TimeRanges timeLength)
        {
            string oldColumnName = _lookup[timeLength.ToString()];
            string newColumnName = FormatColumnName(oldColumnName);
            DataTable tempDT = SetupTempDatatable();
            tempDT.Columns.Add(newColumnName, typeof(double));
            
            foreach (DataRow dr in _datatable.Rows)
            {
                if (dr[oldColumnName] is double)
                {
                    double zeroRate = AlgoBootstrappingUnderOneYr((double)dr[oldColumnName], (double)timeLength / 12);
                    DataRow zeroRow = tempDT.NewRow();
                    zeroRow[newColumnName] = zeroRate;
                    zeroRow[_primaryKey.ColumnName] = dr[_primaryKey.ColumnName];
                    tempDT.Rows.Add(zeroRow);
                }
            }
            _zeroTable.Merge(tempDT, false, MissingSchemaAction.AddWithKey);
            
        }
        /// <summary>
        /// Add to the _datatable with a new column which contains zero rate for underlying timelength
        /// </summary>
        /// <param name="timeLength"></param>
        private void BootstrappingOverOneYr(TimeRanges timeLength)
        {
            if (_zeroTable.Columns.Count == 1)
                return;

            string oldColumnName = _lookup[timeLength.ToString()];
            string newColumnName = FormatColumnName(oldColumnName);
            DataTable tempDT = SetupTempDatatable();
            tempDT.Columns.Add(newColumnName, typeof(double));


            foreach (DataRow dr in _datatable.Rows)
            {
                if (dr[oldColumnName] is double)
                {
                    double zeroRate = AlgoBootstrappingOverOneYr(
                        (double)dr[_zeroTable.Columns.Count - 1], (double)dr[oldColumnName],
                        ((double)timeLength - 6) / 12, (double)timeLength / 12
                        );
                    DataRow zeroRow = tempDT.NewRow();
                    zeroRow[newColumnName] = zeroRate;
                    zeroRow[_primaryKey.ColumnName] = dr[_primaryKey.ColumnName];
                    tempDT.Rows.Add(zeroRow);
                }
            }
            _zeroTable.Merge(tempDT, false, MissingSchemaAction.Add);
        }

        /// <summary>
        /// Infer the zero coupon rate from par yield for treasury with maturity time under one year
        /// </summary>
        /// <param name="parYield">CMT from Treasury yield curve</param>
        /// <param name="time">Time to maturity</param>
        /// <returns>Zero coupon rate</returns>
        /// <remarks Equation: (100 + 0.5* c) * e^rt = 100, c is coupon, r is zero coupon rate, t is maturity time/>
        private double AlgoBootstrappingUnderOneYr(double parYield, double time)
        {
            return 100 * Math.Log((1000 + 0.5 * parYield * 10) / 1000) / time;
        }
        /// <summary>
        /// Subtract the sum of present value of coupons prior year n to simplify the equation to be
        /// an equivalent of that in AlgoBootstrappingUnderOneYr
        /// </summary>
        /// <param name="zeroRateForLastLevel"></param>
        /// <param name="currentParYield"></param>
        /// <param name="previousTime"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private double AlgoBootstrappingOverOneYr(double zeroRateForLastLevel, double currentParYield, double previousTime, double time)
        {
            double pvOfPreviousCoupon = 1000 - 1000 / (Math.Exp(zeroRateForLastLevel / 100 * previousTime));
            return 100 * Math.Log((1000 + 0.5 * currentParYield * 10) / (1000 - pvOfPreviousCoupon)) / time;
        }
        /// <summary>
        /// Create a new column name
        /// </summary>
        /// <param name="oldColumnName"></param>
        /// <returns>If the old column name is "DGS1MO", the new one would be "DGS1MO (Zero Rate)"</returns>
        private string FormatColumnName(string oldColumnName)
        {
            return string.Concat(oldColumnName, "_Zero_Rate");
        }
        private DataTable SetupTempDatatable()
        {
            DataTable temp = new DataTable();
            DataColumn primaryKeySrcTable = _datatable.PrimaryKey.First();
            _primaryKey = new DataColumn(primaryKeySrcTable.ColumnName, primaryKeySrcTable.DataType);
            temp.Columns.Add(_primaryKey);
            temp.PrimaryKey = new DataColumn[] { _primaryKey };
            return temp;
        }

        #endregion Methods

        #region Fields

        public enum TimeRanges
        {
            oneMO = 1, threeMO = 3, sixMO = 6,
            oneYR = 12, twoYR = 24, threeYR = 36,
            fourYR = 48, fiveYR = 60, sevenYR = 84,
            tenYR = 120, twentyYR = 240, thirtyYR = 360
        }

        private DataTable _datatable;
        private DataColumn _primaryKey;
        private IDictionary<string, string> _lookup;
        private DataTable _zeroTable = new DataTable();

        #endregion Fields



    }
}
