# Desafio - API Produto (.NET 8) + Frontend React (Vite) 

Este trabalho foi desenvolvido para teste da Empresa Invent Software, e contém em sua estrutura as tecnologias abaixo:
- **Backend**: API REST em .NET 8 com DDD simplificado, FluentValidation, Swagger, CORS e testes.
- **Frontend**: React (Vite) com tema claro/escuro, Interface Responsiva, modal de edição e proxy `/api`.

## Requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/) e npm

## Como executar

### 1) Backend
```powershell
dotnet restore src\ProductsApi
dotnet build src\ProductsApi
$env:ASPNETCORE_URLS="http://localhost:5000"
dotnet run --project src\ProductsApi\ProductsApi.csproj
```

- Swagger UI: **[http://localhost:5000/swagger](http://localhost:5000/swagger)**

### 2) Frontend (React + Vite)
```powershell
cd frontend-react
npm install
npm run dev
```

- Aplicação web (porta padrão do Vite): **[http://localhost:5173](http://localhost:5173)**

> O proxy do Vite encaminha `/api` para `http://localhost:5000`, então não precisa configurar CORS além do já feito no backend.

## Testes (xUnit)

Os testes ficam em `src/ProductsApi.Tests`.

- Rodar **todos** os testes:
```powershell
dotnet test
```

- Rodar **apenas** o projeto de testes:
```powershell
dotnet test src\ProductsApi.Tests
```

- Rodar **um teste específico**:
```powershell
dotnet test src\ProductsApi.Tests --filter "FullyQualifiedName~ProductsServiceTests.CreateProduct_Should_Set_Id_And_CreatedAtUtc_And_Persist"
```

- Modo **watch** (reexecuta ao salvar):
```powershell
dotnet watch --project src\ProductsApi.Tests test
```

## Onde cada requisito do PDF foi atendido

|  | O que o PDF pede | Onde está no código | Como foi feito |
|---|---|---|---|
| **1. Arquitetura (DDD)** | Separar em camadas | • **Domain**: [`src/ProductsApi/Domain/Product.cs`](src/ProductsApi/Domain/Product.cs) • **DTOs**: [`src/ProductsApi/DTOs/ProductDto.cs`](src/ProductsApi/DTOs/ProductDto.cs) • **Validators**: [`src/ProductsApi/Validators/ProductValidator.cs`](src/ProductsApi/Validators/ProductValidator.cs) • **Repositories**: [`src/ProductsApi/Repositories`](src/ProductsApi/Repositories) • **Services**: [`src/ProductsApi/Services`](src/ProductsApi/Services) • **API**: [`src/ProductsApi/Controllers`](src/ProductsApi/Controllers) • **Composição/DI**: [`src/ProductsApi/Program.cs`](src/ProductsApi/Program.cs) | Camadas isoladas por responsabilidade; Controller → Service → Repository; domínio sem dependência de web/banco. |
| **2. Funcionalidade (CRUD)** | CRUD de Produto | [`Controllers/ProductsController.cs`](src/ProductsApi/Controllers/ProductsController.cs) | Endpoints: `GET /api/products`, `GET /api/products/{id}`, `POST`, `PUT`, `DELETE`. |
| **3. Objeto “Produto”** | Campos e “Disponível” calculado | [`Domain/Product.cs`](src/ProductsApi/Domain/Product.cs) | `Name`, `Category`, `Price`, `QuantityInStock`, `CreatedAt`; `Available` calculado (`QuantityInStock > 0`). |
| **4. Validações** | FluentValidation | [`Validators/ProductValidator.cs`](src/ProductsApi/Validators/ProductValidator.cs) + registro em [`Program.cs`](src/ProductsApi/Program.cs) | Regras: obrigatórios e não-negativos; `AddFluentValidationAutoValidation()` ativa validação automática (400 em caso de erro). |
| **5. Persistência** | Em memória (lista) | [`Repositories/InMemoryProductRepository.cs`](src/ProductsApi/Repositories/InMemoryProductRepository.cs) | Armazenamento em `ConcurrentDictionary<Guid, Product>` (thread-safe), com **seed** inicial. |
| **6. Injeção de Dependência** | DI nativa .NET 8 | [`Program.cs`](src/ProductsApi/Program.cs) | `AddSingleton<IProductRepository, InMemoryProductRepository>()` e `AddScoped<IProductService, ProductService>()`; constructor injection no Controller/Service. |
| **7. Testes** | Unidade (domínio/negócio) | [`ProductsServiceTests.cs`](src/ProductsApi.Tests/ProductsServiceTests.cs) • [`ProductDomainTests.cs`](src/ProductsApi.Tests/ProductDomainTests.cs) | xUnit cobrindo criação, update, delete, busca e propriedade `Available`. Executa com `dotnet test`. |
| **8. Frontend** | Framework à escolha + consumir API | **React (Vite)**: [`frontend-react/`](frontend-react) • Proxy: [`vite.config.js`](frontend-react/vite.config.js) • CRUD: [`src/hooks/useProducts.js`](frontend-react/src/hooks/useProducts.js) • UI: [`src/App.jsx`](frontend-react/src/App.jsx), [`components`](frontend-react/src/components) | React consumindo a API via `fetch` (`GET/POST/PUT/DELETE`). Proxy do Vite aponta `/api` → `http://localhost:5000`. Tema claro/escuro e modal de edição. |

## Endpoints úteis (clique quando a API estiver rodando)
- **Swagger**: [http://localhost:5000/swagger](http://localhost:5000/swagger)  
- **Listar produtos (JSON)**: [http://localhost:5000/api/products](http://localhost:5000/api/products)

## O trabalho está rodando no link abaixo



## 🔧 Feito por:

[João Monferrari](https://www.linkedin.com/in/jo%C3%A3o-monferrari-b278b2223/)
