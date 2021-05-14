## 使用Golang编写的mysql弱口令扫描工具

### 1) 使用帮助

    mysql.exe -h   
    Usage of mysql.exe:
    -Password string
        输入需要爆破的密码 (default "pass.txt")
    -Username string
        输入需要爆破的用户名 (default "root")
    -u string
        输入需要爆破的ip (default "192.168.1.1")
     
### 2) 使用方法
    mysql.exe -u 192.168.2.1 -Username root -Password pass.txt
    mysql.exe -u 192.168.2.1-25 -Username root -Password pass.txt
    mysql.exe -u 192.168.2.0/24 -Username root -Password pass.txt
    端口没有开放，停止爆破mysql 192.168.2.1
    端口没有开放，停止爆破mysql 192.168.2.1
    程序执行耗时(s)： 2.0179336
### 2) 程序功能
    1.支持扫描整个ip段的mysql爆破
    2.写端口扫描功能，扫描是否开启3306，不开启程序就退出
    3.使用sync.WaitGroup
    4.实现命令行下操作
    5.此程序只是练习Golang写出来的，性能一般，但是用来扫内网3306能胜任
