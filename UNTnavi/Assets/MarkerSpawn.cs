using System.Collections;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;

public class MarkerSpawn : MonoBehaviour
{
    [SerializeField]
    AbstractMap _map;

    public Vector3 location;

    void Start()
    {
        _map = GameObject.Find("Map").GetComponent<AbstractMap>();
    }

    private void Update()
    {
        transform.position = location;
        transform.position = new Vector3 (transform.position.x, 5, transform.position.z);
        // transform.localPosition = _map.GeoToWorldPosition(location, true);
    }

    public void DestroyMarker()
    {
        StartCoroutine(DestroyAnimation());
    }

    IEnumerator DestroyAnimation()
    {
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        yield return new WaitForSeconds(.5f);
        for (float i = 1; i > 0; i += -.1f)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, i);
            yield return new WaitForSeconds(.005f);
        }
        gameObject.Destroy();
    }
}
