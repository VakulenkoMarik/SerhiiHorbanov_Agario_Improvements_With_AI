namespace MyEngine.Utils;

public static class CollectionExtensions
{
    public static void TryAdd<T1, T2>(this ICollection<T1> collection, T2 added)
    {
        if (added is T1 addedAsT1)
            collection.Add(addedAsT1);
    }

    public static void TrySwapRemove<T1, T2>(this List<T1> list, T2 item) where T1 : class
    {
        if (item is T1 itemAsT1)
            list.SwapRemove(itemAsT1);
    }
    
    public static void Enqueue<T>(this Queue<T> queue, IEnumerable<T> enqueued)
    {
        foreach (T each in enqueued)
            queue.Enqueue(each);
    }

    public static void SwapRemove<T>(this List<T> list, T item) where T : class
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == item)
            {
                list.SwapRemoveAt(i);
                return;
            }
        }
    }
    
    public static void SwapRemoveAt<T>(this List<T> list, int index)
    {
        list.Swap(index, list.Count - 1);
        list.RemoveAt(list.Count - 1);
    }

    private static void Swap<T>(this List<T> list, int first, int second)
        => (list[first], list[second]) = (list[second], list[first]);
}