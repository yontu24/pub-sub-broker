import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ConcurrentMap;

public class SubscriptionEmitter implements EventEmitter {
    private final Broker broker;

    public SubscriptionEmitter(Broker broker) {
        this.broker = broker;
    }

    @Override
    public void sendMessage() {
        System.out.println("Notified");

        // aici intervine filtrarea
        ConcurrentMap<String, String> filteredPubs = new ConcurrentHashMap<>();
        filteredPubs.put("oldCachedValue", "Yes");
        filteredPubs.put("newCachedValue", "No");

        // si notificam
        broker.sendMessage(Broker.Publications.ON_MARKET, filteredPubs);
    }
}
