using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StockPortfolio
{
    class Program { 

        static void Main(string[] args)
        {
            Console.Out.WriteLine("Welcome to stock portfolio.");
            StreamReader r = new StreamReader("portfolio.json");
            Dictionary<string, int> portfolio = JsonConvert.DeserializeObject<Dictionary<string, int>>(r.ReadToEnd());
            r.Close();
            Boolean menu = true;
            while (menu)
            {
                Console.Out.WriteLine("Enter price, buy, sell, view, export, or quit: ");
                string menuinput = Console.ReadLine();
                switch (menuinput)
                {
                    case "price":
                        GetStockPrice();
                        break;
                    case "buy":
                        portfolio = Buy(portfolio);
                        break;
                    case "sell":
                        portfolio = Sell(portfolio);
                        break;
                    case "view":
                        View(portfolio);
                        break;
                    case "export":
                        Export(portfolio);
                        break;
                    case "quit":
                        menu = false;
                        break;
                    default:
                        Console.Out.WriteLine("Invalid Input.");
                        break;
                }
            }


        }

        private static void GetStockPrice()
        {
            Console.Out.WriteLine("Enter stock symbol: ");
            string stock = Console.ReadLine().ToUpper();
            string uri = "https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol=" + stock + "&interval=1min&apikey=2460Q7OX2FHSSX1T";
            string str = new WebClient().DownloadString(uri);
            JObject json = JObject.Parse(str);
            string time = (string)json["Meta Data"]["3. Last Refreshed"];
            string price = (string)json["Time Series (1min)"][time]["4. close"];
            Console.WriteLine("{0}", price);
        }

        private static void Export(Dictionary<string, int> portfolio)
        {
            string json = JsonConvert.SerializeObject(portfolio);
            File.WriteAllText(@"portfolio.json", json);
        }

        private static void View(Dictionary<string, int> portfolio)
        {
            foreach( KeyValuePair<string, int> kvp in portfolio)
            {
                Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
            }
        }

        static Dictionary<string, int> Buy(Dictionary<string, int> portfolio)
        {
            Console.Out.WriteLine("Enter stock symbol: ");
            string stock = Console.ReadLine();
            Console.Out.WriteLine("Enter number of shares: ");
            string snumber = Console.ReadLine();
            int number;
            if (Int32.TryParse(snumber, out number))
            {
                if (portfolio.ContainsKey(stock))
                {
                    portfolio[stock] = portfolio[stock] + number;
                }
                else
                {
                    portfolio[stock] = number;
                }
            }
            else
            {
                Console.Out.WriteLine("Invalid Input.");
            }
            return portfolio;
        }

        static Dictionary<string, int> Sell(Dictionary<string, int> portfolio)
        {
            Console.Out.WriteLine("Enter stock symbol: ");
            string stock = Console.ReadLine();
            Console.Out.WriteLine("Enter number of shares: ");
            string snumber = Console.ReadLine();
            int number;
            if (Int32.TryParse(snumber, out number))
            {
                if (portfolio.ContainsKey(stock))
                {
                    if (number < portfolio[stock])
                    {
                        portfolio[stock] = portfolio[stock] - number;
                    }
                    else if (number == portfolio[stock])
                    {
                        portfolio.Remove(stock);
                    } else
                    {
                        Console.Out.WriteLine("Invalid Input");
                    }
                } else
                {
                    Console.Out.WriteLine("Invalid Input.");
                }
            }
            else
            {
                Console.Out.WriteLine("Invalid Input");
            }
            return portfolio;
        }
    }
}
