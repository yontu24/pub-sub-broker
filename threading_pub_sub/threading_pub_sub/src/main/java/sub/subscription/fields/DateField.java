package sub.subscription.fields;

import java.time.LocalDate;
import java.util.Objects;

public class DateField implements SubscriptionField {
	private final LocalDate value;
	private final String operator;

	public DateField(LocalDate value, String operator) {
		this.value = value;
		this.operator = operator;
	}

	public LocalDate getValue() {
		return value;
	}

	public String getOperator() {
		return operator;
	}

	@Override
	public String toString() {
		return "DateField{" + "value=" + value + ", operator='" + operator + '\'' + '}';
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
		DateField other = (DateField) obj;
		return operator.equals(other.getOperator()) && value.getMonthValue() == other.getValue().getMonthValue()
				&& value.getYear() == other.getValue().getYear();
	}
}
