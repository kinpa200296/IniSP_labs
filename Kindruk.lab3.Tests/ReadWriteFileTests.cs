using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kindruk.lab3.Tests
{
    [TestClass]
    public class ReadWriteFileTests
    {
        [TestMethod]
        public void BinaryReadWriteTest()
        {
            var log = new LogManager();
            log.BeginLog();
            var answers = new LinkedList<Answer>
            {
                new Answer("string"),
                new Answer("double"),
                new Answer("int")
            };

            var questions = new LinkedList<Question>
            {
                new Question("Какой тип служит для представления текста?", 0, answers),
                new Question("Какой тип служит для представления вещественных чисел?", 1, answers),
                new Question("Какой тип слудит для представления целых знаковых чисел?", 2, answers)
            };
            var quiz = new Quiz("C# test", questions);
            QuizManager.WriteBinaryToFile("C# test.bin", quiz);
            var quiz2 = QuizManager.ReadBinaryFromFile("C# test.bin");
            Assert.AreEqual(true, quiz.SequenceEqual(quiz2));
            log.EndLog();
        }

        [TestMethod]
        public void ReadWriteTest()
        {
            var log = new LogManager();
            log.BeginLog();
            var answers = new LinkedList<Answer>
            {
                new Answer("string"),
                new Answer("double"),
                new Answer("int")
            };

            var questions = new LinkedList<Question>
            {
                new Question("Какой тип служит для представления текста?", 0, answers),
                new Question("Какой тип служит для представления вещественных чисел?", 1, answers),
                new Question("Какой тип слудит для представления целых знаковых чисел?", 2, answers)
            };
            var quiz = new Quiz("C# test", questions);
            QuizManager.WriteToFile("C# test.quiz", quiz);
            var quiz2 = QuizManager.ReadFromFile("C# test.quiz");
            Assert.AreEqual(true, quiz.SequenceEqual(quiz2));
            log.EndLog();
        }

        [TestMethod]
        public void SummaryReadWriteTest()
        {
            var log = new LogManager();
            log.BeginLog();
            var answers = new LinkedList<Answer>
            {
                new Answer("string"),
                new Answer("double"),
                new Answer("int")
            };

            var questions = new LinkedList<Question>
            {
                new Question("Какой тип служит для представления текста?", 0, answers),
                new Question("Какой тип служит для представления вещественных чисел?", 1, answers),
                new Question("Какой тип слудит для представления целых знаковых чисел?", 2, answers)
            };
            var quiz = new Quiz("C# test", questions);
            answers = new LinkedList<Answer>
            {
                new Answer("IEnumrable<T>"),
                new Answer("IEquatable<T>"),
                new Answer("IList<T>"),
                new Answer("IDisposable")
            };
            quiz.Add(new Question("Какой интерфейс позволяет использовать конструкцию foreach?", 0, answers));
            quiz.Add(new Question("Какой интерфейс гарантирует, что класс сравним по равенству?", 1, answers));
            quiz.Add(new Question("Какой интерфейс сообщает сборщику мусора о необходимости высвобождения неуправляемых ресурсов?", 3, answers));
            QuizManager.WriteToFile("C# test 2.quiz", quiz);
            QuizManager.WriteBinaryToFile("C# test 2.bin", quiz);
            var quiz1 = QuizManager.ReadFromFile("C# test 2.quiz");
            var quiz2 = QuizManager.ReadBinaryFromFile("C# test 2.bin");
            Assert.AreEqual(true, quiz1.SequenceEqual(quiz2));
            log.EndLog();
        }
    }
}
