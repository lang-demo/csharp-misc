﻿using System;
using Global;

namespace json;
static class Program
{
    [STAThread]
    static void Main()
    {
        Util.Print(1 + 2, "1+2");
        Util.Print(new { a = 123, b = "hello" });
    }
}
