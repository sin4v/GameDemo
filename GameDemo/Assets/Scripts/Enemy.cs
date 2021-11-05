using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Enemy : Effect
{
    public static Enemy Instance;

    public int RandKey;

    public Text healthPointText;
    public Text scpText;

    public int id;
    public int attack;
    public int chosen_id = 0;
    public string enemyName;
    public int currentStunPoint;
    public int specialCumulativePoint;
    public bool enemyIsStun = false;
    public bool enemyIsActed = false;
    public Animator anime;
    public Animator e_anime;
    public List<string[]> moveList = new List<string[]>();

    public UnityEvent isStun = new UnityEvent();

    private void Awake() 
    {
        Instance = this;
    }

    void Start() // 初始化怪物状态
    {
        healthPoint = EnemyInitiazation.Instance.enemyList[chosen_id].healthPoint;
        specialCumulativePoint = EnemyInitiazation.Instance.enemyList[chosen_id].specialCumulativePoint;
        enemyName = EnemyInitiazation.Instance.enemyList[chosen_id].enemyName;
        moveList = EnemyInitiazation.Instance.enemyList[chosen_id].moveList;

        currentStunPoint = specialCumulativePoint;

        BattleManager.Instance.turnStart.AddListener(Act);
    }

    void Update() 
    {
        StastusDisplay();
        IsStun();
    }

    public Enemy(int _id, string _enemyName, int _healthPoint, int _specialCumulativePoint, List<string[]> _moveList)
    {
        this.id = _id;
        this.enemyName = _enemyName;
        this.healthPoint = _healthPoint;
        this.specialCumulativePoint =  _specialCumulativePoint;
        this.moveList = _moveList;
    } 

    public Enemy(int _id, string _enemyName, int _healthPoint, int _specialCumulativePoint) // 构造函数（随便了）
    {
        this.id = _id;
        this.enemyName = _enemyName;
        this.healthPoint = _healthPoint;
        this.specialCumulativePoint =  _specialCumulativePoint;
    } 

    public void AddMoves(string[] _move) // 添加招式
    {
        this.moveList.Add(_move);
    } 

    public void Act() // 攻击方法
    {
        if (BattleManager.Instance.playerTurnNumer == 1)
        {
            RandKey = UnityEngine.Random.Range(0,EnemyInitiazation.Instance.enemyList[chosen_id].moveList.Count);
        }
        if (enemyIsStun == false)
        {
            Attack(attack);
        }
    }

    public override void Attack(int _amount) // 每回合调用敌人的攻击方法
    {
        switch (BattleManager.Instance.playerTurnNumer)
        {
            case 0:
                StartCoroutine(SwitchAttack(EnemyInitiazation.Instance.enemyList[chosen_id].moveList[RandKey][0], _amount));
                SwitchSkill(EnemyInitiazation.Instance.enemyList[chosen_id].moveList[RandKey][0]);
                break;
            case 1:
                StartCoroutine(SwitchAttack(EnemyInitiazation.Instance.enemyList[chosen_id].moveList[RandKey][1], _amount));
                SwitchSkill(EnemyInitiazation.Instance.enemyList[chosen_id].moveList[RandKey][1]);
                break;
            case 2:
                StartCoroutine(SwitchAttack(EnemyInitiazation.Instance.enemyList[chosen_id].moveList[RandKey][2], _amount));
                SwitchSkill(EnemyInitiazation.Instance.enemyList[chosen_id].moveList[RandKey][2]);
                break;
            case 3:
                StartCoroutine(SwitchAttack(EnemyInitiazation.Instance.enemyList[chosen_id].moveList[RandKey][3], _amount));
                SwitchSkill(EnemyInitiazation.Instance.enemyList[chosen_id].moveList[RandKey][3]);
                break;
            case 4:
                StartCoroutine(SwitchAttack(EnemyInitiazation.Instance.enemyList[chosen_id].moveList[RandKey][4], _amount));
                SwitchSkill(EnemyInitiazation.Instance.enemyList[chosen_id].moveList[RandKey][4]);
                break;
        }
    }

    IEnumerator SwitchAttack(string _attacktype, int _attack) // 攻击效果
    {
        switch (_attacktype)
        {
            case "attack_0":
                BattleManager.Instance.enemywillact = true;
                
                yield return new WaitForSeconds(2.0f);
                
                e_anime.SetBool("IsAttack", true);
                if (postion - Player.Instance.postion == 2)
                {
                    Player.Instance.healthPoint -= Convert.ToInt32(_attack * Player.Instance.defence);
                }
                else if (postion - Player.Instance.postion == 1)
                {
                    Player.Instance.healthPoint -= Convert.ToInt32(_attack * Player.Instance.defence * 2);
                }

                if (Player.Instance.isRebound)
                {
                    currentStunPoint -= attack;
                    Player.Instance.isRebound = false;
                    e_anime.SetBool("IsDamaged", true);
                    anime.SetBool("IsRebound", true);
                    anime.SetBool("IsDefend", false);
                }

                enemyIsActed = true;
                BattleManager.Instance.enemywillact = false;
                anime.SetBool("DefendOver", true);
                Debug.Log("怪物攻击");
                break;

            case "attack_1":
                BattleManager.Instance.enemywillact = true;
                yield return new WaitForSeconds(2.0f);

                e_anime.SetBool("IsAttack", true);
                if (postion - Player.Instance.postion == 2)
                {
                    Player.Instance.healthPoint -= Convert.ToInt32(_attack * Player.Instance.defence);
                    Debug.Log("怪物攻击");
                    if (Player.Instance.isRebound)
                    {
                        currentStunPoint -= attack;
                        Player.Instance.isRebound = false;
                        e_anime.SetBool("IsDamaged", true);
                        anime.SetBool("IsRebound", true);
                        anime.SetBool("IsDefend", false);
                    }
                }
                else
                {
                    postion = Player.Instance.postion + 2;
                    Debug.Log("怪物移动");
                }

                enemyIsActed = true;
                BattleManager.Instance.enemywillact = false;
                anime.SetBool("DefendOver", true);
                
                break;     
        }
    }

    public void SwitchSkill(string _skilltype) // 技能效果
    {
        switch (_skilltype)
        {
            case "skill_0":
                AttackPlus(1);
                enemyIsActed = true;
                break;   
        }
    }

    public void AttackPlus(int _amount) // 攻击力增加
    {
        attack += _amount;
    }

    public void IsStun() // 判断是否敌人眩晕
    {
        if (currentStunPoint <= 0)
        {
            isStun.Invoke();
            currentStunPoint = specialCumulativePoint;
            Debug.Log("怪物眩晕");
            enemyIsStun = true;
        }
    }

    public void StastusDisplay() // 显示血量
    {
        healthPointText.text = healthPoint.ToString();
        scpText.text = currentStunPoint.ToString();
    }

    public override void Move(int _step) // 移动（没用）
    {
        throw new System.NotImplementedException();
    }

    public void AttackOver()
    {
        e_anime.SetBool("IsAttack", false);
        e_anime.SetBool("IsDamaged", false);
    }
}
