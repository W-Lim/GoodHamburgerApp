namespace GoodHamburger.Models;

public class Order
{
    public int Id { get; set; }
    public List<MenuItem> Items { get; set; } = new();
    public decimal Subtotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }
}

public class OrderRequest
{
    public List<int> ItemIds { get; set; } = new();
}