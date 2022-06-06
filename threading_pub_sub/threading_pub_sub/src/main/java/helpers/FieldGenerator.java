package helpers;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.util.Arrays;
import java.util.List;
import java.util.Random;

public class FieldGenerator {
	protected static final List<String> companies = Arrays.asList("Google", "Bing", "Yahoo", "Fluentis", "Mind",
			"Amazon", "Conduent", "Continental", "Bitdefender");
	protected static final List<LocalDate> dates = Arrays.asList(LocalDate.of(2022, 3, 30), LocalDate.of(2021, 2, 11),
			LocalDate.of(2022, 4, 1), LocalDate.of(2011, 4, 20), LocalDate.of(2000, 1, 1));
	protected static final List<String> operators = Arrays.asList("=", ">", "<", ">=", "<=", "!=");
	public static final Random rng = new Random();
	public static final int RANGE_MIN = 90;
	public static final int RANGE_MAX = 99;

	private FieldGenerator() {
	}

	public static double getRandomDouble() {
		double value = RANGE_MIN + (RANGE_MAX - RANGE_MIN) * rng.nextDouble();
		BigDecimal bd = BigDecimal.valueOf(value);
		bd = bd.setScale(1, RoundingMode.HALF_UP); // 1 cifra dupa virgula
		return bd.doubleValue();
//		return RANGE_MIN + (RANGE_MAX - RANGE_MIN) * rng.nextDouble();
	}

	public static String getRandomCompany() {
		return companies.get(rng.nextInt(companies.size()));
	}

	public static LocalDate getRandomDate() {
		return dates.get(rng.nextInt(dates.size()));
	}

	public static String getRandomOperator() {
		return operators.get(rng.nextInt(operators.size()));
	}
}
