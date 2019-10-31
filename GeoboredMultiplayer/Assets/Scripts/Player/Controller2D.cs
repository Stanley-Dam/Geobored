using UnityEngine;

public class Controller2D : MonoBehaviour
{
    #region Variables
    [SerializeField] private float movementSpeed = 10f;
    private Rigidbody2D rb;
    private Vector2 move;
    private Camera cam;
    #endregion
    [SerializeField] private Player player;

    private void Start()
    {
        cam = Camera.main;
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(!player.GetIfMainPlayer())
            return;

        //turns the player to the Mouse
        var diraction = Input.mousePosition - cam.WorldToScreenPoint(this.transform.position);
        var angle = Mathf.Atan2(diraction.y, diraction.x) * Mathf.Rad2Deg;
        //update the rotation
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //moves the player
        if(Input.GetKey(KeyCode.Escape))
            Debug.Log($"{Input.GetAxisRaw("Horizontal")}, {Input.GetAxisRaw("Vertical")}");

            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            move = input.normalized * movementSpeed;
            rb.MovePosition(rb.position + move * Time.deltaTime);

            player.GetNetworkManager().MovePlayerOnServer(player, rb.position + move * Time.deltaTime);
    }
}