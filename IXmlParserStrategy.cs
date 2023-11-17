using System;
namespace Lab2_dorm
{
	public interface IXmlParserStrategy
	{

		public List<Student> SearchStudentInXmlFile(string enteredKeyword, string selectedElement, string selectedId);

	}
}

