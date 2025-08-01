using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed = 5f;
    public float boostSpeedMultiplier = 1.25f;
    public float acceleration = 10.0f;
    public bool grounded = false;

    [Space(2), Header("Rotation")]
    [Range(0.0f, 0.3f)]
    public float rotationSmoothTime = 0.12f;
    public float yawClamp = 45f;
    public float pitchClamp = 30f;
    public float yawMultiplier;
    public float pitchMultiplier;

    [Space(2), Header("Bounds")]
    public Transform clippingCube;
    public float xOffset = 4f;
    public Vector2 yRange;

    private new Rigidbody rigidbody;
    private StarterAssets.Inputs inputs;

    // player
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float pitch;
    private float yaw;

    private GameObject _mainCamera;


    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        inputs = GetComponent<StarterAssets.Inputs>();

        // get a reference to our main camera
        if(_mainCamera == null) {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Update() {
        KeepPlayerInBounds();
    }

    private void FixedUpdate() {
        ApplyRotation();
        Move();
    }

    private void Move() {
        //float targetSpeed = inputs.sprint ? SprintSpeed : MoveSpeed;
        float targetSpeed = movementSpeed;
        if(inputs.move == Vector2.zero) targetSpeed = 0.0f;

        //float currentHorizontalSpeed = new Vector3(rigidbody.velocity.x, 0.0f, rigidbody.velocity.z).magnitude;
        // zero out the forward movevment, not using it since our landscape is moving
        float currentSpeed = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0.0f).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = inputs.analogMovement ? inputs.move.magnitude : 1f;

        // accelerate or decelerate to target speed
        if(currentSpeed < targetSpeed - speedOffset || currentSpeed > targetSpeed + speedOffset) {
            // creates curved result rather than a linear one giving a more organic speed change
            _speed = Mathf.Lerp(currentSpeed, targetSpeed * inputMagnitude, Time.deltaTime * acceleration);
            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        } else {
            _speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * acceleration);
        if(_animationBlend < 0.01f) _animationBlend = 0f;

        // normalise input direction
        Vector3 inputDirection = new Vector3(inputs.move.x, 0.0f, inputs.move.y).normalized;

        // if there is a move input rotate player when the player is moving
        if(inputs.move != Vector2.zero) {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSmoothTime);

            // rotate to face input direction relative to camera position
            //transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // move the player
        float heightClamp = movementSpeed;
        if(grounded) {
            heightClamp = 0.0f;
        }
        rigidbody.velocity = targetDirection * _speed;
        rigidbody.velocity = new Vector3(Mathf.Clamp(rigidbody.velocity.x, -movementSpeed, movementSpeed),
                              Mathf.Clamp(rigidbody.velocity.z, -heightClamp, heightClamp),
                              0.0f);

        if(TryGetComponent(out BoatSail sail)) {
            float x = Mathf.Clamp(rigidbody.velocity.z, -1, 1);
            sail.ChangeWindDirection(x);
        }

        //_controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
        //                 new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        // update animator if using character
        //if(_hasAnimator) {
        //    _animator.SetFloat(_animIDSpeed, _animationBlend);
        //    _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        //}
    }

    void ApplyRotation() {
        Vector3 currentAngle;
        if(inputs.move == Vector2.zero) {
            yaw = 0.0f;
            pitch = 0.0f;

        } else {
            yaw += inputs.move.x * Time.deltaTime * yawMultiplier;
            pitch += inputs.move.y * Time.deltaTime * pitchMultiplier;
            
            yaw = Mathf.Clamp(yaw, -yawClamp, yawClamp);
            pitch = Mathf.Clamp(pitch, -pitchClamp, pitchClamp);

            if(inputs.move.x == 0) yaw = 0.0f;
            if(inputs.move.y == 0) pitch = 0.0f;
            if(grounded) pitch = 0.0f;
        }

        currentAngle = new Vector3(
            Mathf.LerpAngle(transform.eulerAngles.x, -pitch, Time.deltaTime * acceleration),
            //Mathf.SmoothDampAngle(transform.eulerAngles.x, pitch, ref _rotationVelocity, rotationSmoothTime),

            Mathf.LerpAngle(transform.eulerAngles.y, 0.0f, Time.deltaTime * acceleration),
            //Mathf.SmoothDampAngle(transform.eulerAngles.y, 0.0f, ref _rotationVelocity, rotationSmoothTime),

            Mathf.LerpAngle(transform.eulerAngles.z, -yaw, Time.deltaTime * acceleration)
            //Mathf.SmoothDampAngle(transform.eulerAngles.z, yaw, ref _rotationVelocity, rotationSmoothTime)
        );
        transform.eulerAngles = currentAngle;
    }

    void KeepPlayerInBounds() {
        float maxXPosition = clippingCube.position.x + ((clippingCube.localScale.x / 2) - xOffset);
        float minXPosition = clippingCube.position.x - ((clippingCube.localScale.x / 2) - xOffset);

        if(transform.position.x >= maxXPosition) {
            transform.position = new Vector3(maxXPosition, transform.position.y, transform.position.z);

        } else if(transform.position.x <= minXPosition) {
            transform.position = new Vector3(minXPosition, transform.position.y, transform.position.z);
        }

        if(!grounded) {
            if(transform.position.y <= yRange.x) {
                transform.position = new Vector3(transform.position.x, yRange.x, transform.position.z);

            } else if(transform.position.y >= yRange.y) {
                transform.position = new Vector3(transform.position.x, yRange.y, transform.position.z);
            }
        }
    }
}
