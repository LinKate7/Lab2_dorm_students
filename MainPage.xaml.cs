using System;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Lab2_dorm;

public partial class MainPage : ContentPage
{
    string selectedXmlFilePath;
    string selectedXslFilePath;
    string enteredKeyword;
    string selectedElement;
    string selectedId;
    IXmlParserStrategy xmlParser;
    public MainPage()
    {
        InitializeComponent();

    }

    private async void Choose_Xml_File(object sender, EventArgs e)
    {
        var CustomFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            {DevicePlatform.iOS, new[] {"come.adobe.xml"} },
            {DevicePlatform.macOS, new[] {"xml"} },
            {DevicePlatform.Android, new[] {"application/xml"} },
            {DevicePlatform.WinUI, new[] {".xml"} },
            {DevicePlatform.MacCatalyst, new[] {"xml"} },
        });
        FileResult result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Pick an xml file",
            FileTypes = CustomFileType

        });

        if (result != null)
        {
            selectedXmlFilePath = result.FullPath;
            PickStudentId();

        }
    }

    private void OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
          if (SaxRadioButton.IsChecked)
          {
              xmlParser = new Sax(selectedXmlFilePath);
              string element1 = "StudyingYear";
              string element2 = "Cathedra";
              PickStudentElement(element1, element2);
          }
          if (DomRadioButton.IsChecked)
          {
              xmlParser = new Dom(selectedXmlFilePath);
              string element1 = "DormNumber";
              string element2 = "HomeCity";
              PickStudentElement(element1, element2);

          }
          if (LinqToXmlRadioButton.IsChecked)
          {
              xmlParser = new LinqToXml(selectedXmlFilePath);
              string element1 = "Name";
              string element2 = "Faculty";
              PickStudentElement(element1, element2);
          }
            
    }

    private void PickStudentId() 
    {
        XDocument xmlDoc = XDocument.Load(selectedXmlFilePath);

        var studentInfo = xmlDoc.XPathSelectElements("/dorm/student");
        var uniqueId = new HashSet<string>();

        studentIdPicker.Items.Clear();
        foreach (var student in studentInfo)
        {
            var idAttribute = student.Attribute("ID");
            if (idAttribute != null && uniqueId.Add(idAttribute.Value))
            {
                studentIdPicker.Items.Add(idAttribute.Value);
            }
        }

        studentIdPicker.IsEnabled = true;
    }
    private void OnIdPickerSelectedIndexChanged(object sender, EventArgs e) // for id picker to fix selected value
    {
        selectedId = studentIdPicker.SelectedItem as string;
    }

    private void OnPickerSelectedIndexChanged(object sender, EventArgs e) //for student elements picker to fix selected value
    {
       selectedElement = elementPicker.SelectedItem as string;
    }
    private void PickStudentElement(string element1, string element2) 
    {
        XDocument xmlDoc = XDocument.Load(selectedXmlFilePath);

        var studentInfo = xmlDoc.XPathSelectElements("/dorm/student");
        var uniqueEl = new HashSet<string>();

        elementPicker.Items.Clear();
        foreach (var student in studentInfo)
        {
            var searchingElement1 = student.Element(element1);
            var searchingElement2 = student.Element(element2);

            if (searchingElement1 != null && uniqueEl.Add(element1) && searchingElement2 != null && uniqueEl.Add(element2))
            {
                elementPicker.Items.Add(element1);
                
                elementPicker.Items.Add(element2);
               
            }
        }

        elementPicker.IsEnabled = true;
    }

    private void OnEntryCompleted(object sender, EventArgs e)
    { 
        XDocument doc = XDocument.Load(selectedXmlFilePath);
        enteredKeyword = keywordEntry.Text;
        bool keywordExists = IsEnteredValueOk(enteredKeyword, doc, selectedElement);
        if(!keywordExists)
        {
            DisplayAlert("Error", $"{enteredKeyword} does not exist as {selectedElement} in the chosen file! Please try another one.", "OK");
            keywordEntry.Text = string.Empty;
        }
    }

    static bool IsEnteredValueOk(string enteredKeyword, XDocument doc, string selectedElement)
    {
        return doc.Descendants("student").Elements(selectedElement).Any(element => element.Value == enteredKeyword);
    }


    private async void Add_Xsl_File(object sender, EventArgs e)
    {
        var CustomFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            {DevicePlatform.iOS, new[] {"come.adobe.xsl"} },
            {DevicePlatform.macOS, new[] {"xsl"} },
            {DevicePlatform.Android, new[] {"application/xsl"} },
            {DevicePlatform.WinUI, new[] {".xsl"} },
            {DevicePlatform.MacCatalyst, new[] {"xsl"} },
        });
        FileResult result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Pick an xsl file",
            FileTypes = CustomFileType

        });

        if (result != null)
        {
            selectedXslFilePath = result.FullPath;
        }
    }

    private void OnSearchClicked(object sender, EventArgs e)
    {
        List<Student> students = xmlParser.SearchStudentInXmlFile(enteredKeyword, selectedElement, selectedId);

        if(students != null)
        {
            DisplayStudentDetails(students);
        }
        else
        {
            DisplayAlert("Message", "Student is not found! Please check entered data.", "OK");
        }
    }

    private void DisplayStudentDetails(List<Student> students)
    {
        studentInfoLayout.Children.Clear();
        foreach(var student in students)
        {
            var label1 = new Label
            {
                Text = $"Id: {student.Id} \nName: {student.Name} \nMiddleName: {student.MiddleName} \nSurname: {student.Surname} " +
                $"\nFaculty: {student.Faculty} \nCathedra: {student.Cathedra} \nStudying Year: {student.StudyingYear} \nDormNumber: {student.DormNumber} " +
                $"\nHomecity: {student.HomeCity} \nSettlement Day: {student.SettlementDay} \nSettlement Month: {student.SettlementMonth}" +
                $"\nSettlement Year: {student.SettlementYear}"
                
            };
            studentInfoLayout.Children.Add(label1);
        };
        studentInfoLayout.IsVisible = true;
    }

    private void Convert_To_HTML(object sender, EventArgs e)
    {
        XslCompiledTransform xslt = new XslCompiledTransform();
        try
        {
            xslt.Load(selectedXslFilePath);
            DisplayAlert("Message", "Your file is successfully converted to HTML.", "OK");
        }
        catch(Exception ex)
        {
            DisplayAlert("Error!", "Something went wrong! Failed to convert your xml file to html. Please check the xsl file you are adding.", "OK");
        }
        string htmlFile = "/Users/katelagoda/Desktop/dorm_students.html";
        if (htmlFile != null && selectedXmlFilePath != null)
        {
            xslt.Transform(selectedXmlFilePath, htmlFile);
        }
    }

    private async void OnExitButtonClicked(Object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Message", "Do you really want to exit the program?", "Yes", "No");
        if (result)
        {
            Application.Current.Quit();
        }
    }
    private void OnClearButtonClicked(Object sender, EventArgs e)
    {
        studentInfoLayout.Children.Clear();
        keywordEntry.Text = string.Empty;
        elementPicker.SelectedIndex = -1;
        studentIdPicker.SelectedIndex = -1;

    }

}


