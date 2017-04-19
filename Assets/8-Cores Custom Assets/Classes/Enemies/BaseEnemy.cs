using UnityEngine;

[System.Serializable]
public class BaseEnemy : MonoBehaviour
{
    //Used to check if player locked that enemy instance.
    [HideInInspector]
    public bool isLocked;

    //Used to instantiate new over-head health bar.
    //[HideInInspector]
    //public HealthBarEnemy healthbar;

    //Used to store and then generate drops.
    [HideInInspector]
    public Drop[] dropList = new Drop[1];

    //Used to position drops.
    [HideInInspector]
    public Vector3 deathPosition;

    //Array which contains items.
    public BaseCollectibleItem[] dropItems = new BaseCollectibleItem[1];

    //Array which contains quantities.
    public int[] dropQuantities = new int[1];

    //Variable that stores the enemy health.
    public float health;

    private void Start()
    {

        //healthbar = Instantiate(healthbar);

        //healthbar.name = this.name + "_HealthBar";

        //healthbar.transform.parent = this.transform;

        //healthbar.healthbarValue = health;

        //Pre-load items that will be dropped.

        if (dropItems.Length != dropQuantities.Length)
        {
            Debug.Log("EnemyScript.cs: Error!");
        }

        else
        {
            for (int i = 0; i < dropItems.Length; i++)
            {
                    Drop dr = new Drop(dropItems[i], dropQuantities[i]);

                    dropList[i] = dr;

                    System.Array.Resize(ref dropList, dropList.Length + 1);
            }
        }

        System.Array.Resize(ref dropList, dropList.Length - 1);
    }

    private void Update()
    {

        if (health <= 0)
        {
            //Play death animation then destroy.
            deathPosition = this.transform.position;

            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        GenerateDrop(dropList);
    }

    private void GenerateDrop(Drop[] dropList)
    {
        float randomX;
        float randomZ;
        Vector3 randomOffset;

        foreach (Drop drop in dropList)
        {
            if (drop != null)
            {
                int dropChance = (int)drop.item.rarity;

                for (int i = 0; i < drop.quantity; i++)
                {
                    int randomInt = Random.Range(1, 100);

                    if (randomInt > dropChance)
                    {
                        randomX = UnityEngine.Random.Range(0.01f, -0.01f);
                        randomZ = UnityEngine.Random.Range(0.01f, -0.01f);

                        randomOffset = new Vector3(this.gameObject.transform.localScale.x * (i * randomX), 0f, this.gameObject.transform.localScale.z * (i * randomZ));

                        Instantiate(drop.item, this.transform.position + randomOffset, Quaternion.identity);

                    }
                }
            }
        }
    }



 
 //public int RandomItem(int[] items) {
 //   int range = 0;

 //   for (int i = 0; i < items.Length; i++)
 //       {
 //           range += items[i];
 //       }


 //   int rand = Random.Range(0, range);
 //   int top = 0;
     
 //    for (int i = 0; i<items.Length; i++) {
 //        top += items[i];

 //        if (rand<top)
 //           {
 //               return i;
 //           }

 //    }

 //    return 0;
 //}


    [System.Serializable]
    public class Drop
    {
        public BaseCollectibleItem item;
        public int quantity;

        public Drop(BaseCollectibleItem i, int q)
        {
            item = i;
            quantity = q;
        }
    }



}
