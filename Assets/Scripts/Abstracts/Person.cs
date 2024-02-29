using UnityEngine;

public abstract class Person : MonoBehaviour, ILife, IPerson, IActive, IProgressive
{
    //стативные константы
    public const int MIN_HP=1;
    public const int MAX_HP=12;
    public const int MIN_ARM=0;
    public const int MAX_ARM=10;
    public const int MIN_DP=1;
    public const int MAX_DP=22;
    public const int MIN_EXP=1;
    public const int MAX_EXP=6;

    //Эвенты, связанные с состоянием персонажа
    public delegate void Born(Person self, bool isPlayer);
    public static event Born BornEvent;
    public delegate void Changed(Person self, bool isPlayer);
    public static event Changed ChangedEvent;
    public delegate void Death(bool isPlayer, int Costs);
    public static event Death DeathEvent;
    //изменение характеричтик персонажа
    public delegate void Lost(int num,string team, string obj_name);
    public static event Lost LostEvent;
    public delegate void Reach(int num,string team, string obj_name);
    public static event Reach ReachEvent;
    public delegate void LevelUpped(int level,string team);
    public static event LevelUpped GetModifierEvent;

    private Animator anim;

    private void Awake()
    {
        numGenerator.answer_checked_event += find_target; //ловит проверку решения на верность
        Person.DeathEvent += catch_kill;
    }
    private void OnDestroy()
    {
        numGenerator.answer_checked_event -= find_target;
        Person.DeathEvent -= catch_kill;
    }
    
    private void Start()
    {
        max_hp = hp;
        time_armory = 0;
        borned(tag=="Player");
        anim = GetComponent<Animator>();
    }

    public Person(int _hp, int _arm, int _dp)
    {
        hp = _hp;
        armory = _arm;
        dp = _dp;
    }

    //встроенные функции
    public void copy(Person obj)
    {
        hp = obj.HP;
        armory = obj.Armory;
        dp = obj.DP;
    }
    public void power_up(int hp_b, int arm_b, int dp_b, int arm_t)
    {
        max_hp = Mathf.Clamp(max_hp+hp_b,MIN_HP,MAX_HP);
        this.HP += hp_b;
        if(arm_t==0)
            armory = Mathf.Clamp(armory+arm_b,MIN_ARM,MAX_ARM-time_armory); 
        else if(arm_t==1)
            this.Time_Armory =  Mathf.Clamp(time_armory+arm_b,MIN_ARM,MAX_ARM-armory);
        dp = Mathf.Clamp(dp+dp_b,MIN_DP,MAX_DP);
        ChangedEvent?.Invoke(this, tag=="Player");
    }
    public void borned(bool isPlayer)
    {
        lvl = 1;
        exp = 0;
        BornEvent?.Invoke(this, isPlayer);
    }

    //Реализация интерфейсов

    //ILife
    [SerializeField][Range(MIN_HP,MAX_HP)]private int hp;
    private int max_hp;
    [SerializeField][Range(MIN_ARM,MAX_ARM)]private int armory;
    private int time_armory;
    [SerializeField][Range(MIN_EXP,MAX_EXP)]private int cost;

    public int HP
    {
        get{return hp;}
        set
        {
            if(value<hp)
            {
                for(int i=0;i<hp-value;i++)
                    LostEvent?.Invoke(hp-i,tag, "Heart");
            }
            else if(value>hp)
            {
                for(int i=0;i<value-hp;i++)
                    ReachEvent?.Invoke(hp+i+1,tag, "Heart");
            }
            hp=Mathf.Clamp(value,0,this.Max_HP);
            anim.SetTrigger("Take_Damage");
            if(hp<=0)
                anim.SetTrigger("Death");
        }
    }
    public int Max_HP{get{return max_hp;}}
    public int Armory{get{return armory;}}
    public int Time_Armory
    {
        get{return time_armory;}
        set{
            if(value<time_armory)
            {
                for(int i=0;i<time_armory-value;i++)
                    LostEvent?.Invoke(time_armory-i,tag, "Time_Armory");
            }
            else if(value>time_armory)
            {
                for(int i=0;i<value-time_armory;i++)
                    ReachEvent?.Invoke(time_armory+i,tag, "Time_Armory");
            }
            time_armory=Mathf.Clamp(value,0,MAX_ARM-armory);
        }
    }
    public int Cost{get{return cost;}}

