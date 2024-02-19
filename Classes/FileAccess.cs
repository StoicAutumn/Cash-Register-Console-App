using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone.Classes
{
    public class FileAccess
    {
        private string fileName = @"C:\Store\inventory.csv";
        private string logFile = @"C:\Store\Log.txt";
        private DateTime currentTime = DateTime.Now;

        public List<Candy> ReadCandy()
        {
            List<Candy> result = new List<Candy>();

            using (StreamReader sr = new StreamReader(fileName))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] items = line.Split("|");

                    Candy candy = new Candy();
                    candy.ProductType = items[0];
                    candy.InventoryID = items[1];
                    candy.ProductName = items[2];
                    candy.Price = decimal.Parse(items[3]);
                    candy.Wrapper = items[4];

                    result.Add(candy);
                }
            }
            return result;
        }

        public void WriteMoneyReceived(decimal deposit, decimal currentBalance)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(logFile, true))
                {
                    sw.WriteLine(currentTime.ToString() + " MONEY RECEIVED: $" + deposit + " $" + currentBalance);
                }

            }
            catch (IOException ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public void WriteChangeGiven(decimal change)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(logFile, true))
                {
                    sw.WriteLine(currentTime.ToString() + " CHANGE GIVEN: $" + change + " $0.00");
                }

            }
            catch (IOException ex)
            {
                Console.WriteLine("An error occured: " + ex.Message);
            }
        }

        public void LogOrder(int quantityRequested, string idSelected, decimal totalCost, decimal currentBalance)
        {
            Store store = new Store();

            string productName = store.FindProductName(idSelected);

            try
            {
                using (StreamWriter sw = new StreamWriter(logFile, true))
                {

                    sw.WriteLine(currentTime.ToString() + " " + quantityRequested + " " + productName + " " + idSelected + " $" + totalCost + " $" + currentBalance);
                }

            }
            catch (IOException ex)
            {
                Console.WriteLine("An error occured: " + ex.Message);
            }
        }
    }
}
