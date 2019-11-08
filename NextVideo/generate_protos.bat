
@rem Generate the C# code for .proto files
setlocal

@rem change the dir
cd /d %~dp0

set TOOLS_PATH=packages\Grpc.Tools.2.24.0\tools\windows_x64

%TOOLS_PATH%\protoc.exe NextVideo\protos\nextgenvideosvc.proto --csharp_out NextVideo

%TOOLS_PATH%\protoc.exe NextVideo\protos\nextgenvideosvc.proto --grpc_out NextVideo --plugin=protoc-gen-grpc=%TOOLS_PATH%\grpc_csharp_plugin.exe

endlocal