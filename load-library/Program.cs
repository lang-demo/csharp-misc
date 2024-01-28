using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using JavaCommons;
namespace CUI;
static class Program
{
    delegate int MessageBoxW(IntPtr hWnd, IntPtr lpText, IntPtr lpCaption, uint type);
    static void test_MessageBox()
    {
        IntPtr handle = LoadLibraryW("user32.dll");
        IntPtr funcPtr = GetProcAddress(handle, "MessageBoxW");
        MessageBoxW pfMessageBoxW = (MessageBoxW)Marshal.GetDelegateForFunctionPointer(funcPtr, typeof(MessageBoxW));
        IntPtr lpText = Marshal.StringToHGlobalUni("Hello World.ハロー©");
        IntPtr lpCaption = Marshal.StringToHGlobalUni("Captionキャプション©");
        pfMessageBoxW(IntPtr.Zero, lpText, lpCaption, 0);
        Marshal.FreeHGlobal(lpText);
        Marshal.FreeHGlobal(lpCaption);
        FreeLibrary(handle);
    }
    delegate IntPtr greetingW(IntPtr name);
    static void test_qtlib()
    {
        IntPtr handle = LoadLibraryW("qtlib5-x86_64-static.dll");
        IntPtr funcPtr = GetProcAddress(handle, "greetingW");
        greetingW pfGreetingW = (greetingW)Marshal.GetDelegateForFunctionPointer(funcPtr, typeof(greetingW));
        IntPtr pName = Marshal.StringToHGlobalUni("ハロー©");
        IntPtr pResult = pfGreetingW(pName);
        string result = Marshal.PtrToStringUni(pResult);
        Util.Message(result);
        Marshal.FreeHGlobal(pName);
        FreeLibrary(handle);
    }
    [STAThread]
    static void Main()
    {
        Console.WriteLine("Hello World!");
        test_MessageBox();
        test_qtlib();
    }
    [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern IntPtr LoadLibraryW(string lpFileName);
    [DllImport("kernel32", SetLastError = true)]
    internal static extern bool FreeLibrary(IntPtr hModule);
    [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = false)]
    internal static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
}