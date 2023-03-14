using System.Text.Json;
using FinancialDailySummary.Models;
using Newtonsoft.Json;

namespace FinancialDailySummary.Services
{
    public class ChartService
    {
        private readonly CommandsEnum.Commands _index;

        public LinearChart LinearChart { get; private set; }

        public QuickChart.Chart Chart { get; set; }

        public ChartService(
            string[] labels,
            int?[] data,
            CommandsEnum.Commands index)
        {
            //TODO NEED REFACTOR or construct json direcly
            _index = index;


            LinearChart = new LinearChart();
            LinearChart.type = "line";

            //labels
            LinearChart.data.labels = new string[labels.Length];
            LinearChart.data.labels = labels;

            //Dataset
            LinearChart.data.datasets = new Dataset[1];
            LinearChart.data.datasets[0] = new();
            LinearChart.data.datasets[0].label = index.ToString();
            LinearChart.data.datasets[0].data = data;
            LinearChart.data.datasets[0].fill = false;
            LinearChart.data.datasets[0].borderColor = "blue";

            // Options
            LinearChart.options.responsive = true;
            LinearChart.options.legend = false;
            LinearChart.options.title.text = index.ToString(); 
            LinearChart.options.title.display = true;

            LinearChart.options.scales.yAxes = new Yax[1];
            LinearChart.options.scales.yAxes[0] = new Yax();
            LinearChart.options.scales.yAxes[0].ticks = new Ticks();
            LinearChart.options.scales.yAxes[0].ticks.suggestedMin = (int)data.Min() - 5;
            LinearChart.options.scales.yAxes[0].ticks.suggestedMax = (int)data.Max() + 5;


            var chartDataJson = JsonConvert.SerializeObject(LinearChart);


            Chart = new ();
            Chart.Width = 500;
            Chart.Height = 300;
            Chart.Version = "2.9.4";
            Chart.Config = chartDataJson;
            Chart.DevicePixelRatio = 2.0;

            
            // Or write it to a file
            //qc.ToFile($"{index}chart.png");

        }
    }
}
