import java.io.IOException;

import broker.Broker;
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

		Publisher publisher1 = new Publisher(1);
		Publisher publisher2 = new Publisher(2);

		Broker broker1 = new Broker(1);
		Broker broker2 = new Broker(2);
		Broker broker3 = new Broker(3);

		// broker3.subscribe(publisher1);
		// publisher1.subscribe(broker3);
		publisher2.subscribe(broker2);
		broker2.subscribe(publisher2);

		broker1.register(new Client(1));
		broker1.register(new Client(2));
		broker1.register(new Client(3));

//		broker2.register(null);	// err
		broker2.register(new Client(7));

		broker3.register(new Client(4));
		broker3.register(new Client(5));

		broker1.linkTo(broker2);
//		broker1.linkTo(broker3);
		broker2.linkTo(broker3);
		broker3.linkTo(broker1);
//		broker3.linkTo(broker2);	// se face un ciclu

		broker1.createRoutes();
		broker1.showRoutes();
		broker2.createRoutes();
		broker2.showRoutes();
		broker3.createRoutes();
		broker3.showRoutes();

		broker1.shareSubscriptions();
		broker2.shareSubscriptions();
		broker3.shareSubscriptions();

		System.out.println(broker1);
		System.out.println(broker2);
		System.out.println(broker3);

		publisher1.sendMessage();
	}
}
