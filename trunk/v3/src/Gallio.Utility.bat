@echo off
REM Runs Gallio.Utility from within the source tree.
REM Can be run with Mono if the MONO variable is set to the Mono VM path (mono.exe).
%MONO% "%~dp0Gallio\Gallio.Utility\bin\Gallio.Utility.exe" %*