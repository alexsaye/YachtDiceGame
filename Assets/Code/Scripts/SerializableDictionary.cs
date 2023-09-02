
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A serializable dictionary, because Unity still can't serialize Dictionary<TKey, TValue>.
/// </summary>
[Serializable]
public class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    /// <summary>
    /// The actual dictionary through which the interface is implemented.
    /// </summary>
    private readonly IDictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    public ICollection<TKey> Keys => dictionary.Keys;

    public ICollection<TValue> Values => dictionary.Values;

    public int Count => dictionary.Count;

    public bool IsReadOnly => dictionary.IsReadOnly;

    public TValue this[TKey key]
    {
        get => dictionary[key];
        set => dictionary[key] = value;
    }

    public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);

    public void Add(TKey key, TValue value) => dictionary.Add(key, value);

    public void Add(KeyValuePair<TKey, TValue> item) => dictionary.Add(item);

    public bool Remove(TKey key) => dictionary.Remove(key);

    public bool Remove(KeyValuePair<TKey, TValue> item) => dictionary.Remove(item);

    public void Clear() => dictionary.Clear();

    public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);

    public bool Contains(KeyValuePair<TKey, TValue> item) => dictionary.Contains(item);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => dictionary.CopyTo(array, arrayIndex);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dictionary.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)dictionary).GetEnumerator();

    /// <summary>
    /// A serializable dictionary entry, because Unity still can't serialize KeyValuePair<TKey, TValue>.
    /// </summary>
    [Serializable]
    private struct Entry
    {
        public TKey Key;
        public TValue Value;
    }

    /// <summary>
    /// A list of dictionary entries which populate the actual dictionary on deserialization.
    /// </summary>
    [SerializeField]
    private List<Entry> entries;

    public SerializableDictionary()
    {
        entries ??= new List<Entry>();
    }

    private bool ValidateEntries()
    {
        var groups = entries
            .GroupBy(entry => entry.Key)
            .Where(group => group.Count() > 1);

        if (groups.Any())
        {
            Debug.LogWarning($"Duplicate keys in {nameof(SerializableDictionary<TKey, TValue>)}: {string.Join(", ", groups.Select(group => $"{group.Key} ({group.Count()} times)"))}");
            return false;
        }
        return true;
    }

    public void OnAfterDeserialize()
    {
        if (!ValidateEntries())
            return;

        dictionary.Clear();
        foreach (var entry in entries)
        {
            dictionary.Add(entry.Key, entry.Value);
        }
    }

    public void OnBeforeSerialize()
    {
        if (!ValidateEntries())
            return;

        entries.Clear();
        foreach (var entry in dictionary)
        {
            entries.Add(new Entry { Key = entry.Key, Value = entry.Value });
        }
    }
}
