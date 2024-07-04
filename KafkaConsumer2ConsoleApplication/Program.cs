using Confluent.Kafka;
using System;

var config = new ConsumerConfig
{
    GroupId = "my-consumer-group-2", // Different group ID
    BootstrapServers = "localhost:9092",
    AutoOffsetReset = AutoOffsetReset.Earliest // Ensures the consumer starts from the beginning if no previous offset is found
};

using (var consumer = new ConsumerBuilder<Null, string>(config).Build())
{
    consumer.Subscribe("testdata");

    CancellationTokenSource token = new();

    try
    {
        while (true)
        {
            var labelDetails = consumer.Consume(token.Token);
            Console.WriteLine($"Consumer 2: {labelDetails.Message.Value}");
        }
    }
    catch (Exception)
    {
        throw;
    }
        
}