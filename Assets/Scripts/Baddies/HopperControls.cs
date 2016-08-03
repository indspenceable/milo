﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(VerticalMovement))]
[RequireComponent(typeof(HorizontalMovement))]
public class HopperControls : MonoBehaviour {
	private VerticalMovement vert;
	private HorizontalMovement horiz;
	public float gravity = 30f;
	public float waitTime = 2f;
	public float jumpStrength = 10f;
	public float jumpVelocity = 5f;

	// Use this for initialization
	void Start () {
		vert = GetComponent<VerticalMovement>();
		horiz = GetComponent<HorizontalMovement>();
		horiz.vx = jumpStrength;
		StartCoroutine(Fall());
	}

	private IEnumerator Fall() {
		vert.vy = 0;
		while (! vert.CheckGrounded()) {
			vert.vy -= gravity * Time.deltaTime;
			vert.RiseOrFall(Time.deltaTime * vert.vy);
			yield return null;
		}
		StartCoroutine(Jump());
	}

	private IEnumerator Jump() {
		while (true) {
			if (vert.CheckGrounded()) {
				float dt = 0;

				while (dt < waitTime) {
					dt += Time.deltaTime;
					yield return null;
					if (!vert.CheckGrounded()) {
						StartCoroutine(Fall());
						yield break;
					}
				}
				vert.vy = jumpStrength;

			} else {
				vert.vy -= gravity * Time.deltaTime;
				vert.RiseOrFall(Time.deltaTime * vert.vy);
				float startingVx = horiz.vx;
				horiz.MoveLeftOrRight(Time.deltaTime * horiz.vx);
				if (horiz.vx == 0f) {
					horiz.vx = -startingVx;
				}
			}
			yield return null;
		}
	}
}
