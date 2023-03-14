using System.Text.Json;
using FinancialDailySummary.Models;
using Newtonsoft.Json;

namespace FinancialDailySummary.Services
{
    public class ChartService
    {
        private readonly CommandsEnum.Commands _index;

        public LinearChart Chart { get; set; }

        public ChartService(
            string[] labels,
            int?[] data,
            CommandsEnum.Commands index)
        {
            //TODO NEED REFACTOR or construct json direcly
            _index = index;


            Chart = new LinearChart();
            Chart.type = "line";

            //labels
            Chart.data.labels = new string[labels.Length];
            Chart.data.labels = labels;

            //Dataset
            Chart.data.datasets = new Dataset[1];
            Chart.data.datasets[0] = new();
            Chart.data.datasets[0].label = index.ToString();
            Chart.data.datasets[0].data = data;
            Chart.data.datasets[0].fill = false;
            Chart.data.datasets[0].borderColor = "blue";

            // Options
            Chart.options.responsive = true;
            Chart.options.legend = false;
            Chart.options.title.text = index.ToString(); 
            Chart.options.title.display = true;

            Chart.options.scales.yAxes = new Yax[1];
            Chart.options.scales.yAxes[0] = new Yax();
            Chart.options.scales.yAxes[0].ticks = new Ticks();
            Chart.options.scales.yAxes[0].ticks.suggestedMin = (int)data.Min() - 5;
            Chart.options.scales.yAxes[0].ticks.suggestedMax = (int)data.Max() + 5;


            var chartDataJson = JsonConvert.SerializeObject(Chart);


            QuickChart.Chart qc = new ();
            qc.Width = 500;
            qc.Height = 300;
            qc.Version = "2.9.4";
            qc.Config = chartDataJson;
            qc.DevicePixelRatio = 2.0;

            // Or write it to a file
            qc.ToFile($"{index}chart.png");
        }

        public string GetPathImage() 
        {
            return $"{Directory.GetCurrentDirectory()}\\{_index.ToString()}chart.png";
        }
    }
}
