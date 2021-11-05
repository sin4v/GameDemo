using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoSingleton<PlayerController>
{
    public static PlayerController Instance;

    public int a_num{get;set;}
    public int d_num{get;set;}
    public int s_num{get;set;}
    public int step;
    public bool isSkipTurn = false;
    public Animator anime;
    
    public float zoomSize;

    public UnityEvent isUseCard = new UnityEvent();

    Card _nullcard;
    public GameObject nullcard;

    private void Awake() 
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _nullcard = new Card(99,99,"99");
        nullcard.GetComponent<CardDisplay>().card = _nullcard;
    }

    // Update is called once per frame
    void Update()
    {
        IsGetKey();
    }

    public void IsGetKey() // 获取键鼠输入返回值，并进行对应操作
    {
        if (Input.GetMouseButtonDown(0))
        {
            isUseCard.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSkipTurn = true;
        }
        else
        {
            isSkipTurn = false;
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            
            AttackCardSwitch();
            ChosenCard();
            anime.SetBool("Attacked",false);
            anime.SetBool("IsDefend",false);
            anime.SetBool("IsAttack",true);
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            DefendCardSwitch();
            ChosenCard();
            anime.SetBool("DefendOver",false);
            anime.SetBool("IsAttack",false);
            anime.SetBool("IsDefend",true);
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            SkillCardSwitch();
            ChosenCard();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            step = 1;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            step = -1;
        }
    }

    public GameObject ChosenCard() // 获取被选中卡牌
    {
        if (a_num > 0)
        {
            return BattleManager.Instance.playerAttackHands[a_num - 1];
        }
        
        if (d_num > 0)
        {
            return BattleManager.Instance.playerDefendHands[d_num - 1];
        }
        
        if (s_num > 0)
        {
            return BattleManager.Instance.playerSkillHands[s_num - 1];
        }
        
        return nullcard;
    }
    
    public void AttackCardSwitch() // 切换攻击区里的手牌
    {
        if (BattleManager.Instance.playerAttackHands.Count != 0)
        {                      
            DefendCardRezoom(d_num - 1);
            SkillCardRezoom(s_num - 1);
            d_num = 0;
            s_num = 0;

            if (a_num == BattleManager.Instance.playerAttackHands.Count)
            {
                AttackCardRezoom(a_num - 1);
                a_num = 0;
                BattleManager.Instance.playerAttackHands[a_num].transform.localScale = new Vector3(zoomSize, zoomSize, 1.0f);
            }
            else if (a_num > 0)
            {
                BattleManager.Instance.playerAttackHands[a_num].transform.localScale = new Vector3(zoomSize, zoomSize, 1.0f);
                AttackCardRezoom(a_num - 1);
            }
            else if (a_num == 0)
            {
                BattleManager.Instance.playerAttackHands[a_num].transform.localScale = new Vector3(zoomSize, zoomSize, 1.0f);
            }
            a_num++;
        }      
    }

    public void DefendCardSwitch() // 切换防御区里的手牌
    {
        if (BattleManager.Instance.playerDefendHands.Count != 0)
        {                      
            AttackCardRezoom(a_num - 1);
            SkillCardRezoom(s_num - 1);
            s_num = 0;
            a_num = 0;
            

            if (d_num == BattleManager.Instance.playerDefendHands.Count)
            {
                DefendCardRezoom(d_num - 1);
                d_num = 0;
                BattleManager.Instance.playerDefendHands[d_num].transform.localScale = new Vector3(zoomSize, zoomSize, 1.0f);
            }
            else if (d_num > 0)
            {
                BattleManager.Instance.playerDefendHands[d_num].transform.localScale = new Vector3(zoomSize, zoomSize, 1.0f);
                DefendCardRezoom(d_num - 1);
            }
            else if (d_num == 0)
            {
                BattleManager.Instance.playerDefendHands[d_num].transform.localScale = new Vector3(zoomSize, zoomSize, 1.0f);
            }
            d_num++;
        }       
    }

    public void SkillCardSwitch() // 切换技能区里的手牌
    {
        if (BattleManager.Instance.playerSkillHands.Count != 0)
        {                      
            AttackCardRezoom(a_num - 1);
            DefendCardRezoom(d_num - 1);
            d_num = 0;
            a_num = 0;

            if (s_num == BattleManager.Instance.playerSkillHands.Count)
            {
                SkillCardRezoom(s_num - 1);
                s_num = 0;
                BattleManager.Instance.playerSkillHands[s_num].transform.localScale = new Vector3(zoomSize, zoomSize, 1.0f);
            }
            else if (s_num > 0)
            {
                BattleManager.Instance.playerSkillHands[s_num].transform.localScale = new Vector3(zoomSize, zoomSize, 1.0f);
                SkillCardRezoom(s_num - 1);
            }
            else if (s_num == 0)
            {
                BattleManager.Instance.playerSkillHands[s_num].transform.localScale = new Vector3(zoomSize, zoomSize, 1.0f);
            }
            s_num++;
        }       
    }

    public void AttackCardRezoom(int _num) // 取消选中手牌
    {
        if (BattleManager.Instance.playerAttackHands.Count != 0 && _num >= 0 )
        {
            BattleManager.Instance.playerAttackHands[_num].transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    public void DefendCardRezoom(int _num)
    {
        if (BattleManager.Instance.playerDefendHands.Count != 0 && _num >= 0 )
        {
            BattleManager.Instance.playerDefendHands[_num].transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    public void SkillCardRezoom(int _num)
    {
        if (BattleManager.Instance.playerSkillHands.Count != 0 && _num >= 0 )
        {
            BattleManager.Instance.playerSkillHands[_num].transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
