using UnityEngine;
using UnityEngine.AI;

public class RobotWanderRaycast : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float rayDistance = 5f;
    public float rayRadius = 0.6f;
    public float rayHeight = 0.4f;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float wallAvoidStrength = 3f;
    public float wanderChangeInterval = 3f;

    [Header("Layer Masks")]
    public LayerMask boxMask;
    public LayerMask wallMask;

    [Header("Delivery")]
    public Transform shelf;
    public float deliveryDistance = 1.0f;

    private NavMeshAgent agent;
    private RobotPickup pickup;
    private ShelfController shelfController;

    private float wanderTimer = 0f;
    private Vector3 wanderDirection;

    private bool isDone = false;   


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pickup = GetComponent<RobotPickup>();
        shelfController = shelf.GetComponent<ShelfController>();

        agent.speed = moveSpeed;
        agent.acceleration = 8;
        agent.angularSpeed = 240;
        agent.autoBraking = true;

        ChooseRandomWanderDirection();
    }



    void Update()
    {
     
        // 0. STOP MODE 
     
        if (isDone)
        {
            agent.ResetPath();
            return;
        }



        // 1. If the robot has boxes → try to deliver

        if (pickup.GetCarriedCount() >= 1)
        {
          
            if (!shelfController.HasSpace())
            {
                isDone = true;
                agent.ResetPath();
                return;
            }

     
            agent.SetDestination(shelf.position);

            if (Vector3.Distance(transform.position, shelf.position) < deliveryDistance)
                DeliverBox();

            return;
        }



        // 2. Robot has NO boxes → check if shelf is full

        if (!shelfController.HasSpace())
        {
            isDone = true;
            agent.ResetPath();
            return;
        }





 
BoxDetection:

        Vector3 rayOrigin =
            transform.position
            + transform.up * rayHeight
            + transform.forward * 0.2f;

        RaycastHit hit;

        bool hitBox = Physics.SphereCast(
            rayOrigin,
            rayRadius,
            transform.forward,
            out hit,
            rayDistance,
            boxMask,
            QueryTriggerInteraction.Ignore
        );

        if (hitBox && hit.collider.CompareTag("box"))
        {
            BoxController bc = hit.collider.GetComponent<BoxController>();
            if (bc != null && !bc.isPickedUp)
            {
                agent.SetDestination(hit.collider.transform.position);
                return;
            }
        }



        // 4. WALL AVOIDANCE

        if (Physics.Raycast(rayOrigin, transform.forward, out RaycastHit wallHit, 1f, wallMask))
        {
            Vector3 avoidDir = Vector3.Reflect(transform.forward, wallHit.normal);
            Vector3 newPos = transform.position + avoidDir * wallAvoidStrength;

            if (NavMesh.SamplePosition(newPos, out NavMeshHit navHit, 2f, NavMesh.AllAreas))
            {
                agent.SetDestination(navHit.position);
                return;
            }
        }




        wanderTimer += Time.deltaTime;
        if (wanderTimer >= wanderChangeInterval)
            ChooseRandomWanderDirection();

        Vector3 wanderTarget = transform.position + wanderDirection * 2f;

        if (NavMesh.SamplePosition(wanderTarget, out NavMeshHit wanderHit, 2f, NavMesh.AllAreas))
            agent.SetDestination(wanderHit.position);
    }



    void ChooseRandomWanderDirection()
    {
        wanderDirection = new Vector3(
            Random.Range(-1f, 1f),
            0,
            Random.Range(-1f, 1f)
        ).normalized;

        wanderTimer = 0f;
    }



    void DeliverBox()
    {
        if (pickup.GetCarriedCount() == 0) return;

        
        if (!shelfController.HasSpace())
        {
            isDone = true;
            agent.ResetPath();
            return;
        }

        Transform slot = shelfController.GetEmptySlot();
        GameObject box = pickup.GetLastCarriedBox();

        box.transform.SetParent(slot, false);
        box.transform.localPosition = Vector3.zero;
        box.transform.localEulerAngles = Vector3.zero;
        box.transform.localScale = Vector3.one;

     
        box.tag = "stored";
        box.layer = LayerMask.NameToLayer("StoredBox");
        box.GetComponent<BoxController>().isPickedUp = true;
        box.GetComponent<Collider>().enabled = false;

        Rigidbody rb = box.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        pickup.RemoveLastBox();
    }




    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Vector3 rayOrigin =
            transform.position
            + transform.up * rayHeight
            + transform.forward * 0.2f;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(rayOrigin, rayRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayOrigin, rayOrigin + transform.forward * rayDistance);
    }
}
