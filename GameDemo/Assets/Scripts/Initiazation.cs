using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initiazation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CardInitiazation.Instance.LoadCardData();
        PlayerData.Instance.LoadPlayerData();
        EnemyInitiazation.Instance.LoadEnemyData();
        BattleManager.Instance.GameStart();
    }

}
