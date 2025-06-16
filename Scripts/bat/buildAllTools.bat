@echo off
cd ../../

call python ./RUN_THIS.py
call dotnet build -c Tools

pause
