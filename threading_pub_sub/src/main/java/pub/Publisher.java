package pub;

import java.io.IOException;
import java.io.InputStream;
import java.util.List;
import java.util.Properties;

import broker.Broker;
import pub.publication.Publication;
import pub.publication.PublicationGenerator;

public class Publisher implements EventEmitter {
	private Integer publisherId;
	private Broker broker;
	private List<Publication> publications = null;

	public Publisher(Integer id) {
		this.publisherId = id;
	}

	public void subscribe(Broker broker) {
		this.broker = broker;
	}

	public List<Publication> getPublications() {
		return this.publications;
	}

	public Integer getPublisherId() {
		return publisherId;
	}

	public void setPublisherId(Integer publisherId) {
		this.publisherId = publisherId;
	}

	@Override
	public void sendMessage() {
		System.out.println("\nPublisher #" + publisherId + " is notifying broker #" + broker.getBrokerId());

		try (InputStream input = Broker.class.getClassLoader().getResourceAsStream("input.properties")) {
			Properties prop = new Properties();
			prop.load(input);
			int publicationNumber = Integer.parseInt(prop.getProperty("publicationsNumber"));
			PublicationGenerator publicationGenerator = new PublicationGenerator();
			this.publications = publicationGenerator.generatePublications(publicationNumber);

			System.out.println("\n\n\n\n" + broker.getNeighborsBrokers() + "\n\n");
			for (Publication pub : this.publications) {
				broker.getBrokerRoutes().forEach(b -> b.sendMessage(pub.getMapping()));
//				broker.sendMessage(pub.getMapping());
			}
		} catch (IOException ex) {
			ex.printStackTrace();
		}
	}

	@Override
	public String toString() {
		return "Publisher [publisherId=" + publisherId + ", broker=" + broker + ", publications=" + publications + "]";
	}
}
