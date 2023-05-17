using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoxController : MonoBehaviour
{
    public Image itemImage;
    public int maxItemBox;
    public SpawnManager.SpawnInterval spawnItemBoxInterval;
    public ObjectPooling itemBoxPooling;
    public List<ItemBehaviour> items;


    private void Start()
    {
        StartCoroutine(RepeatSpawn());
    }

    void SpawnItemBox()
    {
        if (GameObject.FindGameObjectsWithTag(Tags.ITEM_BOX).Length >= maxItemBox) return;
        // Get an item box, set its attributes and place it on the map
        GameObject itemBox = itemBoxPooling.GetObject();

        itemBox.GetComponent<AudioSource>().Play();

        ItemBoxBehaviour itemBoxBehaviour = itemBox.GetComponent<ItemBoxBehaviour>();
        itemBoxBehaviour.itemBehaviour = items[Random.Range(0, items.Count)];
        itemBoxBehaviour.itemImage = itemImage;
        itemBoxBehaviour.itemBoxPooling = itemBoxPooling;
        while(true)
        {
            Vector3 spawnPointPosition = transform.GetChild(Random.Range(0, transform.childCount)).position;
            Vector3 playerPosition = GameObject.FindWithTag(Tags.PLAYER).transform.position;
            if (Vector3.Distance(spawnPointPosition, playerPosition) > 3f)
            {
                itemBox.transform.position = spawnPointPosition;
                break;
            }
        }
    }

    IEnumerator RepeatSpawn()
    {
        yield return new WaitForSeconds(Random.Range(spawnItemBoxInterval.lowInterval, spawnItemBoxInterval.highInterval));
        while (true)
        {
            SpawnItemBox();
            yield return new WaitForSeconds(Random.Range(spawnItemBoxInterval.lowInterval, spawnItemBoxInterval.highInterval));
        }
    }
}
