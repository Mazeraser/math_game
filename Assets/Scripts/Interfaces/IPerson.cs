public interface IPerson
{
    //добавляет персонажу какие либо действия
    int DP{get;set;}
    void attack(ILife target);
    void catch_kill(bool isPlayer, int costs);
}