using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Capstone.Classes
{
    class UserInterface
    {
        private Store store = new Store();
        FileAccess fileAccess = new FileAccess();

        public void Run()
        {
            bool endProgram = false;
            Console.WriteLine("Welcome to the Silver Shamrock Candy Company! How can we help you today?");

            while (!endProgram)
            {
                DisplayMainMenu();

                string userResponse = Console.ReadLine();

                switch (userResponse)
                {
                    case "1":
                        ListCandy();
                        break;
                    case "2":
                        RunSale();
                        break;
                    case "3":
                        endProgram = true;
                        break;
                    default:
                        Console.WriteLine("Please enter a valid choice.");
                        break;

                }
            }
        }

        public void RunSale()
        {
            bool endSale = false;

            while (!endSale)
            {
                DisplaySaleMenu();

                string userResponse = Console.ReadLine();

                switch (userResponse)
                {
                    case "1":
                        TakeMoney();
                        break;
                    case "2":
                        SelectProduct();
                        break;
                    case "3":
                        CompleteSale();
                        Reset();
                        endSale = true;
                        break;
                    default:
                        Console.WriteLine("Please enter a valid choice.");
                        break;

                }
            }
        }

        public void DisplayMainMenu()
        {
            Console.WriteLine("(1) Show Inventory");
            Console.WriteLine("(2) Make Sale");
            Console.WriteLine("(3) Quit");
        }

        public void DisplaySaleMenu()
        {
            Console.WriteLine();
            Console.WriteLine("(1) Take Money");
            Console.WriteLine("(2) Select Products");
            Console.WriteLine("(3) Complete Sale");
            Console.WriteLine("Current Customer Balance: " + store.CurrentBalance.ToString("C"));
        }

        private void ListCandy()
        {
            Candy[] temp = store.ListCandy();

            Console.WriteLine();
            Console.WriteLine("{0, -7} {1, -20} {2, -12} {3, -10} {4, -8}", "ID", "Name", "Wrapper", "Qty", "Price");

            for (int i = 0; i < temp.Length - 1; i++)
            {
                string wrapperYN = (temp[i].Wrapper == "T") ? "Y" : "N";
                string quantityOrSoldOut = (temp[i].Quantity == 0) ? "SOLD OUT" : temp[i].Quantity.ToString();
                Console.WriteLine("{0, -7} {1, -20} {2, -12} {3, -10} {4, -8}", temp[i].InventoryID, temp[i].ProductName, wrapperYN, quantityOrSoldOut, temp[i].Price.ToString("C"));
            }

            Console.WriteLine();
        }

        public void TakeMoney()
        {
            Console.WriteLine();
            Console.WriteLine("Please deposit up to $100. Whole bills only");
            string depositString = Console.ReadLine();
            decimal deposit = decimal.Parse(depositString);

            if ((deposit > 100 || deposit < 0) || !(deposit % 1 == 0))
            {
                Console.WriteLine("Please enter a valid amount");
                Console.WriteLine();
                deposit = 0;
                TakeMoney();
            }

            if (store.CheckBalance(deposit))
            {
                store.CurrentBalance += deposit;
                decimal currentBalance = store.CurrentBalance;
                fileAccess.WriteMoneyReceived(deposit, currentBalance);
            }
            else
            {
                Console.WriteLine("Balance should not exceed $1000");
                Console.WriteLine();
            }
        }

        public void SelectProduct()
        {
            ListCandy();

            Console.WriteLine();
            Console.Write("Select Candy By ID: ");
            string idSelected = Console.ReadLine();

            if (store.ConfirmIDExists(idSelected) == false)
            {
                Console.WriteLine();
                Console.WriteLine("ID does not exist");
                RunSale();
            }
            else if (store.ProductIsInStock(idSelected) == false)
            {
                Console.WriteLine();
                Console.WriteLine("Product out of stock");
                RunSale();
            }

            Console.WriteLine();
            Console.Write("How much do you want: ");
            string quantityString = Console.ReadLine();
            int quantityRequested = int.Parse(quantityString);

            decimal totalCost = store.CalculateTotal(quantityRequested, idSelected);

            if (store.SufficientStock(quantityRequested, idSelected) == false)
            {
                totalCost = 0;
                Console.WriteLine();
                Console.WriteLine("Not enough stock");
                RunSale();
            }
            else if (store.SufficientBalance(quantityRequested, idSelected) == false)
            {
                totalCost = 0;
                Console.WriteLine();
                Console.WriteLine("Insufficient balance");
                RunSale();
            }
            else
            {
                CompletePurchase(quantityRequested, idSelected, totalCost);
            }
        }

        public void CompletePurchase(int quantityRequested, string idSelected, decimal totalCost)
        {
            store.UpdateBalance(totalCost);

            decimal currentBalance = store.CurrentBalance;

            store.AddToCart(quantityRequested, idSelected);
            fileAccess.LogOrder(quantityRequested, idSelected, totalCost, currentBalance);
            store.UpdateInventory(quantityRequested, idSelected);
        }

        public void CompleteSale()
        {
            PrintReceipt();
            Console.WriteLine(store.ReturnChange());
            Console.WriteLine("Thank you for shopping with us!");
            Console.WriteLine();
        }

        public void Reset()
        {
            store.ResetBalance();
            store.ResetCart();
        }

        public void PrintReceipt()
        {

            CartItems[] temp = store.ListCart();

            Console.WriteLine();
            Console.WriteLine("{0, -10} {1, -20} {2, -30} {3, -10} {4, -10}", "Quantity", "Name", "Product Type", "Price", "Sub-Total");

            for (int i = 0; i < temp.Length; i++)
            {
                decimal subTotal = temp[i].Quantity * temp[i].Price;
                string productType = "";

                if (temp[i].ProductType == "CH")
                {
                    productType = "Chocolate Confectionery";
                }
                else if (temp[i].ProductType == "SR")
                {
                    productType = "Sour Flavored Candies";
                }
                else if (temp[i].ProductType == "LI")
                {
                    productType = "Licorce and Jellies";
                }
                else
                {
                    productType = "Hard Tack Confectionery";
                }
                Console.WriteLine("{0, -10} {1, -20} {2, -30} {3, -10} {4, -10}", temp[i].Quantity, temp[i].ProductName, productType, temp[i].Price.ToString("C"), subTotal.ToString("C"));
            }
            Console.WriteLine();
            Console.WriteLine("Total: " + store.ReturnTotal().ToString("C"));
            Console.WriteLine("Change: " + store.CurrentBalance.ToString("C"));
        }
    }
}
