using GoodHamburger.Models;

namespace GoodHamburger.Api.Services;

public class OrderService
{
    private readonly List<Order> _orders = new();
    private int _nextId = 1;

    public static readonly List<MenuItem> Menu = new() {
        new(1, "X Burger", 5.00m, "Sandwich"),
        new(2, "X Egg", 4.50m, "Sandwich"),
        new(3, "X Bacon", 7.00m, "Sandwich"),
        new(4, "Batata frita", 2.00m, "Side"),
        new(5, "Refrigerante", 2.50m, "Drink")
    };

    public (bool Valid, string Message) ValidateOrder(List<int> itemIds)
    {
        // Usamos FirstOrDefault para não dar erro de sistema (Crash) se o ID for inválido
        var items = itemIds.Select(id => Menu.FirstOrDefault(m => m.Id == id)).ToList();

        // 1. Validar se todos os itens existem
        if (items.Any(i => i == null))
        {
            return (false, "Um ou mais itens informados não existem no cardápio.");
        }

        // 2. Validar duplicidade por categoria (Regra do Desafio)
        var categoriasDuplicadas = items.GroupBy(i => i.Category)
                                       .Where(g => g.Count() > 1)
                                       .Select(g => g.Key).ToList();

        if (categoriasDuplicadas.Any())
        {
            return (false, "Erro: O pedido só pode ter UM item de cada categoria (1 Sanduíche, 1 Batata e 1 Bebida).");
        }

        return (true, "");
    }

    public Order CreateOrder(List<int> itemIds)
    {
        var items = itemIds.Select(id => Menu.First(m => m.Id == id)).ToList();
        decimal subtotal = items.Sum(i => i.Price);

        bool hasSandwich = items.Any(i => i.Category == "Sandwich");
        bool hasSide = items.Any(i => i.Category == "Side");
        bool hasDrink = items.Any(i => i.Category == "Drink");

        decimal discountPercent = 0;
        if (hasSandwich && hasSide && hasDrink) discountPercent = 0.20m;
        else if (hasSandwich && hasDrink) discountPercent = 0.15m;
        else if (hasSandwich && hasSide) discountPercent = 0.10m;

        var order = new Order
        {
            Id = _nextId++,
            Items = items,
            Subtotal = subtotal,
            Discount = subtotal * discountPercent,
            Total = subtotal - (subtotal * discountPercent)
        };
        _orders.Add(order);
        return order;
    }

    public List<Order> GetAll() => _orders;
    public Order GetById(int id) => _orders.FirstOrDefault(o => o.Id == id);
    public bool Update(int id, List<int> newItemIds)
    {
        var index = _orders.FindIndex(o => o.Id == id);
        if (index == -1) return false;
        var updatedOrder = CreateOrder(newItemIds);
        updatedOrder.Id = id;
        _orders[index] = updatedOrder;
        return true;
    }
    public bool Delete(int id) => _orders.RemoveAll(o => o.Id == id) > 0;
}