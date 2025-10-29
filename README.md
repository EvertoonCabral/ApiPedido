<div align="center">

![Version](https://img.shields.io/badge/version-0.1.0-blue.svg?cacheSeconds=2592000)
![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-orange.svg)
![Status](https://img.shields.io/badge/status-active-success.svg)

**Sistema de gerenciamento de Pedidos, Produtos e Clientes**

*Clean Architecture • DDD • CQRS/MediatR • Unit of Work • Generic Repository*

</div>

---

## 📋 Índice

- [📊 Visão Geral](#-visão-geral)
- [🏪 ProdutoService](#-produtoservice)
- [👥 ClienteService](#-clienteservice)
- [🧾 PedidoService](#-pedidoservice)
- [🚀 Tecnologias](#-tecnologias)

---

## 📊 Visão Geral

A **ApiPedidos** é uma API robusta em .NET 9 para gestão de clientes, produtos e pedidos. O projeto segue Clean Architecture com DDD, usa CQRS com MediatR para separar comandos e consultas, AutoMapper para mapeamentos e EF Core com Unit of Work e Repositórios Genéricos.

### Principais Features

-  **Transações Seguras** - Escritas via Unit of Work, garantindo consistência
-  **CQRS com MediatR** - Commands e Queries desacoplados, organizados e testáveis
-  **Mapeamentos Limpos** - AutoMapper para DTOs e projeções de leitura
-  **Pedidos com Itens** - Cálculo de total, manipulação de itens e produtos
-  **Validações no Domínio** - Métodos de fábrica e edição com regras de negócio embutidas
-  **Retornos Padronizados** - Mensagens e DTOs consistentes nos endpoints

---

## 🧾 Produtos

<details>
<summary><strong>📝 Regras de Negócio</strong></summary>

### ✅ Validações Principais
-  **Preços Válidos**: `Preco` e `PrecoVenda` devem ser maiores que zero
-  **Auditoria Automática**: `DataCadastro` e `DataAtualizacao` são ajustadas pelo domínio
-  **Controle de Status**: Métodos `Ativar()` e `Inativar()` controlam o status `IsAtivo`
-  **Edição Segura**: O método `Editar(nome, descricao, preco, precoVenda)` centraliza as regras de alteração

### Funcionalidades

✓ CRUD completo de produtos

✓ Edição com retorno do ProdutoDto atualizado

✓ Ativar/Inativar com mensagens de sucesso padronizadas

✓ Listagem e detalhe via Queries dedicadas


### Padrões de Resposta
- **PUT** `/api/produtos/{id}` → Retorna `200 OK` com o `ProdutoDto` editado


- **PATCH** `/api/produtos/{id}/inativar` → Retorna `200 OK` com `{ "mensagem": "Produto de Id {id} foi inativado." }`


- **PATCH** `/api/produtos/{id}/ativar` → Retorna `200 OK` com `{ "mensagem": "Produto de Id {id} foi ativado." }`

</details>

---

## 🧾 Cliente

<details>
<summary><strong>📝 Regras de Negócio</strong></summary>

### ✅ Validações de Entrada
-  **Campos Obrigatórios**: Nome, e-mail e outros campos são validados na criação e edição
-  **Ativação e Inativação**: Controladas via métodos de domínio para manter a consistência
-  **Consultas Específicas**: Listagem completa, por Id e uma consulta dedicada para clientes que já possuem pedidos

### Funcionalidades
✓ Cadastro e edição de clientes

✓ Ativação e Inativação (Soft Delete)

✓ Listar todos os clientes

✓ Listar apenas clientes com pedidos

✓ Obter cliente por Id via Query


</details>

---

## 🧾 Pedidos

<details>
<summary><strong>📝 Regras de Negócio</strong></summary>

### 🧱 Modelo de Domínio
- **Pedido**: `Id`, `ClienteId`, `DataAbertura`, `DataAtualizacao`, `Status`, `Itens`
- **ItemPedido**: `PedidoId`, `ProdutoId`, `NomeProduto`, `PrecoUnitario`, `Quantidade`, `Produto?`
- **Pedido.Total**: Propriedade computada que soma `PrecoUnitario * Quantidade` de todos os itens

### 🔍 Consultas e Carregamento
- 🔗 **Carregamento Inteligente**: Uso de `Include` e `ThenInclude` para carregar `Itens` e `Produto` quando necessário
- 🎯 **Busca Específica**: Método dedicado no repositório para trazer o pedido com apenas o item do produto alvo:
    - `GetWithItemAndProdutoAsync(pedidoId, produtoId)`

### ➖ Remoção de Produto do Pedido
- **Decremento**: Diminui a quantidade do item no pedido
- **Remoção Automática**: Remove o `ItemPedido` do banco de dados quando a quantidade atinge zero
- **Auditoria**: Atualiza a `DataAtualizacao` do pedido a cada alteração
- **Persistência**: Salva todas as alterações via `UnitOfWork`

### 💡 Funcionalidades
✓ Criar e abrir um novo pedido
✓ Adicionar produtos a um pedido existente
✓ Remover/Decrementar produtos de um pedido
✓ Fechar ou Cancelar um pedido
✓ Obter detalhes de um pedido com seus itens e produtos


</details>

---

## 🚀 Tecnologias

<div align="center">

| Tecnologia                                                                                           | Versão | Uso |
|------------------------------------------------------------------------------------------------------|--------|-----|
| ![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat&logo=dotnet)                         | 9.0    | Framework principal |
| ![Entity Framework](https://img.shields.io/badge/EF%20Core-512BD4?style=flat&logo=microsoft)         | Latest | ORM |
| ![AutoMapper](https://img.shields.io/badge/AutoMapper-BE9A2F?style=flat)                             | Latest | Mapeamento de objetos |
| ![MediatR](https://img.shields.io/badge/MediatR-6.x-green?style=flat)                                | Latest | CQRS (Commands/Queries) |
| ![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=flat&logo=microsoft-sql-server) | 2019+  | Banco de dados |

</div>

---

<div align="center">

### 🌟 **Sistema em Produção**

**Desenvolvido com as melhores práticas de desenvolvimento**

*Padrões SOLID • Clean Architecture • Domain-Driven Design*

---

**⭐ Se este projeto foi útil, considere dar uma estrela!**

</div>
