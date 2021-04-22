using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{

    void OnEnable()
    {
        InputController.moveEvent += OnMoveEvent;
        InputController.buttonEvent += OnButtonEvent;        
        InputController.scrollEvent += OnScrollEvent;

    }
    void OnDisable()
    {
        InputController.moveEvent -= OnMoveEvent;
        InputController.buttonEvent -= OnButtonEvent;
    }

    void OnMoveEvent(object sender, InfoEventArgs<Point> e)
    {
        Debug.Log("Move: " + e.info);
    }

    void OnButtonEvent(object sender, InfoEventArgs<int> e)
    {
        Debug.Log("Button: " + e.buttonToString());
    }

    void OnScrollEvent(object sender, InfoEventArgs<float> e){
        Debug.Log("Zoom: " + e.info);
    }
}
