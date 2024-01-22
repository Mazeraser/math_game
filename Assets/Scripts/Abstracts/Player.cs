using UnityEngine;
using UnityEngine.UI;

public class Player : Person
{
    [SerializeField]private string description;
    [SerializeField]private Sprite icon;
    public string Description
    {
        get{return description;}
    }
    public Sprite Icon
    {
        get{return icon;}
    }

    public Player(int _hp, int _arm, int _dp):base(_hp,_arm,_dp){}

    public override void find_target(bool ans)
    {
        if(ans)
        {
            Person? target = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Person>();
            if(target!=null)
                this.attack(target.GetComponent<ILife>());
            else
                Debug.Log("where...");
        }
    }
}
