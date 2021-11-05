using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoSingleton<CardManager>
{
    public static CardManager Instance;
    
    public TextAsset cardData;
    public bool isUsedCard;
    public bool isAbleUseCard = false;

    public Animator anime;
    public Animator e_anime;

    private void Awake() 
    {
        Instance = this;
    }

    public void CardEfect(Card _card) // 触发卡牌效果
    {
        if (_card is AttackCard && ReturnAttackCard(_card).Cost <= BattleManager.Instance.playerCurrentCosts) //判断费用是否足够
        {
            BattleManager.Instance.playerCurrentCosts -= ReturnAttackCard(_card).Cost;
            Player.Instance.Attack(ReturnAttackCard(_card).Amount);
            isUsedCard = true;
            anime.SetBool("Attacked",true);
            anime.SetBool("IsAttack",false);
            e_anime.SetBool("IsDamaged",true);
        }
        else if (_card is DefendCard && ReturnDefendCard(_card).Cost <= BattleManager.Instance.playerCurrentCosts)
        {
            BattleManager.Instance.playerCurrentCosts -= ReturnDefendCard(_card).Cost;
            Player.Instance.Defend(ReturnAttackCard(_card).Amount);
            isUsedCard = true;
        }
        else if (_card is SkillCard && ReturnSkillCard(_card).Cost <= BattleManager.Instance.playerCurrentCosts)
        {
            Player.Instance.Skill(ReturnSkillCard(_card).id);
            if (Player.Instance.isUsed) // 判断卡牌是否使用成功
            {
                isUsedCard = true;
                BattleManager.Instance.playerCurrentCosts -= ReturnSkillCard(_card).Cost;
            }
            else
            {
                isUsedCard = false;
            } 
        }
        else
        {
            Debug.Log("费用不够");
            isUsedCard = false;
        }
    }

    public AttackCard ReturnAttackCard(Card _card) // 返回卡牌类型为攻击卡牌
    {
        string[] dataRow = cardData.text.Split('\n');
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[1] == _card.id.ToString()) 
            {
                int id = int.Parse(rowArray[1]);
                int cost = int.Parse(rowArray[2]);
                string cardname = rowArray[3];
                int amount = int.Parse(rowArray[4]);

                AttackCard _acard = new AttackCard(id, cost, cardname, amount);

                return _acard;

            }
        }
        return null;
    }

    public DefendCard ReturnDefendCard(Card _card) // 返回卡牌类型为防御卡牌
    {
        string[] dataRow = cardData.text.Split('\n');
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[1] == _card.id.ToString()) 
            {
                int id = int.Parse(rowArray[1]);
                int cost = int.Parse(rowArray[2]);
                string cardname = rowArray[3];
                int amount = int.Parse(rowArray[4]);

                DefendCard _dcard = new DefendCard(id, cost, cardname, amount);

                return _dcard;

            }
        }
        return null;
    }

    public SkillCard ReturnSkillCard(Card _card) // 返回卡牌类型为技能卡牌
    {
        string[] dataRow = cardData.text.Split('\n');
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[1] == _card.id.ToString()) 
            {
                int id = int.Parse(rowArray[1]);
                int cost = int.Parse(rowArray[2]);
                string cardname = rowArray[3];
                string effect = rowArray[4];

                SkillCard _scard = new SkillCard(id, cost, cardname, effect);

                return _scard;

            }
        }
        return null;
    }
}
