using System;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;
using System.Collections.Generic;

namespace HTMLRenderer
{
    public interface IElement
    {
        string Name { get; }
        string TextContent { get; set; }
        IEnumerable<IElement> ChildElements { get; }
        void AddElement(IElement element);
        void Render(StringBuilder output);
        string ToString();
    }

    public interface ITable : IElement
    {
        int Rows { get; }
        int Cols { get; }
        IElement this[int row, int col] { get; set; }
    }

    public interface IElementFactory
    {
        IElement CreateElement(string name);
        IElement CreateElement(string name, string content);
        ITable CreateTable(int rows, int cols);
    }

    public class HTMLElementFactory : IElementFactory
    {
        public IElement CreateElement(string name)
        {
            return new HTMLElement(name);
        }

        public IElement CreateElement(string name, string content)
        {
            return new HTMLElement(name, content);
        }

        public ITable CreateTable(int rows, int cols)
        {
            return new HTMLTable(rows, cols);
        }
    }

    public class HTMLRendererCommandExecutor
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
                  using HTMLRenderer;

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

    public class HTMLElement : IElement
    {
        private string name;
        private string textContent;
        private List<IElement> childElements;

        public HTMLElement(string name)
        {
            this.Name = name;
            this.TextContent = null;
            this.childElements = new List<IElement>();
        }

        public HTMLElement(string name, string content)
            : this(name)
        {
            this.TextContent = content;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            private set
            {
                this.name = value;
            }
        }

        public string TextContent
        {
            get
            {
                return this.textContent;
            }
            set
            {
                this.textContent = value;
            }
        }

        public IEnumerable<IElement> ChildElements
        {
            get { return this.childElements; }
        }

        public void AddElement(IElement element)
        {
            this.childElements.Add(element);
        }

        public virtual void Render(StringBuilder output)
        {
            foreach (var childElement in this.ChildElements)
            {
                output.AppendFormat("<{0}>{1}{2}</{0}>",
                        this.Name, this.TextContent, RenderToString(childElement));
            }
        }

        private string RenderToString(IElement element)
        {
            if (!string.IsNullOrEmpty(element.TextContent))
            {
                return string.Format("<{0}>{1}</{0}>", element.Name, element.TextContent);
            }

            return null;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            this.Render(builder);
            return builder.ToString();
        }
    }

    public class HTMLTable : HTMLElement, ITable
    {
        private const string OpenRow = "<tr>";
        private const string CloseRow = "</tr>";
        private const string OpenCol = "<td>";
        private const string CloseCol = "</td>";
        private const string OpenTable = "<table>";
        private const string CloseTable = "</table>";

        private int rows;
        private int cols;
        private IElement[,] tableElements;
        private string name;
        private string textContent;

        public HTMLTable(int rows, int cols)
            :base("table")
        {
            this.Rows = rows;
            this.Cols = cols;
            tableElements = new IElement[rows, cols];
        }

        public int Rows
        {
            get
            {
                return this.rows;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Rows number must be positive");
                }

                this.rows = value;
            }
        }

        public int Cols
        {
            get
            {
                return this.cols;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Rows number must be positive");
                }

                this.cols = value;
            }
        }

        public IElement this[int row, int col]
        {
            get
            {
                if (isValidCell(row, col))
                {
                    return this.tableElements[row, col];
                }
                else
                {
                throw new IndexOutOfRangeException("Invalid cell");
                }
            }
            set
            {
                if (isValidCell(row, col))
                {
                    this.tableElements[row, col] = value;
                }
                else
                {
                throw new IndexOutOfRangeException("Invalid cell");
                }
            }
        }
        
        public IEnumerable<IElement> ChildElements
        {
            get { throw new ArgumentException("Invalid operation"); }
        }

        public void AddElement(IElement element)
        {
            throw new ArgumentException("Invalid operation");
        }

        public override void Render(StringBuilder output)
        {
            output.AppendFormat(OpenTable);
            for (int row = 0; row < this.Rows; row++)
            {
                output.AppendFormat(OpenRow);
                for (int col = 0; col < this.Cols; col++)
                {
                    output.Append(OpenCol);
                    output.AppendFormat("<{0}>{1}<{0}>", this[row, col].Name, this[row, col].ToString());
                    output.Append(CloseCol);
                }
                output.Append(CloseCol);
            }

            output.Append(CloseTable);
        }

        private bool isValidCell(int row, int col)
        {
            if (row > this.tableElements.GetLength(0) - 1 || row < 0)
            {
                return false;
            }
            if (col > this.tableElements.GetLength(1) - 1 || col < 0)
            {
                return false;
            }

            return true;
        }

    }
}
