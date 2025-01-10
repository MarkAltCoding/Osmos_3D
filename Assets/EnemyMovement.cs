using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;

    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float rotationSpeed;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float airMovementSpeed;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private LayerMask groundLayer;

    [Header("Wall Detection")]
    [SerializeField] private float maxWallAngle;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float wallBounceMultiplier;

    private NavMeshAgent _agent;
    private Rigidbody _rb;
    private bool _isJumping;
    private float _nextJumpTime;
    private bool _isInitialized;

    private void Awake()
    {
        InitializeComponents();
    }

    private void Start()
    {
        if (!_isInitialized) return;
        ConfigureComponents();
        FindTarget();
    }

    private void InitializeComponents()
    {
        _agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
        _isInitialized = _agent != null && _rb != null;

        if (!_isInitialized)
        {
            enabled = false;
            Debug.LogError($"[{gameObject.name}] EnemyMovement: Missing required components! Disabling script.");
        }
    }

    private void ConfigureComponents()
    {
        _agent.speed = movementSpeed;
        _agent.stoppingDistance = stoppingDistance;
        
        _rb.isKinematic = true;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void FindTarget()
    {
        if (target != null) return;
        
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;
        }
        else
        {
            Debug.LogWarning($"[{gameObject.name}] EnemyMovement: No player found with 'Player' tag!");
            enabled = false;
        }
    }

    private void Update()
    {
        if (!_isInitialized || target == null) return;

        if (_isJumping)
        {
            HandleAirMovement();
        }
        else
        {
            HandleGroundMovement();
        }

        TryInitiateJump();
    }

    private void HandleGroundMovement()
    {
        if (!_agent.enabled) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        
        if (distanceToTarget > stoppingDistance)
        {
            _agent.SetDestination(target.position);
        }
        else
        {
            _agent.ResetPath();
        }

        FaceTarget();
    }

    private void HandleAirMovement()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 horizontalMovement = new Vector3(directionToTarget.x, 0, directionToTarget.z);
        _rb.AddForce(horizontalMovement * airMovementSpeed, ForceMode.Force);
    }

    private void TryInitiateJump()
    {
        if (_isJumping || Time.time < _nextJumpTime) return;
        if (IsWallAhead()) return;

        InitiateJump();
    }

    private void InitiateJump()
    {
        _isJumping = true;
        _nextJumpTime = Time.time + jumpCooldown;
        
        _agent.enabled = false;
        _rb.isKinematic = false;
        
        _rb.linearVelocity = Vector3.zero;
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private bool IsWallAhead()
    {
        if (!Physics.Raycast(transform.position, 
            (target.position - transform.position).normalized, 
            out RaycastHit hit, 
            wallCheckDistance))
        {
            return false;
        }

        float surfaceAngle = Vector3.Angle(hit.normal, Vector3.up);
        return surfaceAngle > maxWallAngle;
    }

    private void FaceTarget()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 horizontalDirection = new Vector3(directionToTarget.x, 0, directionToTarget.z);
        
        if (horizontalDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 
                Time.deltaTime * rotationSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isJumping) return;

        Vector3 normal = collision.contacts[0].normal;
        float surfaceAngle = Vector3.Angle(normal, Vector3.up);

        if (surfaceAngle > maxWallAngle)
        {
            HandleWallBounce(collision);
            return;
        }

        LandOnGround();
    }

    private void HandleWallBounce(Collision collision)
    {
        Vector3 reflectedVelocity = Vector3.Reflect(_rb.linearVelocity, collision.contacts[0].normal);
        _rb.linearVelocity = reflectedVelocity * wallBounceMultiplier;
    }

    private void LandOnGround()
    {
        _isJumping = false;
        _rb.isKinematic = true;
        _agent.enabled = true;
        _agent.Warp(transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw wall check ray
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 direction = (target.position - transform.position).normalized;
            Gizmos.DrawRay(transform.position, direction * wallCheckDistance);
        }

        // Draw stopping distance
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}