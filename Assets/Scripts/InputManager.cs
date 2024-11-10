
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    Touch tch;
    Vector2 prev_tch_pos;
    public UnityEvent<Vector2> OnTouch;

    private void Update()
    {
        if (!GameManager.Instance.isInitialized)
        {
            return;
        }
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            if(Input.touchCount>0)
            {
                tch = Input.GetTouch(0);
                switch (tch.phase)
                {
                    case TouchPhase.Began:
                        prev_tch_pos = tch.position;
                        OnTouch?.Invoke(tch.position);
                        break;
                    case TouchPhase.Moved:
                        if(prev_tch_pos!=tch.position)
                        {
                            prev_tch_pos = tch.position;
                            OnTouch?.Invoke(tch.position);
                        }
                        break;
                    case TouchPhase.Stationary:
                        if (prev_tch_pos != tch.position)
                        {
                            prev_tch_pos = tch.position;
                            OnTouch?.Invoke(tch.position);
                        }
                        break;
                    case TouchPhase.Ended:
                        prev_tch_pos = Vector2.zero;
                        break;
                }
                
            }
            
        }
    }
}
