using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text;

//Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
var factory = new ConnectionFactory
{
    HostName = "localhost"
};

//Create the RabbitMQ connection using connection factory details 
var connection = factory.CreateConnection();

//Here we create channel with session and model 
using var channel = connection.CreateModel();

//declare the queue after mentioning name and a few property related to that
channel.QueueDeclare("NoteQueue", exclusive: false);

//Set Event object which listen message from channel which is sent by producer
var consumer  = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Note Message received : {message}");
};

//read message 
channel.BasicConsume(queue: "NoteQueue", autoAck: true ,consumer: consumer);
Console.ReadLine();