using UnityEngine;

public class LadyBugOld : MonoBehaviour
{
    public float moveSpeed = 5f;
    float baseSpeed = 2f;
    public Rigidbody2D rb;
    bool isMoving;
    private GameMapper _gameMap;
    Vector2 targetPosition;
    Vector2 currentPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isMoving = false;
    }

    public void SetMap(GameMapper map)
    {
        _gameMap = map;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isMoving)
        {
            var speedMultiplier = Random.Range(2, 6);
            if (speedMultiplier == 3)
            {
                targetPosition = _gameMap.GetSpawnPosition();
            }
            else
            {
                targetPosition = _gameMap.GetRandomPosition();

            }
            moveSpeed = baseSpeed * speedMultiplier;
            isMoving = true;
        }
    }
    private void FixedUpdate()
    {
        if (isMoving && rb.position != targetPosition)
        {
            Vector3 moveDirection = (Vector3)targetPosition - rb.transform.position;
            if (moveDirection != Vector3.zero)
            {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                rb.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            if ((Time.frameCount * moveSpeed) % 8 == 0)
            {
                Vector3 lTemp = transform.localScale;
                lTemp.y = transform.localScale.y * -1;
                transform.localScale = lTemp;
            }
            //moving
            float step = moveSpeed * Time.deltaTime;
            rb.position = Vector2.MoveTowards(rb.position, targetPosition, step);
        }
        else
        {
            isMoving = false;
        }
        rb.MovePosition(rb.position + currentPosition * moveSpeed * Time.fixedDeltaTime);
    }
    private void OnMouseDown()
    {
        // squishAnimation.SetTrigger("Active");
        Destroy(this.gameObject);
    }
}
