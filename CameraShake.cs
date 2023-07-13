using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public GameObject Player;
    private bool canShake = false;
    private float shakeStrength = 0.1f;

    void Update()
    {
        if (canShake == true)
        {
            transform.position = Player.transform.position;
            Vector2 ShakePos = Random.insideUnitCircle * shakeStrength;
            transform.position = new Vector3(transform.position.x + ShakePos.x, transform.position.y + ShakePos.y, transform.position.z);
        }
    }

    public void ShakeOn(int shakeTime)
    {
        for (int x = shakeTime; x > 0; x--)
        {
            canShake = true;
            StartCoroutine("Wait");
        }
    }

    public void ShakeOff()
    {
        canShake = false;
        transform.position = Player.transform.position;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
    }
}
