using UnityEngine;

public class numGenerator : MonoBehaviour, IGenerator
{
    [SerializeField]private int max_range;
    public int Max_Range{get{return max_range;}}

    public delegate void num_generated(int num);
    public static event num_generated num_generated_event;
    public delegate void answer_checked();
    public static event answer_checked answer_checked_event;

    private int sol;

    private void Awake()
    {
        GameManager.OnUserRequestEvent += Generate;
        UIController.solution_entered_event += check_answer;
        GameManager.TimerEndEvent += check_answer;
    }

    public void Generate(int num)
    {
        int res = Random.Range(num,max_range);
        sol = res;
        num_generated_event?.Invoke(res);
    }
    private void check_answer(int ans)
    {
        if(sol==ans)
        {
            Debug.Log("Right!:)");
        }
        else
            Debug.Log("Wrong!>:(");
        answer_checked_event?.Invoke();
    }
    private void check_answer()
    {
        Debug.Log("Wrong!>:(");
        answer_checked_event?.Invoke();
    }
}
