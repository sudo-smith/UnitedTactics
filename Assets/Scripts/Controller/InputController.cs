using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    Repeater _hor = new Repeater("Horizontal");
    Repeater _ver = new Repeater("Vertical");

    public static event EventHandler<InfoEventArgs<Point>> moveEvent;
    public static event EventHandler<InfoEventArgs<float>> scrollEvent;
    public static event EventHandler<InfoEventArgs<int>> buttonEvent;
    
    string[] _buttons = new string[] { "Select", "Cancel", "Modifier", "Options", "LeftShoulder", "RightShoulder" };

    void Update()
    {
        //check movement
        int x = _hor.Update();
        int y = _ver.Update();
        if (x != 0 || y != 0)
        {
            if (moveEvent != null)
                moveEvent(this, new InfoEventArgs<Point>(new Point(x, y)));
        }

        //check zoom level
        float zoomChange = Input.GetAxis("Mouse ScrollWheel");
        if(zoomChange != 0){
            if(scrollEvent != null)
                scrollEvent(this, new InfoEventArgs<float>(zoomChange));
        }

        //check fire button
        for (int i = 0; i < _buttons.Length; ++i)
        {
            if (Input.GetButtonUp(_buttons[i]))
            {
                if (buttonEvent != null)
                    buttonEvent(this, new InfoEventArgs<int>(i));                    
            }
        }


    }
}


class Repeater
{
    const float threshold = 0.5f;
    const float rate = 0.1f;
    float _next;
    bool _hold;
    string _axis;
    public Repeater(string axisName)
    {
        _axis = axisName;
    }
    public int Update()
    {
        int retValue = 0;
        int value = Mathf.RoundToInt(Input.GetAxisRaw(_axis));
        if (value != 0)
        {
            if (Time.time > _next)
            {
                retValue = value;
                _next = Time.time + (_hold ? rate : threshold);
                _hold = true;
            }
        }
        else
        {
            _hold = false;
            _next = 0;
        }
        return retValue;
    }
}
