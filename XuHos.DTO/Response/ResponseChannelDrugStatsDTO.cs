using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    public class ResponseChannelDrugStatsDTO
    {
        public string Dimension { set; get; }
        public string HospitalName { set; get; }
        public string DrugCode { set; get; }
        public string DrugName { set; get; }
        public int TotalQuantity { set; get; }
        public string Unit { set; get; }
        public decimal CastAmount { set; get; }
        public decimal ChannelAmount { set; get; }
        decimal? _Profit;
        public decimal Profit
        {
            get
            {
                if(!_Profit.HasValue)
                    _Profit = this.ChannelAmount - CastAmount;
                return _Profit.Value;
            }
            set
            {
                _Profit = value;
            }
        }

    }
}
