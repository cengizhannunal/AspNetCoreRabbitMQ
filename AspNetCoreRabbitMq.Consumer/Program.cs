using AspNetCoreRabbitMq.Common.Model;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace AspNetCoreRabbitMq.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };

            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "EmailQuee",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, mq) =>
                {
                    var body = mq.Body;
                    var message = Encoding.UTF8.GetString(body);
                    EmailModel email = JsonConvert.DeserializeObject<EmailModel>(message);
                    Console.WriteLine($"Email adresi kuyruktan alındı.Email Adı: {email.Email}, Açıklama : {email.Body}");
                };

                channel.BasicConsume(queue: "EmailQuee",
                                     autoAck: true,//true ise mesaj otomatik olarak kuyruktan silinir
                                     consumer: consumer);
                Console.ReadKey();

            }
        }
    }
}
