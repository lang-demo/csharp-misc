#! /usr/bin/env bash
set -uvx
set -e
cp -rp ../global/*.dll .
csc Program.cs Global.*.cs -r:Global.dll -r:Newtonsoft.Json.dll
./Program.exe
