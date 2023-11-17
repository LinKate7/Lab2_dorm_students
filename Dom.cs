using System.Xml;

namespace Lab2_dorm
{
	public class Dom : IXmlParserStrategy
    {
        XmlDocument doc = new XmlDocument();
		public Dom(string xmlFilePath)
		{
            doc.Load(xmlFilePath);
		}
        public List<Student> SearchStudentInXmlFile(string enteredKeyword, string selectedElement, string selectedId)
        {
            List<Student> students = new List<Student>();

            string xpathQuery = $"//student[{selectedElement} = '{enteredKeyword}' and @ID = '{selectedId}']";
            XmlNodeList studentNodes = doc.SelectNodes(xpathQuery);

            foreach (XmlNode studentNode in studentNodes)
            {
                Student student = new Student
                {
                    Id = studentNode.Attributes["ID"].Value,
                    Name = studentNode["Name"].InnerText,
                    MiddleName = studentNode["MiddleName"].InnerText,
                    Surname = studentNode["Surname"].InnerText,
                    Faculty = studentNode["Faculty"].InnerText,
                    Cathedra = studentNode["Cathedra"].InnerText,
                    StudyingYear = studentNode["StudyingYear"].InnerText,
                    DormNumber = studentNode["DormNumber"].InnerText,
                    HomeCity = studentNode["HomeCity"].InnerText,
                    SettlementDay= studentNode["SettlementDay"].InnerText,
                    SettlementMonth = studentNode["SettlementMonth"].InnerText,
                    SettlementYear = studentNode["SettlementYear"].InnerText
                };

                students.Add(student);
            }

            return students;
        }
    }
}

