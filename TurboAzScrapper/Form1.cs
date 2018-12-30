using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace TurboAzScrapper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            int count = 1;
            //int queueCount = 0;
            List<String> Data=new List<string>();
            StreamWriter file = File.CreateText("C:/Users/naibt/OneDrive/Desktop/TurboAZData.txt");

            for (int i = 1; i < 900; i++)
            {
                try
                {
                    string url = "https://turbo.az/autos?page="+i;
                    label1.Text = i.ToString();
                    HttpClient http = new HttpClient();
                    var html = await http.GetStringAsync(url);
                    var htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(html);
                    var ProductLinkList = htmlDocument.DocumentNode.Descendants("a").
                         Where(node => node.GetAttributeValue("class", "").Equals("products-i-link"));

                    
                    foreach (var item in ProductLinkList)
                    {
                        //richTextBox1.Text += item.Attributes[2].Value.ToString()+ "\n";
                        var htmlCurrentProduct = await http.GetStringAsync("https://turbo.az" + item.Attributes[2].Value.ToString());
                        var htmlProductDocument = new HtmlDocument();
                        htmlProductDocument.LoadHtml(htmlCurrentProduct);

                        var userDetail = htmlProductDocument.DocumentNode.Descendants("div").
                            Where(node => node.GetAttributeValue("class", "").Equals("seller-contacts"));
                        foreach (var item3 in userDetail)
                        {
                            richTextBox1.Text += $"NUM  :{count}--  " + item3.InnerText + "\n";
                            //Data.Add($"NUM  :{count}--  " + item3.InnerText);
                           await file.WriteLineAsync($"NUM  :{count}--  " + item3.InnerText + "\n");
                            count++;
                           // queueCount = 0;
                        }

                    };
                }
                catch
                {
                    continue;
                }
            }
            file.Close();
            //foreach (var UserItem in list)
            //{
            //    foreach (var item3 in UserItem)
            //    {
            //        richTextBox1.Text += $"NUM  :{count}--  " + item3.InnerText + "\n";
            //        Data.Add($"NUM  :{count}--  " + item3.InnerText);
            //        count++;
            //        queueCount = 0;
            //    }

            //}

            //foreach (var item in Data.Distinct())
            //{
            //    richTextBox2.Text += item + "\n";
            //}
           

        }
    }
}
