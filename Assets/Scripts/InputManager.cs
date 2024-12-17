
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    Touch tch;
    Vector2 prev_tch_pos;
    Vector3 prev_mous_pos;
    public UnityEvent<Vector2> OnTouch;
    public UnityEvent<int> OnTouch_X_Direction;
    public UnityEvent<int> OnTouch_Y_Direction;

    private void Update()
    {
#if UNITY_EDITOR
        if(Input.GetMouseButtonDown(0))
        {
            prev_mous_pos = Input.mousePosition;
            OnTouch?.Invoke(Input.mousePosition);
        }
        else if(Input.GetMouseButton(0))
        {
            if (prev_mous_pos != Input.mousePosition)
            {
                if (Input.mousePosition.x > prev_mous_pos.x)
                {
                    OnTouch_X_Direction?.Invoke(1);
                }
                else if (Input.mousePosition.x < prev_mous_pos.x)
                {
                    OnTouch_X_Direction?.Invoke(-1);
                }

                prev_mous_pos = Input.mousePosition;
                OnTouch?.Invoke(Input.mousePosition);
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            prev_mous_pos = Vector2.zero;
        }
#else
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
                            if(tch.position.x > prev_tch_pos.x)
                            {
                                OnTouch_X_Direction?.Invoke(1);
                            }
                            else if (tch.position.x < prev_tch_pos.x)
                            {
                                OnTouch_X_Direction?.Invoke(-1);
                            }

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
#endif
    }
}
