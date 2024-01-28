using System;
using System.Reflection;
using System.Linq;
using System.IO;

class MainClass
{
    public static string AssemblyDirectory
    {
        get
        {
            string codeBase = typeof(MainClass).Assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
    static void Main()
    {
        Console.WriteLine("Hello, World");
        try
        {
            //Console.WriteLine("(1)");

            /*
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string fileName = new AssemblyName(args.Name).Name + ".dll";
                string assemblyPath = Path.Combine(@"D:\.repo\base14\go\cshrp\loadasm\Class2", fileName);
                var assembly = Assembly.LoadFile(assemblyPath);
                return assembly;
            };
            */
            //Assembly assembly = Assembly.LoadFrom(@"D:\.repo\base14\go\cshrp\loadasm\Class2\Class2.dll");
            Assembly assembly = Assembly.LoadFrom(AssemblyDirectory + @"\Class2\Class2.dll");
            //Console.WriteLine("(2)");
            foreach (Type ti in assembly.GetTypes().Where(x => x.IsClass))
            {
                //Console.WriteLine("(3)");
                //Console.WriteLine(ti.Name);
                if (ti.Name == "Class2")
                {
                    MethodInfo TestMethod1 = ti.GetMethod("TestMethod1");
                    MethodInfo TestMethod2 = ti.GetMethod("TestMethod2");
                    var instance = Activator.CreateInstance(ti);
                    string value = TestMethod1.Invoke(instance, null).ToString();
                    Console.WriteLine("(3.1)");
                    TestMethod2.Invoke(instance, new object[] { 1 });
                    Console.WriteLine("(3.2)");
                }
            }
            //Console.WriteLine("(4)");
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
        }
    }
}
