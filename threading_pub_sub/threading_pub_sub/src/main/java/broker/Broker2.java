package broker;

import java.io.IOException;
import java.util.HashSet;
import java.util.Set;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ConcurrentMap;

import pub.Publisher;
import sub.Subscriber;
import sub.subscription.Subscription;

public class Broker2 {
	private Integer brokerId;
	private Publisher publisher = null;
	private Set<Broker2> brokers = new HashSet<>();
//	private Set<Subscriber> subscribers = new HashSet<>();

	private final ConcurrentMap<Subscription, Set<Subscriber>> subscriptionsFromSubscribers = new ConcurrentHashMap<>();
	private final ConcurrentMap<Broker2, Set<Subscription>> subscriptionsFromBrokers = new ConcurrentHashMap<>();

	public Broker2() {
		super();
	}

	public Broker2(Integer id) {
		this.brokerId = id;
	}

	public void subscribe(Publisher publisher) {
		this.publisher = publisher;
	}

	public void register(Subscriber subscriber) throws IOException {
		for (Subscription subscription : subscriber.generateSubscriptions()) {
			Set<Subscriber> subscriberss = subscriptionsFromSubscribers.get(subscription);
			if (subscriberss == null)
				subscriptionsFromSubscribers.put(subscription, new HashSet<>());
			else {
				subscriberss.add(subscriber);
				subscriptionsFromSubscribers.put(subscription, subscriberss);
			}
		}
//		subscribers.add(subscriber);
	}

	public void linkTo(Broker2 broker) {
		this.brokers.add(broker);
	}
//
//	public Set<Subscriber> getSubscribers() {
//		return subscribers;
//	}
//
//	public void setSubscribers(Set<Subscriber> subscribers) {
//		this.subscribers = subscribers;
//	}

	public Publisher getPublisher() {
		return publisher;
	}

	public void setPublisher(Publisher publisher) {
		this.publisher = publisher;
	}

	public Integer getBrokerId() {
		return brokerId;
	}

	public void setBrokerId(Integer brokerId) {
		this.brokerId = brokerId;
	}

	public Set<Broker2> getBrokers() {
		return brokers;
	}

	public void setBrokers(Set<Broker2> brokers) {
		this.brokers = brokers;
	}

	@Override
	public String toString() {
		return "Broker2 [brokerId=" + brokerId + ", subscriptionsFromSubscribers=" + subscriptionsFromSubscribers + "]";
	}

//	@Override
//	public String toString() {
//		StringBuilder sBuilder = new StringBuilder();
//		for (Broker2 broker2 : brokers)
//			sBuilder.append(broker2.getBrokerId()).append(" ");
//		sBuilder.deleteCharAt(sBuilder.length() - 1);
//		if (publisher == null)
//			return "Broker " + brokerId + " [subscribers=" + subscribers + ", brokers=" + sBuilder.toString() + "]";
//		return "Broker " + brokerId + " [subscribers=" + subscribers + ", publisher=" + publisher.getPublisherId()
//				+ ", brokers=" + sBuilder.toString() + "]";
//	}

}
