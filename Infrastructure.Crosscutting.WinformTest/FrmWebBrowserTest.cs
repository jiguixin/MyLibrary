using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms; 

namespace Infrastructure.Crosscutting.WinformTest
{
    /// <summary>
    /// 该界面可以在用记登录成功后在通过HttpWebRequest实现请求的数据。
    /// </summary>
    public partial class FrmWebBrowserTest : Form
    {
        public FrmWebBrowserTest()
        {
            InitializeComponent();
        }
         
        private void Navigate(String address)
        {
            if (String.IsNullOrEmpty(address)) return;
            if (address.Equals("about:blank")) return;
            if (!address.StartsWith("http://") &&
                !address.StartsWith("https://"))
            {
                address = "http://" + address;
            }
            try
            {
                webBrowser1.Navigate(new Uri(address));
            }
            catch (System.UriFormatException)
            {
                return;
            }
        }

        // Updates the URL in TextBoxAddress upon navigation.
        private void webBrowser1_Navigated(object sender,
            WebBrowserNavigatedEventArgs e)
        {
            textBox1.Text = webBrowser1.Url.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Navigate(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //string s = webBrowser1.Document.Cookie;

           string s =  HttpHelps.GetCookieString("https://lab.alipay.com/user/myAccount/index.htm");

            HttpHelps h = new HttpHelps();
            string response = h.GetHttpRequestStringByNUll_GetBycookie("https://lab.alipay.com/user/myAccount/index.htm", Encoding.Default,
                                                     s);
     

            webBrowser1.DocumentText = response;
        } 
    } 
}
