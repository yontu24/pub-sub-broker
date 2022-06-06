package broker;

import java.util.HashSet;
import java.util.Set;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ConcurrentMap;

import sub.Subscriber;

public class Broker extends Thread {
	private final Object mutex = new Object();
	private static final Broker brokerInstance = new Broker();

	// obiect ce tine evidenta subscriber-ilor la o publicatie (in cazu asta e un
	// String publicatia)
	private final ConcurrentMap<String, Set<Subscriber>> subscribers = new ConcurrentHashMap<>();

	public static Broker getInstance() {
		return brokerInstance;
	}

	public boolean register(String topic, Subscriber subscriber) {
		boolean returnVal;
		synchronized (mutex) {
			if (subscribers.containsKey(topic))
				returnVal = subscribers.get(topic).add(subscriber);
			else {
				Set<Subscriber> sub = new HashSet<>();
				returnVal = sub.add(subscriber);
				subscribers.put(topic, sub);
			}
		}
		return returnVal;
	}

	public void sendMessage(String topic, ConcurrentMap<String, String> map) {
		synchronized (mutex) {
			final Set<Subscriber> sub = this.subscribers.get(topic);
			sub.parallelStream().forEach(subscriber -> subscriber.notify(map));
		}
	}
}
