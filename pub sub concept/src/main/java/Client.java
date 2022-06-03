import java.util.concurrent.ConcurrentMap;

public class Client implements Subscriber {
    @Override
    public void notify(ConcurrentMap<String, String> subscription) {
        System.out.println(subscription.get("oldCachedValue"));
        System.out.println(subscription.get("newCachedValue"));
    }
}
