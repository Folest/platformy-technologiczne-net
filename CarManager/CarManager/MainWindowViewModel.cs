using CarManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CarManager
{
    internal class MainWindowViewModel
    {
        public IList<Car> Cars { get; set; } = new List<Car>()
        {
            new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
            new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
            new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
            new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
            new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
            new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
            new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
            new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
            new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)

        };

        public IEnumerable<object> GetEngineType() => from c in Cars
                                                      group c by c.Motor.Model into cs
                                                      select new
                                                      {
                                                          engineType = cs.Key == "TDI" ? "Diesel" : "Petrol",
                                                          hppl = (cs.Sum(x => x.Motor.HorsePower) / cs.Count())
                                                      };

        private void DoubtfulyUsefulMethod()
        {
            Func<Car, Car, int> arg1 = delegate (Car c1, Car c2)
              {
                  return (int)(c1.Motor.HorsePower - c2.Motor.HorsePower);
              };

            Predicate<Car> arg2 = delegate (Car c)
            {
                return c.Motor.Model == "TDI";
            };

            Action<Car> arg3 = delegate (Car c)
            {
                MessageBox.Show($"Car of model: {c.Model} has an engine of displacement {c.Motor.Displacement}" +
                    $"of type {c.Motor.Model} and horse power {c.Motor.HorsePower}. It was produced in {c.Year}");
            };

            Cars.ToList().Sort(new Comparison<Car>(arg1));
            Cars.ToList().FindAll(arg2).ForEach(arg3);
        }
    }
}
