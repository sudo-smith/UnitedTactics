using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class InfoEventArgs<T> : EventArgs
{
    public T info;

    public InfoEventArgs()
    {
        info = default(T);
    }

    public InfoEventArgs(T info)
    {
        this.info = info;
    }

    public String buttonToString(){
        if(typeof(T) == typeof(int)){
            int n = Convert.ToInt32(info);
            switch(n){
                case 0 : return "Select";
                case 1 : return "Cancel";
                case 2 : return "Modifier";
                case 3 : return "Select";
                case 4:  return "LeftShoulder";
                case 5 : return "RightShoulder";
                default: return "Options";
            }
        }
        return "not a button";
    }
}
