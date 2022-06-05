package pub;

import broker.Broker;
import pub.publication.Publication;
import pub.publication.PublicationGenerator;

import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class Publisher implements EventEmitter {
    private final Broker broker;

    public Publisher(Broker broker) {
        this.broker = broker;
    }

    @Override
    public void sendMessage() {
        System.out.println("Notifying");

        try (InputStream input = Broker.class.getClassLoader().getResourceAsStream("input.properties")) {
            Properties prop = new Properties();
            prop.load(input);
            PublicationGenerator publicationGenerator = new PublicationGenerator();

            for (Publication pub : publicationGenerator.generatePublications(Integer.parseInt(prop.getProperty("publicationsNumber")))) {
                broker.sendMessage("testTopic", pub.getMapping());
            }

        } catch (IOException ex) {
            ex.printStackTrace();
        }
    }
}
