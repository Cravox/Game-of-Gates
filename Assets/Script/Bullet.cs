using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int normalDamage;
    public float normalRange;
    public int spreadDamage;
    public float spreadRange;

    private int damage;
    private float range;

    void Start()
    {
        Destroy(this.gameObject, range);
    }
    
    void Update()
    {

    }

    public enum Type : int
    {
        NORMAL = 0,
        SPREAD
    }

    public void Initialize(Type type){
        switch (type)
        {
            case Type.NORMAL:
                damage = normalDamage;
                range = normalRange;
                break;
            case Type.SPREAD:
                damage = spreadDamage;
                range = spreadRange;
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<Enemy>().hp -= damage;
            Destroy(this.gameObject);
        }
    }
}
