using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GamePhase
{
    turnStart, turnEnd, wholeTurnStart, wholeTurnEnd
}

public class BattleManager : MonoSingleton<BattleManager>
{
    public static BattleManager Instance;

    public GamePhase GamePhase = GamePhase.wholeTurnStart; 
    
    bool playerisacted;
    public bool enemywillact = false;
    public bool isReward = false;
    public bool isAbleUseCard;
    
    public int playerTurnNumer;
    
    public float turnTime = 3.0f;
    public float turntimer;
    
    public int playerCurrentCosts;
    
    public GameObject playerAttackHandsArea;
    public GameObject playerDefendHandsArea;
    public GameObject playerSkillHandsArea;
    public GameObject cardPrefab;

    public List<Card> playerDeckList = new List<Card>();
    public List<Card> playerDiscardList = new List<Card>();
    public List<GameObject> playerAttackHands = new List<GameObject>();
    public List<GameObject> playerDefendHands = new List<GameObject>();
    public List<GameObject> playerSkillHands = new List<GameObject>();

    public UnityEvent turnStart = new UnityEvent();

    private void Awake() 
    {
        Instance = this;
    }

    void Start() 
    {
        PlayerController.Instance.isUseCard.AddListener(UseCard);
        Enemy.Instance.isStun.AddListener(Reward);
        
        playerCurrentCosts = Player.Instance.maxCosts;
        turntimer = turnTime;
        isAbleUseCard = true;
    }

    void Update()
    {       
        ReShuffle();
        TurnChanged();       
        //Debug.Log(turntimer);
    }
        

    public virtual void GameStart() // 游戏开始，读取卡组，抽对应数量手牌
    {
        ReadDeck();
        DrawCard(Player.Instance.agility);
    }

    void UseCard() // 使用卡牌
    {
        if (isAbleUseCard && PlayerController.Instance.ChosenCard().GetComponent<CardDisplay>().card.id != 99) // 判断是否可以使用卡牌
        {
            CardManager.Instance.CardEfect(PlayerController.Instance.ChosenCard().GetComponent<CardDisplay>().card);
            
            if (CardManager.Instance.isUsedCard && CardManager.Instance.isAbleUseCard == false) // 判断是否成功使用了卡牌
            {
                isAbleUseCard = false;
                playerisacted = true;
                Player.Instance.IsCombo(PlayerController.Instance.ChosenCard().GetComponent<CardDisplay>().card);
                DiscardCard();
            } 
            else if (CardManager.Instance.isUsedCard && CardManager.Instance.isAbleUseCard == true) // 判断是否可以再次使用卡牌
            {
                CardManager.Instance.isAbleUseCard = false;
                Player.Instance.IsCombo(PlayerController.Instance.ChosenCard().GetComponent<CardDisplay>().card);
                DiscardCard();
            }
            else
            {
                Debug.Log("卡牌使用失败");
            }
        }
        else if (isAbleUseCard == false)
        {
            Debug.Log("本回合已行动");
        }
        else
        {
            Debug.Log("请选择卡牌");
        }
    }

    public void TurnChanged() //回合管理器（可以用事件触发器优化）
    {
        turntimer -=  Time.deltaTime;

        if (GamePhase == GamePhase.wholeTurnStart)
        {
            playerTurnNumer = 0;
            playerCurrentCosts = Player.Instance.maxCosts;
            GamePhase = GamePhase.turnStart;
            turnStart.Invoke();
        }

        if (GamePhase == GamePhase.turnStart)
        {

        }

        if (PlayerController.Instance.isSkipTurn && enemywillact == false)
        {
            turntimer = turnTime;
            GamePhase = GamePhase.turnEnd;
        }
        else if (turntimer < 0)
        {
            GamePhase = GamePhase.turnEnd; 
        }
        else if (PlayerController.Instance.isSkipTurn)
        {
            Debug.Log("怪物将要行动，无法跳过回合");
        }

        if (GamePhase == GamePhase.turnEnd)
        {
            if (playerisacted == false && Enemy.Instance.enemyIsActed == false)
            {
                DrawCard(1);
                playerCurrentCosts += 1;
            }
            playerTurnNumer++;
            turntimer = turnTime;
            isAbleUseCard = true;
            playerisacted = false;
            Enemy.Instance.enemyIsActed = false;
            Player.Instance.defence = 1.0f;
            GamePhase = GamePhase.turnStart;
            turnStart.Invoke();
        }

        if (playerTurnNumer == Player.Instance.agility)
        {
            GamePhase = GamePhase.wholeTurnEnd;
        }

        if (GamePhase == GamePhase.wholeTurnEnd)
        {
            Enemy.Instance.enemyIsStun = false;
            GamePhase = GamePhase.wholeTurnStart;
        }

    }
    
