package sub.subscription.fields;

import java.util.Objects;

public class DoubleField implements SubscriptionField {
	private final Double value;
	private final String operator;
	private final String fieldName;

	public DoubleField(Double value, String operator, String fieldName) {
		this.value = value;
		this.operator = operator;
		this.fieldName = fieldName;
	}

	@Override
	public String toString() {
		return "value=" + value + ", operator='" + operator + '\'';
	}

	public Double getValue() {
		return value;
	}

	public String getOperator() {
		return operator;
	}

	public String getFieldName() {
		return fieldName;
	}

	@Override
	public int hashCode() {
		return Objects.hash(fieldName, operator, value);
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		DoubleField other = (DoubleField) obj;
		return fieldName.equals(other.getFieldName()) && operator.equals(other.getOperator())
				&& value == other.getValue();
	}
}
