using System;
using System.Collections.Generic;

namespace Nancy.MediatR.Example
{
    public class InMemoryDatabase
    {
        public IList<InventoryItem> Inventory = new List<InventoryItem>
        {
            new InventoryItem(1, "Taylormade M1 460", 599.99m, 5),
            new InventoryItem(2, "Titleist 945D2", 499.99m, 8),
            new InventoryItem(3, "Callaway Golf XR", 299.99m, 3),
            new InventoryItem(4, "Nike Vapor", 329.99m, 1),
            new InventoryItem(5, "Cobra Fly Z+", 399.99m, 2),
        };

        public IList<Guid> Idempotent = new List<Guid>();
    }

    public class InventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int QuantityOnHand { get; set; }

        public InventoryItem(int id, string name, decimal price, int quantityOnHand)
        {
            Id = id;
            Name = name;
            Price = price;
            QuantityOnHand = quantityOnHand;
        }

        public void SellInventory(int quantity)
        {
            if ((QuantityOnHand - quantity) < 0)
            {
                throw new InvalidOperationException("Cannot sell more than on hand.");
            }

            QuantityOnHand -= quantity;
        }
    }
}
