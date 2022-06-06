package pub.publication;

import java.util.ArrayList;
import java.util.List;

import helpers.FieldGenerator;

public class PublicationGenerator {
	public List<Publication> generatePublications(int messageNumber) {
		List<Publication> publications = new ArrayList<>();

		for (int i = 0; i < messageNumber; ++i)
			publications.add(generatePublication());

		return publications;
	}

	public Publication generatePublication() {
		return new Publication(FieldGenerator.getRandomCompany(), FieldGenerator.getRandomDouble(),
				FieldGenerator.getRandomDouble(), FieldGenerator.getRandomDouble(), FieldGenerator.getRandomDate());
	}
}