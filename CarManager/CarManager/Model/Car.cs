namespace CarManager.Model
{
    public class Car
    {
        public string Model { get; set; }
        public Engine Motor { get; set; }
        public int Year { get; set; }

        public Car(string model, Engine engine, int year)
        {
            Model = model;
            Motor = engine;
            Year = year;
        }
    }
}
