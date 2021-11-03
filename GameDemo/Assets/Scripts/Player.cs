using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Effect
{
    public static Player Instance;

    public Text healthPointText;
    public int maxCosts;
    public int agility;
    public float defence;
    public bool isUsed;
    public bool isRebound = false;
    public int plusAmount;
    public List<string> moveList = new List<string>();
    
    private void Awake() 
    {
        Instance = this;
    }

    void Update()
    {
        HealthDisplay();
    }

    public override void Attack(int _amount) // 使用攻击
    {
        if ((Enemy.Instance.postion - postion) == 2)
        {
            Enemy.Instance.healthPoint -= _amount + plusAmount;
        }
        else if ((Enemy.Instance.postion - postion) == 1)
        {
            Enemy.Instance.healthPoint -= (_amount + plusAmount) *2;
            Enemy.Instance.currentStunPoint -= _amount;
        }

        if (BattleManager.Instance.enemywillact == false)
        {
            BattleManager.Instance.RewardAttack();
        }       
    }

    public void Defend(int _amount) // 使用防御
    {
        if (BattleManager.Instance.turntimer > 3f && BattleManager.Instance.turntimer < 5f)
        {
            defence = 0.0f;
            isRebound = true;
            Debug.Log("弹反成功");
            return;
        }
        defence = (100 - _amount) / 100.0f;
    }

    public void Skill(int _id) // 使用技能
    {
        switch (_id)
        {
            case 4 :
                if ((Enemy.Instance.postion - postion <= 3) && (Enemy.Instance.postion - postion >= 1) && PlayerController.Instance.step != 0)
                {
                    Move(PlayerController.Instance.step);
                    CardManager.Instance.isAbleUseCard = true;
                    isUsed = true;
                }
                else
                {
                    isUsed = false;
                    Debug.Log("移动条件不满足");
                }
                break;       
        }
    }
 
    public void IsCombo(Card _card) // 判断连击
    {
        moveList.Add(_card.CardName);
        if (moveList[0] == "挥击" && moveList.Count > 1)
        {
            if (moveList[1] == "挥击" && moveList.Count > 2)
            {
                if (moveList[2] == "重挥击")
                {
                    Enemy.Instance.healthPoint -= CardManager.Instance.ReturnAttackCard(_card).Amount;
                    Enemy.Instance.currentStunPoint -= 2 * CardManager.Instance.ReturnAttackCard(_card).Amount;
                    Debug.Log("连招成功");
                    Removes(moveList, 3);
                }
                else if (moveList[2] == "挥击")
                {
                    Removes(moveList, 1);
                    Debug.Log("释放一个挥击");
                    return;
                }
            }
            else if (moveList[1] == "挥击" && moveList.Count == 2)
            {
                Debug.Log("储存挥击2");
                return;
            }
            else
            {
                Debug.Log("释放连击储存");
                Removes(moveList, 2);
            } 
        }    
        else if (moveList[0] == "挥击" && moveList.Count == 1)
        {
            Debug.Log("储存挥击");
            return;
        }
        else
        {
            Debug.Log("释放连击储存");
            Removes(moveList, 1);
        }
    }

    public void Removes(List<string> _list, int _times)
    {
        for (int i = 0; i < _times; i++)
        {
            _list.RemoveAt(0);
        }
    }

    public void HealthDisplay() // 显示血量
    {
        healthPointText.text = healthPoint.ToString();
    }

    public override void Move(int _step) // 移动
    {
       postion += _step;
       PlayerController.Instance.step = 0;
    }
}
