package sub;

import java.io.IOException;
import java.util.List;
import java.util.concurrent.ConcurrentMap;

import sub.subscription.Subscription;

public interface Subscriber {
	void update(ConcurrentMap<String, String> subscription);

	List<Subscription> generateSubscriptions() throws IOException;
}
