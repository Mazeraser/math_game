using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Selected_Character_Info : MonoBehaviour
{
    [SerializeField]private Image person_image;
    [SerializeField]private TMP_Text person_description;
    public Player Selected_Character
    {
        set
        {
            person_image.sprite=value.Icon;
            person_description.text=value.Description;
        }
    }

    private void Awake()
    {
        GameManager.CharacterSelectedEvent+=set_character;
    }
    private void OnDestroy()
    {
        GameManager.CharacterSelectedEvent-=set_character;
    }

    private void set_character(Player person)
    {
        this.Selected_Character=person;
    }
}
