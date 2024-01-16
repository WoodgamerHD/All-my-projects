using System;
using System.Collections;
using UnityEngine;

namespace MuckHaxx2
{
    class Main : MonoBehaviour
    {
        bool esp = true;
        bool Box_esp = true;
        bool chest_esp = true;
        bool GodMode = false;
        bool InfiniteStamina = false;
        bool InfiniteFood = false;
        bool flyhack = false;
        bool spawnermob = false;
        public static int tabs = 6;
        private ObjectCache<PlayerStatus> Statuses = new ObjectCache<PlayerStatus>();
        private int mobIndex = 0;
        private int amountOfMobs = 1;
        int baseX = (Screen.width / 2) - 100;
        int baseY = (Screen.height / 2) - 100;
        private Vector2 scrollPosition1 = Vector2.zero;
        private String[] mobNames = { "Cow", "Fire Dave", "Electric Dave", "lil Dave", "Water Dave", "Goblin", "Rock Man", "Gronk", "Dragon", "Chunky Man", "Gronk", "Wolf" };
        private int mob = 10000;
     //   private MobSpawner mobHack;
      
        private int damageMultiplier = 1;
        private PlayerStatus playerHack;
        public MobSpawner mobHack;
        class ObjectCache<T> where T : UnityEngine.Object
        {
            public float UpdateInterval { get; private set; }
            public T[] Objects { get; private set; }
            public T Object { get; private set; }
            public bool Single = false;

            public ObjectCache(float updateInterval = 5.0f, bool single = false)
            {
                UpdateInterval = updateInterval;
                Single = single;
            }

            public IEnumerator Update()
            {
                while (true)
                {
                    if (Single)
                        Object = GameObject.FindObjectOfType<T>();
                    else
                        Objects = GameObject.FindObjectsOfType<T>();

                    yield return new WaitForSeconds(UpdateInterval);
                }
            }

            public void Init(MonoBehaviour self)
            {
                self.StartCoroutine(this.Update());
            }
        }


        public void DrawBoxESP(Vector3 footPosition, Vector3 headPosition, Color color)
            {
                System.Single height = headPosition.y - footPosition.y;
                System.Single widthOffset = 2f; System.Single width = height / widthOffset;
                ExtRender.DrawBox(footPosition.x - width / 2, Screen.height - footPosition.y - height, width, height, color, 2f);

            }




            public void OnGUI()
        {
           
            //    ItemGenerator.DrawMenu();
            


            GUI.Box(new Rect(10f, 10f, 300f, 300f), "Menu");

            if (GUI.Button(new Rect(20f, 40f, 80f, 20f), "Box_esp"))
            {
                Box_esp = !Box_esp;
            }
            if (GUI.Button(new Rect(20f, 70f, 80f, 20f), "chest_esp"))
            {
                chest_esp = !chest_esp;
            }
            if (GUI.Button(new Rect(20f, 100f, 80f, 20f), "GodMode"))
            {
                GodMode = !GodMode;
            }

            if (GUI.Button(new Rect(20f, 140f, 80f, 20f), "Inf-Stamina"))
            {
                InfiniteStamina = !InfiniteStamina;
            }
            if (GUI.Button(new Rect(20f, 170f, 80f, 20f), "Inf-Food"))
            {
                InfiniteFood = !InfiniteFood;
            }
            if (GUI.Button(new Rect(120f, 40f, 90f, 20f), "Spawn"))
            {
                GameManager.instance.SpawnPlayer(2, "a", Color.black, new Vector3(0f, 30f, 0f), 50f);
                GameManager.instance.SpawnPlayer(3, "b", Color.red, new Vector3(5f, 30f, 2f), 150f);
            }
            if (GUI.Button(new Rect(120f, 70f, 80f, 20f), "flyhack"))
            {
                flyhack = !flyhack;
            }
            if (GUI.Button(new Rect(120f, 100f, 80f, 20f), "mobspawn"))
            {
                spawnermob = !spawnermob;
            }
            if (spawnermob)
            {
               
            }

            if (esp)
            {

                if (chest_esp)
                {
                    foreach (Chest current_chest in UnityEngine.Object.FindObjectsOfType(typeof(Chest)) as Chest[])
                    {
                        Vector3 enemyBottom = current_chest.transform.position;
                        Vector3 enemyTop;
                        enemyTop.x = enemyBottom.x;
                        enemyTop.z = enemyBottom.z;
                        enemyTop.y = enemyBottom.y + 2f;
                        Vector3 worldToScreenBottom = Camera.main.WorldToScreenPoint(enemyBottom);
                        Vector3 worldToScreenTop = Camera.main.WorldToScreenPoint(enemyTop);

                       
                        Vector2 size = Camera.main.WorldToScreenPoint(enemyBottom);

                        float height = Mathf.Abs(enemyTop.y - enemyBottom.y);

                        if (worldToScreenBottom.z > 0f)
                        {

                            if (chest_esp)
                            {
                                ExtRender.DrawText("chest", worldToScreenTop.x + 30, (float)Screen.height - worldToScreenBottom.y, Color.blue, 0.07f);


                            }



                        }
                        
                    }
                }

                foreach (OnlinePlayer player in UnityEngine.Object.FindObjectsOfType(typeof(OnlinePlayer)) as OnlinePlayer[])
                {

                 
                    Vector3 pivotPos = player.transform.position; //Pivot point NOT at the origin, at the center



                    Vector3 playerFootPos; playerFootPos.x = pivotPos.x; playerFootPos.z = pivotPos.z; playerFootPos.y = pivotPos.y - 2f; //At the feet
                    Vector3 playerHeadPos; playerHeadPos.x = pivotPos.x; playerHeadPos.z = pivotPos.z; playerHeadPos.y = pivotPos.y + 2f; //At the head
                    Vector3 w2s_footpos = Camera.main.WorldToScreenPoint(playerFootPos);
                    Vector3 w2s_headpos = Camera.main.WorldToScreenPoint(playerHeadPos);
                    float height = w2s_headpos.y - w2s_footpos.y;
                    float widthOffset = 2f;
                    float width = height / widthOffset;

                    if (w2s_footpos.z > 0f)
                    {

                        if (Box_esp)
                        {
                         //   DrawBoxESP(w2s_footpos, w2s_headpos, Color.green);
                            ExtRender.DrawBox(w2s_headpos.x - (width / 2), (float)Screen.height - w2s_footpos.y - height, width, height, Color.green, 2f);

                            ExtRender.DrawText(player.name.Replace("(Clone)", "") + "\n" + " Health " + player.hpRatio + "\n" + " Weapon " + player.currentWeaponId, w2s_headpos.x, (float)Screen.height - w2s_footpos.y, Color.blue, 0.07f);

                        }



                    }
                }
                foreach (Mob Mob in UnityEngine.Object.FindObjectsOfType(typeof(Mob)) as Mob[])
                {

                    Vector3 enemyBottom = Mob.transform.position;
                    Vector3 enemyTop;
                    enemyTop.x = enemyBottom.x;
                    enemyTop.z = enemyBottom.z;
                    enemyTop.y = enemyBottom.y + 2f;
                    Vector3 worldToScreenBottom = Camera.main.WorldToScreenPoint(enemyBottom);
                    Vector3 worldToScreenTop = Camera.main.WorldToScreenPoint(enemyTop);


                    Vector2 size = Camera.main.WorldToScreenPoint(enemyBottom);

                    float height = Mathf.Abs(enemyTop.y - enemyBottom.y);

                    if (worldToScreenBottom.z > 0f)
                    {

                        if (Box_esp)
                        {
                          //  ExtRender.DrawBox(w2s_footpos.x - (width / 2), (float)Screen.height - w2s_footpos.y - height, width, height, Color.red, 2f);


                            ExtRender.DrawText(Mob.name.Replace("(Clone)", "") + "\n", worldToScreenTop.x, (float)Screen.height - worldToScreenBottom.y, Color.cyan, 0.07f);

                        }



                    }
                }
            }
        }
      
