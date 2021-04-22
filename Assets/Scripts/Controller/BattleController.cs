using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleController : StateMachine
{
    public CameraRig cameraRig;
    public Board board;
    public LevelData levelData;
    public Transform tileSelectionIndicator;
    public Point pos;

    public GameObject heroPrefab;
    public GameObject enemyPrefab;
    public GameObject pathSegPrefab;
    public Unit currentUnit;
    public List<Unit> currentUnits;
    public Tile currentTile { get { return board.GetTile(pos); } }

    void Start()
    {
        //currentUnit = heroPrefab.GetComponent<Unit>();
        ChangeState<InitBattleState>();
    }
}
