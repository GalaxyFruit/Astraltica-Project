using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShatterStone

{

public class PickupSpawnAnim : MonoBehaviour
{

  [SerializeField] private float jumpDuration = 1;
  [SerializeField] private float jumpDistance = 3;
  [SerializeField] private float jumpHeight = 2;
  [SerializeField] private float spinRate = 20;
  [SerializeField] private AnimationCurve jumpCurve;

  private bool jumpTrigger = true;



 private void Update() // Plays the jump forward animation followed by a constant spin.
  {
    if (jumpTrigger == true) {

      StartCoroutine(Animate());

    }

    else {

      transform.Rotate (0,spinRate*Time.deltaTime,0);

    }


  }


  private IEnumerator Animate() //Jump forward animation coroutine.
  {
    float t = 0;
    Vector3 origin = transform.position;
    Vector3 jumpTo = (transform.forward * jumpDistance) + origin;
    jumpTrigger = false;

    while (t < jumpDuration)
    {
      Vector3 heightJump = Vector3.up * Mathf.Sin ((t/jumpDuration) * Mathf.PI) * jumpHeight; //Height controlled by a Sin function
      float v = jumpCurve.Evaluate(t / jumpDuration); //Forward distance controlled with curve to ease out.
      transform.position = Vector3.Lerp(origin, jumpTo, v) + heightJump;
      t += Time.deltaTime;
      yield return null;
    }




  }

}
}
