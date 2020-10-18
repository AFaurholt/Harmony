using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBehaviour : MonoBehaviour
{
    [SerializeField] bool _showDebug = false;

    [SerializeField] float _speed = 10f;
    [SerializeField] Transform _modelTrans = default;
    [SerializeField] Transform _camTrans = default;
    [SerializeField] float _mouseYMinDeg = -10f;
    [SerializeField] float _mouseYMaxDeg = 90f;
    Vector3 _camOffset = Vector3.zero;
    float _camRadius = 0f;

    CharacterController _cc = default;

    Vector2 _mouseMoved = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        _cc = GetComponent<CharacterController>();
        _camOffset = _camTrans.position - transform.position;
        Vector3 camTransTmp = new Vector3(_camTrans.position.x, 0f, _camTrans.position.z); 
        Vector3 posTmp = new Vector3(transform.position.x, 0f, transform.position.z);
        _camRadius = Vector3.Distance(camTransTmp, posTmp);
        _mouseMoved.x = Mathf.Acos(_camTrans.position.x / _camRadius) / Mathf.Deg2Rad;
    }

    // Update is called once per frame
    void Update()
    {
        if (_showDebug)
        {
            //player
            Debug.DrawLine(transform.position, transform.position + (transform.forward * 3f));
            Debug.DrawLine(transform.position, _cc.velocity + transform.position, Color.red);
            //cam
            Debug.DrawLine(transform.position, _camTrans.position, Color.green);
            Debug.DrawLine(_camTrans.position, _camTrans.position + _camTrans.forward * 3f);
        }

        //get input
        Vector3 inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        _mouseMoved += new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        _mouseMoved.y = Mathf.Clamp(_mouseMoved.y, _mouseYMinDeg, _mouseYMaxDeg);

        //move
        //_camTrans.position = new Vector3(
        //      _camRadius * Mathf.Cos(Mathf.Deg2Rad * _mouseMoved.x)
        //    , _camOffset.y //_camRadius * Mathf.Sin(Mathf.Deg2Rad * _mouseMoved.y)
        //    , _camRadius* Mathf.Sin(Mathf.Deg2Rad * _mouseMoved.x))
        //   + transform.position;



        //_camTrans.LookAt(transform);
        //transform.forward = new Vector3(_camTrans.forward.x, 0f, _camTrans.forward.z);
        _cc.Move(transform.TransformDirection(inputVector) * Time.deltaTime * _speed);
    }
}
