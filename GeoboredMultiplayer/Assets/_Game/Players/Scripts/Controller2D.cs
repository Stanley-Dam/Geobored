using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Controller2D : MonoBehaviour
{
    #region Variables
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private MultiPlayerPlayer player;
    private bool isMainPlayer = false;
    private Rigidbody2D rb;
    private Vector2 move;
    private Camera cam;
    #endregion

    private void Start()
    {
        cam = Camera.main;
        rb = this.GetComponent<Rigidbody2D>();
        if (player.GetIfMainPlayer())
        {
            isMainPlayer = true;
            cam.GetComponent<CameraMovement>().SetPlayer(this.gameObject);
        }
    }

    private void Update()
    {
        if(player != null && !player.GetIfMainPlayer())
            return;

        //turns the player to the Mouse
        Vector2 diraction = Input.mousePosition - cam.WorldToScreenPoint(this.transform.position);
        float angle = Mathf.Atan2(diraction.y, diraction.x) * Mathf.Rad2Deg;
        //update the rotation
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //moves the player
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        move = input.normalized * movementSpeed;
        //update the position
        rb.MovePosition(rb.position + move * Time.deltaTime);

        if(isMainPlayer && player.GetNetworkManager() != null)
            player.GetNetworkManager().MovePlayerOnServer(player, rb.position + move * Time.deltaTime, this.transform.rotation);
    }
}