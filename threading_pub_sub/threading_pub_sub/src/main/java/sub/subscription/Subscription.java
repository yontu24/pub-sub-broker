package sub.subscription;

import sub.subscription.fields.DateField;
import sub.subscription.fields.DoubleField;
import sub.subscription.fields.StringField;

public class Subscription {
    private StringField company;
    private DoubleField value;
    private DoubleField drop;
    private DoubleField variation;
    private DateField date;

    public Subscription(){

    }

    public Subscription(StringField company, DoubleField value, DoubleField drop, DoubleField variation, DateField date)
    {
        this.company = company;
        this.value = value;
        this.drop = drop;
        this.variation = variation;
        this.date = date;
    }

    @Override
    public String toString() {
        String stringContent = "";

        if(company != null)
            stringContent += "company='" + company.toString() + '\'';

        if(value != null)
            stringContent += " value=" + value.toString();

        if(drop != null)
            stringContent += " drop=" + drop.toString();

        if(variation != null)
            stringContent += " variation=" + variation.toString();

        if(date != null)
            stringContent += " date=" + date.toString();

        return "Subscription{" +
                stringContent +
                '}';
    }

    public StringField getCompany() {
        return company;
    }

    public DoubleField getValue() {
        return value;
    }

    public DoubleField getDrop() {
        return drop;
    }

    public DoubleField getVariation() {
        return variation;
    }

    public DateField getDate() {
        return date;
    }

    public void setCompany(StringField company) {
        this.company = company;
    }

    public void setValue(DoubleField value) {
        this.value = value;
    }

    public void setDrop(DoubleField drop) {
        this.drop = drop;
    }

    public void setVariation(DoubleField variation) {
        this.variation = variation;
    }

    public void setDate(DateField date) {
        this.date = date;
    }
}
