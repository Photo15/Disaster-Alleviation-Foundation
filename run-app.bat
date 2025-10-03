@echo off
echo Starting Disaster Alleviation Foundation Web Application...
echo.

REM Check if .NET is installed
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo .NET SDK is not installed or not in PATH.
    echo Please install .NET 8.0 SDK from: https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

REM Apply database migrations
echo Applying database migrations...
dotnet ef database update

REM Run the application
echo Starting the web application...
dotnet run

pause