using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    public delegate void Change_Chars(int char_ind, int val);
    public static event Change_Chars ChangeCharsEvent;

    [Tooltip("Меню")]private GameObject? menu;
    [Tooltip("Выбор сложности")]private GameObject? difficulty_menu;
    [Tooltip("Настройки")]private GameObject? settings;
    [Tooltip("Создание персонажа")]private GameObject? creatin_person;
    private GameObject play;

    [Tooltip("Всего очков баланса")]private TMP_Text? total_balance;
    [Tooltip("Остаток очков баланса")]private TMP_Text? curr_balance;
    [Tooltip("Характеристики игрока")]private TMP_Text[]? player_chars;

    private GameObject[] min;
    private GameObject[] max;

    private GameManager manager;

    private void Awake()
    {
        DiifButtonController.DiffSelectedEvent += set_description; //создаёт расписание в меню выбора сложности
    }
    private void OnDestroy()
    {
        DiifButtonController.DiffSelectedEvent -= set_description;
    }
    private void Start()
    {
        menu = GameObject.Find("Menu");
        difficulty_menu = GameObject.Find("Diff");
        settings = GameObject.Find("Settings");
        creatin_person = GameObject.Find("creating_person");

        total_balance = GameObject.Find("total_balance").GetComponent<TMP_Text>();
        curr_balance = GameObject.Find("curr_balance").GetComponent<TMP_Text>();
        player_chars = new TMP_Text[3];
        player_chars[0] = GameObject.Find("player_hp").GetComponent<TMP_Text>();
        player_chars[1] = GameObject.Find("player_arm").GetComponent<TMP_Text>();
        player_chars[2] = GameObject.Find("player_dp").GetComponent<TMP_Text>();

        min = new GameObject[4];
        min[0]=GameObject.Find("minus_hp");
        min[1]=GameObject.Find("minus_arm");
        min[2]=GameObject.Find("minus_dp");
        min[3]=GameObject.Find("minus_balance");

        max = new GameObject[4];
        max[0]=GameObject.Find("plus_hp");
        max[1]=GameObject.Find("plus_arm");
        max[2]=GameObject.Find("plus_dp");
        max[3]=GameObject.Find("plus_balance");

        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        
        to_menu();
    }
    private void Update()
    {
        if(creatin_person.activeSelf)
        {
            total_balance.text = "Total balance points: "+manager.Balance_Points.ToString();
            curr_balance.text = "Balance points remain: "+(manager.Balance_Points-manager.player_chars.Sum()).ToString();
            if(manager.Balance_Points==manager.player_chars.Sum())
            {
                min[3].SetActive(false);
                max[3].SetActive(true);
            }
            else
            {
                min[3].SetActive(true);
                max[3].SetActive(true);
            }

            player_chars[0].text = "Health points: "+manager.player_chars[0].ToString();
            if(manager.player_chars[0]==Person.MAX_HP||(manager.Balance_Points-manager.player_chars.Sum())==0)
            {
                min[0].SetActive(true);
                max[0].SetActive(false);
            }
            else if(manager.player_chars[0]==Person.MIN_HP&&(manager.Balance_Points-manager.player_chars.Sum())!=0)
            {
                min[0].SetActive(false);
                max[0].SetActive(true);
            }
            else
            {
                min[0].SetActive(true);
                max[0].SetActive(true);
            }

            player_chars[1].text = "Armory: "+manager.player_chars[1].ToString();
            if(manager.player_chars[1]==Person.MAX_ARM||(manager.Balance_Points-manager.player_chars.Sum())==0)
            {
                min[1].SetActive(true);
                max[1].SetActive(false);
            }
            else if(manager.player_chars[1]==Person.MIN_ARM&&(manager.Balance_Points-manager.player_chars.Sum())!=0)
            {
                min[1].SetActive(false);
                max[1].SetActive(true);
            }
            else
            {
                min[1].SetActive(true);
                max[1].SetActive(true);
            }

            player_chars[2].text = "Damage points: "+manager.player_chars[2].ToString();
            if(manager.player_chars[2]==Person.MAX_DP||(manager.Balance_Points-manager.player_chars.Sum())==0)
            {
                min[2].SetActive(true);
                max[2].SetActive(false);
            }
            else if(manager.player_chars[2]==Person.MIN_DP&&(manager.Balance_Points-manager.player_chars.Sum())!=0)
            {
                min[2].SetActive(false);
                max[2].SetActive(true);
            }
            else
            {
                min[2].SetActive(true);
                max[2].SetActive(true);
            }
        }
    }

    public void to_select_difficulty()
    {
        menu?.SetActive(false);
        difficulty_menu?.SetActive(true);
        settings?.SetActive(false);
        creatin_person?.SetActive(false);
    }
    public void to_menu()
    {
        menu?.SetActive(true);
        difficulty_menu?.SetActive(false);
        settings?.SetActive(false);
        creatin_person?.SetActive(false);
    }
    public void to_settings()
    {
        menu?.SetActive(false);
        difficulty_menu?.SetActive(false);
        settings?.SetActive(true);
        creatin_person?.SetActive(false);
    }
    public void to_creating()
    {
        menu?.SetActive(false);
        difficulty_menu?.SetActive(false);
        settings?.SetActive(false);
        creatin_person?.SetActive(true);
    }
    public void exit_game()
    {
        Application.Quit();
    }
    public void load_game()
    {
        SceneManager.LoadScene(1);
    }
    //установка описания в меню
    private void set_description(int min, int addit, int max_range, int timer, string name)
    {
        TMP_Text description = GameObject.Find("desc").GetComponent<TMP_Text>();
        description.text="";
        description.text+="Difficult: "+name.ToLower()+"\n";
        description.text+="Minimal generated number: "+min.ToString()+"\n";
        description.text+="Maximal generated number on start: "+(min+addit).ToString()+"\n";
        description.text+="Maximal length of example: "+max_range.ToString()+"\n";
        description.text+="Time for decision: "+timer.ToString()+" seconds\n";
    }

    public void change_balance(int val) => GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().Balance_Points+=val;
    public void up_char(int char_index) => ChangeCharsEvent?.Invoke(char_index, 1);
    public void down_char(int char_index) => ChangeCharsEvent?.Invoke(char_index, -1);
}
