using System.Xml.Linq;
using System.Xml.Serialization;

namespace Lecture27
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // XmlSerializer

            //List<Person> people = new();
            //people.Add(person);
            //people.Add(new Person()
            //{
            //    PersonId = 2,
            //    PersonAge = 43,
            //    PersonName = "Merabi"
            //});
            //people.Add(new Person()
            //{
            //    PersonId = 3,
            //    PersonAge = 18,
            //    PersonName = "Gio"
            //});

            //var serializer = new XmlSerializer(typeof(Person));

            //using var writer = new StreamWriter(@"../../../File.xml", append: true);
            //serializer.Serialize(writer, person);

            //using var reader = new StreamReader(@"../../../File.xml");

            //Person deserializedPerson = (Person)serializer.Deserialize(reader);



            // LINQ --> To XML

        //    const string _filePath = @"../../../FileForTest.xml";

        //    XDocument doc = LoadOrCreate(_filePath, "People");

        //    IEnumerable<Person> people = doc.Descendants("Person")
        //        .Select(x => new Person()
        //        {
        //            PersonId = int.Parse(x.Element("Id").Value),
        //            PersonName = x.Element("Name").Value,
        //            PersonAge = int.Parse(x.Element("Age").Value)
        //        });


        //    doc.Root.Add(
        //            new XElement("Person",
        //                new XElement("Id", 4),
        //                new XElement("Name", "Akaki"),
        //                new XElement("Age", 22)
        //            )
        //        );
        //    doc.Save(_filePath);
        //}


        //public static XDocument LoadOrCreate(string path, string rootTag)
        //{
        //    if (!File.Exists(path))
        //    {
        //        using (File.Create(path)) { }
        //    }

        //    var doc = new XDocument(new XElement(rootTag));
        //    doc.Save(path);

        //    return doc;
        //}

    }
}
