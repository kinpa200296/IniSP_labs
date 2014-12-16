using Kindruk.lab7.Model;

namespace Kindruk.lab7.ViewModel
{
    public class StudentViewModel : ViewModelBase
    {
        private StudentModel _student;

        public StudentViewModel(StudentModel student)
        {
            _student = student;
        }

        #region Properties

        public string FirstName
        {
            get { return _student.FirstName; }
            set
            {
                _student.FirstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get { return _student.LastName; }
            set
            {
                _student.LastName = value;
                OnPropertyChanged("LastName");
            }
        }

        public string Faculty
        {
            get { return _student.Faculty; }
            set
            {
                _student.Faculty = value;
                OnPropertyChanged("Faculty");
            }
        }

        public long GroupNumber
        {
            get { return _student.GroupNumber; }
            set
            {
                _student.GroupNumber = value;
                OnPropertyChanged("GroupNumber");
            }
        }

        public long StudentCard
        {
            get { return _student.StudentCard; }
            set
            {
                _student.StudentCard = value;
                OnPropertyChanged("StudentCard");
            }
        }

        #endregion
    }
}
