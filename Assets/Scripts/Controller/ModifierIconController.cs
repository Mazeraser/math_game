using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModifierIconController : MonoBehaviour
{
    [SerializeField]private Person_Modifier? modifier;
    public Person_Modifier? Modifier
    {
        set{modifier=value;}
    }

    [SerializeField]private TMP_Text? name_text;
    [SerializeField]private TMP_Text? desc_text;
    [SerializeField]private Image? icon;

    public delegate void Choosed(Person_Modifier mod);
    public static event Choosed ChoosedEvent;

    private void Awake()
    {
        name_text.text = modifier.name;
        desc_text.text = modifier.description;
        //icon = modifier.icon;
    }

    public void choosed()
    {
        ChoosedEvent?.Invoke(modifier);
    }
}
