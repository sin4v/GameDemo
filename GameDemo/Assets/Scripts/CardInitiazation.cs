using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInitiazation : MonoSingleton<CardInitiazation>
{
    public static CardInitiazation Instance;

    public TextAsset cardData;  // 牌的数据
    public List<Card> cardList = new List<Card>();  // 游戏牌池
   
    private void Awake() 
    {
        Instance = this;
    }
    
    public void LoadCardData() // 读取游戏牌池数据
    {
        string[] dataRow = cardData.text.Split('\n');
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "attack")
            {
                int id = int.Parse(rowArray[1]);
                int cost = int.Parse(rowArray[2]);
                string cardname = rowArray[3];
                int amount = int.Parse(rowArray[4]);

                cardList.Add(new AttackCard(id, cost, cardname, amount));

                Debug.Log("攻击卡已读取");

            }
            else if (rowArray[0] == "defend")
            {
                int id = int.Parse(rowArray[1]);
                int cost = int.Parse(rowArray[2]);
                string cardname = rowArray[3];
                int amount = int.Parse(rowArray[4]);

                cardList.Add(new DefendCard(id, cost, cardname, amount));

                Debug.Log("防御卡已读取");
            }
            else if (rowArray[0] == "skill")
            {
                int id = int.Parse(rowArray[1]);
                int cost = int.Parse(rowArray[2]);
                string cardname = rowArray[3];
                string effect = rowArray[4];

                cardList.Add(new SkillCard(id, cost, cardname, effect));

                Debug.Log("技能卡已读取");
            }
            
        }
    }

    public void SaveCard()
    {

    }
}
