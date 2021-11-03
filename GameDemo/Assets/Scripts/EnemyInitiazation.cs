using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInitiazation : MonoSingleton<EnemyInitiazation>
{
    public static EnemyInitiazation Instance;
    public TextAsset enemyData;

    private void Awake() 
    {
        Instance = this;
    }

    public List<Enemy> enemyList = new List<Enemy>();

    public void LoadEnemyData() // 读取怪物数据
    {
        string[] dataRow = enemyData.text.Split('\n');
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")
            {
                continue;
            }
            else if (rowArray[0] == "monster")
            {
                int _id = int.Parse(rowArray[1]);
                string _name = rowArray[2];
                int _hp = int.Parse(rowArray[3]);
                int _scp = int.Parse(rowArray[4]);

                enemyList.Add(new Enemy(_id, _name, _hp, _scp));

                Debug.Log("怪物数据已读取");
            }
            else if (rowArray[0] == "moves")
            {
                int id = int.Parse(rowArray[1]);
                string[] _move = new string[5];

                for (int i = 0; i < 5; i++)
                {
                    _move[i] = rowArray[i+2];
                }

                enemyList[id].AddMoves(_move);
            }
        }
    }
}


