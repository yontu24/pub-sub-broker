package broker;

import java.io.IOException;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ConcurrentMap;

import pub.Publisher;
import sub.Subscriber;
import sub.subscription.Subscription;

public class Broker {
	private Integer brokerId;
	private Publisher publisher = null;
	private Set<Broker> neighbors = new HashSet<>();
	private List<Broker> brokerRoutes = new ArrayList<>();
//	private Set<Subscriber> subscribers = new HashSet<>();

	private final ConcurrentMap<Subscription, Set<Subscriber>> subscriptionsFromSubscribers = new ConcurrentHashMap<>();
//	private final ConcurrentMap<Broker2, Set<Subscription>> subscriptionsFromBrokers = new ConcurrentHashMap<>();

	public Broker() {
		super();
	}

	public Broker(Integer id) {
		this.brokerId = id;
	}

	public void subscribe(Publisher publisher) {
		this.publisher = publisher;
//		this.publisher.sendMessage();
	}

	public Set<Subscription> getSubscriptions() {
		return subscriptionsFromSubscribers.keySet();
	}

	public void registerSubscription(Subscription subscription) {
		Set<Subscriber> subscribers = subscriptionsFromSubscribers.get(subscription);
		if (subscribers == null)
			subscriptionsFromSubscribers.put(subscription, new HashSet<>());
		else {
			subscriptionsFromSubscribers.put(subscription, subscribers);
		}
	}

	public void register(Subscriber subscriber) throws IOException {
		for (Subscription subscription : subscriber.generateSubscriptions()) {
			Set<Subscriber> subscriberss = subscriptionsFromSubscribers.get(subscription);
			if (subscriberss == null)
				subscriptionsFromSubscribers.put(subscription, new HashSet<>(Arrays.asList(subscriber)));
			else {
				subscriberss.add(subscriber);
				subscriptionsFromSubscribers.put(subscription, subscriberss);
			}
		}
//		subscribers.add(subscriber);
	}

	/**
	 * Broker shares his subscriptions with his neighbors brokers
	 * 
	 * @param subscription
	 */
	public void shareSubscriptions() {
		for (Broker neighborBroker2 : neighbors) {
			for (Subscription subscription : this.getSubscriptions()) {
				neighborBroker2.registerSubscription(subscription);
			}
		}
	}

	public void sendMessage(ConcurrentMap<String, String> publication) {
		for (Map.Entry<Subscription, Set<Subscriber>> entry : subscriptionsFromSubscribers.entrySet()) {
			final Set<Subscriber> subs = entry.getValue();
			if (subs.isEmpty())
				continue;
			System.out.println("Broker #" + brokerId + " send matches to subscribers " + subs);
			subs.parallelStream().forEach(subscriber -> subscriber.update(publication));
		}
	}

	public void linkTo(Broker broker) {
		this.neighbors.add(broker);
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

	public Set<Broker> getNeighborsBrokers() {
		return neighbors;
	}

	@Override
	public String toString() {
		StringBuilder sb = new StringBuilder();
		sb.append("\n\tBroker #" + brokerId + " has following subscriptions:\n");
		for (Map.Entry<Subscription, Set<Subscriber>> entry : subscriptionsFromSubscribers.entrySet()) {
			final Set<Subscriber> subs = entry.getValue();
			sb.append(entry.getKey()).append("\n\t").append(subs).append("\n");
		}

		sb.deleteCharAt(sb.length() - 1);
		return sb.toString();
	}

	@Override
	public int hashCode() {
		return Objects.hash(brokerId);
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		Broker other = (Broker) obj;
		return this.brokerId == other.getBrokerId();
	}

	public void createRoutes() {
		Routing routing = new Routing();
		routing.explore(this);
		Broker brokerSubscriber = routing.getBrokerWIthPublisher(); // brokerul care a facut subscribe la publisher
//		System.out.println("Broker " + brokerSubscriber.getBrokerId() + " subscribed to publisher "
//				+ brokerSubscriber.getPublisher().getPublisherId());
		this.brokerRoutes = routing.getRoutes();
//		return this;
	}

	public List<Broker> getBrokerRoutes() {
		return this.brokerRoutes;
	}

	public void showRoutes() {
		StringBuilder sb = new StringBuilder();
		for (Broker broker2 : this.brokerRoutes) {
			sb.append("B").append(broker2.getBrokerId()).append(" -> ");
		}
		sb.delete(sb.length() - 4, sb.length());
		System.out.println("\nBroker " + this.brokerId + " routes: " + sb.toString());
	}

	private class Routing {
		private Broker brokerWithPublisher = null;

		/**
		 * path-ul de la broker-ul curent la broker-ul care a facut subscribe la
		 * publisher
		 */

		/*
		 * 
		 * De creat un Map<Broker2, Boolean> in locul listei daca un Broker2 este
		 * vizitat, setam valoarea pe true
		 * 
		 * Iesim din recursie atunci cand o cheie exista deja in map
		 */

//		private List<Broker> brokersGraph = new ArrayList<>();
		private Map<Broker, Boolean> brokersGraph2 = new HashMap<>();

		private Routing() {
		}

		/**
		 * S ar putea ca brokersGraph2 sa adauge in ordine vecinii, dar getRoutes() ii
		 * ordoneaza
		 * 
		 * @param broker
		 */

		public void explore(Broker broker) {
			if (brokersGraph2.get(broker) == null)
				brokersGraph2.put(broker, false);
			else if (Boolean.FALSE.equals(brokersGraph2.get(broker)))
				brokersGraph2.put(broker, true);
			else {
				brokerWithPublisher = broker;
				return;
			}

//			brokersGraph.add(broker);
//			if (broker.getPublisher() != null) {
//				brokerWithPublisher = broker;
//				return;
//			}

			for (Broker neighbourBroker : broker.getNeighborsBrokers())
				explore(neighbourBroker);
		}

		public Broker getBrokerWIthPublisher() {
			return brokerWithPublisher;
		}

		public List<Broker> getRoutes() {
//			Collections.reverse(brokersGraph);
//			return brokersGraph;
			return new ArrayList<>(brokersGraph2.keySet());
		}
	}
}
