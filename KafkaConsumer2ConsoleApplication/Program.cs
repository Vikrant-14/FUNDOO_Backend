using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

public class KafkaConsumerService
{
    private readonly string _bootstrapServers;
    private readonly string _groupId;
    private readonly string _topic;
    private readonly int _consumerId;

    public KafkaConsumerService(string bootstrapServers, string groupId, string topic, int consumerId)
    {
        _bootstrapServers = bootstrapServers;
        _groupId = groupId;
        _topic = topic;
        _consumerId = consumerId;
    }

    public async Task StartConsumingAsync(CancellationToken cancellationToken)
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = _groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig)
            .SetPartitionsAssignedHandler((c, partitions) =>
            {
                Console.WriteLine($"Consumer {_consumerId} assigned to partitions: [{string.Join(", ", partitions)}]");
            })
            .SetPartitionsRevokedHandler((c, partitions) =>
            {
                Console.WriteLine($"Consumer {_consumerId} revoked from partitions: [{string.Join(", ", partitions)}]");
            })
            .Build();

        var partition = new TopicPartition(_topic, new Partition(1));
        consumer.Assign(partition);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(cancellationToken);
                Console.WriteLine($"Consumer {_consumerId}: {consumeResult.Message.Value} from partition: {consumeResult.Partition}");
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        var bootstrapServers = "localhost:9092";
        var groupId = "my-consumer-group";
        var topic = "KafkaTopic1";

        Console.WriteLine("Consumer started. Press [Enter] to stop.");

        var cancellationTokenSource = new CancellationTokenSource();

        var consumer1 = new KafkaConsumerService(bootstrapServers, groupId, topic, 2);
        var consumer2 = new KafkaConsumerService(bootstrapServers, groupId, topic, 9);

        var consumerTask1 = consumer1.StartConsumingAsync(cancellationTokenSource.Token);
        var consumerTask2 = consumer2.StartConsumingAsync(cancellationTokenSource.Token);

        Console.ReadLine();

        cancellationTokenSource.Cancel(); // Stop consuming

        try
        {
            await Task.WhenAll(consumerTask1, consumerTask2); // Wait for all consumers to complete
        }
        catch (OperationCanceledException ie)
        {
            Console.WriteLine(ie.Message);
        }
    }
}
