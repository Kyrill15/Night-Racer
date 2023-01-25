using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IMoveAble
{
    void Move();
    void Drift();
    void TireSteering();
    void Steer();
    void GroundNormalRotation();

}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] protected Rigidbody rb;


    [SerializeField] protected float currentSpeed = 0;
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float realSpeed;//to show velocity

    [Header("Tires")]
    public Transform frontLeftTire;
    public Transform frontRightTire;
    public Transform backLeftTire;
    public Transform backRightTire;

    //drift and steering stuffz
    [SerializeField] private float steerDirection;
    [SerializeField] private float driftTime;

    public bool isSliding = false;

    private bool touchingGround;

    [Header("Particles Drift Sparks")]
    public Transform leftDrift;
    public Transform rightDrift;
    public Color drift1;
    public Color drift2;
    public Color drift3;

    [HideInInspector]
    public float BoostTime = 0;

    public Transform boostFire;
    public Transform boostExplosion;

    [SerializeField] private float IsGroundedRaycast;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        move();
        tireSteer();
        steer();
        groundNormalRotation();
        driftTimerFunc();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && touchingGround)
        {
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("Hop");
        }
    }

    private void move()
    {
        realSpeed = transform.InverseTransformDirection(rb.velocity).z; //real velocity before setting the value. This can be useful if say you want to have hair moving on the player, but don't want it to move if you are accelerating into a wall, since checking velocity after it has been applied will always be the applied value, and not real

        if (Input.GetKey(KeyCode.W))
            currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime * 0.5f); //speed
        else if (Input.GetKey(KeyCode.S))
            currentSpeed = Mathf.Lerp(currentSpeed, -maxSpeed / 1.75f, 1f * Time.deltaTime);
        else
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * 1.5f); //speed
        Vector3 vel = transform.forward * currentSpeed;
        vel.y = rb.velocity.y; //gravity
        rb.velocity = vel;
    }
    private void steer()
    {
        steerDirection = Input.GetAxisRaw("Horizontal"); // -1, 0, 1
        Vector3 steerDirVect; //this is used for the final rotation of the kart for steering

        float steerAmount;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (steerDirection > 0 && Input.GetKey(KeyCode.D) && driftTime == 0){
                if (transform.GetChild(0).localRotation == Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, 20f, 0), 8f * Time.deltaTime))
                    isSliding = true;

                if (Input.GetAxis("Horizontal") < 0)
                    steerDirection = 1.5f;
                else
                    steerDirection = 0.5f;
                

                transform.GetChild(0).localRotation =
                Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, 20f, 0), 8f * Time.deltaTime);
            }
            else if (steerDirection < 0 && Input.GetKey(KeyCode.A) && driftTime == 0){
                if (transform.GetChild(0).localRotation == Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, -20f, 0), 8f * Time.deltaTime))
                    isSliding = true;

                if (Input.GetAxis("Horizontal") > 0)
                    steerDirection = -1.5f;
                else
                    steerDirection = -0.5f;

                transform.GetChild(0).localRotation =
                Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, -20f, 0), 8f * Time.deltaTime);
            }
            else if (steerDirection == 0){
                driftTime = 0;
                isSliding = false;
                transform.GetChild(0).localRotation =
                Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, 0f, 0), 8f * Time.deltaTime);
            }
        }
        if (!Input.GetKey(KeyCode.LeftShift)){
            transform.GetChild(0).localRotation =
            Quaternion.Lerp(transform.GetChild(0).localRotation, Quaternion.Euler(0, 0f, 0), 8f * Time.deltaTime);
            driftTime = 0;
            isSliding = false;
        }

        //since handling is supposed to be stronger when car is moving slower, we adjust steerAmount depending on the real speed of the kart, and then rotate the kart on its y axis with steerAmount
        if (realSpeed > 30)
            steerAmount = (currentSpeed / 0.8f) * steerDirection;
        else
            steerAmount = (currentSpeed / 1.2f) * steerDirection;

        if (steerAmount > 0)
            steerDirection = 1;
        else if (steerAmount < 0)
            steerDirection = -1;
        else
            steerDirection = 0;

        steerDirVect = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + steerAmount, transform.eulerAngles.z);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, steerDirVect, 3 * Time.deltaTime);
    }
    private void groundNormalRotation()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 0.75f)){
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up * 2, hit.normal) * transform.rotation, 7.5f * Time.deltaTime);
            touchingGround = true;
        }
        else
            touchingGround = false;
    }
    
    private void tireSteer()
    {
        if (Input.GetKey(KeyCode.A))
        {
            frontLeftTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, 155, 0), 5 * Time.deltaTime);
            frontRightTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, 155, 0), 5 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            frontLeftTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, 205, 0), 5 * Time.deltaTime);
            frontRightTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, 205, 0), 5 * Time.deltaTime);
        }
        else
        {
            frontLeftTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, 180, 0), 5 * Time.deltaTime);
            frontRightTire.localEulerAngles = Vector3.Lerp(frontLeftTire.localEulerAngles, new Vector3(0, 180, 0), 5 * Time.deltaTime);
        }

        //tire spinning

        if (currentSpeed > 30)
        {
            frontLeftTire.GetChild(0).Rotate(-90 * Time.deltaTime * currentSpeed * 0.5f, 0, 0);
            frontRightTire.GetChild(0).Rotate(-90 * Time.deltaTime * currentSpeed * 0.5f, 0, 0);
            backLeftTire.Rotate(90 * Time.deltaTime * currentSpeed * 0.5f, 0, 0);
            backRightTire.Rotate(90 * Time.deltaTime * currentSpeed * 0.5f, 0, 0);
        }
        else
        {
            frontLeftTire.GetChild(0).Rotate(-90 * Time.deltaTime * realSpeed * 0.5f, 0, 0);
            frontRightTire.GetChild(0).Rotate(-90 * Time.deltaTime * realSpeed * 0.5f, 0, 0);
            backLeftTire.Rotate(90 * Time.deltaTime * realSpeed * 0.5f, 0, 0);
            backRightTire.Rotate(90 * Time.deltaTime * realSpeed * 0.5f, 0, 0);
        }
    }

    private void driftTimerFunc()
    {
        if (isSliding == true)
            driftTime += Time.deltaTime;
    }
}