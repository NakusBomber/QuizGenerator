namespace QuizGenerator.ViewModel.Other;

public static class CollectionExtensions
{
	/// <summary>
	/// Shuffle collection. Using <see cref="Random.Shuffle{T}(T[])"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="collection"></param>
	public static void Shuffle<T>(this ICollection<T> collection)
	{
		ArgumentNullException.ThrowIfNull(collection);

		var array = collection.ToArray();
		var rand = new Random();

		rand.Shuffle(array);

		collection.Clear();
		foreach (var element in array)
		{
			collection.Add(element);
		}
	}
}
