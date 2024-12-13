using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253503_TSARUK.Domain.Entities;

namespace WEB_253503_TSARUK.Domain.Models
{
    public class Cart
    {
        public Dictionary<int, CartItem> CartItems { get; set; } = new();
        
        public virtual void AddToCart(Jewelry jewelry)
        {
            if (CartItems.ContainsKey(jewelry.Id))
                CartItems[jewelry.Id].Quantity++;
            else
                CartItems[jewelry.Id] = new CartItem
                {
                    Jewelry = jewelry,
                    Quantity = 1
                };
        }

        public virtual void RemoveItems(int id)
        {
            if (CartItems.ContainsKey(id))
                CartItems.Remove(id);   
        }

        public virtual void ClearAll()
        {
            CartItems.Clear();
        }

        public int Count { get => CartItems.Sum(item => item.Value.Quantity); }

        public double TotalPrice
        {
            get => CartItems.Sum(item => item.Value.Jewelry.Price * item.Value.Quantity);
        }
    }
}
