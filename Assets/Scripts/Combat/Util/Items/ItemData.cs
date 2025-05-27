using System.Collections.Generic;

[System.Serializable]
public class ItemData {
    public string itemName;
    public string id;
    public float damageBuff;
    public float liveAmount;
}

public class ItemList {
    public List<ItemData> items;
}