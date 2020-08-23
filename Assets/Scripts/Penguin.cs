using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour
{
    Animator anim;
    bool animIsGroundedState = false;
    bool animIsHeldState = false;

    public enum State { Idle, Held,Dead, None }
    State state = State.Idle;
    bool isGrounded = false;
    new Rigidbody rigidbody;

    [SerializeField] LayerMask iceLayer = 1 << 8;
    [SerializeField] float heldRotationSpeed = 50;

    [Header("Temperature")]
    [SerializeField] float temperatureComfortableRange = 3;
    [SerializeField] float maxTemperatureDelta = 10;
    [SerializeField] float maxHp = 5;
    float freezingHp;
    float hotHp;
    Vector2Int temperatureCoords = Vector2Int.one * 1000;
    int temperatureAnimHash;
    Material material;
    public enum TemperatureState { Comfortable, Freezing, Hot }
    TemperatureState lastNotComfortableState;

    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        freezingHp = maxHp;
        hotHp = maxHp;
        temperatureAnimHash = Animator.StringToHash("Temperature");

        material = GetComponentInChildren<SkinnedMeshRenderer>().material;
    }

    private void Start()
    {
        transform.localEulerAngles = new Vector3(0, Random.Range(-180, 180), 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Dead)
            return;

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
           // if(state!=State.Dead)
          //      AudioManager.Instance.PlayPenguinFallingSound();

            PenguinFall();
            return;
        }

        //Temperature
        UpdateTemperaturePosition();
        UpdateTemperatureState();

        //Rotate while held
        if (state == State.Held)
            transform.Rotate(heldRotationSpeed * Vector3.up * Time.deltaTime, Space.World);
    }

    public void KillPenguin()
    {
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.drag = 0.01f;
        rigidbody.AddTorque(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)) * 0.2f);
        anim.enabled = false;
        state = State.Dead;
        TemperatureManager.Instance.RemovePenguin(this, temperatureCoords);
        PenguinsManager.Instance.PenguinKilled();

        StartCoroutine(AnimMatHeight(1, 0.5f, 1f));
    }

    IEnumerator AnimMatHeight(float start, float end, float time)
    {
        float t = 0;
        while(t<1)
        {
            t += Time.deltaTime/time;
            material.SetFloat("_LocalMaxY", Mathf.Lerp(start, end, t));

            yield return null;
        }
    }

    public void PenguinFall()
    {
        Destroy(gameObject);
        if (state != State.Dead)
        {
            TemperatureManager.Instance.RemovePenguin(this, temperatureCoords);
            PenguinsManager.Instance.PenguinKilled();
        }
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

    void UpdateTemperatureState()
    {
        float temperatureDelta = TemperatureManager.Instance.GetTemperatureDelta(temperatureCoords);
        //If the penguin is comfortable
        if (Mathf.Abs(temperatureDelta) < temperatureComfortableRange)
        {
            freezingHp += Time.deltaTime;
            hotHp += Time.deltaTime;
            freezingHp = Mathf.Clamp(freezingHp, 0, maxHp);
            hotHp = Mathf.Clamp(hotHp, 0, maxHp);
            anim.SetFloat(temperatureAnimHash, 0);
        }
        else
        {
            float temperatureDeltaNormalized = Mathf.Lerp(0, 1, Mathf.Abs(temperatureDelta) / maxTemperatureDelta);
            anim.SetFloat(temperatureAnimHash, temperatureDeltaNormalized * Mathf.Sign(temperatureDelta));

            if (Mathf.Sign(temperatureDelta) > 0)
            {
                lastNotComfortableState = TemperatureState.Hot;
                hotHp -= Time.deltaTime * temperatureDeltaNormalized;
            }
            else
            {
                lastNotComfortableState = TemperatureState.Freezing;
                freezingHp -= Time.deltaTime * temperatureDeltaNormalized;
            }
        }

        material.SetFloat("_Hot", 1 - (hotHp / maxHp));
        material.SetFloat("_Freezing", 1 - (freezingHp / maxHp));

        //kill
        if (freezingHp < 0 || hotHp<0)
        {
            KillPenguin();
            return;
        }
    }

    public void UpdateGroundedState()
    {
        isGrounded = Physics.OverlapSphere(transform.position, 0.1f,iceLayer).Length > 0;
    }

    public void OnPickUp()
    {
        if (state != State.Dead)
        {
            state = State.Held;

            AudioManager.Instance.PlayShortPenguinSound();
        }

        rigidbody.useGravity = false;
    }

    public void OnRelease()
    {
        if (state != State.Dead)
            state = State.Idle;

        rigidbody.useGravity = true;
    }
}
