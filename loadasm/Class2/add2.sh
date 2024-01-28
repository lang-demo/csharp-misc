#! /usr/bin/env bash
set -uvx
set -e
rm -rf add2.dll
#cl  -LD -clr add2.cpp
g++ -shared -o add2.dll add2.cpp -static
#racket ./add2.rkt
