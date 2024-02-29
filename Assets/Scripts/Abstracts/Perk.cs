using UnityEngine;

[CreateAssetMenu(fileName="new Perk", menuName="Perk")]
public class Perk : ScriptableObject
{
    private enum perk_type
    {
        for_kill=0,
        for_turn=1,
    }
    [SerializeField][Tooltip("тип способности")]private perk_type type;
    public int Type
    {
        get{return (int)type;}
    }

    [SerializeField]
    [Tooltip("0-восстанавливает хп\n1-добавляет временную броню\n2-добавляет очки опыта")]
    private int[] chars=new int[3];
    public int HP
    {
        get{return chars[0];}
    }
    public int ARM
    {
        get{return chars[1];}
    }
    public int EXP
    {
        get{return chars[2];}
    }

    [SerializeField]private int unique_id;
    public int ID
    {
        get{return unique_id;}
    }

    [SerializeField]private string description;
    public string Description
    {
        get{return description;}
    }
}
