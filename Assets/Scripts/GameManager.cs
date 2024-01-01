using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public const int MIN_BALANCE=2;
    public const int MAX_BALANCE=44;

    [SerializeField][Tooltip("Числа после запятой")]private int aft;
    [Space]//Игровые модельки
    [SerializeField][Tooltip("Префабы противников")]private GameObject[] enemies;
    [SerializeField][Tooltip("Префабы игроков")]private GameObject[] players;
    [Space]//Настройки игры
    [SerializeField][Tooltip("Нижняя граница")]private int min_range;//нижняя граница задается в менеджере для увеличения сложности
    [SerializeField][Tooltip("Максимальная длина примера")]private int max_ex_range;
    [SerializeField][Tooltip("Прибавка к нижней границе в начале")]private int add_to_max;
    [SerializeField][Tooltip("Время на решение")]private float decide_time;

    private float timer;
    [SerializeField][Tooltip("Очки баланса")]private int balance_points;
    [SerializeField][Tooltip("Характеристики персонажа\n1.ХП\n2.Броня\n3.Очки урона")]public int[] player_chars; //установка характеристик происходит вручную для тестов и перед началом игры
    public int Balance_Points
    {
        get
        {
            return balance_points;
        }
        set
        {
            balance_points=value;
            BalanceChangedEvent?.Invoke(balance_points);
        }
    }

    //запрос на генерацию примера
    public delegate void OnUserRequest(int num);
    public static event OnUserRequest OnUserRequestEvent;
    //состояние таймера
    public delegate void TimerTurnin(float val, float time);
    public static event TimerTurnin TimerTurninEvent;
    public delegate void TimerEnd();
    public static event TimerEnd TimerEndEvent;
    //поражение
    public delegate void Defeat(bool cond = true, string defeat_text="Defeat");
    public static event Defeat DefeatEvent;
    //Изменился баланс
    public delegate void BalanceChanged(int points);
    public static event BalanceChanged BalanceChangedEvent;

    private void Awake()
    {
        
        if(SceneManager.GetActiveScene().buildIndex==0)
        {
            player_chars=new int[3];
            player_chars[0]=Person.MIN_HP; 
            player_chars[1]=Person.MIN_ARM; 
            player_chars[2]=Person.MIN_DP;
            
            set_value(0,0,0,0);
            List<GameObject> arr = new List<GameObject>(GameObject.FindGameObjectsWithTag("GameController"));
            if((new List<GameObject>(GameObject.FindGameObjectsWithTag("GameController"))).Count>=2)
                Destroy(gameObject);
            else
            {
                DiifButtonController.DiffSelectedEvent += set_value;
                DontDestroyOnLoad(gameObject); 
            }
        }
        MenuController.ChangeCharsEvent += change_chars; 
        numGenerator.answer_checked_event += delegate{timer=0;OnUserRequestEvent?.Invoke(min_range);};
        Person.DeathEvent += round_end;
        exampleGenerator.SubscribedEvent += initialize_game;
        exampleGenerator.SubscribedEvent += start_game; //начинает игру когда генератор примера находит менеджер
    }
    private void OnDestroy()
    {
        MenuController.ChangeCharsEvent -= change_chars; 
        numGenerator.answer_checked_event -= delegate{timer=0;OnUserRequestEvent?.Invoke(min_range);};
        Person.DeathEvent -= round_end;
        exampleGenerator.SubscribedEvent -= initialize_game;
        exampleGenerator.SubscribedEvent -= start_game;
    }
    private void Update()
    {
        //таймер
        timer += Time.deltaTime;
        TimerTurninEvent?.Invoke(Mathf.Lerp(0.0f,1.0f,(decide_time-timer)/decide_time),Mathf.Round((decide_time-timer)*Mathf.Pow(10,aft))/Mathf.Pow(10,aft));
        if(timer>=decide_time)
        {
            TimerEndEvent?.Invoke();
            timer = 0;
        }
        //смена примера
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnUserRequestEvent?.Invoke(min_range);
        }
    }

    private void round_end(bool isPlayer)
    {
        if(isPlayer)
        {
            DefeatEvent?.Invoke();
        }
        else
        {
            create_person(enemies[Random.Range(0,enemies.Length)]);
        }
    }

    private void create_person(GameObject vessel)  
    {
        if(vessel.tag=="Player")
        {
            Debug.Log("player");
            int sum=player_chars.Sum();
            Debug.Log(sum);
            if(balance_points>=sum&&sum!=0)
            {
                Debug.Log("Balance is exists");
                vessel.GetComponent<Player>().copy(new Player(player_chars[0],player_chars[1],player_chars[2]));
            }
            else
            {
                Debug.Log("Balance doesn't exists");
                int[] new_chars=load_chars();
                vessel.GetComponent<Player>().copy(new Player(new_chars[0],new_chars[1],new_chars[2]));
            }
        }
        else if(vessel.tag=="Enemy")
        {
            int[] new_chars=load_chars();
            vessel.GetComponent<Enemy>().copy(new Enemy(new_chars[0],new_chars[1],new_chars[2]));
        }
        vessel.transform.position = new Vector3(3.5f*(vessel.tag=="Player"?-1:1),-2,0);
        Instantiate(vessel);
    }
    private int[] load_chars(){
        int hp=1;
        int arm=0;
        int dp=1;
        int points=balance_points-(hp+arm+dp);
        int gen=0;
        while(points>0)
        {
            switch(Random.Range(0,3))
            {
                case 0:
                    gen=Random.Range(Person.MIN_HP,Mathf.Clamp(Person.MAX_HP,Person.MIN_HP,points)+1);
                    hp+=gen;
                    points-=gen; 
                    break;
                case 1:
                    gen=Random.Range(Person.MIN_ARM,Mathf.Clamp(Person.MAX_ARM,Person.MIN_ARM,points)+1);
                    arm+=gen;
                    points-=gen; 
                    break;
                case 2:
                    gen=Random.Range(Person.MIN_DP,Mathf.Clamp(Person.MAX_DP,Person.MIN_DP,points)+1);
                    dp+=gen;
                    points-=gen; 
                    break;
            }
        }
        return new int[3]{hp,arm,dp};
    }
    private void change_chars(int char_ind, int val)
    {
        if(char_ind<=2)
        {
            int[] min_vals=new int[3]{Person.MIN_HP,Person.MIN_ARM, Person.MIN_DP};
            int[] max_vals=new int[3]{Person.MAX_HP,Person.MAX_ARM, Person.MAX_DP};
            player_chars[char_ind]=Mathf.Clamp(player_chars[char_ind]+val,min_vals[char_ind],max_vals[char_ind]);
        }
        else
            this.Balance_Points=Mathf.Clamp(this.Balance_Points+val,MIN_BALANCE,MAX_BALANCE);
    }

    private void initialize_game()
    {
        GameObject? generator = GameObject.FindGameObjectWithTag("Generator");
        if(generator!=null)
        {
            generator.GetComponent<exampleGenerator>().create(max_ex_range);
            generator.GetComponent<numGenerator>().create(min_range,add_to_max,gameObject);
        }
    }
    private void start_game()
    {
        create_person(enemies[Random.Range(0,enemies.Length)]);//создается только в первый раз потому что после смерти сразу создается новый противник
        create_person(players[Random.Range(0,enemies.Length)]);
        OnUserRequestEvent?.Invoke(min_range);
        timer = 0;
    }

    //используется для установки слонжности в меню
    private void set_value(int min, int addit, int max_range, int timer, string name="")
    {   
        min_range = min;
        add_to_max = addit;
        max_ex_range = max_range;
        decide_time = timer;
    }
}