﻿using UnityEngine;
using System.Collections;

public class Nave : MonoBehaviour {

	Animator animator;
	public float speed = 5;	
	public GameObject explosion;
	public Shot shot;
	public Transform frontCannnon;
	bool shooting;

	void Start () {
		animator = GetComponent<Animator>();
        
    if (FireButton.Instance)
    {
        FireButton.Instance.button.onClick.AddListener
        (() => Instantiate(
            shot.gameObject, 
            frontCannnon.position, 
            Quaternion.identity)
        );
    }
	}

	void Update () {
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
    
    if (SingleJoystick.Instance)
    {
        horizontal = 
            SingleJoystick.Instance.GetInputDirection().y;
            
        vertical = 
            SingleJoystick.Instance.GetInputDirection().x;
    }    
        
		Vector3 position = transform.position;

		position.y += horizontal * Time.deltaTime * speed;
		position.x += vertical * Time.deltaTime * speed;

		transform.position = position;
		animator.SetFloat("Fly", horizontal);

		if(Input.GetKey(KeyCode.Space) && !shooting) {
			shooting = true;
			StartCoroutine(Tiro());
		}

	}

	IEnumerator Tiro () {
		yield return new WaitForSeconds (shot.rate);
		Instantiate(shot.gameObject, frontCannnon.position, Quaternion.identity);
		shooting = false;
	}

	public void OnTriggerEnter2D(Collider2D collision) {

		if(collision.GetComponent<Shot>()) return;

		GameObject boom = Instantiate(explosion, transform.position, transform.rotation);

		Destroy(boom, 3);
		Naves.Instance.Respawn();		
		Destroy(gameObject);

	}

}