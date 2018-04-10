# Docker (ASP.NET Core) 

> ####  基本命令

- docker images (显示全部本地images)
- docker ps -a(显示本地container)
- docker rm [ID] (移除本地container)
- docker stop [ID] (停止容器)
- docker rmi [ID] (移除本地image)
- docker build -t [Image] .      (使用本地的Dockerfile构建自己的image, 别忘记结束的那个点" . ")
- docker run -t -i -p \[port\]:\[port\] [ image]  (-t使用当前目录下的Dockerfile配置运行  -i设置为隐式运行  -p指定端口映射到宿主的端口 )
- docker tag \[old\] \[new\] (为image 签名, 签名的内容是: hubdockerId/imageName)
- docker push [image] (image 必须为签名后的imageName)

---

> #### Dockerfile

```
FROM microsoft/aspnetcore:2.0.5
	 // 项目的打包位置
COPY bin/Release/PublishOutput  /app/    

WORKDIR /app
					  // 项目入口
ENTRYPOINT ["dotnet", "DockerDemo1.dll"]
```

---

> #### 参考
- docker 命令大全 
  - http://www.runoob.com/docker/docker-command-manual.html
- Dockerfile 语法详解
  - https://www.jianshu.com/p/690844302df5
- ​

