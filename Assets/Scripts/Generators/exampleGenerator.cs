using System.Collections.Generic;
using UnityEngine;

public class exampleGenerator : MonoBehaviour, IGenerator
{
    [Tooltip("Максимальная длина примера")]private int max_range;
    public int Max_Range{get{return max_range;}}

    public delegate void example_generated(string example);
    public static event example_generated example_generated_event;

    public delegate void subscribed();
    public static event subscribed SubscribedEvent;

    public void create(int max_ex_range)
    {
        max_range=max_ex_range;
    }

    private void Awake()
    {
        numGenerator.num_generated_event+=Generate;
        SubscribedEvent?.Invoke();
    }
    private void OnDestroy()
    {
        numGenerator.num_generated_event-=Generate;
    }

    private int[] find_dividers(int num)
    {
        List<int> res = new List<int>();
        for(int i = 1;i<num/2+1;i++)
        {
            if(num%i==0&&!(i==1))
            {
                res.Add(i);
            }
        }
        if(res.Count>0)
            return res.ToArray();
        else
            return new int[1]{1};
    }
    private bool isSimple(int num){
        List<int> res = new List<int>();
        for(int i = 1;i<num+1;i++)
        {
            if(num%i==0)
                res.Add(i);
        }
        return res.Count==2;
    }
    public void Generate(int num)
    {
        Debug.Log(num);
        int beg_num = num;
        string res = "";
        int?[] multiplier = new int?[max_range];
        char act = ' ';
        int mul_c=0;
        switch(Random.Range(0,2))
        {
            case 0: //умножение
                act = '*';
                for(int i=0;i<Random.Range(2,max_range);i++)
                {
                    if(isSimple(num))
                    {
                        multiplier[i]=num;
                        break;
                    }
                    else
                    {
                        int[] div=find_dividers(num);
                        int ind = Random.Range(0,div.Length);
                        num/=div[ind];
                        multiplier[i]=div[ind];
                        multiplier[i+1]=num;
                    }
                }
                break;
            case 1: //сложение
                act='+';
                for(int i=0;i<Random.Range(2,max_range);i++)
                {
                    if(num>=1)
                    {
                        int n = Random.Range(1,num);
                        multiplier[i]=n;
                        num-=n;
                        multiplier[i+1]=num;
                    }
                    else
                        break;
                }
                break;
        }
        foreach (int? mult in multiplier) 
        {
            if(mult==null)
                break;
            res+=mult.ToString()+act.ToString();
        }
        res=res.Substring(0,res.Length-1);//обрезается последнее умножение или сложение
        example_generated_event?.Invoke(res);
    }
}
