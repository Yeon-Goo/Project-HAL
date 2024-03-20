using UnityEngine;

public class Arrow : Weapon
{
    // 스킬 사용 메서드
    public override void Skill(int num, int level)
    {
        switch (num)
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
        }
    }

    void FanShot()
    {
        Debug.Log("Launching Fan Shot: Fires multiple arrows in a cone shape.");
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
