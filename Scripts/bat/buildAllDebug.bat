@echo off
cd ../../

call git submodule update --init --recursive
call dotnet msbuild -p:Configuration=Debug -verbosity:minimal -consoleloggerparameters:Summary

pause
