using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.IO;

namespace ExecDllFuncTest
{
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

    class Program
    {
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

        private delegate int DlgMessageBox(IntPtr hWnd, string text, string caption, int buttonType);

        static int Main(string[] args)
        {
            PInvokeProcInfo invInfo = new PInvokeProcInfo()
            {
                ProcName = "MessageBox",
                EntryPoint = "MessageBoxW",
                ModuleFile = "User32.dll",
                ReturnType = typeof(Int32),
                ParameterTypes = new Type[] { typeof(IntPtr), typeof(string), typeof(string), typeof(Int32) },
                CallingConvention = CallingConvention.StdCall,
                CharSet = CharSet.Unicode
            };

            //Invokeで実行
            MethodInfo method = CreateMethodInfo(invInfo);
            method.Invoke(null, new object[] { IntPtr.Zero, "Run Invoke", "test1", 0});

            //Delegateで実行
            DlgMessageBox messageBox = (DlgMessageBox)method.CreateDelegate(typeof(DlgMessageBox));
            messageBox(IntPtr.Zero, "Run Delegate", "test2", 0);

            return 0;
        }
    }
}
