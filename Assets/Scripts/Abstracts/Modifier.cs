using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName="New modifier",menuName="Modifiers/Core modifier")]
public abstract class Modifier : ScriptableObject
{
    [SerializeField][Tooltip("Иконка")]private Image icon;
    [SerializeField][Tooltip("Описание")]private string description;
    [Space]
    [SerializeField][Tooltip("Родительские модификаторы")]private Modifier[] parents;
    [SerializeField][Tooltip("Актуальность модификатора")]private int actuality;
    [Tooltip("Стоимость")]public virtual int Cost{get{return 0;}}
}
