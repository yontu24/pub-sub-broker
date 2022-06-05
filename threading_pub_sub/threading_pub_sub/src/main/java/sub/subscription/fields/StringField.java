package sub.subscription.fields;

import java.util.Date;

public class StringField implements SubscriptionField {
    private final String value;
    private final String operator;

    public StringField(String value, String operator)
    {
        this.value = value;
        this.operator = operator;
    }

    @Override
    public String toString() {
        return "StringField{" +
                "value='" + value + '\'' +
                ", operator='" + operator + '\'' +
                '}';
    }
}
