using UnityEngine;

public class Laser : AdvancedMonoBehaviour
{
    Vector3 laserPosition;
    Vector3 laserSize;

    private void Start()
    {
        laserPosition = transform.localPosition;
        laserSize = transform.localScale;
    }
    public void LaserShotAnimation()
    {
        transform.localScale = new Vector3(laserSize.x, laserSize.y, 50f); //scale z == irgendetwas langes;
        transform.localPosition = new Vector3(laserPosition.x, laserPosition.y, 25.5f); // position z == z + 0,5 * irgendetwas langes;
        Invoke(HideLaser, 0.09f);
    }

    void HideLaser()
    {
        transform.localScale = laserSize; // ScaleMode z == 0,1;
        transform.localPosition = laserPosition; // ImagePosition z == 0,55;
    }
}
