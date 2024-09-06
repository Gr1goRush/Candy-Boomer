using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void InputTouchAction(Vector2 position);

public class InputController : Singleton<InputController>
{
    public event InputTouchAction OnTouchFinished;

    private bool skipNextTouch = false;

    private void Update()
    {
        //if (GameController.Instance.Paused)
        //{
        //    skipNextTouch = true;
        //    return;
        //}

        if(skipNextTouch)
        {
            skipNextTouch = false;
            return;
        }

        if (Input.touchSupported)
        {
            int touchesCount = Input.touchCount;
            for (int i = 0; i < touchesCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if(touch.phase == TouchPhase.Ended)
                {
                    OnTouchFinished?.Invoke(touch.position);
                }
            }
        }
        else
        {
            if(Input.GetMouseButtonUp(0))
            {
                OnTouchFinished?.Invoke(Input.mousePosition);
            }
        }
    }

    public void SkipNextTouch()
    {
        skipNextTouch = true;
    }
}
