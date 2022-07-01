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

        //마우스의 X좌표의 움직임의 값만큼 (Y축을 기준으로)Euler값을 회전시키고 타겟의 앞백터를 곱한다.
        _planarDirection = Quaternion.Euler(0, _mouseX, 0) * _planarDirection;
        //현재 거리값을 스크롤한만큼 더한다.
        _targetDistance = Mathf.Clamp(_targetDistance + _zoom, _MinDistance, _MaxDistance);
        //마우스의 Y좌표의 움직임의 값만큼 (X축을 기준으로)Euler값에 더한다.
        _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle + _mouseY, _minVerticalAngle, _maxVerticalAngle);


        //게임이 어설퍼 보임(좀더 수정이 필요)
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

        //카메라의 현재 위치에서 현재 _planarDirection의 회전값에 DrawLine을 한다.
        Debug.DrawLine(_camera.transform.position, _camera.transform.position + _planarDirection, Color.red);

        //Final Target
        _targetRotation = Quaternion.LookRotation(_planarDirection) * Quaternion.Euler(_targetVerticalAngle, 0, 0);
        //연산이 잘못되었나?
        _targetPosition = _focusPosition - (_targetRotation * Vector3.forward) * _targetDistance;

        _newRotation = Quaternion.Slerp(_camera.transform.rotation, _targetRotation, Time.deltaTime * _rotationSharpness);
        _newPosition = Vector3.Lerp(_camera.transform.position, _targetPosition, Time.deltaTime * _rotationSharpness);

        //MoveCamera();

        //_planarDirection의 회전값에 따라 카메라의 로테이션 값을 변화시킨다.
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
        //Block 레이어가 충돌되었을 때 처리한다.
        if(hits.Length != 0)
        {
            foreach(RaycastHit hit in hits)
            {
                //여기서 스위치 컴포넌트를 가져온다.
                swich = hit.transform.gameObject.GetComponent<SwichMaterial>();
                if(swich != null)
                {
                    print("GetRenderer");
                    //컴포넌트가 null값이 아니면은 Renderer를 교체해주는 처리를 한다.
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
