using GoodHamburger.Api.Services;
using Xunit;

namespace GoodHamburger.Tests;

public class OrderTests
{
    private readonly OrderService _service;

    public OrderTests()
    {
        _service = new OrderService();
    }

    // --- TESTES DE DESCONTO ---

    [Fact]
    public void Combo_Completo_Deve_Dar_20_Por_Cento_Desconto()
    {
        // XBurger(5), Batata(2), Refri(2.5) = 9.50. Total: 7.60
        var itemIds = new List<int> { 1, 4, 5 };
        var order = _service.CreateOrder(itemIds);
        Assert.Equal(7.60m, order.Total);
    }

    [Fact]
    public void Sanduiche_Mais_Bebida_Deve_Dar_15_Por_Cento_Desconto()
    {
        // XBurger(5) + Refri(2.5) = 7.50. Desconto 15% (1.125). Total: 6.375
        var itemIds = new List<int> { 1, 5 };
        var order = _service.CreateOrder(itemIds);
        Assert.Equal(6.375m, order.Total);
    }

    [Fact]
    public void Sanduiche_Mais_Batata_Deve_Dar_10_Por_Cento_Desconto()
    {
        // XBurger(5) + Batata(2) = 7.00. Desconto 10% (0.70). Total: 6.30
        var itemIds = new List<int> { 1, 4 };
        var order = _service.CreateOrder(itemIds);
        Assert.Equal(6.30m, order.Total);
    }

    [Fact]
    public void Apenas_Sanduiche_Nao_Deve_Ter_Desconto()
    {
        var itemIds = new List<int> { 1 }; // XBurger(5)
        var order = _service.CreateOrder(itemIds);
        Assert.Equal(5.00m, order.Total);
        Assert.Equal(0, order.Discount);
    }

    [Fact]
    public void Batata_Mais_Bebida_Sem_Sanduiche_Nao_Deve_Ter_Desconto()
    {
        var itemIds = new List<int> { 4, 5 }; // Batata(2) + Refri(2.5) = 4.50
        var order = _service.CreateOrder(itemIds);
        Assert.Equal(4.50m, order.Total);
        Assert.Equal(0, order.Discount);
    }

    // --- TESTES DE VALIDAÇÃO ---

    [Fact]
    public void Itens_Duplicados_Mesma_Categoria_Deve_Retornar_Invalido()
    {
        var itemIds = new List<int> { 1, 2 }; // Dois sanduíches
        var validation = _service.ValidateOrder(itemIds);

        Assert.False(validation.Valid);
        Assert.Contains("UM item de cada categoria", validation.Message);
    }

    [Fact]
    public void Item_Que_Nao_Existe_No_Cardapio_Deve_Retornar_Erro()
    {
        // O OrderService deve lidar com IDs inexistentes (ex: 99)
        // Se sua API ainda não trata isso, adicione um try/catch no Service ou uma checagem de nulo
        var itemIds = new List<int> { 99 };

        // Vamos ajustar o ValidateOrder para tratar isso:
        var validation = _service.ValidateOrder(itemIds);
        Assert.False(validation.Valid);
    }

    [Fact]
    public void Ao_Criar_Pedido_Deve_Incrementar_Lista_Geral()
    {
        var itemIds = new List<int> { 1 };
        _service.CreateOrder(itemIds);

        var allOrders = _service.GetAll();
        Assert.NotEmpty(allOrders);
    }

    [Fact]
    public void Ao_Deletar_Pedido_Deve_Remover_Da_Lista()
    {
        var order = _service.CreateOrder(new List<int> { 1 });
        var deleted = _service.Delete(order.Id);

        Assert.True(deleted);
        Assert.Null(_service.GetAll().FirstOrDefault(o => o.Id == order.Id));
    }

    [Fact]
    public void Item_Inexistente_Deve_Retornar_Erro_De_Validacao()
    {
        var service = new OrderService();
        var itemIds = new List<int> { 999 }; // ID que não existe no cardápio

        var validation = service.ValidateOrder(itemIds);

        Assert.False(validation.Valid);
        Assert.Equal("Um ou mais itens informados não existem no cardápio.", validation.Message);
    }
}