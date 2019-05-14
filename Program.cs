using System.Linq;

using LanguageIntegratedQueries.Model;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;
using System.Xml.XPath;
using System.Xml;

namespace LanguageIntegratedQueries
{
    class Program
    {
        static void createXmlFromLinq(List<Car> myCars)
        {
            IEnumerable<XElement> nodes = from car in myCars
                                          select new XElement("car",
                                                      new XElement("model", car.model),
                                                      new XElement("engine",
                                                          new XAttribute("model", car.motor.model),
                                                          new XElement("displacement", car.motor.displacement),
                                                          new XElement("horsePower", car.motor.horsePower)
                                                      ),
                                                      new XElement("year", car.year)
                                                 );
            XElement rootNode = new XElement("cars", nodes); //create a root node to contain the query results
            rootNode.Save("CarsFromLinq.xml");
        }

        static void Main(string[] args)
        {
            // Zadanie 1
            var myCars = new List<Car>()
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

            var engineTypeWithHpRatio = myCars.Select(x => new
            {
                engineType = x.motor.model == "TDI" ? "diesel" : "petrol",
                hppl = x.motor.horsePower / x.motor.displacement
            });

            var engineTypeWithHpRatioGrouped = engineTypeWithHpRatio.GroupBy(x => x.engineType);

            engineTypeWithHpRatioGrouped.ToList().ForEach(x => System.Console.WriteLine($"{x.Key}: {x.ToList().Average(y => y.hppl)}"));

            // Zadanie 2

            var rootElement = new XmlRootAttribute("cars");

            var serializer = new XmlSerializer(myCars.GetType(), rootElement);

            using (var stream = new FileStream(".\\cars.xml", FileMode.OpenOrCreate))
            {
                serializer.Serialize(stream, myCars);
                stream.Flush();
            }

            // Zadanie 3

            XElement rootNode = XElement.Load("C:\\Users\\Karol\\source\\repos\\LanguageIntegratedQueries\\LanguageIntegratedQueries\\bin\\Debug\\netcoreapp2.2\\cars.xml");

            if (rootNode == null)
                return;

            double avgHP = (double)rootNode.XPathEvaluate("sum(//car/engine[@model!=\"TDI\"]/horsePower) div" +
                                                          " count(//car/engine[@model!=\"TDI\"])");

            var models = rootNode.XPathSelectElements("//car/model[not(. = preceding::model)]");


            // Zadanie 4
            createXmlFromLinq(myCars);

            // Zadanie 5
            XElement xhtml = XElement.Load("template.html");
            XElement body = xhtml.Element("{http://www.w3.org/1999/xhtml}body");
            IEnumerable<XElement> rows = from car in myCars
                                         select new XElement("tr",
                                                     new XElement("td", car.model),
                                                     new XElement("td", car.motor.model),
                                                     new XElement("td", car.motor.displacement),
                                                     new XElement("td", car.motor.horsePower),
                                                     new XElement("td", car.year)
                                                );
            body.Add(new XElement("table", rows));
            xhtml.Save("table.html");


            // Zadanie 6
            XElement root = XElement.Load("CarsCollection.xml");
            foreach (XElement e in root.Elements())
            {
                foreach (XElement e2 in e.Elements())
                {
                    if (e2.Name == "model")
                        e2.SetAttributeValue("year", e.Element("year").Value);
                    foreach (XElement e3 in e2.Elements())
                    {
                        if (e3.Name == "horsePower")
                            e3.Name = "hp";
                    }
                }
                e.SetElementValue("year", null);
            }
            root.Save("CarsCollection_modified.xml");
        }
    }
}
