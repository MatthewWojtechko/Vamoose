using UnityEngine;

public class LavaMovement : MonoBehaviour
{
    public float lavaSpeed;
    public GameObject player;
    public AudioSource ambience;
    private float volume = 0.2f;

	void Start () {
        lavaSpeed = 0.5f;
        ambience.loop = true;
        ambience.Play(0);
	}

    void Update() {
        transform.Translate(Vector3.up * Time.deltaTime * lavaSpeed, Space.World);

        volume = 20 - (player.transform.position.y - this.transform.position.y);
        if (volume < 0.1f)
            volume = 0.1f;
        ambience.volume = volume;
    }
}

