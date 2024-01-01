using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiifButtonController : MonoBehaviour
{
    //смена сложности
    public delegate void diff_selected(int min, int addit, int max_ex_range, int time, string name);
    public static event diff_selected DiffSelectedEvent;

    [SerializeField][Tooltip("Нижняя граница генерации числа для примера")]private int min;
    [SerializeField][Tooltip("Прибавка к минимальной границе(начальная максимальная граница)")]private int add_to_min;
    [SerializeField][Tooltip("Максимальная длина примера")]private int max_ex_range;
    [SerializeField][Tooltip("Время на решение примера")]private int time;

    private void Start()
    {
        if(name.Contains("Medium"))
        {
            OnPointerClick();
        }
    }

    public void OnPointerClick()
    {
        DiffSelectedEvent?.Invoke(min,add_to_min,max_ex_range,time, name);
    }
}