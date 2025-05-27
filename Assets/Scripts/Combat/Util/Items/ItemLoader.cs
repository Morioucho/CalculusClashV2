using UnityEngine;

using System.IO;
using System.Collections.Generic;

public static class ItemLoader {

    public static ItemList GetAllItems() {
        string folder = Path.Combine(Application.streamingAssetsPath, "Items");
        string path = Path.Combine(folder, "items.json");

        if (!File.Exists(path)) {
            Debug.LogError("Item JSON not found: " + path);
            return null;
        }

        string rawJson = File.ReadAllText(path);
        string wrappedJson = "{\"items\":" + rawJson + "}";

        ItemList items = JsonUtility.FromJson<ItemList>(wrappedJson);

        if (items.items.Count == 0) {
            Debug.LogWarning("No items found in list.");
            return null;
        }

        return items;
    }
}
