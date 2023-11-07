using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorController : MonoBehaviour
{
    public bool isArmorHit;
    public LightController armorLight;
    public SpriteRenderer armorSprite;
    public enum ArmorNum
    {
        Num1 = 0,
        Num2 = 1,
        Num3 = 2,
        Num4 = 3,
        Num5 = 4,
        Sentry = 5,
        Outpost = 6,
        Base = 7,
    }
    public ArmorNum armorNum;
    public Sprite[] sprites;
    private float hitTime;
    // Start is called before the first frame update
    private void Start()
    {
        armorSprite.sprite = sprites[(int)armorNum];
        hitTime = 0.0f;
    }
    // Update is called once per frame
    private void Update()
    {
        if (isArmorHit)
        {
            armorLight.TurnOff();
            Debug.Log("hit");
            hitTime += Time.deltaTime;
            if (hitTime > 0.1f)
            {
                isArmorHit = false;
                hitTime = 0.0f;
            }
        }
        else
            armorLight.TurnOn();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            isArmorHit = true;
            collision.collider.tag = "Untagged";
        }
    }
}
