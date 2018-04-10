>### 基本命令
- `docker pull [imagename]`(获取远程镜像)
- `docker images` (显示全部本地images)
- `docker ps -a`(显示本地container)
- `docker rm [ID]` (移除本地container)
- `docker stop [ID]` (停止容器)
- `docker rmi [ID]` (移除本地image)
- `docker build -t [Image] .`(使用本地的Dockerfile构建自己的image, 别忘记结束的那个点" . ")
- `docker run -d -p [port]:[port] [ image]`  (-d 后台运行返回容器Id， -p指定端口映射到宿主的端口 )
- `docker tag [old] [new]` (为image 签名, 签名的内容是: hubdockerId/imageName)
- `docker push [image]` (image 必须为签名后的imageName)

- docker exec -it [ID] bash  (使用正在运行的容器的命令行)

>### Dockerfile
- 选用远程镜像
`FROM [远程镜像]`
- 复制本地文件到镜像中 
`COPY [复制的本地路径]  [被复制image路径]`    
- 工作目录
`WORKDIR /app`
- 项目启动之前可以运行一些命令
`RUN [xxx xxx-xxx]`
- 项目入口
`ENTRYPOINT ["dotnet", "[程序入口文件]"]`

参考

- docker 命令大全 
  - http://www.runoob.com/docker/docker-command-manual.html
- Dockerfile 语法详解
  - https://www.jianshu.com/p/690844302df5
- ENTRYPOINT 具体用法
  - todo
