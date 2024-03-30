using UnityEngine;

public class Archer : Weapon
{
    [SerializeField]
    private GameObject arrowPrefab;
    public float arrowSpeed = 10f;

    public override void Skill(int num, int level)
    {
        FanShot();
        /*switch (num)
        {
            case 1:
                FanShot();
                break;
            case 2:
                Barrage();
                break;
            case 3:
                PiercingArrow();
                break;
            case 4:
                BombArrow();
                break;
            case 5:
                NormalShot();
                break;
            case 6:
                ArrowRain();
                break;
            case 7:
                RapidFire();
                break;
            case 8:
                GuidedArrow();
                break;
            case 9:
                SwingBow();
                break;
            default:
                Debug.Log("Unknown skill.");
                break; 
        } */
    }

    void FanShot()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = -Camera.main.transform.position.z;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0;

        Debug.Log(mouseWorldPosition);

        if (arrowPrefab != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            Vector2 direction = (mouseWorldPosition - this.transform.position).normalized;

            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * arrowSpeed;

                // 화살의 회전을 마우스 위치의 방향으로 설정합니다.
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
        else
        {
            Debug.Log("Arrow Prefab Null Error");
        }
    }

    void Barrage()
    {
        Debug.Log("Launching Barrage: Rapid fire at a single target with charge and bonus damage.");
    }

    void PiercingArrow()
    {
        Debug.Log("Launching Piercing Arrow: Fires a projectile that pierces through enemies in a line.");
    }

    void BombArrow()
    {
        Debug.Log("Launching Bomb Arrow: Arrow explodes at the target location, dealing area damage.");
    }

    void NormalShot()
    {
        Debug.Log("Launching Normal Shot: A basic attack.");
    }

    void ArrowRain()
    {
        Debug.Log("Launching Arrow Rain: Area attack with multiple arrows.");
    }

    void RapidFire()
    {
        Debug.Log("Launching Rapid Fire: Shoots 2-3 arrows rapidly.");
    }

    void GuidedArrow()
    {
        Debug.Log("Launching Guided Arrow: Arrow seeks the nearest enemy.");
    }

    void SwingBow()
    {
        Debug.Log("Swinging Bow: Knocks back nearby enemies.");
    }
}
