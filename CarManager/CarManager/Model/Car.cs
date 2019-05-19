namespace CarManager.Model
{
    public class Car
    {
        public string Model;
        public Engine Motor;
        public int Year;

        public Car(string model, Engine engine, int year)
        {
            Model = model;
            Motor = engine;
            Year = year;
        }
    }
}
