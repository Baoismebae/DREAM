using UnityEngine;
using TMPro;

public class ItemPrice : MonoBehaviour
{
    public GameObject priceUI; // Put it to PriceTag

    // When collide
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            priceUI.SetActive(true); // Show the price
        }
    }

    // When char move away
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            priceUI.SetActive(false); // Hide the price
        }
    }
}