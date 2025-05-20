using UnityEngine;
using System.Collections.Generic;

public class BossData
{
    private string name;
    private int hp;
    private List<string> dialogue;

    public BossData(string name, int hp, List<string> dialogue) {
        this.name = name;
        this.hp = hp;
        this.dialogue = dialogue;
    }
    
    public string Name {
        get { return name; }
        set { name = value; }
    }
    
    public int HP {
        get { return hp; }
        set { hp = value; }
    }

    public List<string> Dialogue {
        get { return dialogue; }
        set { dialogue = value; }
    }
}