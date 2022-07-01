using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private Camera _camera;
    private Ray _currentRay = new Ray();
    private Vector3 _rayCenter = Vector3.zero;
    private Vector3 _rayCameraPosition = Vector3.zero;
    private Vector3 _rayTargetPosition = Vector3.zero;
    private float _rayDistance = Mathf.Infinity;
    private CameraController _cameraControl;

    public LayerMask CollisionLayer;

    [HideInInspector]
    public bool Colliding = false;
    [HideInInspector]
    public Vector3 CollisionCheckPos;

    public void Init(Camera cam)
    {
        _camera = cam;
        _cameraControl = GetComponent<CameraController>();
    }

    public float HandleCollisionZoomDistance(Vector3 cameraPos,Vector3 targetPos,float minOffsetDist,float MaxDistance)
    {
        float raycastLength = Vector3.Distance(targetPos, cameraPos);
        if(raycastLength < 0)
        {
            return 0;
        }

        Vector3 newrestCameraPosition = targetPos - _camera.transform.position * minOffsetDist;

        var hit = new Vector3[4];
        hit[0] = _camera.ViewportToWorldPoint(new Vector3(0f, 1, _camera.nearClipPlane));
        hit[1] = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));
        hit[2] = _camera.ViewportToWorldPoint(new Vector3(1, 0, _camera.nearClipPlane));
        hit[3] = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));

        var currentNormal = Vector3.zero;
        var currentPoint = Vector3.zero;

        float normalDistance = MaxDistance * 0.9f;

        MaxDistance *= 2;

        bool noneClipHit = true;
        bool insideCollide = false;
        bool isSameFace = true;
        bool missed = false;
        bool needClipCollision = true;

        float fCollide = Mathf.Infinity;
        float cCollide = Mathf.Infinity;

        for(var i = 0; i < hit.Length; i++)
        {
            bool hasEnd = false;
            var start = hit[i];
            var end = hit[0];
            if (i < 3) end = hit[i + 1];

            RaycastHit hitR;
            if(Physics.Raycast(new Ray(start,end - start),out hitR, Vector3.Distance(start, end), CollisionLayer))
            {
                noneClipHit = false;
                Debug.DrawRay(start, end - start, Color.green);

                var hitPoint = hitR.point;
                var center = Vector3.zero;

                if(Physics.Raycast(new Ray(end,start - end),out hitR, Vector3.Distance(start, end), CollisionLayer))
                {
                    center = new Vector3((hitPoint.x + hitR.point.x) / 2, (hitPoint.y + hitR.point.y) / 2, (hitPoint.z + hitR.point.z) / 2);
                }
                else
                {
                    center = new Vector3((hitPoint.x + end.x) / 2, (hitPoint.y + end.y) / 2, (hitPoint.z + end.z) / 2);
                    hasEnd = true;
                }

                Vector3 offsetToCorner = center - cameraPos;
                Vector3 rayStart = newrestCameraPosition + offsetToCorner;
                var coRay = new Ray(rayStart, center - rayStart);
                if(Physics.Raycast(coRay,out hitR, MaxDistance, CollisionLayer))
                {
                    Debug.DrawRay(rayStart, center - rayStart, Color.black);
                    if(hitR.distance < cCollide)
                    {
                        _currentRay = coRay;
                        _rayCenter = center;
                        _rayCameraPosition = cameraPos;
                        _rayTargetPosition = targetPos;
                        cCollide = hitR.distance;
                    }
                }

                if (hasEnd)
                {
                    center = end;
                    offsetToCorner = center - cameraPos;
                    rayStart = newrestCameraPosition + offsetToCorner;
                    coRay = new Ray(rayStart, center - rayStart);
                    if (Physics.Raycast(coRay, out hitR, MaxDistance, CollisionLayer))
                    {
                        Debug.DrawRay(rayStart, center - rayStart, Color.grey);

                        if(hitR.distance < cCollide)
                        {
                            _currentRay = coRay;
                            _rayCenter = center;
                            _rayCameraPosition = cameraPos;
                            _rayTargetPosition = targetPos;
                            cCollide = hitR.distance;
                        }
                    }
                }
            }

            if (needClipCollision)
            {
                var offsetToCorrner1 = start - cameraPos;
                var rayStart1 = newrestCameraPosition + offsetToCorrner1;
                var coRay1 = new Ray(rayStart1, start - rayStart1);

                var coRay2 = new Ray(start, rayStart1 - start);

                if(Physics.Raycast(coRay1,out hitR, MaxDistance, CollisionLayer))
                {
                    Debug.DrawRay(rayStart1, start - rayStart1, Color.yellow);
                    insideCollide = true;

                    if(hitR.distance < fCollide)
                    {
                        fCollide = hitR.distance;
                    }
                }
            }

            if(hitR.collider == null)
            {
                missed = true;
            }
            else
            {
                if(currentNormal == Vector3.zero)
                {
                    currentNormal = hitR.normal;
                    currentPoint = hitR.point;
                }
                else
                {
                    if (currentNormal != hitR.normal) isSameFace = false;
                }

                if (missed) isSameFace = false;
            }
        }

        if (noneClipHit && !insideCollide) ClearPreCollision();

        cCollide = cCollide * 0.6f;
        fCollide = fCollide * 0.6f;

        if (isSameFace)
        {
            var temp = Mathf.Min(fCollide, cCollide);
            if (temp != Mathf.Infinity && temp > _rayDistance) _rayDistance += (temp - _rayDistance) * 0.08f;
            else
                _rayDistance = temp;
        }
        else
        {
            _rayDistance = Mathf.Min(fCollide, cCollide, _rayDistance);
        }

        if (_rayDistance < 0.2) _rayDistance = 0.2f;

        return _rayDistance;
    }

    public void ClearPreCollision()
    {
        _currentRay = new Ray();
        _rayDistance = Mathf.Infinity;
    }
}
