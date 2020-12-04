using GGsDB.Models;
using GGsDB.Repos;
using System.Collections.Generic;

namespace GGsLib
{
    public class LineItemService : ILineItemService
    {
        private ILineItemRepo repo;
        public LineItemService(IRepo repo)
        {
            this.repo = repo;
        }
        public void AddLineItem(LineItem item)
        {
            repo.AddLineItem(item);
        }
        public void DeleteLineItem(int id)
        {
            repo.DeleteLineItem(id); 
        }
        public List<LineItem> GetAllLineItems(int orderId)
        {
            return repo.GetAllLineItems(orderId);
        }
        public LineItem GetLineItemById(int id)
        {
            return repo.GetLineItemById(id);
        }
        public void UpdateLineItem(LineItem item)
        {
            repo.UpdateLineItem(item);
        }
    }
}