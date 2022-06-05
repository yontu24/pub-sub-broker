package sub.subscription.fields;

import java.util.Date;

public class DateField implements SubscriptionField{
    private final Date value;
    private final String operator;

    public DateField(Date value, String operator)
    {
        this.value = value;
        this.operator = operator;
    }

    @Override
    public String toString() {
        return "DateField{" +
                "value=" + value +
                ", operator='" + operator + '\'' +
                '}';
    }
}
