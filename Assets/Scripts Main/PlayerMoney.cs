using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMoney : MonoBehaviour
{
    public static float MONEY;
    public TextMeshProUGUI moneyDisplay;
    // Start is called before the first frame update
    void Start()
    {
        MONEY = getMoney();
    }

    void Update()
    {
        moneyDisplay.text = ((int)(PlayerPrefs.GetFloat("money") / 97)).ToString();
    }

    public static void saveMoney(){
        PlayerPrefs.SetFloat("money", MONEY * 97);
    }

    public static float getMoney(){
        return (int)PlayerPrefs.GetFloat("money") / 97;
    }
}
