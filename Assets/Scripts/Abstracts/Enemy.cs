using UnityEngine;

public class Enemy : Person, IActive
{
    private void Awake()
    {
        numGenerator.answer_checked_event += find_target;
    }

    public void find_target(bool ans)
    {
        if(!ans)
        {
            Debug.Log("Wrong!>:(");
            Person? target = GameObject.FindGameObjectWithTag("Player").GetComponent<Person>();
            this.attack(target.GetComponent<ILife>());
        }
    }
}
