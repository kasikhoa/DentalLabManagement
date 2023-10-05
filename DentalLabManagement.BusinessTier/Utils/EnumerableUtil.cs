namespace DentalLabManagement.BusinessTier.Utils;

public static class EnumerableUtil
{
	public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> source)
	{
		return source ?? Enumerable.Empty<T>();
	}
}