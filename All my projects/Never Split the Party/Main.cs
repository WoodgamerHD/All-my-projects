
using HarmonyLib;
using Legend;
using Newtonsoft.Json;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.Networking;

namespace NeverSplittheParty
{

    class Main : MonoBehaviour
    {
    

      
       
        bool godmode = false;
       

       
        public static List<Enemy> EnemyBase = new List<Enemy>();
        public static List<Player> PlayerBase = new List<Player>();
        public static List<Pickup> Pickup = new List<Pickup>();
        public static List<CharacterEditorButton> CharacterEditorButton = new List<CharacterEditorButton>();
   
  
   
        float natNextUpdateTime;
      
        public static Camera cam;
   
       
     
        public static Color TestColor
        {
            get
            {
                return new Color(1f, 0f, 1f, 1f);
            }
        }
   
   
        public void OnGUI()
        {

            ESPUtils.DrawString(new Vector2(100, 400), 
                $"F1: Godmode: {godmode}" + "\n" +
                "F2: Spawn Items" + "\n" +
                "F3: Unlock All" + "\n" + 
                "F4: TP Items" + "\n" + 
                "F5: Kill All Projectile " + "\n" +
                "F6: Cheat Max" + "\n" +
                "F7: Treasure Room" + "\n" +
                "F8: Boss Room" + "\n" +
                "F9: Fake Player", TestColor, true, 20, FontStyle.Bold);

        }
      
        public void Start()
        {
        
        }
      
        public void Update()
        {

            try
            {
                if (godmode)
                {
                    try
                    {
                        foreach (Player player in PlayerBase)
                        {
                            if (player.isLocalPlayer)
                            {
                                player.Health.CallRpcHeal(float.MaxValue);
                                player.Health.AddShield(4);
                            }
                        }
                    }
                    catch (Exception e) { Console.ExecuteCommand(e.Message); }
                }
            }
            catch (Exception e) { Console.ExecuteCommand(e.Message); }
            if (Input.GetKeyDown(KeyCode.F1))
            {
                godmode = !godmode;

            }

            if(Input.GetKeyDown(KeyCode.F2))
            {
                foreach (Player player in PlayerBase)
                {
                    if (player.isLocalPlayer)
                    {
                        try
                        {
                            for (int i = 0; i < 167; i++)
                            {
                                player.CallRpcAddItem((ItemId)i, false);
                            }
                        }
                        catch (Exception e) { Console.ExecuteCommand(e.Message); }
                    }
                }


                 //       foreach (Enemy Enemy in EnemyBase)
               //   {
              //        Enemy.Health.CmdExplode(Enemy.transform.position, 999999);
               //   }
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                try
                {
                    for (int i = 0; i < 58; i++)
                    {
                        SaveData.Current.GrantUnlock((Unlock)i);
                        SaveData.Current.IsUnlocked((Unlock)i);

                    }
                    for (int i = 0; i < 10; i++)
                    {

                        SaveData.Current.SetLevel((CharacterClassType)i, 1000);
                        SaveData.Current.SetXp((CharacterClassType)i, 1000);
                    }
                }
                catch (Exception e)
                {
                    Console.ExecuteCommand(e.Message);
                }

                foreach (Player player in PlayerBase)
                {
                    try
                    {
                        if (player.isLocalPlayer)
                        {
                            player.ClassLevel = 1000;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.ExecuteCommand(e.Message);
                    }
                }


                    }
            if(Input.GetKeyDown(KeyCode.F4))
            {
                foreach (Pickup Pickup in Pickup)
                {
                    foreach (Player player in PlayerBase)
                    {
                        if (player.isLocalPlayer)
                        {
                            Pickup.transform.position = player.transform.position;
                        }
                    }
                }

                }
            if(Input.GetKeyDown(KeyCode.F5) && Player.Instance != null && Player.Instance.Room != null)
            {
                foreach (Projectile projectile in UnityEngine.Object.FindObjectsOfType<Projectile>())
                {
                    if ((projectile.Targets == TargetType.Players || projectile.Targets == TargetType.All) && Player.Instance.Room.ContainsPoint(projectile.transform.position))
                    {
                        projectile.ExplodeOnCollision = false;
                        projectile.SpawnOnDestroy = null;
                        UnityEngine.Object.Destroy(projectile.gameObject);
                    }
                }
                foreach (Enemy enemy in Player.Instance.Room.GetEnemies())
                {
                    Move[] componentsInChildren = enemy.GetComponentsInChildren<Move>();
                    for (int i = 0; i < componentsInChildren.Length; i++)
                    {
                        componentsInChildren[i].StopAllCoroutines();
                    }
                    enemy.IsExplodeOnDeath = false;
                    enemy.Health.Die(new Damage
                    {
                        Type = DamageType.Projectile
                    });
                    if (!enemy.Health.IsDestroyOnDie)
                    {
                        enemy.Health.FadeAndDestroy();
                    }
                }

            }
            if(Input.GetKeyDown(KeyCode.F6))
            {
                Player.Instance.Coins = 50;
                Player.Instance.Bombs = 50;
                Player.Instance.Keys = 50;
                Player.Instance.Health.NetworkMaxHealth = 8f;
                Player.Instance.Health.Health = 8f;
                Player.Instance.Health.Shield = 8f;
                Player.Instance.Damage = 30f;
                Player.Instance.AttackRate = 30f;
                Player.Instance.Speed = 30f;
                Player.Instance.BulletSpeed = 30f;
                Player.Instance.Range = 30f;
                Player.Instance.Roles = (Roles.Vitalist | Roles.Cartographer | Roles.Treasurer | Roles.Loremaster);
                MiniMap.Instance.UpdateMap(true, true);

            }
            if(Input.GetKeyDown(KeyCode.F7))
            {
                if (Player.Instance != null)
                {
                    Room room = (from r in Room.AllRooms
                                 where r.Type == RoomType.Treasure
                                 select r).RandomOrDefault<Room>();
                    if (room == null)
                    {
                        global::Console.Write("Unable to find room.");
                    }
                    Player.Instance.CallCmdGoto(room.gameObject, Direction.None, 0f);
                }

            }
            if(Input.GetKeyDown(KeyCode.F8))
            {
                if (Player.Instance != null)
                {
                    Room room = (from r in Room.AllRooms
                                 where r.Type == RoomType.Boss
                                 select r).RandomOrDefault<Room>();
                    if (room == null)
                    {
                        global::Console.Write("Unable to find spawn room.");
                    }
                    Player.Instance.CallCmdGoto(room.gameObject, Direction.None, 0f);
                }

            }
            if(Input.GetKeyDown(KeyCode.F9))
            {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(NetworkManager.singleton.playerPrefab, Vector3.zero, Quaternion.identity);
                Player component = gameObject.GetComponent<Player>();
                component.Visual.NetworkName = string.Format("WoodGamerHD {0}", Player.Players.Count);
                component.transform.position += new Vector3(0.5f, 0f) * (float)Player.Players.Count;
                NetworkServer.Spawn(gameObject);
                component.GotoSpawnRoom(null);


            }

            natNextUpdateTime += Time.deltaTime;

            if (natNextUpdateTime >= 0.1f)
            {


                EnemyBase = FindObjectsOfType<Enemy>().ToList();
                PlayerBase = FindObjectsOfType<Player>().ToList();
                Pickup = FindObjectsOfType<Pickup>().ToList();
            
            
              
       
                natNextUpdateTime = 0f;
            }
          
              

                   
                
            



            cam = Camera.main;

        }
    }
}

