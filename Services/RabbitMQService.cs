using RabbitMQ.Client;
using System;
using System.Text;

namespace GerenciamentoTarefasAPI.Services
{
    public class RabbitMQService : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQService()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void EnviarMensagem(string fila, string mensagem)
        {
            _channel.QueueDeclare(queue: fila,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(mensagem);

            _channel.BasicPublish(exchange: "",
                                 routingKey: fila,
                                 basicProperties: null,
                                 body: body);
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
