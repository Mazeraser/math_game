using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class perk_icon : MonoBehaviour
{
    [SerializeField]private int ID;

    [SerializeField]private TMP_Text name;
    [SerializeField]private TMP_Text description;
    [SerializeField]private Image icon;

    private void Awake()
    {
        GameManager.CharacterSelectedEvent+=set_perk;
    }
    private void OnDestroy()
    {
        GameManager.CharacterSelectedEvent-=set_perk;
    }

    private void set_perk(Player ob)
    {
        Perk perk = ob.get_perks[ID];

        name.text = perk.name;
        description.text = perk.Description;
        icon.sprite = perk.Icon;
    }
}
