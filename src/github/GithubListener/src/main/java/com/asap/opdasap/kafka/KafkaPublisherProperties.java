package com.asap.opdasap.kafka;

import lombok.Data;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.stereotype.Component;

@Data
@Component
@ConfigurationProperties(prefix = "kafka.publisher")
public class KafkaPublisherProperties {
    private String bootstrapServers;
    private String topic;
    private int batchSize = 16384; // Default Kafka batch
    private int lingerMs = 1;
    private int chunkSize = 100; // Аналогично твоему chunk size
}