    public void die()
    {
        DeathEvent?.Invoke(tag=="Player", this.Cost);
        Destroy(anim);
        Destroy(gameObject);
        Destroy(this);
    }
    public void take_damage(int damage)
    {
        if(damage<=this.Time_Armory)
            this.Time_Armory-=Mathf.Clamp(damage-this.Armory,0,this.Time_Armory);
        else
        {
            damage=Mathf.Clamp(damage-this.Time_Armory+this.Armory,MIN_DP,MAX_DP);
            this.Time_Armory=0;
            this.HP=hp-Mathf.Clamp(damage-this.Armory,1,hp);
        }
    }
    public void heal(int heal_points)
    {
        this.HP=hp+heal_points;
    }
    //IPerson
    [SerializeField][Range(MIN_DP,MAX_DP)]private int dp;
    public int DP
    {
        get{return dp;}
        set{dp=value;}
    }

    [SerializeField]private Perk[] perks;

    public void attack(ILife target)
    {
        anim.SetTrigger("Attack");
        target.take_damage(this.DP);
        if((int)GET==0)
            Get_Exp(1);
        activate_perks(1);
    }
    public void catch_kill(bool isPlayer, int costs)
    {
        if((!isPlayer&&tag=="Player"||isPlayer&&tag=="Enemy")&&(int)GET==1)
        {
            Get_Exp(costs);
            activate_perks(0);
        }
    }
    public void activate_perks(int on_attack)
    {
        foreach(Perk curr_perk in perks)
        {
            if(on_attack==curr_perk.Type)
            {
                switch(curr_perk.ID)
                {
                    case 12:
                        if(Random.Range(0,2)!=0)
                            break;
                        else
                            goto case 0;
                    case 13:
                        if(Random.Range(0,3)!=0)
                            break;
                        else
                            goto case 0;
                    case 15:
                        if(Random.Range(0,5)!=0)
                            break;
                        else
                            goto case 0;
                    case 2:
                        if(Random.Range(0,10)!=0)
                        {
                            int r = Random.Range(0,2);
                            power_up(1-r,r,0,0);
                        }
                        break;
                    case 3:
                        if(Random.Range(0,10)!=0)
                        {
                            power_up(0,0,1,0);
                        }
                        break;
                    case 0:
                        if(curr_perk.HP>0)
                            heal(curr_perk.HP);
                        if(curr_perk.ARM>0)
                            power_up(0,curr_perk.ARM,0,1);
                        if(curr_perk.EXP>0)
                            Get_Exp(curr_perk.EXP);
                        break;
                }
                        
            }
        }
    }
    //IActive
    public virtual void find_target(bool ans)=>Debug.Log(ans);
    //IProgressive
    public int Level
    {
        get{return lvl;}
    }
    public int Add_Exp_PerLevel{get{return add_perlevel;}}
    public int Exp_Need{get{return Mathf.Clamp(this.Add_Exp_PerLevel*this.Level,MIN_EXP,MAX_EXP);}}
    public int Exp_Curr
    {
        set
        {
            if(value>=this.Exp_Need)
            {
                exp=0;
                for(int i=1;i<=Exp_Need;i++)
                    LostEvent?.Invoke(exp+i,tag,"Experience");
                Level_Up();
            }
            else
            {
                for(int i=1;i<=value-exp;i++)
                    ReachEvent?.Invoke(exp+i,tag,"Experience");
                exp = value;
            }
        }
    }
    public int Get_Modifier_Level
    {
        get
        {
            return get_modifier_level;
        }
    }
    private enum get_experience_type
    {
        on_attack=0,
        on_death=1,
    }

    [SerializeField][Tooltip("Количество опыта, необходимое для уровня(увеличивается для эту переменную каждый новый уровень)")]private int add_perlevel=3;
    [SerializeField][Tooltip("Тип получения опыта")]private get_experience_type GET;
    [SerializeField][Tooltip("Уровень для получения модификатора")][Range(1,8)]private int get_modifier_level;
    private int lvl;
    private int exp;
    public void Get_Exp(int xp)
    {
        this.Exp_Curr=exp+xp;
    }
    public void Level_Up()
    {
        lvl+=1;
        //this.heal(this.Max_HP);
        ChangedEvent?.Invoke(this, tag=="Player");
        if(lvl%get_modifier_level==0)
            GetModifierEvent?.Invoke(this.Level,tag);
    }
}