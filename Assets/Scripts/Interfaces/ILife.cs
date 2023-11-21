public interface ILife
{
   int HP{get;set;}
   int Max_HP{get;}
   int Armory{get;}

   void die();
   void take_damage(int damage);
}