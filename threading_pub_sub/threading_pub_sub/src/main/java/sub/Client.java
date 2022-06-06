package sub;

import java.io.IOException;
import java.io.InputStream;
import java.util.List;
import java.util.Properties;
import java.util.concurrent.ConcurrentMap;

import sub.subscription.Subscription;
import sub.subscription.SubscriptionGenerator;
import sub.subscription.SubscriptionGeneratorSetup;

public class Client implements Subscriber {
	private Integer clientId;

	public Client(Integer clientId) {
		this.clientId = clientId;
	}

	public Integer getClientId() {
		return clientId;
	}

	public void setClientId(Integer clientId) {
		this.clientId = clientId;
	}

	@Override
	public void notify(ConcurrentMap<String, String> subscription) {
		System.out.println(subscription);
	}

	// TODO: prop unique for each client?
	@Override
	public List<Subscription> generateSubscriptions() throws IOException {

		List<Subscription> list = null;
		try (InputStream input = Client.class.getClassLoader().getResourceAsStream("input.properties")) {
			Properties prop = new Properties();
			prop.load(input);
			SubscriptionGeneratorSetup subscriptionGeneratorSetup = new SubscriptionGeneratorSetup(prop);
			SubscriptionGenerator subscriptionGenerator = new SubscriptionGenerator(subscriptionGeneratorSetup, 10, 10);

			list = subscriptionGenerator.generateSubscriptions();
		}
		return list;
	}

	@Override
	public String toString() {
		return "clientId=" + clientId;
	}
}
