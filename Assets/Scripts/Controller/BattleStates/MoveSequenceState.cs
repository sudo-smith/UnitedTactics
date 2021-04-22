using UnityEngine;
using System.Collections;
public class MoveSequenceState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine("Sequence");
    }

    bool AllUnitsMoved()
    {
        foreach(Unit u in owner.board.AllyUnits)
        {
            Movement m = u.GetComponent<Movement>();
            if(m.rangeLeft != 0)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator Sequence()
    {
        Movement m = owner.currentUnit.GetComponent<Movement>();
        m.rangeLeft -= owner.currentTile.distance;
        if(m.rangeLeft < 0)
        {
            throw new System.Exception("Range Left less than 0");
        }
        yield return StartCoroutine(m.Traverse(owner.currentTile));
        if (AllUnitsMoved())
        {
            Debug.Log("All Units Have Moved"); //todo print this on the UI
            owner.ChangeState<PhaseChangeState>(StateMachine.Phase.Combat);
        }
        else
        {
            owner.ChangeState<SelectUnitState>();
        }
    }
}
