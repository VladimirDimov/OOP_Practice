namespace HTMLRenderer
{
    using HTMLRenderer.Interfaces;
    using Microsoft.CSharp;
    using System;
    using System.CodeDom.Compiler;
    using System.Reflection;
    using System.Text;

    public class HTMLRendererCommandExecutor
    {
        static void Main()
        {
            IElementFactory htmlFactory = new HTMLElementFactory();
            IElement html = htmlFactory.CreateElement("html");
            IElement h1 = htmlFactory.CreateElement("h1", "Welcome!");
            html.AddElement(h1);
            Console.WriteLine(html);
            ITable table = htmlFactory.CreateTable(3, 2);
            table[0, 0] = htmlFactory.CreateElement("b", "First Name");
            table[0, 1] = htmlFactory.CreateElement("b", "Last Name");
            table[1, 0] = htmlFactory.CreateElement(null, "Svetlin");
            table[1, 1] = htmlFactory.CreateElement(null, "Nakov");
            table[2, 0] = htmlFactory.CreateElement(null, "George");
            table[2, 1] = htmlFactory.CreateElement(null, "Georgiev");
            html.AddElement(table);
            IElement br = htmlFactory.CreateElement("br", null);
            html.AddElement(br);
            IElement div = htmlFactory.CreateElement("div",
              "(c) Nakov & Joro @ <Telerik Software Academy>");
            html.AddElement(div);
            Console.WriteLine(html);
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
}
