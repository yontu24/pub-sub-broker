public class Main {
    public static void main(String[] args) {
        Broker broker = Broker.getInstance();
        Client subscriberClient = new Client();
        broker.register(Broker.Publications.ON_MARKET, subscriberClient);

        SubscriptionEmitter emitter = new SubscriptionEmitter(broker);
        emitter.sendMessage();
    }
}
