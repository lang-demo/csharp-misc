using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ApiTest01
{
    public class Class1
    {
        [DllExport]
        public static int add2(int a, int b)
        {
            return a + b;
        }
        static ThreadLocal<IntPtr> helloPtr = new ThreadLocal<IntPtr> ();
        [DllExport]
        public static IntPtr hello()
        {
            if (helloPtr.Value != IntPtr.Zero)
            {
                Console.WriteLine("Freeing helloPtr...");
                Marshal.FreeHGlobal(helloPtr.Value);
                helloPtr.Value = IntPtr.Zero;
            }
            string x = "hello ハロー©";
            helloPtr.Value = Marshal.StringToHGlobalUni(x);
            return helloPtr.Value;
        }
        [DllExport]
        public static void puts(IntPtr ws)
        {


            var s = Marshal.PtrToStringUni(ws);
            Console.WriteLine($"Hello {s}");
        }
    }
}
