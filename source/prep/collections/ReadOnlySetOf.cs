using System.Collections;
using System.Collections.Generic;

namespace prep.collections
{
  public class ReadOnlySetOf<Item> : IEnumerable<Item>
  {
    private readonly IList<Item> _items;

    public ReadOnlySetOf(IList<Item> items) {
      _items = items;
    }

    public IEnumerator<Item> GetEnumerator() {
      return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }
  }
}