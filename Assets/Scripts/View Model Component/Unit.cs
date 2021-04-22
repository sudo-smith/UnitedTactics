using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Unit : MonoBehaviour
{
    public Tile tile { get; protected set; }
    public Directions dir;
    public int maxHp;
    public int curHp;
    public bool isAlly; //todo do something better than this

    public Color selectedEmColor;
    Color baseEmColor;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        curHp = maxHp;
        baseEmColor = Color.black;
    }

    public void Place(Tile target)
    {
        // Make sure old tile location is not still pointing to this unit
        if (tile != null && tile.content == gameObject)
            tile.content = null;

        // Link unit and tile references
        tile = target;

        if (target != null)
            target.content = gameObject;

        //todo eventually have unit store neighbor units
        //set if tiles are next to an enemy
        if (!isAlly)
        {
            foreach (Tile t in target.NeighborTiles)
            {
                t.nextToEnemy = true;
            }
        }
        
    }
    
    public void Match()
    {
        transform.localPosition = tile.center;
        transform.localEulerAngles = dir.ToEuler();
    }

    public void damage(int damage){
        curHp -= damage;
    }

    public void heal(int heal){
        curHp += heal;
        if(curHp > maxHp)
            curHp = maxHp;
    }

    public void select(){
        gameObject.transform.GetChild(1).GetComponent<Renderer>().material.SetColor("_EmissiveColor", selectedEmColor);
    }

    public void deselect(){
        gameObject.transform.GetChild(1).GetComponent<Renderer>().material.SetColor("_EmissiveColor", baseEmColor);
    }
}
