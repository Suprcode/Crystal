@echo off
rd /s/q ..\Build\Server\Data
rd /s/q ..\Build\Server\Envir
del ..\Build\Server\Server.MirDB
pause
xcopy ..\..\mir2-database\Wym\Data ..\Build\Server\Data /s/e/i/y
xcopy ..\..\mir2-database\Wym\Envir ..\Build\Server\Envir /s/e/i/y
copy ..\..\mir2-database\Wym\Server.MirDB ..\Build\Server\Server.MirDB
pause