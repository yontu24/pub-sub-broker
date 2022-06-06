import java.io.IOException;

import broker.Broker2;
import pub.Publisher;
import sub.Client;

public class Main {
	public static void main(String[] args) throws IOException {
//		Broker broker = Broker.getInstance();
//		Client subscriberClient = new Client(1);
//		broker.register("testTopic", subscriberClient);
//
//		List<Subscription> subscriptions = subscriberClient.generateSubscriptions();
//		subscriptions.forEach(System.out::println);
//
//		Publisher emitter = new Publisher(broker, 1);
//		emitter.sendMessage();

		Publisher publisher1 = new Publisher(null, 1);
		Publisher publisher2 = new Publisher(null, 2);

		Broker2 broker1 = new Broker2(1);
		Broker2 broker2 = new Broker2(2);
		Broker2 broker3 = new Broker2(3);

		broker2.subscribe(publisher1);

		broker1.register(new Client(1));
		broker1.register(new Client(2));
		broker1.register(new Client(3));

//		broker2.register(null);	// err

		broker3.register(new Client(3));
		broker3.register(new Client(4));

		broker1.linkTo(broker2);
		broker2.linkTo(broker3);
		broker3.linkTo(broker1);

		System.out.println(broker1);
		System.out.println(broker2);
		System.out.println(broker3);
	}
}
