using GGsDB.Entities;
using GGsDB.Models;
using System.Collections.Generic;

namespace GGsDB.Mappers
{
    public interface ILineItemMapper
    {
        LineItem ParseLineItem(Lineitems item);
        Lineitems ParseLineItem(LineItem item);
        List<LineItem> ParseLineItem(ICollection<Lineitems> items);
        ICollection<Lineitems> ParseLineItem(List<LineItem> items);

    }
}