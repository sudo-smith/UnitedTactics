using UnityEngine;
using System.Collections;
public class InitBattleState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Init());
    }
    IEnumerator Init()
    {
        board.Load(levelData);
        Point p = new Point((int)levelData.tiles[0].x, (int)levelData.tiles[0].z);
        SelectTile(p);
        SpawnTestUnits();
        yield return null;
        owner.ChangeState<PhaseChangeState>(StateMachine.Phase.Movement);
    }

    void SpawnTestUnits()
    {
        for (int i = 0; i < 3; ++i)
        {
            GameObject heroObject = Instantiate(owner.heroPrefab) as GameObject;
            heroObject.tag = "Hero";
            Point p = new Point((int)levelData.tiles[i].x, (int)levelData.tiles[i].z);
            Unit heroUnit = heroObject.GetComponent<Unit>();
            heroUnit.isAlly = true;
            heroUnit.Place(board.GetTile(p));
            heroUnit.Match();
            Movement m = heroObject.AddComponent(typeof(WalkMovement)) as Movement;
            m.maxRange = 5;
            m.jumpHeight = 1;
            board.AllyUnits.Add(heroUnit);
        }

        int numTiles = levelData.tiles.Count;
        for(int i = 20; i < 21; i++)
        {
            GameObject enemyObject = Instantiate(owner.enemyPrefab) as GameObject;
            enemyObject.tag = "Enemy";
            Point p = new Point((int)levelData.tiles[i].x, (int)levelData.tiles[i].z);
            Unit enemyUnit = enemyObject.GetComponent<Unit>();
            enemyUnit.isAlly = false;
            enemyUnit.Place(board.GetTile(p));
            enemyUnit.Match();
            board.EnemyUnits.Add(enemyUnit);
        }
    }
}
