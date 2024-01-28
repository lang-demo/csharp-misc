#! /usr/bin/env bash
set -uvx
set -e
cp -rp ../global/*.dll .
csc Program.cs -r:Global.dll -r:Newtonsoft.Json.dll
./Program.exe
