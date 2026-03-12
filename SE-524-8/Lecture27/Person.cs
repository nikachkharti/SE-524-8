using System.Xml.Serialization;

namespace Lecture27
{
    [XmlRoot("Person")]
    public class Person
    {
        [XmlElement("Id")]
        public int PersonId { get; set; }

        [XmlElement("Name")]
        public string PersonName { get; set; }

        [XmlElement("Age")]
        public int PersonAge { get; set; }
    }
}
