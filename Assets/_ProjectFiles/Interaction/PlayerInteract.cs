using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 3f;
    public float holdDuration = 1.5f;

    public LayerMask interactMask;
    public Camera playerCamera;
    
    public TMP_Text interactText;

    public Transform pickableContainer;
    
    [Inject] private FPController _fpController;

    public PickableItem InHandItem => pickableContainer.GetComponentInChildren<PickableItem>();

    private PlayerInput _input;

    private IInteractable _current;
    
    private Transform _camTransform;

    private bool _isHolding;
    private float _holdTimer;
    
    enum PlayerState
    {
        Normal,
        Inspecting
    }

    private PlayerState _state = PlayerState.Normal;

    public Transform inspectPoint;
    public float inspectRotationSpeed = 150f;

    private PickableItem _inspectingItem;
    private HashSet<ItemType> _inspectedTypes = new();
    
    void Awake()
    {
        _input = new PlayerInput();
    }

    void Start()
    {
        _camTransform = playerCamera.transform;
    }

    void OnEnable()
    {
        _input.Enable();

        _input.Player.Interact.started += _ => StartInteraction();
        _input.Player.Interact.canceled += _ => EndInteraction();
    }

    void OnDisable()
    {
        _input.Disable();
    }

    void Update()
    {
        if (_state == PlayerState.Inspecting)
        {
            HandleInspectRotation();
            return;
        }
        
        HandleRaycast();
        HandleInteraction();
    }

    void HandleInspectRotation()
    {
        if (!Mouse.current.leftButton.isPressed)
            return;

        Vector2 mouse = Mouse.current.delta.ReadValue();

        float rotX = mouse.y * inspectRotationSpeed * Time.deltaTime;
        float rotY = -mouse.x * inspectRotationSpeed * Time.deltaTime;

        Transform cam = _camTransform;

        _inspectingItem.transform.Rotate(cam.up, rotY, Space.World);
        _inspectingItem.transform.Rotate(cam.right, rotX, Space.World);
    }

    void UpdateUI()
    {
        if (_current != null)
        { 
            interactText.text = _current.GetInteractionName().ToDisplayString();
        }
        else
        {
            interactText.text = "";
        }
    }

    void HandleRaycast()
    {
        Ray ray = new Ray(_camTransform.position, _camTransform.forward);

        IInteractable newTarget = null;

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactMask))
        {
            newTarget = hit.collider.GetComponentInParent<IInteractable>();
        }
        
        if (_current != newTarget)
        {
            _current?.OnHoverExit();
            
            if (_current != null && _current.GetInteractionType() == InteractionType.Continuous)
            {
                _current.OnInteractEnd(this);
            }
            _current = newTarget;
            _current?.OnHoverEnter();
            
            UpdateUI();
        }
    }

    void StartInteraction()
    {
        if (_state == PlayerState.Inspecting)
        {
            EndInspect();
            return;
        }

        if (_current == null)
        {
            Drop();
            return;
        }

        _isHolding = true;
        _holdTimer = 0f;
        
        _current.OnInteractStart(this);

        if (_current.GetInteractionType() == InteractionType.Instant)
        {
            _current.OnInteract(this);
            _isHolding = false;
        }

        interactText.text = "";
    }

    void HandleInteraction()
    {
        if (!_isHolding || _current == null) return;

        var type = _current.GetInteractionType();

        if (type == InteractionType.Continuous)
        {
            _current.OnInteract(this);
        }
        else if (type == InteractionType.Hold)
        {
            _holdTimer += Time.deltaTime;

            if (_holdTimer >= holdDuration)
            {
                _current.OnInteract(this);
                _isHolding = false;
            }
        }
    }

    void EndInteraction()
    {
        if (_current?.GetInteractionType() != InteractionType.Continuous)
        {
            _current?.OnInteractEnd(this); 
            return;
        }
        
        if (!_isHolding) return;

        _current?.OnInteractEnd(this);

        _isHolding = false;
        _holdTimer = 0f;
    }
    
    public void PickUp(PickableItem item)
    {
        if (_state != PlayerState.Normal)
            return;

        if (!_inspectedTypes.Contains(item.itemType))
        {
            _inspectedTypes.Add(item.itemType);
            StartInspect(item);
            return;
        }

        if (pickableContainer.childCount == 0)
        {
            SetLayerRecursively(item.gameObject, LayerMask.NameToLayer("InHandItem")); 
            item.rb.isKinematic = true;

            Transform t = item.transform;
            t.SetParent(pickableContainer);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
        }
    }
    
    void StartInspect(PickableItem item)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        _state = PlayerState.Inspecting;
        _inspectingItem = item;

        item.rb.isKinematic = true;

        SetLayerRecursively(item.gameObject, LayerMask.NameToLayer("InHandItem"));

        Transform t = item.transform;
        t.SetParent(inspectPoint);
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;

        _fpController.DisablePlayerControl();
        item.OnInspect();
    }
    
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
    
    void EndInspect()
    {
        _state = PlayerState.Normal;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;     

        _fpController.EnablePlayerControl();

        if (pickableContainer.childCount == 0)
        {
            Transform t = _inspectingItem.transform;
            t.SetParent(pickableContainer);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
        }

        _inspectingItem = null;
    }

    public void Drop()
    {
        if (pickableContainer.childCount == 0)
            return;
        
        PickableItem item = pickableContainer.GetChild(0).GetComponent<PickableItem>();
        SetLayerRecursively(item.gameObject, LayerMask.NameToLayer("Interactable")); 
        item.transform.SetParent(null);
        item.rb.isKinematic = false;
    }

}