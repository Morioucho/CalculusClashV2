using UnityEngine;

public class BossData
{
    private string name;
    private int hp;
    private list<string> dialogue;
    private object sprite;

    public BossData(string name, int hp, List<string> dialogue, object sprite)
    {
        this.name = name;
        this.hp = hp;
        this.dialogue = dialogue;
        this.sprite = sprite;
    }
    
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    
    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }

    public List<string> Dialogue
    {
        get { return dialogue; }
        set { dialogue = value; }
    }

    public object Sprite
    {
        get { return sprite; }
        set { sprite = value; }
    }
}