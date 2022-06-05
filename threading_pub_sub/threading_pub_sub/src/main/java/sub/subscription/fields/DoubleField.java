package sub.subscription.fields;

public class DoubleField implements SubscriptionField{
    private final Double value;
    private final String operator;
    private final String fieldName;

    public DoubleField(Double value, String operator, String fieldName)
    {
        this.value = value;
        this.operator = operator;
        this.fieldName = fieldName;
    }

    @Override
    public String toString() {
        return "DoubleField{" +
                "value=" + value +
                ", operator='" + operator + '\'' +
                '}';
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
}
