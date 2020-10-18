using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _modelTransform = default;
    [SerializeField] private Transform _cameraArmTransform = default;
    [SerializeField] private Transform _cameraTransform = default;
    [SerializeField] private Transform _cameraFocusTransform = default;
    [SerializeField] private float _cameraRadius = 3f;

    [SerializeField] private float _speed = 10f;

    private CharacterController _characterController = default;
    private Vector2 _mouseMoved = Vector2.zero;

    private readonly float _debugLineLength = 3f;
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        #region Debug
        //player
        Debug.DrawLine(transform.position, transform.position + transform.forward * _debugLineLength, Color.blue);
        Debug.DrawLine(transform.position, _characterController.velocity + transform.position, Color.red);

        //model
        Debug.DrawLine(_modelTransform.position, _modelTransform.position + _modelTransform.forward * _debugLineLength);

        //cam
        Debug.DrawLine(_cameraTransform.position, _cameraTransform.position + _cameraTransform.forward * _debugLineLength);
        #endregion

        //get input
        Vector3 inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        

        _mouseMoved += new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        _cameraRadius += Input.GetAxis("Mouse ScrollWheel");
        _cameraArmTransform.rotation = Quaternion.Euler(0f, _mouseMoved.x, 0f);

        Vector3 camMove = new Vector3(
              0f
            , Mathf.Sin(Mathf.Deg2Rad * _mouseMoved.y) * _cameraRadius
            , Mathf.Cos(Mathf.Deg2Rad * _mouseMoved.y) * -_cameraRadius
            );

        //move
        _cameraTransform.localPosition = camMove;
        _cameraTransform.LookAt(_modelTransform);
        Vector3 test = _cameraTransform.right;
        _characterController.Move(test * Time.deltaTime * _speed);
    }
}
