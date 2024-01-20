interface IProgressive
{
    int Level{get;}
    int Add_Exp_PerLevel{get;}
    int Exp_Need{get;}
    int Exp_Curr{set;}
    int Get_Modifier_Level{get;}
    void Level_Up();
}