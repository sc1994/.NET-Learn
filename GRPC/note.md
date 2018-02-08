# GRPC (.NET Core)
> ### *基本结构* 
- GRPC.Core
```
// 依赖关系
{
    "Google.Protobuf",
    "Google.Protobuf.Tools",
    "Grpc",
    "Grpc.Tools"
}
```
- GRPC.Server
```
{
    "GRPC.Core"
}
```
- CRPC.Client
```
{
    "GRPC.Core"
}
```
> ### *proto (接口原型)* 
- 以 proto 为后缀的文件
- package
- 包含接口和参数
- 一个.proto 文件大致长这样
```
syntax = "proto3";

package Grpc.Core; // 接口所属的命名空间(namespace)
/////////////////////////////////ThingsToDo类开始/////////////////////////////////
// 注释1
service ThingsToDo { // 接口类
        // 接口   // 请求参数           // 响应参数
	rpc SayHello (HelloRequest) returns (HelloReply) {}
}
// 注释2
message HelloRequest { // 请求参数对象
	string name = 1;
}
// 注释3
message HelloReply { // 响应参数对象
    // 属性注释
	string message = 1;
}
/////////////////////////////////ThingsToDo类结束/////////////////////////////////
```
> ### Build
- 依赖 proto 生产 接口的基本代码
- 利用批处理(bat)执行这些命令
- 一个.bat 文件大致长这样
```zsh
@rem 复制时,删除全部注释
@rem 特别留意变量结束后不能含有空格
setlocal

@rem enter this directory
cd /d %~dp0

set TOOLS_PATH=tools\Grpc.Tools.1.8.0\tools\windows_x64 @rem 代码工具位置
set DOC_TOOL_PATH=tools\doc @rem 日志工具位置
set CODE_PATH=..\GRPC.CoreDemo @rem 代码代输出位置
set PROTO_PATH=protos/hello.proto @rem 接口原型文件(.proto)位置

@rem 代码输出
%TOOLS_PATH%\protoc.exe -I protos --csharp_out %CODE_PATH%  %PROTO_PATH% --grpc_out %CODE_PATH% --plugin=protoc-gen-grpc=%TOOLS_PATH%\grpc_csharp_plugin.exe
@rem 日志输出(日志和接口原型文件在同一目录下)
%TOOLS_PATH%\protoc.exe --plugin=protoc-gen-doc=%DOC_TOOL_PATH%\protoc-gen-doc.exe --doc_out=markdown,document.md:./protos/ %PROTO_PATH%

pause @rem 结束之后不立即关闭窗口, 发生异常可以排查
endlocal 
```
> ### 实现接口(ThingsToDo)的虚方法
```Csharp
class ThingsToDoImpl : ThingsToDo.ThingsToDoBase // 接口类
{
    // Server side handler of the SayHello RPC
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply {Message = "Hello " + request.Name});
    }
}
```
> ### Server
```
var server = new Server
{
    Services = 
    { 
        ThingsToDo.BindService(new ThingsToDoImpl()) // 绑定接口和他的实现
    },
    Ports = 
    { 
        new ServerPort(
        "localhost", // ip
        Port, // 端口
        ServerCredentials.Insecure) 
    }
};
server.Start(); // 启动服务

Console.WriteLine("ThingsToDo server listening on port " + Port);
Console.WriteLine("Press any key to stop the server...");
Console.ReadKey();

server.ShutdownAsync().Wait();
```
> ### Client
```
// 初始化连接通道
var channel = new Channel("127.0.0.1", 50051, ChannelCredentials.Insecure);
// 初始化客户端
var client = new ThingsToDo.ThingsToDoClient(channel);
// 请求参数
var request  = new HelloRequest 
{
  Name = "youself"
};
// 发送请求,获得响应值
var reply = client.SayHello(request);
// 打印...
Console.WriteLine("ThingsToDo: " + reply.Message);
channel.ShutdownAsync().Wait();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
```











