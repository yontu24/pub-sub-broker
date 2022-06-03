package org.example;
import java.time.Duration;
import java.util.Properties;
import java.util.Arrays;
import org.apache.kafka.clients.consumer.KafkaConsumer;
import org.apache.kafka.clients.consumer.ConsumerRecords;
import org.apache.kafka.clients.consumer.ConsumerRecord;

public class ConsumerGroup {
    public static void main(String[] args) throws Exception {
//        if(args.length < 2){
//            System.out.println("Usage: consumer <topic> <groupname>");
//            return;
//        }
//
//        String topic = args[0].toString();
//        String group = args[1].toString();

        String[] topics = {"testTopic_1", "testTopic_2", "testTopic_3"};
        // can only belong to on group, i guess
        // String[] groups = {"testGroup_1", "testGroup_2", "testGroup_3"};
        String group = "testGroup_1";

        Properties props = new Properties();
        props.put("bootstrap.servers", "localhost:9092");
        props.put("group.id", group);
        props.put("enable.auto.commit", "true");
        props.put("auto.commit.interval.ms", "1000");
        props.put("session.timeout.ms", "30000");
        props.put("key.deserializer",
                "org.apache.kafka.common.serialization.ByteArrayDeserializer");
        props.put("value.deserializer",
                "org.apache.kafka.common.serialization.StringDeserializer");
        KafkaConsumer<String, String> consumer = new KafkaConsumer<String, String>(props);

        consumer.subscribe(Arrays.asList(topics));
        System.out.println("Subscribed to topic " + topics.toString());
        int i = 0;

        while (true) {
            ConsumerRecords<String, String> records = consumer.poll(Duration.ofSeconds(100));
            for (ConsumerRecord<String, String> record : records)
                System.out.printf("offset = %d, key = %s, value = %s\n",
                        record.offset(), record.key(), record.value());
        }
    }
}
