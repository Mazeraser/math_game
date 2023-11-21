using UnityEngine;

public class Player : Person, IActive
{
    private void Awake()
    {
        numGenerator.answer_checked_event += find_target;
    }

    public void find_target(bool ans)
    {
        if(ans)
        {
            Debug.Log("Right!:)");
            Person? target = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Person>();
            this.attack(target.GetComponent<ILife>());
        }
    }
}
