import java.util.concurrent.ConcurrentMap;

public interface Subscriber {
    void notify(ConcurrentMap<String, String> subscription);
}
