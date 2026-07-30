using UnityEngine;

public class SawMovement : MonoBehaviour
{
    [Header("Cài ??t bánh r?ng")]
    public float moveSpeed = 3f;      // T?c ?? l?n
    public float rotateSpeed = 360f;  // T?c ?? xoay (360 ?? = 1 vòng/giây)
    public float moveDistance = 3f;   // S? ô (unit) nó s? ?i ra xa tr??c khi quay ??u

    private float startPositionX; // ?i?m xu?t phát
    private float rightEdgeX;     // ?i?m quay ??u
    private bool movingRight = true; // ?ang l?n qua ph?i ?úng không?

    void Start()
    {
        // V?a vào game, t? ??ng ghi nh? v? trí ??ng hi?n t?i làm m?c
        startPositionX = transform.position.x;

        // Tính ra cái v?ch ?ích n?m cách m?c 3 ô v? bên ph?i
        rightEdgeX = startPositionX + moveDistance;
    }

    void Update()
    {
        // 1. Cái c?a lúc nào c?ng xoay vòng vòng (thu?n chi?u kim ??ng h?)
        transform.Rotate(0f, 0f, -rotateSpeed * Time.deltaTime);

        // 2. Ch?y qua ch?y l?i
        if (movingRight)
        {
            // L?n t? t? sang PH?I
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime, Space.World);

            // N?u l?n l? qua cái v?ch ?ích (3 ô) thì b?t ??u ch?y ng??c l?i
            if (transform.position.x >= rightEdgeX)
            {
                movingRight = false;
            }
        }
        else
        {
            // L?n t? t? sang TRÁI (?i lùi v? m?c)
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime, Space.World);

            // N?u lùi v? ??ng l?i ?úng ?i?m xu?t phát thì l?i l?n qua ph?i
            if (transform.position.x <= startPositionX)
            {
                movingRight = true;
            }
        }
    }
}