using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    Collider _collider;
    Rigidbody _rigidbody;
    public Transform target;
    float speed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (target.position - transform.position).magnitude;
        transform.position = new Vector3( Mathf.Lerp(0, 10, 5),0,0);
        //Vector3.MoveTowards(transform.position, target.position, distance);
        //transform.Translate(target.position * speed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.T))
        {
            
            //_rigidbody.AddForce(target.position, ForceMode.Impulse);
            //transform.position = Vector3.MoveTowards(transform.position, target.position, distance);
            //Vector3 newPos = Vector3.MoveTowards(transform.position, target.position, distance);
            //transform.position = newPos;
        }
    }
}
