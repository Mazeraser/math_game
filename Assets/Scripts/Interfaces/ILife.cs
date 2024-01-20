public interface ILife
{
   //отвечает за жизнеспособность персонажа и все что с ним связано
   int HP{get;set;}
   int Max_HP{get;}
   int Armory{get;}
   int Time_Armory{set;}
   int Cost{get;}

   void die();
   void take_damage(int damage);
   void heal(int heal_points);
}