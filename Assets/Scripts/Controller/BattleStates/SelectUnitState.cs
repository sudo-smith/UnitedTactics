using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectUnitState : BattleState
{

    protected bool multiSelect;
    public override void Enter(){
        base.Enter();
        multiSelect = false;
        // owner.currentUnit.deselect();
        owner.currentUnits = new List<Unit>();
    }
    
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }
    
    protected override void OnButton(object sender, InfoEventArgs<int> e)
    {
        if (e.buttonToString() == "Modifier"){
            GameObject content = owner.currentTile.content;
            if (content != null && content.tag == "Hero"){
                Unit u = content.GetComponent<Unit>();
                if(owner.currentUnits.Contains(content.GetComponent<Unit>()))
                    owner.currentUnits.Remove(u);
                else
                    owner.currentUnits.Add(u);
            }
        }
        if (e.buttonToString() == "Select")
        {
            if(owner.currentUnits.Count > 0){
                owner.currentUnit = null;
                owner.ChangeState<MoveTargetState>();
            }
            GameObject content = owner.currentTile.content;
            if (content != null && content.tag == "Hero")
            {
                owner.currentUnit = content.GetComponent<Unit>();
                owner.currentUnit.select();
                //todo change depending on combat or movement phase
                owner.ChangeState<MoveTargetState>();
            }
        }
    }
}
