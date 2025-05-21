# 📁 DocSpider Manager

**DocSpider** é uma aplicação web ASP.NET Core 3.1 desenvolvida com o padrão MVC. Ela permite gerenciar documentos com funcionalidades de **upload**, **edição**, **listagem** com filtros e **remoção** de arquivos.

---

## 🚀 Funcionalidades

- Cadastro de documentos com upload de arquivo
- Edição dos dados e substituição de arquivos
- Filtros de pesquisa na listagem
- Exclusão de documentos
- Validação de tamanho e tipo de arquivos (até 10MB; tipos `.exe`, `.bat` e `.zip` são bloqueados)

---

## 🧰 Tecnologias utilizadas

- ASP.NET Core MVC 3.1
- Entity Framework Core
- SQL Server
- Bootstrap 4.3

---

## ⚙️ Requisitos

- .NET Core SDK 3.1
- Visual Studio 2022 (ou superior)
- SQL Server (Express, Developer ou outro)

---

## 🔧 Configuração do projeto

1. Clone o repositório:
   ```bash
   git clone https://github.com/jr-quint/docspider-manager
   ```

2. Configure a connection string no arquivo `appsettings.json`:

   Se não houver uma senha.
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=SEU_SERVIDOR;Database=DOCSPIDERAPP;Trusted_Connection=True; trustservercertificate=true"
   }
   ```
   Caso haja um usuaro e senha
   
   ```json
   "ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR;Database=DOCSPIDERAPP;User Id=SEU_ID;Password=SUA_SENHA;"
   }
    ```

4. Restaure o banco de dados com o comando:
   ```bash
   dotnet ef database update
   ```

5. Execute o projeto via Visual Studio (`F5`) ou com o comando:
   ```bash
   dotnet run
   ```

---

## ❗ Observações

- Os arquivos de upload são limitados a **10MB**
- Tipos de arquivos **não permitidos**: `.exe`, `.bat`, `.zip`
- Não há sistema de autenticação implementado

---

## 📄 Licença

Projeto desenvolvido para fins de candidatura profissional. Uso livre com atribuição.
