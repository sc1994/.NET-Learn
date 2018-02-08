setlocal

cd /d %~dp0

set TOOLS_PATH=tools\Grpc.Tools.1.8.0\tools\windows_x64
set DOC_TOOL_PATH=tools\doc
set CODE_PATH=..\GRPC.CoreDemo
set PROTO_PATH=protos\hello.proto

%TOOLS_PATH%\protoc.exe -I protos --csharp_out %CODE_PATH%  %PROTO_PATH% --grpc_out %CODE_PATH% --plugin=protoc-gen-grpc=%TOOLS_PATH%\grpc_csharp_plugin.exe
%TOOLS_PATH%\protoc.exe --plugin=protoc-gen-doc=%DOC_TOOL_PATH%\protoc-gen-doc.exe --doc_out=markdown,document.md:./protos/ %PROTO_PATH%

pause
endlocal