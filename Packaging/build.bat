ECHO off

SET /P APPVEYOR_BUILD_NUMBER=Please enter a build number: 
SET /P PACKAGE_VERSION=Please enter your package version: 
SET /P UMBRACO_PACKAGE_PRERELEASE_SUFFIX=Please enter your package release suffix or leave empty: 
SET /P APPVEYOR_REPO_TAG=If you want to simulate a GitHub tag for a release: 

if "%APPVEYOR_BUILD_NUMBER%" == "" (
  SET APPVEYOR_BUILD_NUMBER=1
)

if "%PACKAGE_VERSION%" == "" (
  SET PACKAGE_VERSION=1.0.0
)

SET APPVEYOR_BUILD_VERSION=%PACKAGE_VERSION%.%APPVEYOR_BUILD_NUMBER%

build-appveyor.bat

@IF %ERRORLEVEL% NEQ 0 GOTO err
@EXIT /B 0
:err
@PAUSE
@EXIT /B 1