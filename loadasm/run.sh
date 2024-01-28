#! /usr/bin/env bash
set -uvx
set -e
cwd=`pwd`
cd $cwd/Class2
g++ -shared -o add2.dll add2.cpp -static
csc -target:library Dep.cs
csc -target:library -r:Dep.dll Class2.cs
cd $cwd
csc MainClass.cs
./MainClass.exe
