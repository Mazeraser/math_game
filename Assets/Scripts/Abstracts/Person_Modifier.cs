using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName="New person modifier", menuName="Modifiers/Person modifier")]
public class Person_Modifier : Modifier
{
    private enum armType
    {
        prm=0,
        tml=1,
    };
    [SerializeField][Tooltip("Тип защиты")]private armType armory_type;
    [SerializeField][Tooltip("Характеристики персонажа\n1.ХП\n2.Броня\n3.Очки урона")]private int[] chars = new int[3];
    public override int Cost{get{return chars.Sum()-chars[1]*(int)armory_type;}}
    public int HP{get{return chars[0];}}
    public int Arm{get{return chars[1];}}
    public int DP{get{return chars[2];}}
    public int ARM_T{get{return (int)armory_type;}}
}
