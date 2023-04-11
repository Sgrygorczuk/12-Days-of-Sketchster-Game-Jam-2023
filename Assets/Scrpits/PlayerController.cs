using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    
    //========= Components  
    //Controls Character Movement 
    private Rigidbody2D _rigidbody2D;
    //Controls Player Animations 
    private Animator _animator;

    //========= Flags 
    //Tells us if the player can move, if the game is won or lost player loses control over the character 
    private bool _canPlay = true;
    //Tells us if the character has the sword, if they do they can now defeat enemies 
    private bool _isEquipped; 
    //Tells us if the player is near the ladder, if they are they can begin to climb 
    private bool _canClimb;
    //Tells us if the player is currently climbing or not. If they are they cannot jump 
    private bool _isClimbing;
    //Tells us if the player is currently touching ground, if they are they can jump 
    private bool _isGrounded;

    //========= Movement 
    //Controls the height the player will jump 
    [SerializeField] private float ySpeed = 10;
    //Controls the lenght of the ground check
    public float distanceToGround = 0.1f;
    //Tells us which layer the collision is looking for. [Layer = Floor] 
    public LayerMask groundLayer;

    
    //========= External Objects 
    //Game Object that holds all of the collectibles, once it's empty door will be destroyed  
    private GameObject _collectibles;
    //Game Object of door that will be destroyed once all collectible are collected 
    private GameObject _door;
    //Game Controller which will pick which end canvas will pop up for the player to use 
    private GameController _gameController;

    
    //==================================================================================================================
    // Basic Methods  
    //==================================================================================================================

    /// <summary>
    /// Connects the Components and Game Object to their scene counter parts 
    /// </summary>
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        
        _gameController = GameObject.Find("Controller").GetComponent<GameController>();
        _collectibles = GameObject.Find("Collectible").gameObject;
        _door = GameObject.Find("Door").gameObject;
    }

    /// <summary>
    /// As long as the player can play the game listens for button inputs and does ground checks 
    /// </summary>
    private void Update()
    {
        if (!_canPlay) return;
        Controls();
        GroundCheck();
    } 
    
    //==================================================================================================================
    // Movement  
    //==================================================================================================================

    /// <summary>
    /// Listens to the button inputs and based on the state allows the player to Walk, Jump, and Climb.
    /// </summary>
    private void Controls()
    {
        //If the player is holding horizontal keys they will move left and right 
        if (Input.GetAxis("Horizontal") != 0)
        {
            WalkUpdate();
        }
        //If the player stopped holding horizontal inputs the player stops moving horizontally 
        else
        {
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
            _animator.SetBool($"isWalking", false);
        }
        
        //If the player clicks the jump input while being ground and not climbing they will jump 
        if (Input.GetButtonDown("Jump") && _isGrounded && !_isClimbing)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, ySpeed);
        }

        //If the player is near a ladder they can click vertical inputs to move up and down the ladder 
        if (Input.GetAxis("Vertical") != 0 && _canClimb) { Climbing(); }
    }
    
    /// <summary>
    /// Updates the player horizontal movement 
    /// </summary>
    private void WalkUpdate()
    {
        //While the player is not climbing, clicking left or right will make the scale go between -1 and 1 to change the way the sprite is facing 
        if(!_isClimbing){transform.localScale = Input.GetAxis("Horizontal") < 0 ? new Vector3(-1, 1, 1) : new Vector3(1,1,1);}
        //Adds the speed to the x axis while holding the vertical speed 
        _rigidbody2D.velocity = new Vector2(Input.GetAxis("Horizontal"), _rigidbody2D.velocity.y);
        //Sets the animator to to animate walking 
        _animator.SetBool($"isWalking", true);
    }

    /// <summary>
    /// Allows the player to move up and down on the ladder 
    /// </summary>
    private void Climbing()
    {
        //Tells us the player is actively going up or down the ladder 
        _isClimbing = true;
        //Sets gravity to 0 so that the player doesn't slide down the ladder 
        _rigidbody2D.gravityScale = 0;
        //Moves the player up or down based on the player input 
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, Input.GetAxis("Vertical") );
        //Set the animation to be climbing 
        _animator.SetBool($"isClimbing", true);
    }

    //==================================================================================================================
    // Ground Checks   
    //==================================================================================================================

    /// <summary>
    /// Checks if the player is currently standing on ground 
    /// </summary>
    private void GroundCheck()
    {
        //Casts a box bellow the player at distanceToGround checking for a collider with the groundLayer 
        var hit = Physics2D.BoxCast(transform.position, new Vector2(1f, 0.1f), 0f, Vector2.down, distanceToGround, groundLayer);
        //Copies the result to _isGrounded 
        _isGrounded = hit.collider != null;
        //Updates the animator 
        _animator.SetBool($"isJumping", !_isGrounded);
    }

    /// <summary>
    /// Draws a box for us to see when testing the game 
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        //Picks color for the gizmo 
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        //Draws the gizmo 
        Gizmos.DrawWireCube(transform.position - new Vector3(0f, distanceToGround / 2, 0f), new Vector3(1f, distanceToGround, 1f));
    }

    //==================================================================================================================
    // Triggers   
    //==================================================================================================================

    /// <summary>
    /// Checks if the player walks into any Trigger Colliders 
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter2D(Collider2D col)
    {
        //Checks if player walked into a ladder is if so they can climb it
        if (col.gameObject.CompareTag($"Ladder"))
        {
            _canClimb = true;
        }

        //Checks if the player walked into a collectible, if so destroy it, if player got all collectibles destroy door 
        if (col.gameObject.CompareTag($"Collectible"))
        {
            Destroy(col.gameObject);
            if (_collectibles.transform.childCount <= 1)
            {
                Destroy(_door);
            }
        }

        //Checks if player walked into a death items such as Dragon or Fire, if they don't have a sword they die
        //If they do have a sword they kill it
        if (col.gameObject.CompareTag($"Death"))
        {
            //Kill Enemy 
            if (_isEquipped)
            {
                Destroy(col.gameObject);
            }
            //Die 
            else
            {
                _canPlay = false;
                _gameController.EndGame(true);   
            }
        }

        //Checks if the player fell off the map, if so they die
        if (col.gameObject.CompareTag($"FloorDeath"))
        {
            _canPlay = false;
            _gameController.EndGame(true);   
        }

        //Checks if the player got the sword, if so they now change animation set and now can kill enemies 
        if (col.gameObject.CompareTag($"Sword"))
        {
            _animator.SetBool($"isEquiped", true);
            Destroy(col.gameObject);
            _isEquipped = true;
        }
        
        //Checks if the player touched the people if so they won the game and get the pop up screen 
        if (col.gameObject.CompareTag($"People"))
        {
            _canPlay = false;
            _gameController.EndGame(false);   
        }

    }

    /// <summary>
    /// Checks if the player walked off the ladder
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerExit2D(Collider2D col)
    {
        //Checks if the player left the ladder 
        if (col.gameObject.CompareTag($"Ladder"))
        {
            //They no longer can climb
            _canClimb = false;
            //They are no longer climbing 
            _isClimbing = false;
            //Gravity goes back to normal 
            _rigidbody2D.gravityScale = 1;
            //Changes the animator ot stander actions 
            _animator.SetBool($"isClimbing", false);
        }
    }
}
