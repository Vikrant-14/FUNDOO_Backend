using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

public class KafkaTopicCreator
{
    private readonly ProducerConfig _producerConfig;
    private readonly IConfiguration _configuration;
    private readonly ILogger<KafkaTopicCreator> _logger;

    public KafkaTopicCreator(ProducerConfig producerConfig, IConfiguration configuration, ILogger<KafkaTopicCreator> logger)
    {
        _producerConfig = producerConfig;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task CreateTopicAsync()
    {
        var topicConfig = _configuration.GetSection("TopicConfiguration").Get<TopicConfiguration>();

        using (var adminClient = new AdminClientBuilder(_producerConfig).Build())
        {
            try
            {
                var topicSpecification = new TopicSpecification
                {
                    Name = topicConfig.TopicName,
                    NumPartitions = topicConfig.NumPartitions,
                    ReplicationFactor = topicConfig.ReplicationFactor
                };

                await adminClient.CreateTopicsAsync(new[] { topicSpecification });

                await Console.Out.WriteLineAsync($"Topic {topicSpecification.Name} created successfully");
                _logger.LogInformation($"Topic {topicConfig.TopicName} created successfully.");
            }
            catch (CreateTopicsException e)
            {
                if (e.Results[0].Error.Code != ErrorCode.TopicAlreadyExists)
                {
                    _logger.LogError($"An error occurred creating topic {topicConfig.TopicName}: {e.Results[0].Error.Reason}");
                }
                else
                {
                    _logger.LogInformation($"Topic {topicConfig.TopicName} already exists.");
                }
            }
        }
    }
}

public class TopicConfiguration
{
    public string TopicName { get; set; }
    public int NumPartitions { get; set; }
    public short ReplicationFactor { get; set; }
}
 