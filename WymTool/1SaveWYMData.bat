@echo off
rd /s/q ..\..\mir2-database\Wym\Data
rd /s/q ..\..\mir2-database\Wym\Envir
del ..\..\mir2-database\Wym\Server.MirDB
pause
xcopy ..\Build\Server\Data ..\..\mir2-database\Wym\Data /s/e/i/y
xcopy ..\Build\Server\Envir ..\..\mir2-database\Wym\Envir /s/e/i/y
copy ..\Build\Server\Server.MirDB ..\..\mir2-database\Wym\Server.MirDB
pause