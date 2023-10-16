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
using WpfPro;

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
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("https://opentdb.com/api_category.php");
                JObject categories = JObject.Parse(json);
                for (int i = 0; i < categories.Count; i++)
                {
                    Category.Items.Add(categories.GetValue("trivia_categories")["name"].ToString());
                }
                //Kell.Text = categories.ToString();
            }
        }

        public void SaveToJson(int amount, int cata, string diff, string type)
        {


            using (WebClient wc = new WebClient())
            {
                
                var json = wc.DownloadString($"https://opentdb.com/api.php?amount={amount}&category={cata}&difficulty={diff}&type={type}&encode=base64");
                JObject q = JObject.Parse(json);
                for (int i = 0; i < 10; i++)
                {
                    Questions quest = new Questions();
                    quest.question = Decode(q, "question", i);
                    quest.correctAnswer = Decode(q, "correct_answer", i);
                    for (int f = 0; f < 3; f++)
                    {
                        quest.incorrectAnswer[f] = Decode(q, "incorrect_answers", i, true, f);
                    }
                    AllData.AllQuest.Add(quest);
                }
            }
        }

        public void GEt(object o, RoutedEventArgs s)
        {

            SaveToJson(10, 15, "medium", "multiple");
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("https://opentdb.com/api_category.php");
                Kell.Text = json;
            }
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
    public class Questions
    {
        //public static string category { get; set; }
        //public static string type { get; set; }
        //public static string difficulty { get; set; }
        public string question { get; set; }
        public string correctAnswer { get; set; }
        public string[] incorrectAnswer = new string[3];
       
    }
    public static class AllData
    {
        public static List<Questions> AllQuest { get; set; } = new List<Questions>();
    }
}
