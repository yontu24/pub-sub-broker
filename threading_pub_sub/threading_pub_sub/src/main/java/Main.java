import broker.Broker;
import pub.Publisher;
import sub.Client;

public class Main {
    public static void main(String[] args) {
        Broker broker = Broker.getInstance();
        Client subscriberClient = new Client();
        broker.register("testTopic", subscriberClient);

        Publisher emitter = new Publisher(broker);
        emitter.sendMessage();
    }
}
