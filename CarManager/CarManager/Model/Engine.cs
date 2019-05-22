namespace CarManager.Model
{
    public class Engine
    {
        public double Displacement { get; set; }
        public double HorsePower { get; set; }
        public string Model { get; set; }

        public Engine(double displacement, double horsePower, string model)
        {
            Displacement = displacement;
            HorsePower = horsePower;
            Model = model;
        }

        public string GetDetails => $"{Model} {Displacement} ({HorsePower} hp)";
    }
}
