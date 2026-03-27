using UnityEngine;

public class SokobanGoal : MonoBehaviour
{
    private SpriteRenderer sr;
    public Color filledColor = Color.green;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void ActivateGoal()
    {
        sr.color = filledColor; // Đổi màu sang xanh lá
    }
}