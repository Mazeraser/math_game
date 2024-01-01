using UnityEngine;

public class numGenerator : MonoBehaviour, IGenerator
{
    [SerializeField]private int max_range;
    public int Max_Range{get{return max_range;}}

    public delegate void num_generated(int num);
    public static event num_generated num_generated_event;
    public delegate void answer_checked(bool ans);
    public static event answer_checked answer_checked_event;

    private int sol;

    public void create(int min,int addit, GameObject boss)
    {
        max_range = min+addit;
    }
    
    private void Awake()
    {
        GameManager.OnUserRequestEvent += Generate;
        UIController.solution_entered_event += check_answer;
        GameManager.TimerEndEvent += check_answer;
    }
    private void OnDestroy()
    {
        GameManager.OnUserRequestEvent -= Generate;
        UIController.solution_entered_event -= check_answer;
        GameManager.TimerEndEvent -= check_answer;
    }

    public void Generate(int num)
    {
        int res = Random.Range(num,max_range);
        sol = res;
        num_generated_event?.Invoke(res);
    }
    private void check_answer(int ans)
    {
       answer_checked_event?.Invoke(sol==ans);
    }
    private void check_answer()
    {
        answer_checked_event?.Invoke(false);
    }
}
