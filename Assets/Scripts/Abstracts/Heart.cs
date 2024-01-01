using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField]private Sprite? active;
    [SerializeField]private Sprite? not_active;
    public int number;

    private void Awake()
    {
        Person.LostHPEvent+=update_hp;//меняет изображение когда персонаж теряет хп
    }
    private void OnDestroy()
    {
        Person.LostHPEvent-=update_hp;
    }
    private void update_hp(int num,string team)
    {
        if(number==num&&tag==team)
        {
            GetComponent<Image>().sprite=not_active;
        }
    }
}
