using UnityEngine;
using System.Collections;

public class SokobanPlayer : MonoBehaviour
{
    [Header("Cài đặt")]
    public float moveSpeed = 5f;
    public LayerMask solidLayer;

    private Animator animator;
    private bool isMoving = false;

    // Tên animation đã được khớp với hình ảnh của bạn
    private string animWalkUp = "run up";
    private string animWalkDown = "run down";
    private string animWalkLeft = "run left";
    private string animWalkRight = "run right";
    private string animIdle = "idle";

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Đang đi thì không nhận thêm nút
        if (isMoving) return;

        Vector2 direction = Vector2.zero;
        string animToPlay = animIdle;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) { direction = Vector2.up; animToPlay = animWalkUp; }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) { direction = Vector2.down; animToPlay = animWalkDown; }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) { direction = Vector2.left; animToPlay = animWalkLeft; }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) { direction = Vector2.right; animToPlay = animWalkRight; }

        if (direction != Vector2.zero)
        {
            StartCoroutine(TryMoveCoroutine(direction, animToPlay));
        }
    }

    IEnumerator TryMoveCoroutine(Vector2 direction, string animToPlay)
    {
        isMoving = true;

        // Bật animation đi bộ
        animator.Play(animToPlay);

        Vector2 startPos = transform.position;
        Vector2 targetPos = startPos + direction;

        Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.1f, solidLayer);
        bool canMove = false;

        // ---> THÊM ĐÚNG 1 DÒNG NÀY VÀO ĐÂY <---
        if (hit != null) Debug.Log("Bị chặn bởi: " + hit.gameObject.name + " (Layer: " + LayerMask.LayerToName(hit.gameObject.layer) + ")");

        // Kiểm tra xem phía trước có trống không hoặc có đẩy được thùng không
        if (hit == null)
        {
            canMove = true;
        }
        else if (hit.CompareTag("Crate"))
        {
            SokobanCrate crate = hit.GetComponent<SokobanCrate>();
            if (crate != null && crate.TryPush(direction))
            {
                canMove = true;
            }
        }

        // PHẦN SỬA LỖI KẸT CỨNG (Dùng Timer)
        if (canMove)
        {
            float elapsedTime = 0f;
            // Tính toán thời gian cần để đi hết 1 ô
            float timeToMove = 1f / moveSpeed;

            // Vòng lặp này chắc chắn 100% sẽ kết thúc khi hết thời gian
            while (elapsedTime < timeToMove)
            {
                transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / timeToMove);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Căn chuẩn nhân vật vào chính giữa ô lưới khi đi xong
            transform.position = targetPos;
        }

        // Tới nơi (hoặc bị tường chặn), phát animation đứng im và mở khóa phím
        animator.Play(animIdle);
        isMoving = false;
    }
}