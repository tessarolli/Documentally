@echo off
setlocal EnableDelayedExpansion

if "%~1" == "" (
  echo Please provide the plain text connection string as a parameter.
  exit /b 1
)

set plainText=%~1

echo Encrypting connection string:
echo %plainText%

echo %plainText% > plain.txt
certutil -encode plain.txt encoded.txt > nul 2>&1
set /p cipherText=<encoded.txt

echo Encrypted string:
echo %cipherText%

del plain.txt

endlocal & set "cipherText=%cipherText%"
exit /b 0
