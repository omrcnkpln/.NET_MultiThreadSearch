using Microsoft.VisualBasic.FileIO;
using MultiThreadSearch.WPF.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace MultiThreadSearch.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ICollection<CModel> Complaints = new List<CModel>();
        public static ICollection<MatchModel> Matches = new List<MatchModel>();
        public string Column;
        public int Thread;
        public int Threshold;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Column = columnCombo.Text;
            Thread = Convert.ToInt32(threadCombo.Text);
            Threshold = Convert.ToInt32(threshold.Text);

            if (Complaints.Count == 0)
            {
                ReadCsv();
                Compare();

                //Thread thread1 = new Thread(SomeMethod);
                //Thread thread1 = new Thread(Compare);
                //thread1.Start();
                
                //int j = 0;
                //while (j < 50)
                //{
                //    Thread thread2 = new Thread(Compare);
                //    thread1.Start();
                //}
            }
        }
        
        public void Compare()
        {
            int baseIndex = 0;
            int prencipalIndex = 0;

            if (Column == "Product")
            {
                foreach (CModel parentItem in Complaints)
                {
                    if (baseIndex == 10)
                    {
                        break;
                    }

                    var parentWords = parentItem.Product.Split(" ");

                    foreach (CModel childItem in Complaints)
                    {
                        if (prencipalIndex == 10)
                        {
                            break;
                        }

                        var childWords = childItem.Product.Split(" ");

                        if (parentItem.ComplaintId != childItem.ComplaintId)
                        {
                            int payda = childWords.Count();
                            string[] longWords = childWords;
                            string[] shortWords = parentWords;

                            if (parentWords.Count() > childWords.Count())
                            {
                                payda = parentWords.Count();
                            }

                            double matchCount = 0.0;

                            foreach (var parentWord in longWords)
                            {
                                foreach (var childWord in shortWords)
                                {
                                    if (childWord == parentWord)
                                    {
                                        matchCount++;
                                    }
                                }
                            }

                            double oran = ((matchCount / payda) * 100);

                            if (oran >= Threshold)
                            {
                                Matches.Add(new MatchModel()
                                {
                                    Product1 = parentItem.Product,
                                    Issue1 = parentItem.Issue,
                                    Company1 = parentItem.Company,
                                    State1 = parentItem.State,
                                    ComplaintId1 = parentItem.ComplaintId,

                                    Product2 = childItem.Product,
                                    Issue2 = childItem.Issue,
                                    Company2 = childItem.Company,
                                    State2 = childItem.State,
                                    ComplaintId2 = childItem.ComplaintId,
                                });
                            }
                        }

                        prencipalIndex++;
                    }

                    baseIndex++;
                }
            }

            //if (Column == 1)
            //{
            //    foreach (CModel model in Complaints)
            //    {
            //        aaa.Add(model.Issue);
            //    }
            //}

            //if (Column == 2)
            //{
            //    foreach (CModel model in Complaints)
            //    {
            //        aaa.Add(model.Company);
            //    }
            //}

            //if (Column == 3)
            //{
            //    foreach (CModel model in Complaints)
            //    {
            //        aaa.Add(model.State);
            //    }
            //}
        }

        //public void GetDatasInColumn()
        //{
        //    var aaa = new List<String>();

        //    if (Column == "Product")
        //    {
        //        foreach (CModel model in Complaints)
        //        {
        //            aaa.Add(model.Product);
        //        }
        //    }

        //    if (Column == 1)
        //    {
        //        foreach (CModel model in Complaints)
        //        {
        //            aaa.Add(model.Issue);
        //        }
        //    }

        //    if (Column == 2)
        //    {
        //        foreach (CModel model in Complaints)
        //        {
        //            aaa.Add(model.Company);
        //        }
        //    }

        //    if (Column == 3)
        //    {
        //        foreach (CModel model in Complaints)
        //        {
        //            aaa.Add(model.State);
        //        }
        //    }
        //}

        public void ReadCsv()
        {
            using (TextFieldParser parser = new TextFieldParser(@"C:\Users\kapla\source\repos\yazlab1-proje2-2022\MultiThreadSearch.WPF\data\rows.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                int i = 0;
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    if (i != 0)
                    {
                        var complaint = new CModel()
                        {
                            Product = TrimTheSentences(fields[0]),
                            Issue = TrimTheSentences(fields[1]),
                            Company = TrimTheSentences(fields[2]),
                            State = TrimTheSentences(fields[3]),
                            ZipCode = TrimTheSentences(fields[4]),
                            ComplaintId = TrimTheSentences(fields[5])
                        };

                        this.Complaints.Add(complaint);
                    }

                    i++;
                }
            }
        }

        public string TrimTheSentences(string item)
        {
            char[] charsToTrim = { '?', '.' };

            var trimItem = item.Replace("\r", "").Replace("\n", "").Replace("\t", "");
            trimItem = trimItem.Trim(charsToTrim);

            return trimItem;
        }

        //public async Task<bool> Dene()
        //{

        //    int number = Process.GetCurrentProcess().Threads.Count;

        //    return true;
        //}
    }
}
