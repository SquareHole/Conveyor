// Based on the post https://khalidabuhakmeh.com/implement-kotlins-withindex-in-csharp
using System.Collections.Generic;
using System.Linq;

namespace Talista.Extensions
{
    public static class EnumerableExtensions
    {
	    /// <summary>
	    /// Get an indexed enumerable tuple(int, T) for the source list
	    /// </summary>
	    /// <param name="source">Source IEnumerable[T]</param>
	    /// <param name="zeroBased">When true (default) indexing starts af zero. When false, indexing starts at one.</param>
	    /// <typeparam name="T"></typeparam>
	    /// <returns></returns>
	    public static IEnumerable<(int, T)> WithIndex<T>(this IEnumerable<T> source, bool zeroBased = true )
	    {
		    var positionShift = zeroBased ? 0 : 1;
		    return source.Select((value, index) => ((index + positionShift), value));
	    }
    }
}
