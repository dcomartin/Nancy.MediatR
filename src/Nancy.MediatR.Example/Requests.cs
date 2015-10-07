using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Nancy.MediatR.Example
{
    public class SellInventory : IAsyncRequest
    {
        public int InventoryId { get; set; }
        public int Quantity { get; set; }

        public SellInventory() { }

        public SellInventory(int id, int quantity)
        {
            Quantity = quantity;
            InventoryId = InventoryId;
        }
    }

    public class SellInventoryHandler : IAsyncRequestHandler<SellInventory, Unit>
    {
        private readonly InMemoryDatabase _db;

        public SellInventoryHandler(InMemoryDatabase db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(SellInventory message)
        {
            var item = (from x in _db.Inventory where x.Id == message.InventoryId select x).SingleOrDefault();
            if (item == null) throw new InvalidOperationException("Inventory item not found.");

            item.SellInventory(message.Quantity);
            
            return Unit.Value;
        }
    }

    public class GetInventoryItem : IAsyncRequest<InventoryItem>
    {
        public int Id { get; private set; }

        public GetInventoryItem(int id)
        {
            Id = id;
        }
    }

    public class GetInventoryItemHandler : IAsyncRequestHandler<GetInventoryItem, InventoryItem>
    {
        private readonly InMemoryDatabase _db;

        public GetInventoryItemHandler(InMemoryDatabase db)
        {
            _db = db;
        }

        public async Task<InventoryItem> Handle(GetInventoryItem message)
        {
            var item = _db.Inventory.FirstOrDefault(x => x.Id == message.Id);
            return await Task.FromResult(item);
        }
    }
}
