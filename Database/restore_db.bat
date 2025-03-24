@echo off
SETLOCAL ENABLEDELAYEDEXPANSION

:: Database credentials
SET MYSQL_USER=root
SET MYSQL_PASSWORD=Admin@123456789
SET DATABASE_NAME=blog

:: Check if MySQL is accessible
mysql -u %MYSQL_USER% -p%MYSQL_PASSWORD% -e "exit" >nul 2>&1
IF %ERRORLEVEL% NEQ 0 (
    echo MySQL is not accessible or password is incorrect.
    pause
    exit /b
)

:: Check if the database exists
echo Checking if database '%DATABASE_NAME%' exists...
mysql -u %MYSQL_USER% -p%MYSQL_PASSWORD% -e "SHOW DATABASES LIKE '%DATABASE_NAME%';" | findstr /C:"%DATABASE_NAME%" >nul
IF %ERRORLEVEL% EQU 0 (
    echo Database '%DATABASE_NAME%' exists. Dropping it...
    mysql -u %MYSQL_USER% -p%MYSQL_PASSWORD% -e "DROP DATABASE %DATABASE_NAME%;"
) ELSE (
    echo Database '%DATABASE_NAME%' does not exist.
)

:: Create a new database
echo Creating new database '%DATABASE_NAME%'...
mysql -u %MYSQL_USER% -p%MYSQL_PASSWORD% -e "CREATE DATABASE %DATABASE_NAME%;"

:: Restore database from SQL file
echo Restoring database from 'blog.sql'...
mysql -u %MYSQL_USER% -p%MYSQL_PASSWORD% %DATABASE_NAME% < E:\GitRepository\WP_Dotnet_Govind\wp_dotnet\Database\blog.sql

echo Database restoration completed successfully!
pause
