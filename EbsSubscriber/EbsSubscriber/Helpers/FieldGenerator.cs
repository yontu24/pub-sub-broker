namespace EbsSubscriber.Helpers
{
    public static class FieldGenerator
    {
		private static readonly List<String> Companies = new List<string>() { "Google", "Bing", "Yahoo", "Fluentis", "Mind",
			"Amazon", "Conduent", "Continental", "Bitdefender" };
		private static readonly List<DateTime> Dates = new List<DateTime>() { new DateTime(2022, 3, 30), new DateTime(2021, 2, 11),
				new DateTime(2022, 4, 1), new DateTime(2011, 4, 20), new DateTime(2000, 1, 1) };
		private static readonly List<String> Operators = new List<string>() { "=", ">", "<", ">=", "<=", "!=" };
		public static readonly Random rng = new Random();
		public static readonly int RANGE_MIN = 90;
		public static readonly int RANGE_MAX = 99;

		public static double GetRandomDouble()
		{
			double value = RANGE_MIN + (RANGE_MAX - RANGE_MIN) * rng.NextDouble();
			return value;

			/*BigDecimal bd = BigDecimal.valueOf(value);
			bd = bd.setScale(1, RoundingMode.HALF_UP); // 1 cifra dupa virgula
			return bd.doubleValue();*/
			//		return RANGE_MIN + (RANGE_MAX - RANGE_MIN) * rng.nextDouble();
		}

		public static String GetRandomCompany()
		{
			return Companies[rng.Next(Companies.Count)];
		}

		public static DateTime GetRandomDate()
		{
			return Dates[rng.Next(Dates.Count)];
		}

		public static String GetRandomOperator()
		{
			return Operators[rng.Next(Operators.Count)];
		}
	}
}
