#!/usr/bin/env python3
# conding:utf-8

import re
import requests
import socket
import html
import urllib3
from requests.adapters import HTTPAdapter
from multiprocessing.dummy import Pool as ThreadPool

urllib3.disable_warnings()
headers = {
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) Chrome/52.0.2743",
    "Accept-Language": "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3",
    "Accept-Encoding": "gzip, deflate",
    "Connection": "close",
}
patterns = (
    '<meta[\s]*http-equiv[\s]*=[\s]*[\'"]refresh[\'"][\s]*content[\s]*=[\s]*',
    'window.location[\s]*=[\s]*[\'"](.*?)[\'"][\s]*;',
    'window.location.href[\s]*=[\s]*[\'"](.*?)[\'"][\s]*;',
    'window.location.replace[\s]*\([\'"](.*?)[\'"]\)[\s]*;',
    'window.navigate[\s]*\([\'"](.*?)[\'"]\)',
    'location.href[\s]*=[\s]*[\'"](.*?)[\'"]',
)

#编码问题。中文、UTF-8及Unicoded等转换。
def page_decode(url,html_content):
    raw_content = html_content
    try:
        html_content = raw_content.decode("utf-8")
    except UnicodeError:
        try:
            html_content = raw_content.decode("gbk")
        except UnicodeError:
            try:
                html_content = raw_content.decode("gb2312")
            except UnicodeError:
                try:
                    html_content = raw_content.decode("big5")
                except:
                    return "DecodeHtmlError"
    return html_content


#匹配网站标题的正则表达式的正确书写。
def match_title(content):
    title = re.findall("document\.title[\s]*=[\s]*['\"](.*?)['\"]",content,re.I)
    if title and len(title) >=1:
        return title[0]
    else:
        title = re.findall("<title.*?>(.*?)</title>",content,re.I)
        if title and len(title) >=1:
            return title[0]
        else:
            return False

#转换HTML实体化
def html_decoder(html_entries):
    try:
        hp = html.unescape(html_entries)
        return hp
    except Exception as e:
        return html_entries


def out_format(url,title):
    try:
        print(url,"",title)
    except UnicodeError:
        print(url,"UnicodeError")
    

#获取网页标题
def Get_title(url):
    origin = url
    if "://" not in url:
        url = "http://" + str(url).strip()
        url = url.rstrip("/") + "/"
    try:
        s = requests.Session()
        s.mount('http://', HTTPAdapter(max_retries=1))
        s.mount('https://', HTTPAdapter(max_retries=1))
        req = s.get(url, headers=headers, verify=False, allow_redirects=True, timeout=15)
        html_content = req.content
        req.close()
    except requests.ConnectionError:
        return out_format(origin, "ConnectError")
    except requests.Timeout:
        return out_format(origin, "RequestTimeout")
    except socket.timeout:
        return out_format(origin, "SocketTimeout")
    except requests.RequestException:
        return out_format(origin, "RequestException")
    except Exception as e:
        return out_format(origin, "OtherException")
    html_content = page_decode(url,html_content)
    if html_content:
        title = match_title(html_content)
    else:
        exit(0)
    try:
        if title:
            if re.findall("\$#\d{3,};",title):
                title = html_decoder(title)
            return out_format(origin,title)
    except Exception as e:
        return out_format(origin,"FirstTitleError")
    for pattern in patterns:
        jump = re.findall(pattern, html_content, re.I | re.M)
        if len(jump) == 1:
            if "://" in jump[0]:
                url = jump[0]
            else:
                url += jump[0]
            break
    try:
        s = requests.Session()
        s.mount('http://', HTTPAdapter(max_retries=1))
        s.mount('https://', HTTPAdapter(max_retries=1))
        req = s.get(url, headers=headers, verify=False, allow_redirects=True, timeout=15)
        html_content = req.content
        req.close()
    except requests.ConnectionError:
        return out_format(origin, "ConnectError")
    except requests.Timeout:
        return out_format(origin, "RequestTimeout")
    except socket.timeout:
        return out_format(origin, "SocketTimeout")
    except requests.RequestException:
        return out_format(origin, "RequestException")
    except Exception as e:
        return out_format(origin, "OtherException")
    html_content = page_decode(url,html_content)
    if html_content:
        title = match_title(html_content)
    else:
        exit(0)
    try:
        if title:
            if re.findall("\$#\d{3,};",title):
                title = html_decoder(title)
            return out_format(origin,"NoTitle")
    except Exception as e:
        return out_format(origin,"SecondTitleError")

if __name__=="__main__":
    urls = []
    thrad = 10
    with open(r'C:\Users\xian\Desktop\node\demo\1.txt',"r") as f:
        for url in f.readlines():
            urls.append(url.strip())
    try:
        pool = ThreadPool(thrad)
        pool.map(Get_title, urls)
        pool.close()
        pool.join()
    except KeyboardInterrupt:
        exit("[*] User abort")