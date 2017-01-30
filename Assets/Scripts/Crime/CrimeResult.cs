using UnityEngine;
using System.Collections;

public class CrimeResult : MonoBehaviour
{
    public SpriteRenderer sprite;
    private float timer = 1f;

	void Update ()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
            Destroy(gameObject);
	}
}
