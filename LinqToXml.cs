using System.Xml.Linq;

namespace Lab2_dorm
{
	public class LinqToXml: IXmlParserStrategy
    {
        XDocument doc;
		public LinqToXml(string xmlFilePath)
		{
            doc = XDocument.Load(xmlFilePath);
		}

        public List<Student> SearchStudentInXmlFile(string enteredKeyword, string selectedElement, string selectedId)
        {
            var query = from d in doc.Descendants("student")
                       where (d.Element(selectedElement).Value == enteredKeyword && d.Attribute("ID").Value == selectedId)
                        select new Student
                        {
                            Id = d.Attribute("ID")?.Value,
                            Name = d.Element("Name")?.Value,
                            MiddleName = d.Element("MiddleName")?.Value,
                            Surname = d.Element("Surname")?.Value,
                            Faculty = d.Element("Faculty")?.Value,
                            Cathedra = d.Element("Cathedra")?.Value,
                            StudyingYear = d.Element("StudyingYear")?.Value,
                            DormNumber = d.Element("DormNumber")?.Value,
                            HomeCity = d.Element("HomeCity")?.Value,
                            SettlementDay = d.Element("SettlementDay")?.Value,
                            SettlementMonth = d.Element("SettlementMonth")?.Value,
                            SettlementYear = d.Element("SettlementYear")?.Value

                        };

            List<Student> students = query.ToList();
            return students;

        }
    }
}

