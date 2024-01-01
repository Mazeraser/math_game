using UnityEngine;

public class Enemy : Person
{
    public Enemy(int _hp, int _arm, int _dp):base(_hp,_arm,_dp){}

    public override void find_target(bool ans)
    {
        if(!ans)
        {
            Debug.Log("Wrong!>:(");
            Person? target = GameObject.FindGameObjectWithTag("Player").GetComponent<Person>();
            if(target!=null)
                this.attack(target.GetComponent<ILife>());
            else
                Debug.Log("where...");
        }
    }
}
