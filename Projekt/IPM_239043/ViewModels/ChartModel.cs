namespace IPM_239043.ViewModels
{
    public class ChartModel
    {
        public ChartModel(string date, double mid)
        {
            Date = date;
            Mid = mid;
        }

        public string Date { get; set; }
        public double Mid { get; set; }

    }
}
