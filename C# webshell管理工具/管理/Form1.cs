using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 管理
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        //http链接
        private string strValue;
        //密码
        private string strPwd;
        //类型
        private string strphp;

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
        public string Strphp
        {
            set
            {
                strphp = value;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void 添加数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Form2 lForm = new Form2();
            lForm.Owner = this;//重要的一步，主要是使Form2的Owner指针指向Form1
            lForm.ShowDialog();
            //MessageBox.Show(strValue);//显示返回的值
            ListViewItem item = new ListViewItem();
            item.SubItems[0].Text = strValue;
            item.SubItems.Add(strPwd);
            item.SubItems.Add(strphp);
            listView1.Items.Add(item);

        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            //文件没有被选中
            if (this.listView1.SelectedItems.Count == 0)
            {
                return;
            }
            //获取选中文件
            var selectedItem = this.listView1.SelectedItems[0];
            
            Form3 lForm = new Form3();
            lForm.Owner = this;//重要的一步，主要是使Form3的Owner指针指向Form1
            lForm.StrValue = selectedItem.SubItems[0].Text; ;//使用父窗口指针赋值
            lForm.StrPwd = selectedItem.SubItems[1].Text;
            lForm.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void 删除数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //文件没有被选中
            if (this.listView1.SelectedItems.Count == 0)
            {
                return;
            }
           

            foreach (ListViewItem item in listView1.SelectedItems)
            {
                this.listView1.Items.Remove(item);
            }
        }
    }
}
