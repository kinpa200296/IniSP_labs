namespace Kindruk.lab7.Model
{
    public class StudentModel
    {
        #region Constructors

        public StudentModel()
        {
            const string s = "Описание не предоставлено";
            FirstName = s;
            LastName = s;
            Faculty = s;
            GroupNumber = 0;
            StudentCard = 0;
        }

        public StudentModel(string firstName, string lastName, string faculty, long groupNumber, long studentCard)
        {
            FirstName = firstName;
            LastName = lastName;
            Faculty = faculty;
            GroupNumber = groupNumber;
            StudentCard = studentCard;
        }

        #endregion

        #region Properties

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Faculty { get; set; }

        public long GroupNumber { get; set; }

        public long StudentCard { get; set; }

        #endregion
    }
}
