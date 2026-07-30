using UnityEngine;
using System.Collections; // B?t bu?c ph?i có dòng này ?? dùng l?nh Ch? (Wait)

public class SpikeHeadMovement : MonoBehaviour
{
    [Header("Cài ??t Tu?n Tra")]
    // 1. T?o m?t m?ng (danh sách) các ?i?m t?a ?? (X, Y) ?? di chuy?n qua
    public Vector3[] pathPoints;
    public float moveSpeed = 16f;     // T?c ?? di chuy?n
    public float delayTime = 0.5f;    // Th?i gian ch? t?i m?i ?i?m

    private int currentPointIndex = 0; // ?i?m hi?n t?i ?ang nh?m t?i
    private bool isWaiting = false;    // Bi?n ki?m tra xem có ?ang ch? hay không

    void Start()
    {
        // 2. V?a vào game, t? d?ch chuy?n c?c gai t?i ?i?m ??u tiên cho ch?c ch?n
        if (pathPoints.Length > 0)
        {
            transform.position = pathPoints[0];
        }
    }

    void FixedUpdate()
    {
        // 3. N?u danh sách ?i?m tr?ng ho?c ?ang b?n "ch?", thì không làm gì h?t
        if (pathPoints.Length == 0 || isWaiting) return;

        // 4. Di chuy?n t?nh ti?n t?i ?i?m hi?n t?i (`currentPointIndex`)
        transform.position = Vector3.MoveTowards(transform.position, pathPoints[currentPointIndex], moveSpeed * Time.fixedDeltaTime);

        // 5. Ki?m tra n?u ?ã ch?m t?i ?i?m ?ích (kho?ng cách r?t nh?)
        if (Vector3.Distance(transform.position, pathPoints[currentPointIndex]) < 0.01f)
        {
            // B?t ??u quy trình Ch? và ch?n ?i?m ti?p theo
            StartCoroutine(WaitAndMoveToNext());
        }
    }

    // 6. Quy trình Coroutine: Ch? m?t lát r?i m?i ?i
    IEnumerator WaitAndMoveToNext()
    {
        isWaiting = true; // B?t tr?ng thái "?ang b?n ch?"

        // L?nh Ch? trong Unity (ph?i có System.Collections)
        yield return new WaitForSeconds(delayTime);

        // Ch?n ?i?m ti?p theo. Dùng toán t? chia l?y d? `%` ?? t? ??ng quay l?i 0 khi h?t danh sách.
        currentPointIndex = (currentPointIndex + 1) % pathPoints.Length;

        isWaiting = false; // T?t tr?ng thái ch?, cho phép di chuy?n ti?p
    }
}