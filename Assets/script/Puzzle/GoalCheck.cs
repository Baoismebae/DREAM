using UnityEngine;

public class Goal : MonoBehaviour
{
    public Color activatedColor = Color.green;
    bool activated = false;

    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag("stone"))
        {
            activated = true;

            sr.color = activatedColor;

            Destroy(other.gameObject);

            PuzzleManager.instance.GoalCompleted();
        }
    }
}