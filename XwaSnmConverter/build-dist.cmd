@echo off
setlocal

cd "%~dp0"

For %%a in (
"XwaSnmConverter\bin\Release\*.dll"
"XwaSnmConverter\bin\Release\*.exe"
"XwaSnmConverter\bin\Release\*.config"
) do (
xcopy /s /d "%%~a" dist\
)
