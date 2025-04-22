@echo off
cd ../../

call git submodule update --init --recursive
call dotnet build --no-incremental -c Debug -v minimal -consoleloggerparameters:Summary /nowarn:CS0618,CS1998,CS0414,CS0672,CS0105,CS0612

pause
