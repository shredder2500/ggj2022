using System.Collections.Generic;

namespace AI.Behavior {
public class Memory {
  private readonly Dictionary<string, object> _cache = new();

  public void Set<T>(string key, T value) {
    if (_cache.ContainsKey(key))
      _cache[key] = value;
    _cache.Add(key, value);
  }

  public T Get<T>(string key) => (T)_cache[key];

  public bool Contains(string key) => _cache.ContainsKey(key);

  public void Clear() => _cache.Clear();
}
}