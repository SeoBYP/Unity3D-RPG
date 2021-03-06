using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private Transform _followtransform;
    [SerializeField]
    private Vector2 _framing = new Vector2(0, 0);
    PlayerController player;

    SwichMaterial swich;
    CollisionHandler handler;

    [SerializeField] private float _zoomSpeed = 10f;
    [SerializeField] private float _defaultDistance = 5f;
    [SerializeField] private float _MinDistance = 1f;
    [SerializeField] private float _MaxDistance = 10f;

    [SerializeField] private float _rotationSharpness = 25.0f;
    [SerializeField] private float _defaultVerticalAngle = 20f;
    [SerializeField] [Range(-90, 90)] private float _minVerticalAngle = 0;
    [SerializeField] [Range(-90, 90)] private float _maxVerticalAngle = 90;

    [SerializeField] private float _checkRadious = 0.2f;
    [SerializeField] private LayerMask _obstructionLayers;
    //private List<Collider> _ignoreCollider = new List<Collider>();

    public Vector3 CameraPlanerDirection { get { return _planarDirection; } }

    private float _targetVerticalAngle;
    private float _targetDistance;
    private Vector3 _planarDirection;
    private Vector3 _targetPosition;
    private Quaternion _targetRotation;

    private Vector3 _newPosition;
    private Quaternion _newRotation;

    private void OnValidate()
    {
        _defaultDistance = Mathf.Clamp(_defaultDistance, _MinDistance, _MaxDistance);
        _defaultVerticalAngle = Mathf.Clamp(_defaultVerticalAngle, _minVerticalAngle, _maxVerticalAngle);
    }

    public void Init()
    {
        _camera = Camera.main;
        player = FindObjectOfType<PlayerController>();
        _followtransform = player.followTransform;
        handler = GetComponent<CollisionHandler>();
        //_ignoreCollider.AddRange(GetComponentsInChildren<Collider>());
        _planarDirection = _followtransform.forward;
        _targetDistance = _defaultDistance;
        _targetVerticalAngle = _defaultVerticalAngle;
        _targetRotation = Quaternion.LookRotation(_planarDirection) * Quaternion.Euler(_targetVerticalAngle, 0, 0);
        _targetPosition = _followtransform.position - (_targetRotation * Vector3.forward) * _targetDistance;

        if(handler != null)
        {
            handler.Init(_camera);
        }
    }

    private void LateUpdate()
    {
        if ( ItemIcon.dragging || BaseUI.IsOpenPopup || StageClear.IsStageClear)
            return;

        //Handle Input
        float _zoom = InputManager.MouseScroll * _zoomSpeed;
        float _mouseX = InputManager.MouseX;
        float _mouseY = InputManager.MouseY;


        Vector3 _focusPosition = _followtransform.position + new Vector3(_framing.x, _framing.y, 0);

        //???????????? X????????? ???????????? ????????? (Y?????? ????????????)Euler?????? ??????????????? ????????? ???????????? ?????????.
        _planarDirection = Quaternion.Euler(0, _mouseX, 0) * _planarDirection;
        //?????? ???????????? ?????????????????? ?????????.
        _targetDistance = Mathf.Clamp(_targetDistance + _zoom, _MinDistance, _MaxDistance);
        //???????????? Y????????? ???????????? ????????? (X?????? ????????????)Euler?????? ?????????.
        _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle + _mouseY, _minVerticalAngle, _maxVerticalAngle);


        //????????? ????????? ??????(?????? ????????? ??????)
        float _smallesDistance = _targetDistance;
        RaycastHit[] _hits = Physics.SphereCastAll(transform.position, _checkRadious, _targetRotation * -Vector3.forward, _targetDistance, _obstructionLayers);
        if (_hits.Length != 0)
        {
            foreach (RaycastHit hit in _hits)
            {
                float hitdistance = Vector3.Distance(_targetPosition, hit.transform.position);
                //print(hitdistance);
                if (hitdistance < _smallesDistance)
                {
                    //_camera.transform.position = Vector3.Lerp(transform.position, hit.transform.position, Time.deltaTime);
                    _targetDistance = Mathf.Lerp(_targetDistance, hitdistance, 3 * Time.deltaTime);
                    if (_targetDistance < _MinDistance)
                        _targetDistance = _MinDistance;
                }
            }

        }

        RayCastHit(_focusPosition, _targetDistance);

        //???????????? ?????? ???????????? ?????? _planarDirection??? ???????????? DrawLine??? ??????.
        Debug.DrawLine(_camera.transform.position, _camera.transform.position + _planarDirection, Color.red);

        //Final Target
        _targetRotation = Quaternion.LookRotation(_planarDirection) * Quaternion.Euler(_targetVerticalAngle, 0, 0);
        //????????? ????????????????
        _targetPosition = _focusPosition - (_targetRotation * Vector3.forward) * _targetDistance;

        _newRotation = Quaternion.Slerp(_camera.transform.rotation, _targetRotation, Time.deltaTime * _rotationSharpness);
        _newPosition = Vector3.Lerp(_camera.transform.position, _targetPosition, Time.deltaTime * _rotationSharpness);

        //MoveCamera();

        //_planarDirection??? ???????????? ?????? ???????????? ???????????? ?????? ???????????????.
        _camera.transform.rotation = _newRotation;
        _camera.transform.position = _newPosition;
    }
    float _aVel;
    Vector3 _destination = Vector3.zero;

    public void MoveCamera()
    {
        if(handler != null)
        {
            float Distance = 5.0f;
            float dist = 15;
            float _commonAdjustmentDistance = 3.0f;

            Vector3 CurrentCameraPos = _camera.transform.position;
            var dis = handler.HandleCollisionZoomDistance(_destination, transform.position, 0.18f, Distance);
            if (dis != Mathf.Infinity && dis < Distance)
            {
                _targetDistance = dis;

                _destination = _targetPosition;

                dist = Mathf.Abs(dis - _commonAdjustmentDistance);
                if (dis < _commonAdjustmentDistance)
                    _destination -= transform.forward * dis;
                else
                {
                    _destination -= transform.forward * _commonAdjustmentDistance;
                }
            }
            else
            {
                transform.position = CurrentCameraPos;
            }
            _commonAdjustmentDistance = Mathf.SmoothDamp(_commonAdjustmentDistance, _targetDistance, ref _aVel, dist * Time.deltaTime);
            transform.position = _destination;
        }
    }

    public void Clear()
    {
        _camera = null;
        player = null;
        _followtransform = null;
        _planarDirection = Vector3.zero;
        _targetDistance = 0;
        _targetVerticalAngle = 0;
        _targetRotation = Quaternion.identity;
        _targetPosition = Vector3.zero;
    }

    public void RayCastHit(Vector3 focusposition, float targetdistance)
    {
        RaycastHit[] hits = Physics.SphereCastAll(focusposition, _checkRadious, _targetRotation * -Vector3.forward, targetdistance, 1 << LayerMask.NameToLayer("Block"));
        //Block ???????????? ??????????????? ??? ????????????.
        if(hits.Length != 0)
        {
            foreach(RaycastHit hit in hits)
            {
                //????????? ????????? ??????????????? ????????????.
                swich = hit.transform.gameObject.GetComponent<SwichMaterial>();
                if(swich != null)
                {
                    print("GetRenderer");
                    //??????????????? null?????? ???????????? Renderer??? ??????????????? ????????? ??????.
                    swich.Swich();
                }
            }
        }
        if(hits.Length == 0)
        {
            if (swich != null)
                swich.SetDefault();
        }
            
    }
    
}
