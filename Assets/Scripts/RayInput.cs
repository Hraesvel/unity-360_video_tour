using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Input;
using UnityEngine.XR;

public class RayInput : MonoBehaviour
{
    public float RayDistancce = 100f;

    [SerializeField] private Controls ctrl;
    [SerializeField] private bool drawDebugLine;
    [SerializeField] private LayerMask uiLayerMask;

    private Ray _ray;
    private RaycastHit _rayHit;
    private PointerEventData eventData;

    // Start is called before the first frame update

    private void Start()
    {
        ctrl.BasicCtrl.Enable();

        uiLayerMask = LayerMask.NameToLayer("UI");
        eventData = new PointerEventData(EventSystem.current);
        eventData.pointerId = 0;

        if (XRSettings.enabled)
        {
            Debug.Log("VR ON!");

            eventData.position = new Vector2(XRSettings.eyeTextureWidth / 2f,
                XRSettings.eyeTextureHeight / 2f);
        }
        else
        {
            eventData.position = new Vector2(Screen.width / 2f, Screen.height / 2f);
        }

        eventData.pressPosition = eventData.position;
    }

    // Update is called once per frame
    private void Update()
    {
        RayIntersect();
        CheckHit();
    }

    private void RayIntersect()
    {
        var transform1 = transform;
        _ray = new Ray(transform1.position, transform1.forward);

        if (drawDebugLine)
            Debug.DrawLine(transform.position, transform.TransformDirection(Vector3.forward) * RayDistancce, Color.red);
        
        Physics.Raycast(_ray, out _rayHit, RayDistancce);
       
        if (_rayHit.transform == null)
        {
            LookAway();
            return;
        }

        eventData.pointerCurrentRaycast = ConvertRayCastHitToRaycastResult(_rayHit);

        if (eventData.pointerEnter == _rayHit.transform.gameObject)
        {
            return;
        }

        LookAway();

        eventData.pointerEnter = _rayHit.transform.gameObject;
        ExecuteEvents.ExecuteHierarchy(eventData.pointerEnter, eventData, ExecuteEvents.pointerEnterHandler);
    }

    private RaycastResult ConvertRayCastHitToRaycastResult(RaycastHit hit)
    {
        RaycastResult rayResult = new RaycastResult();
        rayResult.gameObject = hit.transform.gameObject;
        rayResult.distance = _rayHit.distance;
        rayResult.worldPosition = _rayHit.point;
        rayResult.worldNormal = _rayHit.normal;

        return rayResult;
    }

    private void CheckHit()
    {
        if (ctrl.BasicCtrl.A.enabled && eventData.pointerEnter != null)
        {
            eventData.pointerPressRaycast = eventData.pointerCurrentRaycast;
            eventData.pointerPress =
                ExecuteEvents.ExecuteHierarchy(eventData.pointerEnter, eventData, ExecuteEvents.pointerDownHandler);
        }
        else if (!ctrl.BasicCtrl.A.enabled)
        {
            if (eventData.pointerPress != null)
                ExecuteEvents.ExecuteHierarchy(eventData.pointerPress, eventData, ExecuteEvents.pointerUpHandler);

            if (eventData.pointerPress == eventData.pointerEnter)
                ExecuteEvents.ExecuteHierarchy(eventData.pointerEnter, eventData, ExecuteEvents.pointerClickHandler);

            eventData.pointerPress = null;
        }
    }

    void LookAway()
    {
        if (eventData.pointerEnter != null)
        {
            ExecuteEvents.ExecuteHierarchy(eventData.pointerEnter, eventData, ExecuteEvents.pointerExitHandler);
            eventData.pointerEnter = null;
        }
    }

    private void OnEnable()
    {
        // ctrl.BasicCtrl.A.performed += AOnperformed;
        ctrl.BasicCtrl.A.Enable();
    }

    private void OnDisable()
    {
        // ctrl.BasicCtrl.A.performed -= AOnperformed;
        ctrl.BasicCtrl.A.Disable();
    }

    // private void AOnperformed(InputAction.CallbackContext obj)
    // {
    //     // CheckHit();
    // }
}