    public void DrawCard(int _num) // 抽牌并按卡片类型分配到不同区域
    {
        for (int i = 0; i < _num; i++)
        {
            if(CardNum() < Player.Instance.agility || isReward)
            {
                if (playerDeckList[0] is AttackCard)
                {
                    GameObject newCard = GameObject.Instantiate(cardPrefab, playerAttackHandsArea.transform);
                    newCard.GetComponent<CardDisplay>().card = playerDeckList[0];
                    playerDeckList.RemoveAt(0);
                    playerAttackHands.Add(newCard);
                }
                else if (playerDeckList[0] is DefendCard)
                {
                    GameObject newCard = GameObject.Instantiate(cardPrefab, playerDefendHandsArea.transform);
                    newCard.GetComponent<CardDisplay>().card = playerDeckList[0];
                    playerDeckList.RemoveAt(0);
                    playerDefendHands.Add(newCard);
                }
                else if (playerDeckList[0] is SkillCard)
                {
                    GameObject newCard = GameObject.Instantiate(cardPrefab, playerSkillHandsArea.transform);
                    newCard.GetComponent<CardDisplay>().card = playerDeckList[0];
                    playerDeckList.RemoveAt(0);
                    playerSkillHands.Add(newCard);
                }
            }
            else
            {
                Debug.Log("手牌已满");
            }      
        }
    }

    public void ReShuffle() // 弃牌堆重新洗回抽牌堆
    {
        if (playerDeckList.Count == 0)
        {
            for (int i = 0; i < playerDiscardList.Count; i++)
            {
                Card card = playerDiscardList[0];
                playerDiscardList.RemoveAt(0);
                playerDeckList.Add(card);
            }
            ShuffletDeck();
        }
    }
    
    public void DiscardCard() // 丢弃手牌到弃牌堆
    {            
        if (PlayerController.Instance.ChosenCard().GetComponent<CardDisplay>().card is AttackCard)
        {
            GameObject newCard = PlayerController.Instance.ChosenCard();
            Destroy(playerAttackHands[PlayerController.Instance.a_num - 1]);
            playerAttackHands.RemoveAt(PlayerController.Instance.a_num - 1);
            PlayerController.Instance.a_num = 0;
            playerDiscardList.Add(newCard.GetComponent<CardDisplay>().card);
        }

        if (PlayerController.Instance.ChosenCard().GetComponent<CardDisplay>().card is SkillCard)
        {
            Card newCard = PlayerController.Instance.ChosenCard().GetComponent<CardDisplay>().card;
            Destroy(playerSkillHands[PlayerController.Instance.s_num - 1]);
            playerSkillHands.RemoveAt(PlayerController.Instance.s_num - 1);
            PlayerController.Instance.s_num = 0;
            playerDiscardList.Add(newCard);
        }

        if (PlayerController.Instance.ChosenCard().GetComponent<CardDisplay>().card is DefendCard)
        {
            Card newCard = PlayerController.Instance.ChosenCard().GetComponent<CardDisplay>().card;
            Destroy(playerDefendHands[PlayerController.Instance.d_num - 1]);
            playerDefendHands.RemoveAt(PlayerController.Instance.d_num - 1);
            PlayerController.Instance.d_num = 0;
            playerDiscardList.Add(newCard);
        }           
        
    }

    public void ReadDeck() // 读取玩家数据中的牌组并随机洗牌
    {
        for (int i = 0; i < PlayerData.Instance.playerCards.Length; i++)
        {
            if (PlayerData.Instance.playerCards[i] != 0)
            {
                int counter = PlayerData.Instance.playerCards[i];
                for (int j = 0; j < counter; j++)
                {
                    playerDeckList.Add(PlayerData.Instance.CopyCard(i));
                }
            }
        }
        ShuffletDeck();   
    }

    public void ShuffletDeck() // 随机洗牌
    {
        // 洗牌算法的基本思路是遍历整个卡组，对于每一张牌，都和随机的一张牌调换位置。
        for (int i = 0; i < playerDeckList.Count; i++)
                {
                    int rad = Random.Range(0, playerDeckList.Count);
                    Card temp = playerDeckList[i];
                    playerDeckList[i] = playerDeckList[rad];
                    playerDeckList[rad] = temp;
                }
    }

    public int CardNum() // 获取当前手牌总数
    {
        int _num = playerDefendHands.Count + playerSkillHands.Count + playerAttackHands.Count;
        return _num;
    }

    public void Reward() // 眩晕奖励
    {
        Debug.Log("奖励启用");
        DrawCard(2);
        Enemy.Instance.currentStunPoint = Enemy.Instance.specialCumulativePoint;
    }

    public void RewardAttack()
    {
        isReward = true;
        DrawCard(1);
        isReward = false;
    }
}
