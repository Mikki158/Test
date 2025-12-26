using System.Collections;
using UnityEngine;
using UnityEngine.Splines.ExtrusionShapes;

public class Icicle : MonoBehaviour
{
    [Header("Detection")]
    public float triggerDistance = 1.5f;

    [Header("Swing")]
    public float swingSpeed = 2f;
    public float swingAngle = 5f;
    public Transform spriteTransform;

    public Rigidbody2D rb;
    public Rigidbody2D rbParent;
    private Transform player;
    private bool hasFallen = false;
    private Quaternion startRotation;
    private Vector3 startPosition;


    void Awake()
    {
        rbParent = transform.parent.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        startPosition = transform.parent.position;
        startRotation = transform.parent.rotation;

        rbParent.bodyType = RigidbodyType2D.Kinematic;
        rbParent.gravityScale = 0f;
        rbParent.simulated = false;

        hasFallen = false;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (hasFallen) return;

        Swing();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasFallen) return;

        Debug.Log("Hi");

        if (other.CompareTag("Player"))
            Fall();
    }


    void Swing()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        spriteTransform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    bool IsPlayerClose()
    {
        return Vector2.Distance(transform.position, player.position) <= triggerDistance;
    }

    void Fall()
    {
        hasFallen = true;
        rbParent.bodyType = RigidbodyType2D.Dynamic;
        rbParent.gravityScale = 1f;
        rbParent.simulated = true;

        StartCoroutine(RespawnIcicle(2f));
    }

    public void ResetIcicle()
    {
        hasFallen = false;

        rbParent.linearVelocity = Vector2.zero;
        rbParent.angularVelocity = 0f;
        rbParent.bodyType = RigidbodyType2D.Kinematic;
        rbParent.gravityScale = 0f;

        transform.parent.position = startPosition;
        transform.parent.rotation = startRotation;

        spriteTransform.localRotation = Quaternion.identity;

        gameObject.SetActive(true);
    }

    IEnumerator RespawnIcicle(float delay)
    {
        yield return new WaitForSeconds(delay);

        ResetIcicle();
    }
}
