using System;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using System.Collections.Generic;

namespace SoftwareAcademy
{
    public class Teacher : ITeacher
    {
        private string name;
        private List<ICourse> courses;

        public Teacher(string name)
        {
            this.Name = name;
            this.courses = new List<ICourse>();
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Name cannot be null or empty");
                }

                this.name = value;
            }
        }

        public void AddCourse(ICourse course)
        {
            if (course as Course != null)
            {
                this.courses.Add(course);
            }
            else
            {
                throw new ArgumentException("Invalid course");
            }
        }

        public override string ToString()
        {
            // Teacher: Name=(teacher name); Courses=[(course names – comma separated)]
            var result = new List<string>();

            result.Add(string.Format("Teacher: Name={0}", this.Name));
            if (CoursesToString() != null)
            {
            result.Add(this.CoursesToString());                
            }

            return string.Join("; ", result);
        }

        private string CoursesToString()
        {
            if (this.courses.Count == 0)
            {
                return null;
            }
            else
            {
                return string.Format("Courses=[{0}]", string.Join(", ", this.courses.Select(x => x.Name)));
            }
        }
    }

    public class Course : ICourse
    {
        private string name;
        private ITeacher teacher;
        private List<string> topics;

        public Course(string name, ITeacher teacher)
        {
            this.name = name;
            this.Teacher = teacher;
            this.topics = new List<string>();
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Name cannot be null or empty");
                }

                this.name = value;
            }
        }

        public ITeacher Teacher
        {
            get
            {
                return this.teacher;
            }
            set
            {
                this.teacher = value;
            }
        }

        public void AddTopic(string topic)
        {
            if (!string.IsNullOrEmpty(topic))
            {
                this.topics.Add(topic);
            }
            else
            {
                throw new ArgumentException("Topic cannot be null or empty");
            }
        }

        public override string ToString()
        {
            // (course type): Name=(course name); Teacher=(teacher name); Topics=[(course topics – comma separated)]; Lab=(lab name – when applicable); Town=(town name – when applicable);
            var result = new List<string>();
            result.Add(string.Format("{0}: Name={1}", this.GetType().Name, this.Name));
            if (this.TeacherToString() != null)
            {
                result.Add(this.TeacherToString());
            }
            if (this.TopicsToString() != null)
            {
                result.Add(this.TopicsToString());
            }

            return string.Join("; ", result);
        }

        private string TopicsToString()
        {
            if (this.topics.Count == 0)
            {
                return null;
            }
            else
            {
                return string.Format("Topics=[{0}]", string.Join(", ", this.topics));
            }
        }

        private string TeacherToString()
        {
            if (this.Teacher == null)
            {
                return null;
            }
            else
            {
                return string.Format("Teacher={0}", this.Teacher.Name);
            }
        }
    }

    public class LocalCourse : Course, ILocalCourse
    {
        private string lab;

        public LocalCourse(string name, ITeacher teacher, string lab)
            :base(name, teacher)
        {
            this.Lab = lab;
        }

        public string Lab
        {
            get
            {
                return this.lab;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.lab = value;
                }
                else
                {
                    throw new ArgumentException("Lab cannot be null or empty");
                }
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append(string.Format("; Lab={0}", this.Lab));
            return builder.ToString();
        }
    }

    public class OffsiteCourse : Course, IOffsiteCourse
    {
        private string town;

        public OffsiteCourse(string name, ITeacher teacher, string town)
            :base(name, teacher)
        {
            this.Town = town;
        }

        public string Town
        {
            get
            {
                return this.town;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.town = value;
                }
                else
                {
                    throw new ArgumentException("Town name cannot be null or empty");
                }
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append(string.Format("; Town={0}", this.Town));
            return builder.ToString();
        }
    }

    public interface ITeacher
    {
        string Name { get; set; }
        void AddCourse(ICourse course);
        string ToString();
    }

    public interface ICourse
    {
        string Name { get; set; }
        ITeacher Teacher { get; set; }
        void AddTopic(string topic);
        string ToString();
    }

    public interface ILocalCourse : ICourse
    {
        string Lab { get; set; }
    }

    public interface IOffsiteCourse : ICourse
    {
        string Town { get; set; }
    }

    public interface ICourseFactory
    {
        ITeacher CreateTeacher(string name);
        ILocalCourse CreateLocalCourse(string name, ITeacher teacher, string lab);
        IOffsiteCourse CreateOffsiteCourse(string name, ITeacher teacher, string town);
    }

    public class CourseFactory : ICourseFactory
    {
        public ITeacher CreateTeacher(string name)
        {
            return new Teacher(name);
        }

        public ILocalCourse CreateLocalCourse(string name, ITeacher teacher, string lab)
        {
            return new LocalCourse(name, teacher, lab);
        }

        public IOffsiteCourse CreateOffsiteCourse(string name, ITeacher teacher, string town)
        {
            return new OffsiteCourse(name, teacher, town);
        }
    }

    public class SoftwareAcademyCommandExecutor
    {
        static void Main()
        {
            string csharpCode = ReadInputCSharpCode();
            CompileAndRun(csharpCode);
        }

        private static string ReadInputCSharpCode()
        {
            StringBuilder result = new StringBuilder();
            string line;
            while ((line = Console.ReadLine()) != "")
            {
                result.AppendLine(line);
            }
            return result.ToString();
        }

        static void CompileAndRun(string csharpCode)
        {
            // Prepare a C# program for compilation
            string[] csharpClass =
            {
                @"using System;
                  using SoftwareAcademy;

                  public class RuntimeCompiledClass
                  {
                     public static void Main()
                     {"
                        + csharpCode + @"
                     }
                  }"
            };

            // Compile the C# program
            CompilerParameters compilerParams = new CompilerParameters();
            compilerParams.GenerateInMemory = true;
            compilerParams.TempFiles = new TempFileCollection(".");
            compilerParams.ReferencedAssemblies.Add("System.dll");
            compilerParams.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
            CSharpCodeProvider csharpProvider = new CSharpCodeProvider();
            CompilerResults compile = csharpProvider.CompileAssemblyFromSource(
                compilerParams, csharpClass);

            // Check for compilation errors
            if (compile.Errors.HasErrors)
            {
                string errorMsg = "Compilation error: ";
                foreach (CompilerError ce in compile.Errors)
                {
                    errorMsg += "\r\n" + ce.ToString();
                }
                throw new Exception(errorMsg);
            }

            // Invoke the Main() method of the compiled class
            Assembly assembly = compile.CompiledAssembly;
            Module module = assembly.GetModules()[0];
            Type type = module.GetType("RuntimeCompiledClass");
            MethodInfo methInfo = type.GetMethod("Main");
            methInfo.Invoke(null, null);
        }
    }
}
