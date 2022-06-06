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
	private final Broker broker;
	private List<Publication> publications = null;

	public Publisher(Broker broker, Integer id) {
		this.broker = broker;
		this.publisherId = id;
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
		System.out.println("Notifying");

		try (InputStream input = Broker.class.getClassLoader().getResourceAsStream("input.properties")) {
			Properties prop = new Properties();
			prop.load(input);
			int publicationNumber = Integer.parseInt(prop.getProperty("publicationsNumber"));
			PublicationGenerator publicationGenerator = new PublicationGenerator();
			this.publications = publicationGenerator.generatePublications(publicationNumber);

			for (Publication pub : this.publications) {
				broker.sendMessage("testTopic", pub.getMapping());
			}

		} catch (IOException ex) {
			ex.printStackTrace();
		}
	}

	@Override
	public String toString() {
		return "Publisher [publisherId=" + publisherId + ", broker=" + broker.getId() + ", publications=" + publications
				+ "]";
	}
}
