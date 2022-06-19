using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace SwarmPortal.Common;

public sealed class HierarchichalDictionary<TValue> : IDictionary<string, HierarchichalDictionary<TValue>>
{  
    
    HierarchichalDictionary<TValue> IDictionary<string, HierarchichalDictionary<TValue>>.this[string key] { get => this[key]; set => this[key] = value; }
    public HierarchichalDictionary<TValue> this[string key] 
    {
        get => Children[key];
        private set 
        {
            Children[key] = value;
        }
    }
    public HierarchichalDictionary<TValue>? NavigateTo(IEnumerable<string> keys)
    {
        if (keys.Any())
        {
            return this.ContainsChild(keys.First()) ? this[keys.First()].NavigateTo(keys.Skip(1)) : null;
        }
        else 
        {
            return this;
        }
    }

    private Dictionary<string, HierarchichalDictionary<TValue>> Children { get; set; } = new Dictionary<string, HierarchichalDictionary<TValue>>();
    public TValue? Value { get; set; } = default(TValue);
    public bool ContainsChild(string key) => Children.ContainsKey(key);
    public ICollection<string> Keys => Children.Keys;

    ICollection<HierarchichalDictionary<TValue>> IDictionary<string, HierarchichalDictionary<TValue>>.Values => Children.Values;

    int ICollection<KeyValuePair<string, HierarchichalDictionary<TValue>>>.Count => Children.Count;

    bool ICollection<KeyValuePair<string, HierarchichalDictionary<TValue>>>.IsReadOnly => ((ICollection<KeyValuePair<string, HierarchichalDictionary<TValue>>>)Children).IsReadOnly;


    public static HierarchichalDictionary<TValue> ConvertToHierarchy(IDictionary<string, TValue> labels)
    {
        Dictionary<string, HierarchichalDictionary<TValue>> labelLevels = new();
        var rootLevel = new HierarchichalDictionary<TValue>{ 
            Children = labelLevels
        };
        HierarchichalDictionary<TValue> currentLevel = rootLevel;
        foreach (var label in labels)
        {
            var parts = label.Key.Split('.', StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                if (currentLevel.Children.ContainsKey(part))
                {
                    currentLevel = currentLevel.Children[part];
                }
                else
                {
                    currentLevel.Children[part] = new HierarchichalDictionary<TValue> { Children = new Dictionary<string, HierarchichalDictionary<TValue>>() };
                    currentLevel = currentLevel.Children[part];
                }
            }
            currentLevel.Value = label.Value;
            currentLevel = rootLevel;
        }
        return rootLevel;
    }

    void IDictionary<string, HierarchichalDictionary<TValue>>.Add(string key, HierarchichalDictionary<TValue> value)
    {
        Children.Add(key, value);
    }

    bool IDictionary<string, HierarchichalDictionary<TValue>>.ContainsKey(string key)
    {
        return Children.ContainsKey(key);
    }

    bool IDictionary<string, HierarchichalDictionary<TValue>>.Remove(string key)
    {
        return Children.Remove(key);
    }

    bool IDictionary<string, HierarchichalDictionary<TValue>>.TryGetValue(string key, out HierarchichalDictionary<TValue> value)
    {
        return Children.TryGetValue(key, out value);
    }

    void ICollection<KeyValuePair<string, HierarchichalDictionary<TValue>>>.Add(KeyValuePair<string, HierarchichalDictionary<TValue>> item)
    {
        Children.Add(item.Key, item.Value);
    }

    void ICollection<KeyValuePair<string, HierarchichalDictionary<TValue>>>.Clear()
    {
        Children.Clear();
    }

    bool ICollection<KeyValuePair<string, HierarchichalDictionary<TValue>>>.Contains(KeyValuePair<string, HierarchichalDictionary<TValue>> item)
    {
        return Children.Contains(item);
    }

    void ICollection<KeyValuePair<string, HierarchichalDictionary<TValue>>>.CopyTo(KeyValuePair<string, HierarchichalDictionary<TValue>>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<string, HierarchichalDictionary<TValue>>>)Children).CopyTo(array, arrayIndex);
    }

    bool ICollection<KeyValuePair<string, HierarchichalDictionary<TValue>>>.Remove(KeyValuePair<string, HierarchichalDictionary<TValue>> item)
    {
        return Children.Remove(item.Key);
    }

    IEnumerator<KeyValuePair<string, HierarchichalDictionary<TValue>>> IEnumerable<KeyValuePair<string, HierarchichalDictionary<TValue>>>.GetEnumerator()
    {
        return Children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Children.GetEnumerator();
    }
}
