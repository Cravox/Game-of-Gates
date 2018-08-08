using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ultimateBullet : MonoBehaviour
{
    public int normalDamage;
    public int spreadDamage;
    public GameObject impact;
    
    private int damage;

    public enum Type : int
    {
        NORMAL = 0,
        SPREAD
    }

    public void Initialize(Type type)
    {
        switch (type)
        {
            case Type.NORMAL:
                damage = normalDamage;
                break;
            case Type.SPREAD:
                damage = spreadDamage;
                break;
            default:
                break;
        }
    }

    void Start()
    {
        Destroy(this.gameObject, 4f);
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            Instantiate(impact, new Vector3(this.transform.position.x - 0.5f, this.transform.position.y, this.transform.position.z), this.transform.rotation);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<Jussi>().hp -= damage;
        }

        if(col.gameObject.CompareTag("PeanutMissile"))
        {
            col.gameObject.GetComponent<Jussi_peanutMissile>().hp -= damage;
        }
    }
}
