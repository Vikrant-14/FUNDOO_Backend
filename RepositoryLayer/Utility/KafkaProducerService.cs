using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Utility
{
    public class KafkaProducerService
    {
        public static async Task produceMessage(Label result, IConfiguration _configuration, ProducerConfig _prodConfig)
        {
            int partition1 = 0;
            int partition2 = 1;

            string serializedData = JsonConvert.SerializeObject(result);
            var topic = _configuration.GetValue<string>("TopicConfiguration:TopicName");

            using (var producer = new ProducerBuilder<int, string>(_prodConfig).Build())
            {
                if (result.Id % 2 == 0)
                {
                    await producer.ProduceAsync(new TopicPartition(topic, new Partition(partition1)), new Message<int, string>
                    {
                        Key = partition1,
                        Value = serializedData
                    });
                }
                else
                {
                    await producer.ProduceAsync(new TopicPartition(topic, new Partition(partition2)), new Message<int, string>
                    {
                        Key = partition2,
                        Value = serializedData
                    });
                }
            }
        }
    }
}
