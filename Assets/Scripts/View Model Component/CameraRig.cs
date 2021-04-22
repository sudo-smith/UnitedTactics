using UnityEngine;
using System.Collections;
public class CameraRig : MonoBehaviour
{
    public float speed = 3f;
    public Transform follow;
    Transform _transform;

    void OnEnable()
    {
        InputController.buttonEvent += OnButtonEvent;
        InputController.scrollEvent += OnScrollEvent;
    }

    void OnButtonEvent(object sender, InfoEventArgs<int> e){
        //change camera rotation
        float startAngle = transform.GetChild(0).localRotation.eulerAngles.y;
        float endAngle = 0;
        if(e.buttonToString() == "LeftShoulder")
             endAngle = startAngle + 90;
        else if(e.buttonToString() == "RightShoulder")
            endAngle = startAngle - 90;
        if(endAngle != 0){
            StartCoroutine(TurnCamera(new Vector3(0, endAngle, 0)));
        }
        
    }

    void OnScrollEvent(object sender, InfoEventArgs<float> e){
        //change camera zoom
        float newZoom = transform.GetComponentInChildren<Camera>().orthographicSize - e.info;
        if(newZoom > .1)
            transform.GetComponentInChildren<Camera>().orthographicSize = newZoom;
    }

    IEnumerator TurnCamera (Vector3 eulerAngle){
        TransformLocalEulerTweener t = (TransformLocalEulerTweener)transform.GetChild(0).RotateToLocal(eulerAngle, 0.25f, EasingEquations.EaseInOutQuad);
        while (t != null)
            yield return null;
    }

    void Awake()
    {
        _transform = transform;
    }

    void Update()
    {
        if (follow)
            _transform.position = Vector3.Lerp(_transform.position, follow.position, speed * Time.deltaTime);
        
    }
}
