using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]private int min_range;//нижняя граница задается в менеджере для увеличения сложности, можно заменить на длину примера

    public float decide_time;
    private float timer;

    public delegate void OnUserRequest(int num);
    public static event OnUserRequest OnUserRequestEvent;

    public delegate void TimerTurnin(float val);
    public static event TimerTurnin TimerTurninEvent;
    public delegate void TimerEnd();
    public static event TimerEnd TimerEndEvent;

    private void Awake()
    {
        numGenerator.answer_checked_event += delegate{timer=0;OnUserRequestEvent?.Invoke(min_range);};
    }
    private void Start()
    {
        OnUserRequestEvent?.Invoke(min_range);
    }
    private void Update()
    {
        timer += Time.deltaTime;
        TimerTurninEvent?.Invoke(Mathf.Lerp(0,timer/decide_time,timer));
        if(timer>=decide_time)
        {
            TimerEndEvent?.Invoke();
            timer = 0;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnUserRequestEvent?.Invoke(min_range);
        }
    }
}