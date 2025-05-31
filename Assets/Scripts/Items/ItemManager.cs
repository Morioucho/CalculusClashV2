// <summary>
// Class <c>ItemManager</c> manages a collection of items in a game.
// </summary>

using System.Collections.Generic;

public class ItemManager {
    public static ItemManager Instance { get; set; }
    private readonly List<Item> items;

    public ItemManager() {
        if (Instance == null)
            Instance = this;
        else
            return;

        // TODO: Move item loading from Combat.Util.Items to here.
        // TODO: Remove item loading only being combat specific, allow for editing out of combat.

        items = new List<Item> {
            // TODO: Add proper items
            new("Health Potion", 10, 0f, 50f),
            new("Mana Potion", 10, 0f, 50f),
            new("Sword", 1, 10f, 0f)
        };
    }

    /// <summary>
    ///     Method <c>GetItems</c> returns a list of all items managed by this ItemManager Instance.
    /// </summary>
    /// <returns>A list of items of type <c>Item</c>.</returns>
    public List<Item> GetItems() {
        return items;
    }

    /// <summary>
    ///     Method <c>GetItem</c> retrieves an Item's specific Instance by its name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns>An item Instance of type <c>Item</c>.</returns>
    public Item GetItem(string name) {
        foreach (var item in items)
            if (item.Name.Equals(name))
                return item;

        return null;
    }
}