using AspNetCoreRabbitMq.Common.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace AspNetCoreRabbitMq.Controllers
{
    public class DefaultController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("")]
        public IActionResult Index(EmailModel model)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "EmailQuee",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var message = JsonConvert.SerializeObject(model);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "EmailQuee",
                                     basicProperties: null,
                                     body: body);
            }
            return View();
        }
    }
}