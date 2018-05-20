using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XuHos.DTO
{
    public class ResponsePlotChartData
    {
        public ResponsePlotChartData() { this.Series = new List<ResponsePlotChartSeries>(); }
        public List<ResponsePlotChartSeries> Series { get; set; }

        public List<ResponseAxisTick> YAxisTicks { get; set; }
    }
    public class ResponseAxisTick
    {
        public decimal Value { get; set; }
        public string Label { get; set; }
    }
    public class ResponsePlotChartSeries
    {
        public string Label { get; set; }
        public List<ResponsePlotChartPoint> Data { get; set; }
    }
    public class ResponsePlotChartPoint
    {
        public decimal X { get; set; }
        public object Y { get; set; }
        public ResponsePlotChartPointData Data { get; set; }
    }
    public class ResponsePlotChartPointData
    {
        public string ID { get; set; }
        public string Tooltip { get; set; }
    }
}
