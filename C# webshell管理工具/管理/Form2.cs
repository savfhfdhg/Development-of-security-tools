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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string url = textBox4.Text;
            string passwd = textBox3.Text;
            string php = comboBox1.Text;
            Form1 lForm1 = (Form1)this.Owner;//把Form2的父窗口指针赋给lForm1
            lForm1.StrValue = url;//使用父窗口指针赋值
            lForm1.StrPwd = passwd;
            lForm1.Strphp = php;
            this.Close();
        }
    }
}
