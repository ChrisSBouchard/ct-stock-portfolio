using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace StockPortfolio
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Out.WriteLine("Welcome to stock portfolio.");
            Dictionary<string, int> portfolio = new Dictionary<string, int>();
            Boolean menu = true;
            while (menu)
            {
                Console.Out.WriteLine("Enter buy, sell, view, export, or quit: ");
                string menuinput = Console.ReadLine();
                switch (menuinput)
                {
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

        private static void Export(Dictionary<string, int> portfolio)
        {
            string json = JsonConvert.SerializeObject(portfolio);
            File.WriteAllText(@"C:\Users\Chris\source\repos\StockPortfolio\StockPortfolio\portfolio.json", json);
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
