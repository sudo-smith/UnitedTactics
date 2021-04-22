using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseChangeState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine("Sequence");
    }

    void ResetMovement(List<Unit> units)
    {
        foreach(Unit u in units)
        {
            Movement m = u.GetComponent<Movement>();
            m.rangeLeft = m.maxRange;
        }
    }

    IEnumerator Sequence()
    {
        string msg;
        switch (owner.CurrentPhase)
        {
            case StateMachine.Phase.Movement:
                ResetMovement(board.AllyUnits);
                msg = "Start Movement Phase";
                break;
            case StateMachine.Phase.Combat:
                msg = "Start Combat Phase";
                break;
            default:
                msg = "Error out of phase";
                break;
        }
        Debug.Log(msg);
        //todo actually add some type of message animation
        yield return null;

        //phase is still the same so no need to update it
        owner.ChangeState<SelectUnitState>();
    }

}

