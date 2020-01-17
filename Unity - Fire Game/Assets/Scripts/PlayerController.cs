using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float minGroundNormalY = .65f;
    public float gravityModifier = 1f;

    protected Vector2 targetVelocity;
    protected bool grounded;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);


    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private SpriteRenderer spriteRenderer;

    public Transform playerTransform;
    public static bool hitTop = false;
    public ManageGame instance;
    public bool playerHit = false;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }
        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.00f) : (move.x < 0.00f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
        targetVelocity = move * maxSpeed;
    }

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    void FixedUpdate()
    {
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        grounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);
        move = Vector2.up * deltaPosition.y;
        Movement(move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        rb2d.position = rb2d.position + move.normalized * distance;
    }

    public void SetInstance(ManageGame gmInstance)
    {
        instance = gmInstance;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("movingCollider"))
        {
            //player escaped fire
            hitTop = true;
            Camera.main.GetComponent<CameraController>().SetPlayerTransform(playerTransform.transform);
            Camera.main.GetComponent<CameraController>().fireInView = false;
        }
        else if (other.CompareTag("fire") && !playerHit)
        {
            //player hit by fire
            playerHit = true;
            ManageGame temp = GameObject.FindObjectOfType(typeof(ManageGame)) as ManageGame;
            temp.LoseLife();
        }
        else if (other.CompareTag("blade") && !playerHit)
        {
            //player hit by blade
            playerHit = true;
            ManageGame temp = GameObject.FindObjectOfType(typeof(ManageGame)) as ManageGame;
            temp.LoseLife();
        }
        else if (other.CompareTag("spike") && !playerHit)
        {
            //player hit by spike
            playerHit = true;
            ManageGame temp = GameObject.FindObjectOfType(typeof(ManageGame)) as ManageGame;
            temp.LoseLife();
        }
        else if (other.CompareTag("win"))
        {
            ManageGame temp = GameObject.FindObjectOfType(typeof(ManageGame)) as ManageGame;
            temp.WinGame();
        }
    }
}