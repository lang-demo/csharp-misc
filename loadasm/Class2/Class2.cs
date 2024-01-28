using System;
using System.Reflection;
using System.Linq;
using System.IO;


internal class Initializer
{
    public static string AssemblyDirectory
    {
        get
        {
            string codeBase = typeof(Initializer).Assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
    static Initializer()
    {
        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
        {
            string fileName = new AssemblyName(args.Name).Name + ".dll";
            //string assemblyPath = Path.Combine(@"D:\.repo\base14\go\cshrp\loadasm\Class2", fileName);
            string assemblyPath = Path.Combine(@AssemblyDirectory, fileName);
            var assembly = Assembly.LoadFile(assemblyPath);
            return assembly;
        };
    }
    public static void initialize()
    {
        ;
    }
}

public class Class2
{
    static Class2()
    {
        Initializer.initialize();
    }
    public string TestMethod1()
    {
        return "hello world!";
    }
    public void TestMethod2(int test)
    {
        Console.WriteLine(Dep.Add2(test, 100));
    }
}
