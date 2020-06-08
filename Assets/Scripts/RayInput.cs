using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class RayInput : MonoBehaviour
{
    private bool _aAction;


    private Ray _ray;
    private RaycastHit _rayHit;

    [SerializeField] private Controls ctrl;
    [SerializeField] private bool drawDebugLine;
    private PointerEventData eventData;
    public float RayDistancce = 100f;
    [SerializeField] private LayerMask uiLayerMask;


    private void Awake()
    {
        ctrl.BasicCtrl.A.performed += start => AOnperformed();
        ctrl.BasicCtrl.A.cancelled += stop => AOnCancel();
        _aAction = false;
    }

    private void AOnperformed()
    {
        Debug.Log("A on");
        _aAction = true;
    }

    private void AOnCancel()
    {
        Debug.Log("A off");
        _aAction = false;
    }

    private void Start()
    {
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

        var rHandInputs = new List<InputDevice>();
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
            return;

        LookAway();

        eventData.pointerEnter = _rayHit.transform.gameObject;
        ExecuteEvents.ExecuteHierarchy(eventData.pointerEnter, eventData, ExecuteEvents.pointerEnterHandler);
    }

    private RaycastResult ConvertRayCastHitToRaycastResult(RaycastHit hit)
    {
        var rayResult = new RaycastResult();
        rayResult.gameObject = hit.transform.gameObject;
        rayResult.distance = _rayHit.distance;
        rayResult.worldPosition = _rayHit.point;
        rayResult.worldNormal = _rayHit.normal;

        return rayResult;
    }

    private void CheckHit()
    {
        if (_aAction && eventData.pointerEnter != null)
        {
            eventData.pointerPressRaycast = eventData.pointerCurrentRaycast;
            eventData.pointerPress =
                ExecuteEvents.ExecuteHierarchy(eventData.pointerEnter, eventData, ExecuteEvents.pointerDownHandler);
        }
        else if (!_aAction)
        {
            if (AntiSpamSingleton.Instance.IsTransitioning)
                return;

            if (eventData.pointerPress != null)
                ExecuteEvents.ExecuteHierarchy(eventData.pointerPress, eventData, ExecuteEvents.pointerUpHandler);

            if (eventData.pointerPress == eventData.pointerEnter && eventData.pointerPress != null &&
                eventData.pointerEnter != null)
                ExecuteEvents.ExecuteHierarchy(eventData.pointerEnter, eventData, ExecuteEvents.pointerClickHandler);


            eventData.pointerPress = null;
        }
    }

    private void LookAway()
    {
        if (eventData.pointerEnter != null)
        {
            ExecuteEvents.ExecuteHierarchy(eventData.pointerEnter, eventData, ExecuteEvents.pointerExitHandler);
            eventData.pointerEnter = null;
        }
    }

    private void OnEnable()
    {
        ctrl.Enable();
    }

    private void OnDisable()
    {
        ctrl.Disable();
    }
}