IF [%1] == [] GOTO error
@Echo Off
dotnet pack -o _build --version-suffix %1
EXIT /B
:error
Echo version required.

