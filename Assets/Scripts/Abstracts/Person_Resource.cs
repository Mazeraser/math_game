using UnityEngine;
using UnityEngine.UI;

public class Person_Resource : MonoBehaviour
{
    [SerializeField]private Sprite? active;
    [SerializeField]private Sprite? not_active;
    public int number;

    private void Awake()
    {
        Person.LostEvent+=disable;//меняет изображение когда персонаж теряет хп
        Person.ReachEvent+=enable;
    }
    private void OnDestroy()
    {
        Person.LostEvent-=disable;
        Person.ReachEvent-=enable;
    }
    private void enable(int num,string team, string obj_name)
    {
        if(number==num&&tag==team&&name.Contains(obj_name))
        {
            Debug.Log(name);
            GetComponent<Image>().sprite=active;
        }
    }
    private void disable(int num,string team, string obj_name)
    {
        if(number==num&&tag==team&&name.Contains(obj_name))
        {
            GetComponent<Image>().sprite=not_active;
        }
    }
}
