using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // [SerializeField]
    // private float moveSpeed;
    //private int life = 3;
    //private enum PlayerClass
    //{
    //    None, Warrior, Mage, Archer
    //}

    //private enum PlayerWeapon
    //{
    //    BasicSword, 
    //}

    //[SerializeField]
    //private PlayerClass pClass = PlayerClass.None;

    //[SerializeField]
    //private PlayerWeapon pWeapon = PlayerWeapon.BasicSword;

    ////[SerializeField]
    ////private GameObject[] weapons;
    ////private int weaponIndex = 0;

    ////[SerializeField]
    ////private Transform shootTransform;

    ////[SerializeField]
    ////private float shootInterval;
    ////private float lastShotTime;

    ////[SerializeField]
    ////private AudioSource coinAudio;
    
    ////[SerializeField]
    ////private AudioSource hurtAudio;

    ////[SerializeField]
    ////private AudioSource upgradeAudio;


    //void Update()
    //{
    //    // float horizontalInput = Input.GetAxisRaw("Horizontal");
    //    // float verticalInput = Input.GetAxisRaw("Vertical");
    //    // float horizontalInput = Keyboard.current.leftArrowKey.isPressed ? -1 : (Keyboard.current.rightArrowKey.isPressed ? 1 : 0);
    //    // float verticalInput = Keyboard.current.upArrowKey.isPressed ? 1 : (Keyboard.current.downArrowKey.isPressed ? -1 : 0);

    //    // Vector3 moveTo = new Vector3(horizontalInput, verticalInput, 0f);
    //    // transform.position += moveTo * moveSpeed * Time.deltaTime;

    //    // Vector3 moveTo = new Vector3(moveSpeed * Time.deltaTime, 0, 0);
    //    // if (Keyboard.current.leftArrowKey.isPressed)
    //    // {
    //    //     transform.position -= moveTo;
    //    // }
    //    // else if (Keyboard.current.rightArrowKey.isPressed)
    //    // {
    //    //     transform.position += moveTo;
    //    // }

    //    //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    //    //float toX = Mathf.Clamp(mousePos.x, -2.35f, 2.35f);
    //    //transform.position = new Vector3(toX, transform.position.y, transform.position.z);

    //    // if(!GameManager.instance.isGameOver) Shoot();
    //}

    //void Shoot()
    //{
    //    //if (Time.time - lastShotTime > shootInterval)
    //    //{
    //    //    Instantiate(weapons[weaponIndex], shootTransform.position, Quaternion.identity);
    //    //    lastShotTime = Time.time;
    //    //}
    //}

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    //if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
    //    //{
    //    //    life--;
    //    //    hurtAudio.PlayOneShot(hurtAudio.clip);
    //    //    if (collision.gameObject.tag == "Boss") life = 0;

    //    //    if(life <= 0)
    //    //    {
    //    //        // Debug.Log("Game Over");
    //    //        GameManager.instance.SetGameOver();
    //    //        Destroy(gameObject);
    //    //    }
    //    //}

    //    //else if (collision.gameObject.tag == "Coin" || collision.gameObject.tag == "BigCoin")
    //    //{
    //    //    coinAudio.PlayOneShot(coinAudio.clip);

    //    //    if (collision.gameObject.tag == "Coin") GameManager.instance.IncreaseCoin(false);
    //    //    else GameManager.instance.IncreaseCoin(true);
            
    //    //    Destroy(collision.gameObject);
    //    //}

    //}
    
    //public void UpgradeWeapon()
    //{
    //    //weaponIndex++;
    //    //if (weaponIndex >= weapons.Length) weaponIndex = weapons.Length - 1;
    //    //upgradeAudio.PlayOneShot(upgradeAudio.clip);
    //}
}
