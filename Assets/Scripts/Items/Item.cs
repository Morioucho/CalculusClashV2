/// <summary>
///     Class <c>Item</c> models an item in a game, this class is a POJO.
/// </summary>
public class Item {
    public string Name { get; }
    public int Amount { get; }
    public float HealthModifier { get; }
    public float DamageModifier { get; }

    public Item(string name, int amount, float damageModifier, float healthModifier) {
        Name = name;
        Amount = amount;

        DamageModifier = damageModifier;
        HealthModifier = healthModifier;
    }

    public Item(string name, int amount) {
        Name = name;
        Amount = amount;
        HealthModifier = 0f;
        DamageModifier = 0f;
    }
}