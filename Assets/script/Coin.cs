using UnityEngine;

public class Coin : MonoBehaviour {
    public int value = 1; // Giá trị mỗi đồng xu

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            // Giả sử Player có script PlayerStats để giữ tiền
            collision.GetComponent<PlayerStats>().AddCoins(value);
            Destroy(gameObject); // Nhặt xong thì biến mất
        }
    }
}
