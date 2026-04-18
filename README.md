# 🍔 Good Hamburger - Sistema de Gestão de Pedidos

Este projeto é uma solução completa para o desafio técnico da **Good Hamburger**. A aplicação consiste em uma API REST robusta, um Frontend moderno desenvolvido em Blazor WebAssembly e uma suíte de testes unitários para garantir a precisão total das regras de negócio e descontos.

---

## 🚀 Como Executar o Projeto

1. **Pré-requisitos:**
   - Ter o [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.
   - Visual Studio 2022 (preferencialmente atualizado).

2. **Clone o repositório:**
   ```bash
   git clone https://github.com/seu-usuario/nome-do-repositorio.git

3. **Abrir a Solução:**
   - Abra o arquivo `GoodHamburgerApp.sln` no Visual Studio.

4. **Configurar Inicialização Dupla (Obrigatório para o Front falar com o Back):**
   - No **Solution Explorer**, clique com o botão direito na **Solution** (ícone do topo).
   - Selecione **Set Startup Projects...**.
   - Marque a opção **Multiple startup projects**.
   - Defina as ações para os projetos abaixo:
     - `GoodHamburger.Api`: **Start**
     - `GoodHamburger.Client`: **Start**
   - Clique em **Apply** e **OK**.

5. **Executar:**
   - Pressione `F5` ou clique no botão **Start**. O navegador abrirá o terminal de vendas e a documentação Swagger da API.

---

## 🏗️ Arquitetura e Decisões Técnicas

O projeto foi estruturado seguindo princípios de **Clean Code** e **Separação de Preocupações (SoC)**:

- **Service Layer (Camada de Serviço):** Toda a inteligência de cálculo de descontos e as validações de regras de negócio foram centralizadas no `OrderService`. Isso isola a inteligência do sistema dos controladores da API, facilitando a manutenção e permitindo testes unitários rigorosos sem depender de HTTP.
- **In-Memory Storage:** Conforme os requisitos do desafio, a persistência de dados é feita em memória utilizando o padrão `Singleton`. Os dados dos pedidos permanecem disponíveis enquanto a API estiver em execução.
- **Frontend SPA (Blazor WebAssembly):** Criado com Blazor para proporcionar uma experiência de Single Page Application. O design foi personalizado com o tema **"Modern Gourmet"**, utilizando uma paleta de cores focada em UX/UI para terminais de vendas (Preto e Amarelo GH).
- **Validação Rigorosa:** A API atua como a única fonte da verdade. Qualquer tentativa de violar as regras (como adicionar itens duplicados de uma mesma categoria) é barrada pela camada de serviço, retornando mensagens de erro claras ao usuário no frontend.

---

## 💰 Regras de Desconto Implementadas

O sistema identifica as combinações de itens no carrinho e aplica o desconto mais vantajoso automaticamente:

- **Combo Trio (20% de Desconto):** Quando o pedido contém exatamente 1 Sanduíche + 1 Acompanhamento + 1 Bebida.
- **Combo Duplo A (15% de Desconto):** Quando o pedido contém exatamente 1 Sanduíche + 1 Bebida.
- **Combo Duplo B (10% de Desconto):** Quando o pedido contém exatamente 1 Sanduíche + 1 Acompanhamento.

> **Regra de Validação:** Cada pedido pode conter apenas um item de cada categoria (Sanduíche, Acompanhamento, Bebida). Caso o usuário tente adicionar itens duplicados (ex: 2 sanduíches), o sistema retornará um erro 400 (Bad Request).

---

## 🛠️ Tecnologias Utilizadas

- **Backend:** C# .NET 8 ASP.NET Core Web API.
- **Frontend:** Blazor WebAssembly com Bootstrap 5.
- **Testes:** xUnit para testes de unidade.
- **Documentação:** Swagger UI (OpenAPI) disponível automaticamente ao rodar a API.

---

## 🧪 Testes Automatizados

Foi desenvolvida uma suíte de testes unitários que cobre os cenários críticos do desafio:

- [x] **Cálculos de Desconto:** Validação dos percentuais de 10%, 15% e 20%.
- [x] **Regras de Categoria:** Teste de bloqueio para itens duplicados na mesma categoria.
- [x] **Tratamento de Erros:** Validação de comportamento ao receber IDs de itens que não constam no cardápio.
- [x] **Persistência Temporária:** Testes de criação e remoção (CRUD) na base de dados em memória.

**Para rodar os testes:** No Visual Studio, acesse o menu superior **Test > Run All Tests** ou abra o **Test Explorer**.

---

## 📌 O que ficou fora (Out of Scope)

Para manter o foco na simplicidade e nos requisitos centrais do desafio, os seguintes itens não foram implementados nesta versão:

- **Persistência em Banco de Dados:** Uso de banco físico (SQL Server/PostgreSQL), mantendo os dados em memória (In-memory).
- **Segurança:** Sistema de Autenticação e autorização de usuários (Login/Roles).
- **Pagamentos:** Integração com gateways de pagamento reais ou PIX.
- **Funcionalidades de Atendimento:** Registro do nome do cliente que realizou o pedido e a opção de escolha entre "Comer no Local" ou "Para Viagem".

---

**Desenvolvido como parte do Desafio Técnico Good Hamburger.**
