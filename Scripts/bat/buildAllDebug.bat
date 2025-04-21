@echo off
cd ../../

call git submodule update --init --recursive
call dotnet msbuild -p:Configuration=Debug -verbosity:minimal -consoleloggerparameters:Summary -p:NoWarn=CS0618

pause
