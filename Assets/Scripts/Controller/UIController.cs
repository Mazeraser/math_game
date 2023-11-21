using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]private TMP_Text example_ui;
    [SerializeField]private TMP_InputField field_ui;
    [SerializeField]private Image timer;

    public delegate void solution_entered(int solution);
    public static event solution_entered solution_entered_event;

    private void Awake()
    {
        exampleGenerator.example_generated_event += show_example;
        GameManager.TimerTurninEvent += set_timer;
    }
    private void OnDestroy()
    {
        exampleGenerator.example_generated_event -= show_example;
        GameManager.TimerTurninEvent -= set_timer;
    }

    private void show_example(string example)
    {
        example_ui.text = example;
        field_ui.text = "";
    }
    public void share_solution(string sol)
    {
        solution_entered_event?.Invoke(int.Parse(sol));
    }
    private void set_timer(float val)
    {
        timer.fillAmount = val;
    }
}
