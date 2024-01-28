#! /usr/bin/env bash
set -uvx
set -e
csc -out:Global.dll -t:library Global.*.cs -r:Newtonsoft.Json.dll
