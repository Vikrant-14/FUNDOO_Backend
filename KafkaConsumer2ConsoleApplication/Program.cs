using Confluent.Kafka;

var config = new ConsumerConfig
{
    GroupId = "my-consumer-group",
    BootstrapServers = "localhost:9092"
};

using (var consumer = new ConsumerBuilder<Null, string>(config).Build())
{
    consumer.Subscribe("testdata");

    while (true)
    {
        var labelDetails = consumer.Consume();
        Console.WriteLine(labelDetails.Message.Value);
    }
}