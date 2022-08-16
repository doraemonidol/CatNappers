using UnityEngine;

public class Obstacle : MonoBehaviour
    {
	public int health = 0;

	public GameObject deathEffect;

	public void TakeDamage(int damage)
	{
		health -= damage;

		if (health <= 0)
		{
			Die();
		}
	}

	void Die()
	{
		Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
