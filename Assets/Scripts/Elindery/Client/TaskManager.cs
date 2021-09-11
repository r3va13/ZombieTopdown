using System.Collections;
using System.Collections.Generic;
using Elindery.Game;
using Elindery.Internals;
using Elindery.WindowControllers;
using UnityEngine;

namespace Elindery.Client
{
    internal class TaskManager
    {
        public TaskManager()
        {
            Client.OnRecieve += OnRecieve;
        }

        static void OnRecieve(object sender, string message)
        {
            Debug.Log(message);
            
            string[] args = message.Split('|');

            switch (args[0])
            {
                case "connected_to_lobby":
                    if (Storage.ClientID == "") Client.Send("create_new_player");
                    LogInController.Instance.Hide();
                    RoomController.Instance.Show();
                    break;
                case "id":
                    Storage.ChangeCliendID(args[1]);
                    break;
                case "join_room":
                    RoomController.Instance.Hide();
                    RoomLobbyController.Instance.Show();
                    Client.ConnectToRoomID(args[1]);
                    break;
                case "join_lobby":
                    Client.ConnectToRoomID("/Lobby");
                    break;
                case "connected_to_room":
                    Client.Send("join_room|" + Client.RoomID + "|" + Storage.ClientID);
                    break;
                case "refresh_room":
                    RoomLobbyController.Instance.RefreshRoomUsers(args);
                    break;
                case "join_game":
                    RoomLobbyController.Instance.StartCountdown(args);
                    break;
                case "create_survivor":
                    GameController.Instance.CreateSurvivor(args);
                    break;
                case "set_weapon":
                    SurvivorsController.Instance.SetWeapon(args);
                    break;
                case "set_survivor_config":
                    SurvivorPlayerController.SetSurvivorConfig(args);
                    break;
                case "wait_players":
                    GameController.Instance.WaitPlayers(args);
                    break;
                case "game_start":
                    GameController.Instance.GameStart(args);
                    break;
                case "user_states":
                    SurvivorsController.Instance.UserStates(args);
                    break;
                case "zombie_states":
                    EnemiesController.Instance.SetZombieStates(args);
                    break;
                case "player_shoot":
                    SurvivorsController.Instance.UserShoot(args);
                    break;
                case "zombie_status":
                    EnemiesController.Instance.SetZombieStatus(args);
                    break;
                case "survivor_damage":
                    SurvivorsController.Instance.SurvivorDamage(args);
                    break;
                case "survivor_ammo":
                    SurvivorsController.Instance.SurvivorAmmo(args);
                    break;
                case "container_positions":
                    ContainerController.Instance.SetContainers(args);
                    break;
                case "container_states":
                    ContainerController.Instance.SetContainerStates(args);
                    break;
                case "container_opened":
                    ContainerController.Instance.SetContainerOpened(args);
                    break;
                case "item_created":
                    ItemsController.Instance.CreateItem(args);
                    break;
                case "item_taken":
                    ItemsController.Instance.TakeItem(args);
                    break;
            }
        }
    }
}

