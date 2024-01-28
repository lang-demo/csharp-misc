using System;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.InteropServices;


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
    private delegate int Add2(int a, int b);
    public void TestMethod2(int test)
    {
        Console.WriteLine(Dep.Add2(test, 100));
        PInvokeProcInfo invInfo = new PInvokeProcInfo()
        {
            ProcName = "add2",
            EntryPoint = "add2",
            ModuleFile = Initializer.AssemblyDirectory + @"\add2.dll",
            ReturnType = typeof(Int32),
            ParameterTypes = new Type[] { typeof(Int32), typeof(Int32) },
            CallingConvention = CallingConvention. Cdecl,
            CharSet = CharSet.Unicode
        };

        //Invokeで実行
        MethodInfo method = CreateMethodInfo(invInfo);
        var answer = method.Invoke(null, new object[] { 11, 22 });
        Console.WriteLine(answer);

        //Delegateで実行
        Add2 add2 = (Add2)method.CreateDelegate(typeof(Add2));
        var answer2 = add2(33, 44);
        Console.WriteLine(answer2);
    }
    /// <summary>
    /// PInvoke関数情報から、メソッドのメタデータを作成する。
    /// </summary>
    /// <param name="invInfo">PInvoke関数情報</param>
    /// <returns>PInvoke関数メタデータ</returns>
    public static MethodInfo CreateMethodInfo(PInvokeProcInfo invInfo)
    {
        string moduleName = Path.GetFileNameWithoutExtension(invInfo.ModuleFile).ToUpper();
        AssemblyBuilder asmBld = AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName("Asm" + moduleName), AssemblyBuilderAccess.Run);

        ModuleBuilder modBld = asmBld.DefineDynamicModule(
            "Mod" + moduleName);

        TypeBuilder typBld = modBld.DefineType(
            "Class" + moduleName,
            TypeAttributes.Public | TypeAttributes.Class);

        MethodBuilder methodBuilder = typBld.DefinePInvokeMethod(
            invInfo.ProcName,
            invInfo.ModuleFile,
            invInfo.EntryPoint,
            MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.PinvokeImpl | MethodAttributes.HideBySig,
            CallingConventions.Standard,
            invInfo.ReturnType,
            invInfo.ParameterTypes.ToArray(),
            invInfo.CallingConvention,
            invInfo.CharSet);
        methodBuilder.SetImplementationFlags(MethodImplAttributes.PreserveSig);

        return typBld.CreateType().GetMethod(invInfo.ProcName);
    }
}

/// <summary>
/// PInvoke関数情報
/// </summary>
public class PInvokeProcInfo
{
    /// <summary>
    /// 関数名
    /// </summary>
    public string ProcName { get; set; }
    /// <summary>
    /// DLLファイル
    /// </summary>
    public string ModuleFile { get; set; }
    /// <summary>
    /// エントリポイント
    /// </summary>
    public string EntryPoint { get; set; }
    /// <summary>
    /// 戻り値の型（戻り値無しはSystem.Void）
    /// </summary>
    public Type ReturnType { get; set; } = typeof(void);
    /// <summary>
    /// 関数のパラメータの型
    /// </summary>
    public Type[] ParameterTypes { get; set; } = { };
    /// <summary>
    /// 呼び出し規約
    /// </summary>
    public CallingConvention CallingConvention { get; set; } = CallingConvention.StdCall;
    /// <summary>
    /// メソッドのキャラクターセット
    /// </summary>
    public CharSet CharSet { get; set; } = CharSet.Auto;
}
