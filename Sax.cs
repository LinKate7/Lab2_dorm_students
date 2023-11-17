using System.Xml;

namespace Lab2_dorm
{
	public class Sax : IXmlParserStrategy
    { 
        string path;
        Student student;
        public Sax(string xmlFilePath)
		{
            path = xmlFilePath;
        }
        private XmlTextReader CreateNewXmlReader(string xmlFilePath)
        {
            return new XmlTextReader(xmlFilePath);
        }
        public List<Student> SearchStudentInXmlFile(string enteredKeyword, string selectedElement, string selectedId)
        {
            List<Student> allStudents = new List<Student>();


            using (XmlTextReader xmlReader = CreateNewXmlReader(path))
            {
                while (xmlReader.Read())
                {

                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        switch (xmlReader.Name)
                        {
                            case "student":
                                student = new Student();
                                if (xmlReader.MoveToAttribute("ID"))
                                {
                                    student.Id = xmlReader.Value;
                                    xmlReader.MoveToElement();
                                };
                                break;
                            case "Name":

                                if (student != null)
                                    student.Name = xmlReader.ReadElementContentAsString();
                                break;
                            case "MiddleName":
                                if (student != null)
                                    student.MiddleName = xmlReader.ReadElementContentAsString();
                                break;
                            case "Surname":
                                if (student != null)
                                    student.Surname = xmlReader.ReadElementContentAsString();
                                break;
                            case "Faculty":
                                if (student != null)
                                    student.Faculty = xmlReader.ReadElementContentAsString();
                                break;
                            case "Cathedra":
                                if (student != null)
                                    student.Cathedra = xmlReader.ReadElementContentAsString();
                                break;
                            case "StudyingYear":
                                if (student != null)
                                    student.StudyingYear = xmlReader.ReadElementContentAsString();
                                break;
                            case "DormNumber":
                                if (student != null)
                                    student.DormNumber = xmlReader.ReadElementContentAsString();
                                break;
                            case "HomeCity":
                                if (student != null)
                                    student.HomeCity = xmlReader.ReadElementContentAsString();
                                break;
                            case "SettlementDay":
                                if (student != null)
                                    student.SettlementDay = xmlReader.ReadElementContentAsString();
                                break;
                            case "SettlementMonth":
                                if (student != null)
                                    student.SettlementMonth = xmlReader.ReadElementContentAsString();
                                break;
                            case "SettlementYear":
                                if (student != null)
                                    student.SettlementYear = xmlReader.ReadElementContentAsString();
                                break;

                        }


                    }
                    if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "student" && student != null)
                    {
                        allStudents.Add(student);
                    }

                }
            }
           return FilterSelectedStudents(allStudents, enteredKeyword, selectedElement, selectedId);
        }


        public List<Student> FilterSelectedStudents(List<Student> allStudents, string enteredKeyword, string selectedElement, string selectedId)
        {
            List<Student> students = new List<Student>();
            if(selectedElement == "StudyingYear")
            {
                foreach(var student in allStudents)
                {
                   if (student !=null && student.StudyingYear == enteredKeyword && student.Id == selectedId)
                   {
                        students.Add(student);
                   }
                }
            }
            if (selectedElement == "Cathedra")
            {
                foreach (var student in allStudents)
                {
                    if (student != null && student.Cathedra == enteredKeyword && student.Id == selectedId)
                    {
                        students.Add(student);
                    }
                }
            }

            return students;
        }
    }
}

