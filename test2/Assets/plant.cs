using UnityEngine;

public class plant : MonoBehaviour
{
    private bool isActive = false;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameObject tril_obj;

    void Start()
    {
        // Get the SpriteRenderer component of the circle
        spriteRenderer = GetComponent<SpriteRenderer>();
       
    }

    void Update()
    {
        // Check if the mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null)
            {
                // Check if the clicked object is this circle
                if (hit.collider.gameObject == gameObject)
                {
                    ToggleActivation();
                }
                else
                {
                    // Deactivate this circle if clicked elsewhere
                    Deactivate();
                }
            }
            else
            {
                // Deactivate this circle if clicked in the air
               // Deactivate();
            }
        }
    }

    void ToggleActivation()
    {
        isActive = !isActive;

        // Change color based on activation status
        if (isActive)
        {
            spriteRenderer.color = Color.red;
            tril_obj.GetComponent<TrailGrowTowardsMouse>().enabled = true;
            Debug.Log("Circle activated!");
        }
        else
        {
            spriteRenderer.color = Color.white;
            tril_obj.GetComponent<TrailGrowTowardsMouse>().enabled = false;
            Debug.Log("Circle deactivated!");
        }
    }

    void Deactivate()
    {
        isActive = false;
        tril_obj.GetComponent<TrailGrowTowardsMouse>().enabled = false;
        spriteRenderer.color = Color.white;
    }
}

