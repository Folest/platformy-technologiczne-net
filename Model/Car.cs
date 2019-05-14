using System.Xml.Serialization;

namespace LanguageIntegratedQueries.Model
{
    [XmlType("car")]
    public class Car
    {
        [XmlElement(ElementName = "model")]
        public string model;
        [XmlElement(ElementName = "engine")]
        public Engine motor;
        [XmlElement(ElementName = "year")]
        public int year;

        public Car(string model, Engine motor, int year)
        {
            this.model = model;
            this.motor = motor;
            this.year = year;
        }

        public Car() { }
    }
}
