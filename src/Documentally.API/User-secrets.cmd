@echo off
setlocal enabledelayedexpansion

:: Decrypt Secret Key
set "cipherText=bXlfc3VwZXJfc2VjcmV0X2tleSANCg=="
echo !cipherText! > encoded.txt
certutil -decode -f encoded.txt plain.txt >nul 2>&1
set /p plainText=<plain.txt
del plain.txt encoded.txt

:: Use decrypted string as parameter for the command
dotnet user-secrets set "JwtSettings:Secret" "!plainText!"

:: Decrypt Connection String
set "cipherText=U2VydmVyPWJhYmFyLmRiLmVsZXBoYW50c3FsLmNvbTtQb3J0PTU0MzI7RGF0YWJhc2U9Ynd2dW12Z2w7VXNlciBJZD1id3Z1bXZnbDtQYXNzd29yZD1BOWNqV2pYYWlmN1poNGNrcDRIV2k0VllXVndHRGNnODs="
echo !cipherText! > encoded.txt
certutil -decode -f encoded.txt plain.txt >nul 2>&1
set /p plainText=<plain.txt
del plain.txt encoded.txt

:: Use decrypted string as parameter for the command
dotnet user-secrets set "PostgresSqlConnectionString" "!plainText!"

endlocal