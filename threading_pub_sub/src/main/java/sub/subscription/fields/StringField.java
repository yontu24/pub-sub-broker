package sub.subscription.fields;

import java.util.Objects;

public class StringField implements SubscriptionField {
	private final String value;
	private final String operator;

	public StringField(String value, String operator) {
		this.value = value;
		this.operator = operator;
	}

	public String getValue() {
		return value;
	}

	public String getOperator() {
		return operator;
	}

	@Override
	public String toString() {
		return value + '\'' + ", operator='" + operator;
	}

	@Override
	public int hashCode() {
		return Objects.hash(operator, value);
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		StringField other = (StringField) obj;
		return operator.equals(other.getOperator()) && value.equals(other.getValue());
	}
}
