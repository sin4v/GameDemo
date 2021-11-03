using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData : MonoSingleton<PlayerData>
{
   public static PlayerData Instance;
   
   public int[] playerCards;
   public TextAsset playerData;
    
    private void Awake() 
    {
        Instance = this;
    }
    
    public void LoadPlayerData() //读取玩家数据
    {
        playerCards = new int[CardInitiazation.Instance.cardList.Count];
        
        string[] dataRow = playerData.text.Split('\n');
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "card")
            {
                int id = int.Parse(rowArray[1]);
                int num = int.Parse(rowArray[2]);
                playerCards[id] = num;

                Debug.Log("卡已读取入角色");
            }
        }
    }

    public void SavePlayerData() //保存玩家数据
    {
        string path = Application.dataPath + "/Datas/playerdata.csv"; 
        
        List<string> datas = new List<string>();
        for (int i = 0; i < playerCards.Length; i++)
        {
            if (playerCards[i] != 0)
            {
                datas.Add("card," + i.ToString() + "," + playerCards[i].ToString());                
            }
        }

        File.WriteAllLines(path, datas);
    }

    public Card CopyCard(int _id) // 从卡牌数据中复制一个实体，这个实体的改变不会影响原始数据
    {
        Card card = CardInitiazation.Instance.cardList[_id];
        Card copyCard = new Card(_id, card.Cost, card.CardName);
        if (card is AttackCard)
        {
            var attackcard = card as AttackCard;
            copyCard = new AttackCard(_id, attackcard.Cost, attackcard.CardName, attackcard.Amount);
        }
        if (card is DefendCard)
        {
            var defendcard = card as DefendCard;
            copyCard = new DefendCard(_id, defendcard.Cost, defendcard.CardName, defendcard.Amount);
        }
        if (card is SkillCard)
        {
            var skillcard = card as SkillCard;
            copyCard = new SkillCard(_id, skillcard.Cost, skillcard.CardName, skillcard.Effect);
        }
        return copyCard;
    }

}
