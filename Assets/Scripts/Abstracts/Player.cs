using UnityEngine;

public class Player : Person
{
    public Player(int _hp, int _arm, int _dp):base(_hp,_arm,_dp){}

    public override void find_target(bool ans)
    {
        if(ans)
        {
            Debug.Log("Right!:)");
            Person? target = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Person>();
            if(target!=null)
                this.attack(target.GetComponent<ILife>());
            else
                Debug.Log("where...");
        }
    }
}
