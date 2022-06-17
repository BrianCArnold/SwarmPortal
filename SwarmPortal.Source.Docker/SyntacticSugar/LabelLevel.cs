using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace SwarmPortal.Source.Docker;

public sealed class LabelHierarchy : IDictionary<string, LabelHierarchy>
{  
    
    LabelHierarchy IDictionary<string, LabelHierarchy>.this[string key] { get => this[key]; set => this[key] = value; }
    public LabelHierarchy this[string key] 
    {
        get => Children[key];
        private set 
        {
            Children[key] = value;
        }
    }
    public LabelHierarchy? NavigateTo(IEnumerable<string> keys)
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

    private Dictionary<string, LabelHierarchy> Children { get; set; } = new Dictionary<string, LabelHierarchy>();
    public string Value { get; set; } = string.Empty;
    public bool ContainsChild(string key) => Children.ContainsKey(key);
    public ICollection<string> Keys => Children.Keys;

    ICollection<LabelHierarchy> IDictionary<string, LabelHierarchy>.Values => Children.Values;

    int ICollection<KeyValuePair<string, LabelHierarchy>>.Count => Children.Count;

    bool ICollection<KeyValuePair<string, LabelHierarchy>>.IsReadOnly => ((ICollection<KeyValuePair<string, LabelHierarchy>>)Children).IsReadOnly;


    public static LabelHierarchy ConvertToHierarchy(IDictionary<string, string> labels)
    {
        Dictionary<string, LabelHierarchy> labelLevels = new();
        var rootLevel = new LabelHierarchy{ 
            Children = labelLevels
        };
        LabelHierarchy currentLevel = rootLevel;
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
                    currentLevel.Children[part] = new LabelHierarchy { Children = new Dictionary<string, LabelHierarchy>() };
                    currentLevel = currentLevel.Children[part];
                }
            }
            currentLevel.Value.Add(label.Value);
            currentLevel = rootLevel;
        }
        return rootLevel;
    }

    void IDictionary<string, LabelHierarchy>.Add(string key, LabelHierarchy value)
    {
        Children.Add(key, value);
    }

    bool IDictionary<string, LabelHierarchy>.ContainsKey(string key)
    {
        return Children.ContainsKey(key);
    }

    bool IDictionary<string, LabelHierarchy>.Remove(string key)
    {
        return Children.Remove(key);
    }

    bool IDictionary<string, LabelHierarchy>.TryGetValue(string key, out LabelHierarchy value)
    {
        return Children.TryGetValue(key, out value);
    }

    void ICollection<KeyValuePair<string, LabelHierarchy>>.Add(KeyValuePair<string, LabelHierarchy> item)
    {
        Children.Add(item.Key, item.Value);
    }

    void ICollection<KeyValuePair<string, LabelHierarchy>>.Clear()
    {
        Children.Clear();
    }

    bool ICollection<KeyValuePair<string, LabelHierarchy>>.Contains(KeyValuePair<string, LabelHierarchy> item)
    {
        return Children.Contains(item);
    }

    void ICollection<KeyValuePair<string, LabelHierarchy>>.CopyTo(KeyValuePair<string, LabelHierarchy>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<string, LabelHierarchy>>)Children).CopyTo(array, arrayIndex);
    }

    bool ICollection<KeyValuePair<string, LabelHierarchy>>.Remove(KeyValuePair<string, LabelHierarchy> item)
    {
        return Children.Remove(item.Key);
    }

    IEnumerator<KeyValuePair<string, LabelHierarchy>> IEnumerable<KeyValuePair<string, LabelHierarchy>>.GetEnumerator()
    {
        return Children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Children.GetEnumerator();
    }
}
