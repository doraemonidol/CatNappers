using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController2D controller;
    private AxieFigure figure;

    [SerializeField] public int type = -1; // water - metal - wood - fire
    [SerializeField] GameObject GunPivot;
    [SerializeField] GameObject FireBallPivot;

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    private void Start()
    {
        figure = gameObject.GetComponentInChildren<AxieFigure>();
        resetSkills();
    }

    public void resetSkills()
    {
        GunPivot.SetActive(false);
        this.GetComponent<FireballShooter>().enabled = false;
        this.GetComponent<SpringJoint2D>().enabled = false;
    }

    private void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

    }

    void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        // Restart
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
