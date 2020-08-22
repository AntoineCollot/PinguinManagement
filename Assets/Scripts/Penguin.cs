using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour
{
    Animator anim;
    bool animIsGroundedState = false;
    bool animIsHeldState = false;

    public enum State { Idle, Held, None }
    State state = State.Idle;
    bool isGrounded = false;
    new Rigidbody rigidbody;

    [SerializeField] LayerMask iceLayer = 1 << 8;

    [Header("Temperature")]
    [SerializeField] float temperatureComfortableRange = 3;
    [SerializeField] float maxTemperatureDelta = 10;
    [SerializeField] float maxHp = 5;
    float hp;
    Vector2Int temperatureCoords = Vector2Int.one * 1000;
    int temperatureAnimHash;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        hp = maxHp;
        temperatureAnimHash = Animator.StringToHash("Temperature");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGroundedState();

        if (animIsGroundedState != isGrounded)
        {
            anim.SetBool("IsGrounded", isGrounded);
            animIsGroundedState = isGrounded;
        }

        if (animIsHeldState != (state == State.Held))
        {
            animIsHeldState = state == State.Held;
            anim.SetBool("IsHeld", state == State.Held);
        }

        //Destroy penguin
        if (transform.position.y < -3)
        {
            KillPenguin();
            return;
        }

        UpdateTemperaturePosition();

        float temperatureDelta = TemperatureManager.Instance.GetTemperatureDelta(temperatureCoords);
        //If the penguin is comfortable
        if (Mathf.Abs(temperatureDelta)<temperatureComfortableRange)
        {
            hp += Time.deltaTime;
            anim.SetFloat(temperatureAnimHash, 0);
        }
        else
        {
            float temperatureDeltaNormalized = Mathf.Lerp(0, 1, Mathf.Abs(temperatureDelta) / maxTemperatureDelta);
            hp -= Time.deltaTime * temperatureDeltaNormalized;
            anim.SetFloat(temperatureAnimHash, temperatureDeltaNormalized * Mathf.Sign(temperatureDelta));
        }
        //kill
        if(hp<0)
        {
            KillPenguin();
            return;
        }
    }

    public void KillPenguin()
    {
        Destroy(gameObject);
        TemperatureManager.Instance.RemovePenguin(this, temperatureCoords);
    }

    void UpdateTemperaturePosition()
    {
        Vector2Int newCoords = TemperatureManager.Instance.GetCoordsFromPosition(transform.position);
        if(temperatureCoords != newCoords)
        {
            TemperatureManager.Instance.UpdatePenguinPosition(this, temperatureCoords, newCoords);
            temperatureCoords = newCoords;
        }
    }

    public void UpdateGroundedState()
    {
        isGrounded = Physics.OverlapSphere(transform.position, 0.1f,iceLayer).Length > 0;
    }

    public void OnPickUp()
    {
        state = State.Held;

        rigidbody.useGravity = false;
    }

    public void OnRelease()
    {
        state = State.Idle;
        rigidbody.useGravity = true;

    }
}
