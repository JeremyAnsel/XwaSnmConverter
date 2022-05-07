@echo off
setlocal

cd "%~dp0"

For %%a in (
"XwaSnmConverter\bin\Release\net48\*.dll"
"XwaSnmConverter\bin\Release\net48\*.exe"
"XwaSnmConverter\bin\Release\net48\*.config"
) do (
xcopy /s /d "%%~a" dist\
)
