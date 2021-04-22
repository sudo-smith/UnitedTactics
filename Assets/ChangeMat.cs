using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMat : MonoBehaviour {

	public Material selected;
    public Material ghost;
    private Material orig;

    void select(){
        GetComponent<Renderer>().material = selected;
    }
}
