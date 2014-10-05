@echo ON

SET MSBUILD="..\tools\MSBuild\MSBuild.exe"

%MSBUILD% build.proj /property:Configuration=Release