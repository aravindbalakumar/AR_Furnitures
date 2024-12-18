
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class InputManager : MonoBehaviour
{
    Vector2 prev_tch_pos,tch_ps;
    TouchPhase touchPhase;
    public UnityEvent<Vector2> OnTouch;
    public UnityEvent<int> OnTouch_X_Direction;
    public UnityEvent<int> OnTouch_Y_Direction;
    [SerializeField]PlayerInput playerInput;

    private InputAction TouchPositionAction;
    private InputAction TouchPhaseAction;
    private void Awake()
    {
        TouchPositionAction = playerInput.actions.FindAction("TouchPosition");
        TouchPhaseAction = playerInput.actions.FindAction("TouchPhase");

    }
    public void OnEnable()
    {
        TouchPhaseAction.Enable();
        TouchPositionAction.Enable();
        
    }
    public void OnDisable()
    {
        TouchPhaseAction.Disable();
        TouchPositionAction.Disable();
    }
    private void Update()
    {

        tch_ps = TouchPositionAction.ReadValue<Vector2>();
        touchPhase = (TouchPhase)TouchPhaseAction.ReadValue<TouchPhase>();

        if (!GameManager.Instance.isInitialized)
        {
            return;
        }


        switch (touchPhase)
        {
            case TouchPhase.Began:
                prev_tch_pos = tch_ps;
                OnTouch?.Invoke(tch_ps);
                break;
            case TouchPhase.Moved:
                if (prev_tch_pos != tch_ps)
                {
                    if (tch_ps.x > prev_tch_pos.x)
                    {
                        OnTouch_X_Direction?.Invoke(1);
                    }
                    else if (tch_ps.x < prev_tch_pos.x)
                    {
                        OnTouch_X_Direction?.Invoke(-1);
                    }

                    prev_tch_pos = tch_ps;
                    OnTouch?.Invoke(tch_ps);
                }
                break;
            case TouchPhase.Stationary:
                if (prev_tch_pos != tch_ps)
                {
                    prev_tch_pos = tch_ps;
                    OnTouch?.Invoke(tch_ps);
                }
                break;
            case TouchPhase.Ended:
                prev_tch_pos = Vector2.zero;
                tch_ps = Vector2.zero;
                break;
            default:
                prev_tch_pos = Vector2.zero;
                tch_ps = Vector2.zero;
                break;
        }
    }
}
