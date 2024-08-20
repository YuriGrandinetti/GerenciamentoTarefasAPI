# GerenciamentoTarefasAPI

## Visão Geral

O projeto `GerenciamentoTarefasAPI` é uma aplicação backend desenvolvida em .NET 6 que fornece uma API para gerenciar tarefas dos usuários. A API oferece funcionalidades como criação, alteração, consulta e exclusão de tarefas, além de autenticação de usuários, notificações via RabbitMQ e organização das tarefas por status (pendente, concluída, cancelada).

## Tecnologias Utilizadas

- **ASP.NET Core 6**: Framework principal para a construção da API.
- **Entity Framework Core**: ORM utilizado para a interação com o banco de dados.
- **PostgreSQL**: Banco de dados relacional utilizado para armazenar as informações.
- **RabbitMQ**: Sistema de mensageria utilizado para notificações de eventos importantes.
- **Swagger**: Ferramenta para documentação e testes da API.
- **JWT (JSON Web Token)**: Sistema de autenticação e autorização dos usuários.

## Estrutura do Projeto

- **Controllers**: Contém os controladores responsáveis pelas rotas da API.
  - `UsuariosController.cs`: Gerencia as operações relacionadas aos usuários, como registro e login.
  - `TarefasController.cs`: Gerencia as operações relacionadas às tarefas, como criação, edição, exclusão e consulta.
  
- **Domain**: Contém as entidades do sistema que representam os modelos de dados.
  - `Usuario.cs`: Representa os usuários do sistema.
  - `Tarefa.cs`: Representa as tarefas associadas aos usuários.
  - `Enumeradores.cs`: Contém os enumeradores utilizados no projeto.

- **Repository**: Contém as classes que interagem diretamente com o banco de dados.
  - `TarefasRepository.cs`: Contém as operações CRUD relacionadas às tarefas.
  - `GerenciamentoTarefasContext.cs`: Contexto do Entity Framework, mapeia as entidades para o banco de dados.

- **Services**: Contém serviços auxiliares utilizados pela aplicação.
  - `RabbitMQService.cs`: Serviço para enviar mensagens para o RabbitMQ.
  - `NotificationService.cs`: Serviço para gerenciar notificações.
  - `UsuarioService.cs`: Serviço para gerenciar operações relacionadas aos usuários.

## Configuração do Ambiente

1. **Clonar o Repositório**

   ```bash
   git clone https://github.com/seu-usuario/GerenciamentoTarefasAPI.git
   cd GerenciamentoTarefasAPIConfigurar o Banco de Dados
   ```
Certifique-se de ter o PostgreSQL instalado e em execução.
Crie um banco de dados para a aplicação.
Atualize a string de conexão em appsettings.json com as credenciais corretas.

## Configuração do Ambiente

### 1. Configurar o Banco de Dados

Certifique-se de que o PostgreSQL está instalado e em execução em seu ambiente. Siga os passos abaixo para configurar o banco de dados:

1. Crie um banco de dados para a aplicação. Você pode fazer isso executando o seguinte comando no PostgreSQL:

   ```sql
   CREATE DATABASE GerenciamentoTarefasDB;
   ```
   
   Atualize a string de conexão no arquivo appsettings.json com as credenciais do banco de dados que você acabou de criar:
   
   "ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=GerenciamentoTarefasDB;Username=seu_usuario;Password=sua_senha"
}

## Script do Banco de dados : 
Esta na raiz do projeto o arquivo Dump do Banco de Dados para que o projeto Funcione : script_gerenciamento_tarefas.sql

2. Instalar Dependências
Depois de configurar o banco de dados, você deve instalar as dependências do projeto. Para isso, utilize o comando abaixo:
dotnet restore
Executar o Projeto
Agora você pode iniciar o projeto utilizando o comando:
```bash
   dotnet run

   ```
   
   A API estará disponível em http://localhost:5000.

5. Acessar a Documentação
A documentação da API pode ser acessada via Swagger em http://localhost:5000/swagger.

Endpoints Principais
Aqui estão alguns dos principais endpoints da API:

/api/usuarios/registrar [POST]: Registrar um novo usuário.
/api/usuarios/login [POST]: Autenticar um usuário e obter um token JWT.
/api/tarefas [GET]: Obter todas as tarefas.
/api/tarefas/{id} [GET]: Obter uma tarefa por ID.
/api/tarefas [POST]: Criar uma nova tarefa.
/api/tarefas/{id} [PUT]: Atualizar uma tarefa existente.
/api/tarefas/{id}/iniciar [PUT]: Iniciar uma tarefa.
/api/tarefas/{id}/concluir [PUT]: Concluir uma tarefa.
/api/tarefas/{id}/cancelar [PUT]: Cancelar uma tarefa.
/api/tarefas/{id} [DELETE]: Excluir uma tarefa.
Testes
A API foi testada utilizando o Swagger para simular chamadas de API.
Testes unitários podem ser criados para validar a lógica de negócios.
Contribuição
Faça um fork do projeto.
Crie uma branch para sua feature (git checkout -b feature/nova-feature).
Commit suas mudanças (git commit -m 'Adiciona nova feature').
Envie para o repositório remoto (git push origin feature/nova-feature).
Abra um Pull Request.
Licença
Este projeto é licenciado sob a Licença MIT - veja o arquivo LICENSE.md para mais detalhes.


### Pré-requisitos

- **.NET 6.0 SDK**: [Instalar .NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- **SQL Server ou PostgreSQL**: Banco de dados para armazenar as informações das tarefas.
- **RabbitMQ**: Para gerenciamento de filas de mensagens.

### Passos para Configurar o Ambiente

1. **Clone o repositório**:
   ```bash
   git clone https://github.com/YuriGrandinetti/GerenciamentoTarefasAPI.git
   cd GerenciamentosTarefasAPI
2. **Configuração do Banco de Dados:**  

Atualize a string de conexão no appsettings.json com as informações do seu banco de dados.
3. **Migrar o Banco de Dados:**

Use as migrações do Entity Framework para configurar o banco de dados:
 ```bash
dotnet ef database update
```

4. **Instalação e Configuração do RabbitMQ** 

Usando Docker (Recomendado)
Execute o seguinte comando no terminal para iniciar o RabbitMQ usando Docker:
 ```bash
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:management
```
**Instalação Manual**
**Windows**:

Baixe o instalador do RabbitMQ aqui.
Siga as instruções de instalação.
Após a instalação, inicie o serviço RabbitMQ através do RabbitMQ Service no Windows Services.
Acesse a interface de gerenciamento em http://localhost:15672/ (o login padrão é guest e a senha é guest).

Para mais informações ou dúvidas, entre em contato com:
- **Yuri Grandinetti Lemes** - [yurigrandi@gmail.com](mailto:yurigrandi@gmail.com)

  ![image](https://github.com/user-attachments/assets/b707c7ce-0648-4fb5-b40e-a129bbc5c097)

  **Diagrama de banco de dados:**

  ![image](https://github.com/user-attachments/assets/1653b287-2cce-490a-b8ea-f27331eabcc6)


