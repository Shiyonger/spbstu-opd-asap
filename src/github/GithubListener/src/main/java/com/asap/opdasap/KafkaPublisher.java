package com.asap.opdasap;

import lombok.RequiredArgsConstructor;
import org.apache.kafka.clients.producer.RecordMetadata;
import org.springframework.kafka.core.KafkaTemplate;
import org.springframework.kafka.support.SendResult;
import org.springframework.stereotype.Service;

import java.util.concurrent.CompletableFuture;

@Service
@RequiredArgsConstructor
public class KafkaPublisher {

    private final KafkaTemplate<String, Object> kafkaTemplate;

    public void publish(String topic, Object value) {
        publish(topic, null, value); // null — если ключ не требуется
    }

    public void publish(String topic, String key, Object value) {
        CompletableFuture<SendResult<String, Object>> future = kafkaTemplate.send(topic, key, value);
        future.whenComplete((result, ex) -> {
            if (ex != null) {
                System.err.println("❌ Failed to send: " + ex.getMessage());
            } else {
                RecordMetadata metadata = result.getRecordMetadata();
                System.out.printf("✅ Sent to %s-%d offset=%d%n",
                        metadata.topic(), metadata.partition(), metadata.offset());
            }
        });
    }
}
