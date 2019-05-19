namespace CarManager.Model
{
    public class Engine
    {
        public double Displacement;
        public double HorsePower;
        public string Model;

        public Engine(double displacement, double horsePower, string model)
        {
            Displacement = displacement;
            HorsePower = horsePower;
            Model = model;
        }
    }
}
