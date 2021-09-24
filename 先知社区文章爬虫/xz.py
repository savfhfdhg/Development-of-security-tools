from bs4 import BeautifulSoup
from multiprocessing import Pool
import requests
import random
import chardet
import re,os
import time
from queue import Queue
from threading import Thread



#随机UA
USER_AGENTS = [
 "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; AcooBrowser; .NET CLR 1.1.4322; .NET CLR 2.0.50727)",
 "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; Acoo Browser; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; .NET CLR 3.0.04506)",
 "Mozilla/4.0 (compatible; MSIE 7.0; AOL 9.5; AOLBuild 4337.35; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)",
 "Mozilla/5.0 (Windows; U; MSIE 9.0; Windows NT 9.0; en-US)",
 "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Win64; x64; Trident/5.0; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET CLR 2.0.50727; Media Center PC 6.0)",
 "Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET CLR 1.0.3705; .NET CLR 1.1.4322)",
 "Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727; InfoPath.2; .NET CLR 3.0.04506.30)",
 "Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN) AppleWebKit/523.15 (KHTML, like Gecko, Safari/419.3) Arora/0.3 (Change: 287 c9dfb30)",
 "Mozilla/5.0 (X11; U; Linux; en-US) AppleWebKit/527+ (KHTML, like Gecko, Safari/419.3) Arora/0.6",
 "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.2pre) Gecko/20070215 K-Ninja/2.1.1",
 "Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN; rv:1.9) Gecko/20080705 Firefox/3.0 Kapiko/3.0",
 "Mozilla/5.0 (X11; Linux i686; U;) Gecko/20070322 Kazehakase/0.4.5",
 "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.0.8) Gecko Fedora/1.9.0.8-1.fc10 Kazehakase/0.5.6",
 "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11",
 "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_3) AppleWebKit/535.20 (KHTML, like Gecko) Chrome/19.0.1036.7 Safari/535.20",
 "Opera/9.80 (Macintosh; Intel Mac OS X 10.6.8; U; fr) Presto/2.9.168 Version/11.52",
]
UserAgent=random.choice(USER_AGENTS)
headers = {'User-Agent': UserAgent,
"Host":"xz.aliyun.com",
"Referer":"https://xz.aliyun.com/",
"Cookie":"_uab_collina=163240159682135816453893; UM_distinctid=17b536f17987f3-0eb9d88546162e-4343363-144000-17b536f1799af2; _alicloud_ab_trace_id=b24549a0-1c41-11ec-8a89-b1e1ffa65c9c; cna=ye2bGXnmjjUCAWo66oQAT251; aliyun_country=CN; aliyun_site=CN; aliyun_lang=zh; csrftoken=3Ou4OmYTgvYXjCw4YJDWuAWxrFOMWfMTAy31dKZSinwV66bZPaIHeZIVhIOA7uHe; isg=BPf3sst2WoLtB94OQYebbABEhutBvMsexdmSRkmkKEYt-BY6UY_ebATR3limFKOW; l=eBTgaSvHgRIrW4c_BOfChurza77TvIRbmuPzaNbMiOCP9TjW54scW6FKy5JXCnGVnsMMR3kQee-kBP8aEPU6gxv9-e_gfMoxndLh.; tfstk=c04RB_YIgtXlxGGY_uIcO0ftPNeCacA-KQMpJsEVuDC52OLwSsqYSPU2kgGeeqCA.; acw_tc=76b20ffd16324650840307160e7658b48ab95ddc523b8d422b3c22b5b1228b; acw_sc__v2=614d70bc8a60d4832a634d31478bc9f9635be145; CNZZDATA1260716569=1841961423-1629188863-https%253A%252F%252Fxz.aliyun.com%252F%7C1632462225",
}
title = []

#把图片的源地址指向本地
def format_pic_url(old_url):
    old_host = 'https://xzfile.aliyuncs.com/media/upload/picture'
    new_host = '../picture'
    return old_url.replace(old_host,new_host)

#获取图片地址
def getImgUlr(resp_text):
    re_rule = re.compile(r'<img src="https://xzfile.aliyuncs.com.*?">')
    result = []
    for i in re_rule.findall(resp_text):
        result.append(i.replace('<img src="','').replace('">',''))
    return result

#下载图片
def download_img(img_url):
    response = requests.get(img_url)
    img = response.content
    local_img = format_pic_url(img_url).replace('..','.')
    try:
        if not os.path.exists(local_img):
            with open(local_img, 'wb') as f:
                f.write(img)
        else:
            pass
    except Exception as e:
        pass
        

#写文件
def writeFile(target_file,STR):
    if not os.path.exists(target_file):
        with open(target_file, 'w',encoding='utf-8') as f:
            f.write(STR)
    else:
        pass

#获取内容html
def get_content(url):
    
    urls = "https://xz.aliyun.com/t/%s"%url
    #print(urls)
    html = requests.get(url=urls,headers=headers)
    tit = re.findall('<title>(.*)</title>', html.text)[0]
    content = html.text
    for img_url in getImgUlr(content):
        download_img(img_url)
            
    #保存文章
    t_path = './essay/{}.html'.format(filename_filter(tit))
    print("正在保存文件",urls,t_path)
    resp_text = format_pic_url(content)
    writeFile(t_path,resp_text)


#过滤掉部分可能导致文件创建异常的字符
def filename_filter(filename):  
    string1="\/:*?\"<>|"
    for s1 in string1:
        filename= filename.replace(s1," ")
    return(filename)
#获取文章id 并保存
def get_page(url):
    html = requests.get(url,headers=headers)
    soup = BeautifulSoup(html.text,"html.parser")
    a = soup.findAll("p",class_="topic-summary")
    id = re.findall('href="/t/(.*)">', str(a))
    id = '\n'.join(id)
    with open("tid.txt","a+") as f:
        f.write(id)
        f.write("\n")
        f.close()

#多线程类
class ThreadWorker(Thread):
    def __init__(self, queue):
        Thread.__init__(self)
        self.queue = queue

    def run(self):
        while True:
            url = self.queue.get()
            get_content(url)
            self.queue.task_done()


if __name__=="__main__":
    # 创建队列
    queue = Queue()

    for i in range(30):
        worker = ThreadWorker(queue)
        worker.daemon = True  # 如果工作线程在等待更多的任务时阻塞了，主线程也可以正常退出
        worker.start()  # 启动线程
    #打开文章id 文件
    with open("i.txt","r") as f:
        for id in f.readlines():
            queue.put(id.strip())

    queue.join()