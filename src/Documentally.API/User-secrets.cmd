@echo off
setlocal enabledelayedexpansion

dotnet user-secrets clear

:: -------------- JwtSettings --------------------------

:: Decrypt Secret Key
set "cipherText=bXlfc3VwZXJfc2VjcmV0X2tleSANCg=="
echo !cipherText! > encoded.txt
certutil -decode -f encoded.txt plain.txt >nul 2>&1
set /p plainText=<plain.txt
del plain.txt encoded.txt

:: Use decrypted string as parameter for the command
dotnet user-secrets set "JwtSettings:Secret" "!plainText!"

:: --------- PostgresSqlConnectionString ---------------

:: Decrypt Connection String
set "cipherText=U2VydmVyPWJhYmFyLmRiLmVsZXBoYW50c3FsLmNvbTtQb3J0PTU0MzI7RGF0YWJhc2U9Ynd2dW12Z2w7VXNlciBJZD1id3Z1bXZnbDtQYXNzd29yZD1BOWNqV2pYYWlmN1poNGNrcDRIV2k0VllXVndHRGNnODs="
echo !cipherText! > encoded.txt
certutil -decode -f encoded.txt plain.txt >nul 2>&1
set /p plainText=<plain.txt
del plain.txt encoded.txt

:: Use decrypted string as parameter for the command
dotnet user-secrets set "PostgresSqlConnectionString" "!plainText!"

:: -------------- AzureBlobsService ---------------------

:: Decrypt Connection String
set "cipherText=RGVmYXVsdEVuZHBvaW50c1Byb3RvY29sPWh0dHBzO0FjY291bnROYW1lPWRvY3VtZW50YWxseTtBY2NvdW50S2V5PUxlVGtnTFhBdGpiSkNyWTZxVGhlVEszUVc1U2xrUGtUaE5ac0cxQkI1THdKbUs0MndIQ1kzVWNKTyt5ckovQUFUNzNoUXdhc0JEVlYrQVN0Q0xTKzhBPT07IA0K"
echo !cipherText! > encoded.txt
certutil -decode -f encoded.txt plain.txt >nul 2>&1
set /p plainText=<plain.txt
del plain.txt encoded.txt

:: Use decrypted string as parameter for the command
dotnet user-secrets set "AzureBlobStorageConnectionString" "!plainText!"

endlocal