#! /usr/bin/env bash
set -uvx
set -e
csc Program.cs Global.*.cs -r:Newtonsoft.Json.dll
./Program.exe
