using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShatterStone

{

public class OreNode : MonoBehaviour
{
    [SerializeField] private GameObject pieces;
    [SerializeField] private GameObject refinedPickup;
    [SerializeField] private int dropOnHit;
    [SerializeField] private int hitsToDestroy;
    [SerializeField] private int dropOnDestroy;
    [SerializeField] private Vector3 knockAngle;
    [SerializeField] private AnimationCurve knockCurve;
    [SerializeField] private float knockDuration = 1;
    private int dropIndex;
    private int hitIndex;


    // On click trigger.
    void OnMouseDown()
    {
      oreHit();
    }

    // Sets number of pickups to spawn.
    public void oreHit()
    {
      hitIndex++;
      if (hitIndex < hitsToDestroy)
      {
          dropIndex = dropOnHit;
      }
      else
      {
          dropIndex = dropOnDestroy;
      }

      // Gets node bounds for pickup spawn location.
      Renderer renderer = GetComponent<Renderer>();
      Bounds worldBounds = renderer.bounds;
      float minX = worldBounds.min.x;
      float maxX = worldBounds.max.x;
      float centerY = worldBounds.center.y;
      float minZ = worldBounds.min.z;
      float maxZ = worldBounds.max.z;

      for (int i = 0; i < dropIndex; i++)
      {
          Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), centerY, Random.Range(minZ, maxZ));

          Instantiate(refinedPickup, randomPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));
      }

      if (hitIndex < hitsToDestroy) //Controls when to shatter.
      {
          // Knock animation.
          StartCoroutine(Animate());

      }
      else
      {
          //Spawn pieces and destroy.
          Vector3 position = transform.position;
          Quaternion rotation = transform.rotation;
          Instantiate(pieces, position, rotation);
          Destroy(this.gameObject);
      }



    }


    private IEnumerator Animate() //Knock animation coroutine.
    {
      float t = 0;
      while (t < knockDuration)
      {
        float v = knockCurve.Evaluate(t / knockDuration);
        transform.localRotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(knockAngle), v);
        t += Time.deltaTime;
        yield return null;
      }
    }
}
}
