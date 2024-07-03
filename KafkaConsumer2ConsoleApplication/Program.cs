using Confluent.Kafka;
using System;

var config = new ConsumerConfig
{
    GroupId = "my-consumer-group-2", // Different group ID
    BootstrapServers = "localhost:9092"
};

using (var consumer = new ConsumerBuilder<Null, string>(config).Build())
{
    consumer.Subscribe("testdata");

    while (true)
    {
        var labelDetails = consumer.Consume();
        Console.WriteLine($"Consumer 2: {labelDetails.Message.Value}");
    }
}