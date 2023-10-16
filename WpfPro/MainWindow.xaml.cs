using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;
using System.Web;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.X509Certificates;

namespace WpfPro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void SaveToJson(TextBlock KEll)
        {
            string filePath = "Data.json";


            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString($"https://opentdb.com/api.php?amount=10&category=15&difficulty=medium&type=multiple&encode=base64");
                JObject q = JObject.Parse(json);
                for (int i = 0; i < 10; i++)
                {
                    q.GetValue("results")[i]["category"] = Decode(q, "category", i);
                    q.GetValue("results")[i]["type"] = Decode(q, "type", i);
                    q.GetValue("results")[i]["difficulty"] = Decode(q, "difficulty", i);
                    q.GetValue("results")[i]["question"] = Decode(q, "question", i);
                    q.GetValue("results")[i]["correct_answer"] = Decode(q, "correct_answer", i);
                    for (int f = 0; f < 3; f++)
                    {
                        q.GetValue("results")[i]["incorrect_answers"][f] = Decode(q, "incorrect_answers", i, true, f);
                    }
                    
                }
            }
        }

        public void GEt(object o, RoutedEventArgs s) { 
        
            SaveToJson(Kell);
        }
        private string Decode(JObject jobi, string cata, int index, bool liste = false, int listindex = 0) {

            string decodedString = "";
            if (!liste)
            {
                byte[] data = Convert.FromBase64String(jobi.GetValue("results")[index][cata].ToString());
                decodedString = System.Text.Encoding.UTF8.GetString(data);
            }
            else
            {
                byte[] data = Convert.FromBase64String(jobi.GetValue("results")[index][cata][listindex].ToString());
                decodedString = System.Text.Encoding.UTF8.GetString(data);
            }
            return decodedString;
        }

    }
    public static class Questions
    {

        public static JObject q { get; set; }
        
    }
}
