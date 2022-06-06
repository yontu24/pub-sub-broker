package pub.publication;

import java.time.LocalDate;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ConcurrentMap;

public class Publication {
	private final String company;
	private final double value;
	private final double drop;
	private final double variation;
	private final LocalDate date;

	public Publication(String company, double value, double drop, double variation, LocalDate date) {
		this.company = company;
		this.value = value;
		this.drop = drop;
		this.variation = variation;
		this.date = date;
	}

	@Override
	public String toString() {
		return "Publication{" + "company='" + company + '\'' + ", value=" + value + ", drop=" + drop + ", variation="
				+ variation + ", date=" + date + '}';
	}

	public ConcurrentMap<String, String> getMapping() {
		ConcurrentMap<String, String> mapping = new ConcurrentHashMap<>();
		mapping.put("company", company);
		mapping.put("value", String.valueOf(value));
		mapping.put("drop", String.valueOf(drop));
		mapping.put("variation", String.valueOf(variation));
		mapping.put("date", date.toString());

		return mapping;
	}

	public String getCompany() {
		return company;
	}

	public double getValue() {
		return value;
	}

	public double getDrop() {
		return drop;
	}

	public double getVariation() {
		return variation;
	}

	public LocalDate getDate() {
		return date;
	}
}
