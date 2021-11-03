using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public Text current_Turn;
    public Text current_Costs;
    public Text postion;
    void Update()
    {
        HudDisplay();
    }

    public void HudDisplay()
    {
        current_Costs.text = BattleManager.Instance.playerCurrentCosts.ToString();
        current_Turn.text = (BattleManager.Instance.playerTurnNumer + 1).ToString();
        postion.text = (Enemy.Instance.postion- Player.Instance.postion).ToString();
    }
}
