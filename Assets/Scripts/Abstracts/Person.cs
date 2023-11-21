using UnityEngine;

public abstract class Person : MonoBehaviour, ILife, IPerson
{
    private void Start()
    {
        max_hp = hp;
    }

    //ILife
    [SerializeField]private int hp;
    private int max_hp;
    [SerializeField]private int armory;

    public int HP
    {
        get{return hp;}
        set
        {
            hp=Mathf.Clamp(value,0,this.Max_HP);
            if(hp<=0)
                die();
        }
    }
    public int Max_HP{get{return max_hp;}}
    public int Armory{get{return armory;}}

    public void die()
    {
        Destroy(this);
    }
    public void take_damage(int damage)
    {
        this.HP=hp-Mathf.Clamp(damage-this.Armory,1,hp);
    }
    //IPerson
    [SerializeField]private int dp;
    public int DP
    {
        get{return dp;}
        set{dp=value;}
    }

    public void attack(ILife target){target.take_damage(this.DP);}
}