        public void Start()
        {
            Statuses.Init(this);
            playerHack = FindObjectOfType<PlayerStatus>();
            mobHack = FindObjectOfType<MobSpawner>();
        }
        public void SpawnMob()
        {
            mobHack.SpawnMob(playerHack.transform.position, mobIndex, mob, damageMultiplier, damageMultiplier);
        }
        public void addToIndexMob()
        {
            mob++;
        }
        public void FixedUpdate()
        {
            if (flyhack)
            {
                if (!Input.GetKey(InputManager.jump) || !Input.GetKey(InputManager.forward) || !Input.GetKey(InputManager.backwards) || !Input.GetKey(InputManager.left) || !Input.GetKey(InputManager.right))
                {
                    Rigidbody rb1 = PlayerMovement.Instance.GetRb();
                    rb1.AddForce(Vector3.up * 65);
                }
            }
        }
        public void Update()
        {
            if (flyhack)
            {
                PlayerMovement.Instance.GetRb().velocity = new Vector3(0f, 0f, 0f);
                float speed = Input.GetKey(KeyCode.LeftControl) ? 0.5f : (Input.GetKey(InputManager.sprint) ? 1f : 0.5f);
                if (Input.GetKey(InputManager.jump))
                {
                    PlayerStatus.Instance.transform.position = new Vector3(PlayerStatus.Instance.transform.position.x, PlayerStatus.Instance.transform.position.y + speed, PlayerStatus.Instance.transform.position.z);
                }
                Vector3 playerTransformPosVec = PlayerStatus.Instance.transform.position;
                if (Input.GetKey(InputManager.forward))
                {
                    PlayerStatus.Instance.transform.position = new Vector3(playerTransformPosVec.x + Camera.main.transform.forward.x * Camera.main.transform.up.y * speed, playerTransformPosVec.y + Camera.main.transform.forward.y * speed, playerTransformPosVec.z + Camera.main.transform.forward.z * Camera.main.transform.up.y * speed);
                }
                if (Input.GetKey(InputManager.backwards))
                {
                    PlayerStatus.Instance.transform.position = new Vector3(playerTransformPosVec.x - Camera.main.transform.forward.x * Camera.main.transform.up.y * speed, playerTransformPosVec.y - Camera.main.transform.forward.y * speed, playerTransformPosVec.z - Camera.main.transform.forward.z * Camera.main.transform.up.y * speed);
                }
                if (Input.GetKey(InputManager.right))
                {
                    PlayerStatus.Instance.transform.position = new Vector3(playerTransformPosVec.x + Camera.main.transform.right.x * speed, playerTransformPosVec.y, playerTransformPosVec.z + Camera.main.transform.right.z * speed);
                }
                if (Input.GetKey(InputManager.left))
                {
                    PlayerStatus.Instance.transform.position = new Vector3(playerTransformPosVec.x - Camera.main.transform.right.x * speed, playerTransformPosVec.y, playerTransformPosVec.z - Camera.main.transform.right.z * speed);
                }
            }

            foreach (PlayerStatus Status in Statuses.Objects)
            {
                if (GodMode)
                    Status.hp = Status.maxHp;
                if (InfiniteStamina)
                    Status.stamina = Status.maxStamina;
                if (InfiniteFood)
                    Status.hunger = Status.maxHunger;
            }

            
           
        }

    }
        }

