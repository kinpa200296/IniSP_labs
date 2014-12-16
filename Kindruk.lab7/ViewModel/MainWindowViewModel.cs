using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;
using Kindruk.lab7.Model;

namespace Kindruk.lab7.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<StudentViewModel> StudentsModels { get; set; }
        public StudentViewModel CurrentStudent { get; set; }

        #region Contructors

        public MainWindowViewModel()
        {
            StudentsModels = new ObservableCollection<StudentViewModel>();
            CurrentStudent = new StudentViewModel(new StudentModel());
            StudentsModels.Add(CurrentStudent);
        }

        public MainWindowViewModel(IEnumerable<StudentModel> students)
        {
            StudentsModels = new ObservableCollection<StudentViewModel>(students.Select(s => new StudentViewModel(s)));
            CurrentStudent = StudentsModels.First();
        }

        #endregion

        public void AddStudentExecute()
        {
            StudentsModels.Add(new StudentViewModel(
                new StudentModel
                {
                    FirstName = CurrentStudent.FirstName,
                    LastName = CurrentStudent.LastName,
                    Faculty = CurrentStudent.Faculty,
                    GroupNumber = CurrentStudent.GroupNumber,
                    StudentCard = CurrentStudent.StudentCard
                }));
        }

        public void SaveToFileExecute()
        {
            var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            var root = new XElement("Students");
            foreach (var model in StudentsModels)
            {
                root.Add(new XElement("Student",
                    new XElement("FirstName", model.FirstName),
                    new XElement("LastName", model.LastName), 
                    new XElement("Faculty", model.Faculty), 
                    new XElement("GroupNumber", model.GroupNumber),
                    new XElement("StudentCard", model.StudentCard)));
            }
            doc.Add(root);
            doc.Save("Students.xml");
        }

        public void LoadFromFileExecute()
        {
            var students = XDocument.Load("Students.xml").Root;
            StudentsModels.Clear();
            foreach (var student in students.Elements())
            {
                var studentModel = new StudentViewModel(
                    new StudentModel(
                    student.Element("FirstName").Value, student.Element("LastName").Value, student.Element("Faculty").Value,
                    long.Parse(student.Element("GroupNumber").Value), long.Parse(student.Element("StudentCard").Value)));
                StudentsModels.Add(studentModel);
            }
        }

        public ICommand AddStudent
        {
            get { return new RelayCommand(AddStudentExecute); }
        }

        public ICommand SaveToFile
        {
            get { return new RelayCommand(SaveToFileExecute); }
        }

        public ICommand LoadFromFile
        {
            get { return new RelayCommand(LoadFromFileExecute); }
        }
    }
}
