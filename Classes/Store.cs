using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class Store
    {
        private List<Candy> inventory = new List<Candy>();
        private List<CartItems> cart = new List<CartItems>();


        public decimal CurrentBalance { get; set; }

        public Store()
        {
            FileAccess fileaccess = new FileAccess();
            inventory = fileaccess.ReadCandy();
            inventory.Sort();
        }

        public Candy[] ListCandy()
        {
            return inventory.ToArray();
        }

        public CartItems[] ListCart()
        {
            return cart.ToArray();
        }

        public bool CheckBalance(decimal deposit)
        {
            if (CurrentBalance + deposit > 1000)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ConfirmIDExists(string idSelected)
        {
            Candy[] temp = ListCandy();

            bool idExists = false;

            for (int i = 0; i < temp.Length - 1; i++)
            {
                if (idSelected == temp[i].InventoryID)
                {
                    idExists = true;
                }
            }
            return idExists;
        }

        public bool ProductIsInStock(string idSelected)
        {
            Candy[] temp = ListCandy();

            bool inStock = false;

            for (int i = 0; i < temp.Length - 1; i++)
            {
                if (idSelected == temp[i].InventoryID)
                {
                    if (temp[i].Quantity > 0)
                    {
                        inStock = true;
                    }
                }
            }
            return inStock;
        }

        public bool SufficientStock(int quantityRequested, string idSelected)
        {
            Candy[] temp = ListCandy();

            bool sufficientStock = false;

            for (int i = 0; i < temp.Length - 1; i++)
            {
                if (idSelected == temp[i].InventoryID)
                {
                    if (temp[i].Quantity >= quantityRequested)
                    {
                        sufficientStock = true;
                    }
                }
            }
            return sufficientStock;
        }

        public decimal CalculateTotal(int quantityRequested, string idSelected)
        {
            Candy[] temp = ListCandy();

            decimal total = 0;

            for (int i = 0; i < temp.Length - 1; i++)
            {
                if (idSelected == temp[i].InventoryID)
                {
                    total = temp[i].Price * quantityRequested;
                }
            }
            return total;
        }

        public bool SufficientBalance(int quantityRequested, string idSelected)
        {
            decimal total = CalculateTotal(quantityRequested, idSelected);
            bool sufficientBalance = false;

            if (CurrentBalance >= total)
            {
                sufficientBalance = true;
            }
            return sufficientBalance;
        }

        public void UpdateBalance(decimal totalCost)
        {
            CurrentBalance -= totalCost;
        }

        public void AddToCart(int quantityRequested, string idSelected)
        {
            Candy[] temp = ListCandy();

            for (int i = 0; i < temp.Length - 1; i++)
            {
                if (idSelected == temp[i].InventoryID)
                {
                    CartItems cartitem = new CartItems();
                    cartitem.Quantity = quantityRequested;
                    cartitem.ProductName = temp[i].ProductName;
                    cartitem.ProductType = temp[i].ProductType;
                    cartitem.Price = temp[i].Price;

                    cart.Add(cartitem);
                }
            }
        }

        public void UpdateInventory(int quantityRequested, string idSelected)
        {
            Candy[] temp = ListCandy();

            for (int i = 0; i < temp.Length - 1; i++)
            {
                if (idSelected == temp[i].InventoryID)
                {
                    temp[i].Quantity -= quantityRequested;
                }
            }
        }

        public decimal ReturnTotal()
        {
            CartItems[] temp = ListCart();
            decimal total = 0M;

            for (int i = 0; i < temp.Length; i++)
            {
                total += temp[i].Price * temp[i].Quantity;
            }

            return total;
        }

        public string ReturnChange()
        {
            FileAccess fileAccess = new FileAccess();
            decimal change = CurrentBalance;
            int twenties = 0;
            int tens = 0;
            int fives = 0;
            int ones = 0;
            int quarters = 0;
            int dimes = 0;
            int nickels = 0;

            fileAccess.WriteChangeGiven(change);

            if (change >= 20)
            {
                twenties = (int)change / 20;
                change = change % 20;
            }

            if (change >= 10)
            {
                tens = (int)change / 10;
                change = change % 10;
            }

            if (change >= 5)
            {
                fives = (int)change / 5;
                change = change % 5;
            }

            if (change >= 1)
            {
                ones = (int)change / 1;
                change = change % 1;
            }

            if (change >= 0.25M)
            {
                quarters = (int)(change / 0.25M);
                change = change % 0.25M;
            }

            if (change >= 0.10M)
            {
                dimes = (int)(change / 0.10M);
                change = change % 0.10M;
            }

            if (change >= 0.05M)
            {
                nickels = (int)(change / 0.05M);
            }

            return $"Your Change: ({twenties}) Twenties, ({tens}) Tens, ({fives}) Fives, ({ones}) Ones, ({quarters}) Quarters, ({dimes}) Dimes, ({nickels}) Nickels";
        }

        public void ResetBalance()
        {
            CurrentBalance = 0;
        }

        public void ResetCart()
        {
            while(cart.Count > 0)
            {
                cart.Remove(cart[0]);
            }
        }

        public string FindProductName(string idSelected)
        {
            Candy[] temp = ListCandy();

            string productName = "";
            for (int i = 0; i < temp.Length - 1; i++)
            {
                if (idSelected == temp[i].InventoryID)
                {
                    productName = temp[i].ProductName;
                }
            }

            return productName;
        }
    }
}
