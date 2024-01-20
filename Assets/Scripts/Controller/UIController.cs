using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class UIController : MonoBehaviour
{
    //объекты интерфейса
    [SerializeField][Tooltip("Префаб сердца")]private GameObject heart;
    [SerializeField][Tooltip("Префаб брони")]private GameObject shield;
    [SerializeField][Tooltip("Префаб временной брони")]private GameObject time_shield;
    [SerializeField][Tooltip("Префаб атаки")]private GameObject attack;
    [SerializeField][Tooltip("Префаб опыта")]private GameObject experience;
    [SerializeField][Tooltip("Префаб уровня")]private GameObject level;
    [SerializeField][Tooltip("Префаб иконки модификатора")]private GameObject ModifierIcon;
    //поля
    [SerializeField][Tooltip("Текст примеров")]private TMP_Text? example_ui;
    [SerializeField][Tooltip("Поле ввода")]private TMP_InputField? field_ui;
    [SerializeField][Tooltip("Шкала таймера")]private Image? timer;
    [SerializeField][Tooltip("Текст таймера")]private TMP_Text? timer_text_ui;
    [SerializeField][Tooltip("Игровое меню")]private GameObject? game_menu;
    [SerializeField][Tooltip("Панель выбора модификаторов")]private GameObject? modifiers_panel;
    [SerializeField]private GameObject? continue_button;
    [SerializeField][Tooltip("Текст в меню")]private TMP_Text? text_menu;

    private bool menu_condition; //состояние меню(обычное меню или продолжение) 

    public delegate void solution_entered(int solution);
    public static event solution_entered solution_entered_event;

    public delegate void Restart();
    public static event Restart RestartEvent;

    public delegate void UseModifier(Person_Modifier mod);
    public static event UseModifier UseModifierEvent;

    private void Awake()
    {
        exampleGenerator.example_generated_event += show_example;
        GameManager.TimerTurninEvent += set_timer;
        GameManager.DefeatEvent += active_menu;
        GameManager.ModifierChoosedEvent += show_modifier;
        Person.BornEvent += init_person;
        Person.ChangedEvent += init_person;
        ModifierIconController.ChoosedEvent += delegate{
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Modifier"))
                Destroy(obj);
            modifiers_panel.SetActive(false);
        };
    }
    private void OnDestroy()
    {
        exampleGenerator.example_generated_event -= show_example;
        GameManager.TimerTurninEvent -= set_timer;
        GameManager.DefeatEvent -= active_menu;
        Person.BornEvent -= init_person;
        Person.ChangedEvent -= init_person;
        GameManager.ModifierChoosedEvent -= show_modifier;
        ModifierIconController.ChoosedEvent -= delegate{
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Modifier"))
                Destroy(obj);
            modifiers_panel.SetActive(false);
        };
    }
    private void Start()
    {
        if(game_menu==null)
            game_menu = GameObject.Find("game_menu");
        active_menu(false);
        modifiers_panel.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            active_menu(!menu_condition,"Menu");
        else if(Input.anyKeyDown)
        {
            if("0123456789".Contains(Input.inputString))
                add_num(Input.inputString);
            else
            {
                KeyCode SomeKeyCode=KeyCode.Space;
                foreach(KeyCode KCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(KCode))
                    {
                        SomeKeyCode = KCode; 
                        break;
                    }
                }
                switch (SomeKeyCode) 
                {
                    case(KeyCode.Return):
                        share_solution();
                        break;
                    case(KeyCode.Backspace):
                        delete_sym();
                        break;
                }
            }
        }
    }

    private void show_example(string example)
    {
        example_ui.text = example;
        field_ui.text = "";
    }
    public void share_solution()
    {
        solution_entered_event?.Invoke(int.Parse(field_ui.text));
    }
    private void set_timer(float val, float time)
    {
        timer.fillAmount = val;
        timer_text_ui.text = time.ToString();
    }
    public void add_num(string num)
    {
        field_ui.text=field_ui.text+num;
    }
    public void init_person(Person ob, bool is_player){
        create_obj(ob.Max_HP,is_player,GameObject.Find((is_player?"player":"enemy")+"_hp"),heart);
        create_obj(ob.Armory,is_player,GameObject.Find((is_player?"player":"enemy")+"_shield"),shield);
        create_obj(ob.Time_Armory,is_player,GameObject.Find((is_player?"player":"enemy")+"_timeshield"),time_shield);
        create_obj(ob.DP,is_player,GameObject.Find((is_player?"player":"enemy")+"_atk"),attack);
        create_obj(ob.Exp_Need,is_player,GameObject.Find((is_player?"player":"enemy")+"_exp"),experience);
        create_obj(ob.Level,is_player,GameObject.Find((is_player?"player":"enemy")+"_lvl"),level);
    }
    public void create_obj(int n, bool is_player, GameObject par, GameObject obj)
    {
        foreach(Transform child in par.transform) 
        {
            Destroy(child.gameObject);
        }
        for(int i=0;i<n;i++)
        {
            GameObject clone = Instantiate(obj);
            clone.tag=is_player?"Player":"Enemy";
            clone.transform.SetParent(par.transform);
            if(clone.GetComponent<Person_Resource>())
                clone.GetComponent<Person_Resource>().number=i+1;
        }
    }
    public void delete_sym()
    {
        field_ui.text = field_ui.text.Substring(0,field_ui.text.Length-1);
    }
    private void change_menu() => active_menu(!menu_condition,"Menu");
    private void active_menu(bool cond, string label_menu="")
    {
        continue_button.SetActive(!label_menu.Contains("Defeat"));
        menu_condition = cond;
        text_menu.text = label_menu;
        game_menu.SetActive(menu_condition);
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        RestartEvent?.Invoke();
        active_menu(false);
    }
    public void exit()=>SceneManager.LoadScene(0);
    public void continue_func()=>active_menu(false);
    private void show_modifier(Person_Modifier mod)
    {
        modifiers_panel.SetActive(true);
        ModifierIcon.GetComponent<ModifierIconController>().Modifier = mod;
        GameObject clone = Instantiate(ModifierIcon);
        clone.transform.SetParent(modifiers_panel.transform);
    }
}
