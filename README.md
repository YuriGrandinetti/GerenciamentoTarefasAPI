# GerenciamentosTarefasAPI

Este projeto é uma API para gerenciamento de tarefas, desenvolvida com .NET 6.0. Ele permite que os usuários cadastrem, consultem, atualizem e excluam tarefas, além de enviar notificações sobre as mudanças de status das tarefas. A aplicação também faz uso do RabbitMQ para gerenciar filas de mensagens de forma assíncrona.

## Funcionalidades

- **Cadastro de Tarefas**: Crie novas tarefas com descrições, datas e status.
- **Consulta de Tarefas**: Obtenha uma lista de tarefas ou detalhes de uma tarefa específica.
- **Atualização de Tarefas**: Atualize os detalhes de uma tarefa existente.
- **Exclusão de Tarefas**: Exclua tarefas existentes.
- **Notificações**: Envio de notificações sobre mudanças no status das tarefas.
- **Integração com RabbitMQ**: Processamento assíncrono de tarefas usando filas de mensagens.

## Tecnologias Utilizadas

- **.NET 6.0**: Framework utilizado para o desenvolvimento da API.
- **Entity Framework**: Utilizado para o mapeamento objeto-relacional (ORM) e interação com o banco de dados.
- **RabbitMQ**: Utilizado para o gerenciamento de filas de mensagens.
- **SQL Server ou PostgreSQL**: O banco de dados utilizado para armazenar as informações das tarefas.
- **Docker**: (Opcional) Para containerização da aplicação.

## Estrutura do Projeto

- `Controllers`: Contém os controladores da API, responsáveis por lidar com as requisições HTTP.
- `Models`: Define as entidades utilizadas no sistema.
- `Repository`: Contém os repositórios para interação com o banco de dados.
- `Services`: Contém os serviços que implementam a lógica de negócios, incluindo o serviço de notificação e integração com RabbitMQ.
- `Program.cs`: Arquivo principal que configura e executa a aplicação.

## Configuração e Instalação

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


