using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed;    
    [HideInInspector] public Quaternion _targetRotation = Quaternion.identity;    
    Rigidbody _rb;
    Vector3 _velocity;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        speed = GameManager.GAME.LevelSpeed;

    }

        void FixedUpdate()
    {
        if (!GameManager.GAME.Paused)
        {
            _rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 500f * Time.deltaTime);
        }

        if (transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        if (gameObject.tag == "Ball1") GameManager.GAME.RightPlaying = false;
        if (gameObject.tag == "Ball2") GameManager.GAME.LeftPlaying = false;
        GameManager.GAME.BlowUpBall();
    }
}


