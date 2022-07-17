using System.Threading.Tasks;

namespace IPM_239043.Logic
{
    public class Incrementor
    {
        public string Step
        {
            get { return $"Krok: {Counter}"; }
            set { }
        }
        public int Counter { get; set; } = 0;

        public int HelloCounter { get; set; } = 0;
        public string Hello { get; set; }

        public Incrementor()
        {
            Step = $"Krok: {Counter}";
            Hello = $"Hello po raz: {HelloCounter}";
        }

        public async Task<string> IncrementCounter()
        {
            await Task.Delay(500);
            Counter++;
            Step = $"Krok: {Counter}";
            return Step;
        }

        public string IncrementHelloCounter()
        {
            HelloCounter++;
            Hello = $"Hello po raz: {HelloCounter}";
            return Hello;
        }

    }
}
