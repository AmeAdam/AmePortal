namespace AmeWeb.Data
{
    public class FroniusData
    {
        public int Power { get; set; }
        public decimal DayPower { get; set; }
        public decimal TotalPower { get; set; }

        public int MaxPower => 8250;

        public int PowerPercent => Power * 100 / MaxPower; 
    }
}
