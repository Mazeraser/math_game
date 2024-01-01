using UnityEngine;

public abstract class Person : MonoBehaviour, ILife, IPerson, IActive
{
    //стативные константы
    public const int MIN_HP=1;
    public const int MAX_HP=12;
    public const int MIN_ARM=0;
    public const int MAX_ARM=10;
    public const int MIN_DP=1;
    public const int MAX_DP=22;

    //Эвенты, связанные с состоянием персонажа
    public delegate void Born(Person self, bool isPlayer);
    public static event Born BornEvent;
    public delegate void Death(bool isPlayer);
    public static event Death DeathEvent;
    //изменение характеричтик персонажа
    public delegate void LostHP(int num,string team);
    public static event LostHP LostHPEvent;

    private Animator anim;

    private void Awake()
    {
        numGenerator.answer_checked_event += find_target; //ловит проверку решения на верность
    }
    private void OnDestroy()
    {
        numGenerator.answer_checked_event -= find_target;
        UIController.RestartEvent -= die;
    }
    
    private void Start()
    {
        max_hp = hp;
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
    public void power_up(int hp_b, int arm_b, int dp_b)
    {
        hp = Mathf.Clamp(hp+hp_b,MIN_HP,MAX_HP); 
        armory = Mathf.Clamp(armory+arm_b,MIN_ARM,MAX_ARM); 
        dp = Mathf.Clamp(dp+dp_b,MIN_DP,MAX_DP); 
    }
    public void borned(bool isPlayer)
    {
        BornEvent?.Invoke(this, isPlayer);
    }

    //Реализация интерфейсов

    //ILife
    [SerializeField][Range(MIN_HP,MAX_HP)]private int hp;
    private int max_hp;
    [SerializeField][Range(MIN_ARM,MAX_ARM)]private int armory;

    public int HP
    {
        get{return hp;}
        set
        {
            for(int i=0;i<hp-value;i++)
                LostHPEvent?.Invoke(hp-i,tag);
            hp=Mathf.Clamp(value,0,this.Max_HP);
            anim.SetTrigger("Take_Damage");
            if(hp<=0)
                anim.SetTrigger("Death");
        }
    }
    public int Max_HP{get{return max_hp;}}
    public int Armory{get{return armory;}}

    public void die()
    {
        DeathEvent?.Invoke(tag=="Player");
        Destroy(anim);
        Destroy(gameObject);
        Destroy(this);
    }
    public void take_damage(int damage)
    {
        this.HP=hp-Mathf.Clamp(damage-this.Armory,1,hp);
    }
    //IPerson
    [SerializeField][Range(MIN_DP,MAX_DP)]private int dp;
    public int DP
    {
        get{return dp;}
        set{dp=value;}
    }

    public void attack(ILife target)
    {
        anim.SetTrigger("Attack");
        target.take_damage(this.DP);
    }
    //IActive
    public virtual void find_target(bool ans)=>Debug.Log(ans);
}
