using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName="New modifier",menuName="Modifiers/Core modifier")]
public abstract class Modifier : ScriptableObject
{
    [Tooltip("Иконка")]public Image icon;
    [Tooltip("Описание")]public string description;
    [Space]
    [Tooltip("Родительские модификаторы")]public Modifier[] parents;
    [Tooltip("Актуальность модификатора(наиболее подходящий уровень когда можно предложить этот модификатор)")]public int actuality;
    [Tooltip("Стоимость")]public virtual int Cost{get{return 0;}}
}
