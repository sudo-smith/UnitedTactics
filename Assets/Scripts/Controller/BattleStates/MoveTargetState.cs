using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MoveTargetState : BattleState
{
    List<Tile> tiles;
    List<GameObject> pathSegments;

    public override void Enter()
    {
        base.Enter();
        if(owner.currentUnits.Count > 0){
            Debug.Log("Entered movement with multiple units selected");
        } else{
            Movement mover = owner.currentUnit.GetComponent<Movement>();
            tiles = mover.GetTilesInRange(board);
            pathSegments = new List<GameObject>();
            board.SelectTiles(tiles);
        }
    }

    public override void Exit()
    {
        base.Exit();
        board.DeSelectTiles(tiles);
        tiles = null;
    }

    void renderPathSeg(Vector3 startPos, Vector3 endPos)
    {
        GameObject pathSeg = Instantiate(owner.pathSegPrefab) as GameObject;
        pathSegments.Add(pathSeg);
        pathSeg.transform.localPosition = startPos;
        Vector3 newScale = new Vector3(pathSeg.transform.localScale.x, pathSeg.transform.localScale.y, (endPos - startPos).magnitude);
        pathSeg.transform.localScale = newScale;
        pathSeg.transform.localRotation = Quaternion.FromToRotation(Vector3.forward, endPos - startPos);
    }

    void clearPath()
    {
        foreach (GameObject path in pathSegments)
        {
            Destroy(path);
        }
    }

    void renderPath()
    {

        //TODO: optimize this to not destroy segments that are reused
        Tile curTile = null;
        foreach (Tile t in tiles)
        {
            if (t.pos == pos)
            {
                curTile = t;
                break;
            }
        }
        if (owner.currentTile == owner.currentUnit.tile)
        {
            clearPath();
            return;
        }
        if (curTile == null)
            return;
        clearPath();
        while (curTile.prev != null)
        {
            Vector3 start = curTile.prev.center + new Vector3(0, Tile.stepHeight / 2f, 0);
            Vector3 end = curTile.center + new Vector3(0, Tile.stepHeight / 2f, 0);
            if (start.y != end.y)
            {
                Vector3 midPoint1, midPoint2;
                Vector3 vecToEndXZ = (new Vector3(end.x, start.y, end.z) - start); //vector from start to end in just xz
                float midPointStepDistance = (start.y < end.y) ? 0.25f : 0.75f; //depending on if going up or down need to go different distances to first midpoint
                midPoint1 = (vecToEndXZ * midPointStepDistance) + start;
                midPoint2 = new Vector3(midPoint1.x, end.y, midPoint1.z);
                renderPathSeg(start, midPoint1);
                renderPathSeg(midPoint1, midPoint2);
                start = midPoint2;
            }
            renderPathSeg(start, end);
            curTile = curTile.prev;
        }
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        bool didMove = !(pos == (e.info + pos) || !board.tiles.ContainsKey(e.info + pos));
        SelectTile(e.info + pos);
        if (didMove)
        {
            renderPath();
        }
    }

    protected override void OnButton(object sender, InfoEventArgs<int> e)
    {
        if (e.buttonToString() != "Select")
        {
            return;
        }
        if (tiles.Contains(owner.currentTile))
        {
            foreach (GameObject path in pathSegments)
            {
                Destroy(path);
            }
            owner.ChangeState<MoveSequenceState>();
        }

    }
}
