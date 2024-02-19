using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Capstone.Classes
{
    public class Candy : IComparable<Candy>
    {
        public string ProductType { get; set; }
        public string InventoryID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Wrapper { get; set; }
        public int Quantity { get; set; } = 100;


        public int CompareTo(Candy otherCandy)
        {
            if (InventoryID.CompareTo(otherCandy.InventoryID) < 0)
            {
                return -1;
            }
            else if (InventoryID.CompareTo(otherCandy.InventoryID) == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
