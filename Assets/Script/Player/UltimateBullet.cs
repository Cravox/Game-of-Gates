using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateBullet : MonoBehaviour
{
    public int normalDamage;
    public int spreadDamage;
    
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

    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<Enemy>().hp -= damage;
            print("hola");
        }
    }
}
