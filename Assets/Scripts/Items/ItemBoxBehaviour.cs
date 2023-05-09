using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoxBehaviour : MonoBehaviour
{
    [HideInInspector]
    public ItemBehaviour itemBehaviour;
    [HideInInspector]
    public Image itemImage;
    [HideInInspector]
    public ObjectPooling itemBoxPooling;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag(Tags.PLAYER))
        {
            itemImage.sprite = itemBehaviour.sprite;
            collision.GetComponent<PlayerController>().itemToUse = itemBehaviour;
            itemBoxPooling.SaveObject(gameObject);
        }
    }
}
