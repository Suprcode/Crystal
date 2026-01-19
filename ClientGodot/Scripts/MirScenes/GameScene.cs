using System.Collections.Generic;
using System.Drawing; // Add
using Godot;
using ClientPackets;
using ServerPackets;
using ClientGodot.Scripts.MirObjects;
using ClientGodot.Scripts.MirControls;

namespace ClientGodot.Scripts.MirScenes
{
    public partial class GameScene : MirScene
    {
        public static GameScene Scene;

        public MapControl MapControl;
        public UserObject User;

        // Windows
        public InventoryWindow InventoryWin;
        public NPCWindow NPCWin;

        public override void _Ready()
        {
            Scene = this;

            // Setup Map Control
            MapControl = new MapControl();
            AddChild(MapControl);

            // Setup UI
            var hudRes = GD.Load<PackedScene>("res://Scenes/HUD.tscn");
            if (hudRes != null)
            {
                var hud = hudRes.Instantiate<HUD>();
                AddChild(hud);
            }

            // Inventory Window
            var invRes = GD.Load<PackedScene>("res://Scenes/Windows/InventoryWindow.tscn");
            if (invRes != null)
            {
                InventoryWin = invRes.Instantiate<InventoryWindow>();
                InventoryWin.Position = new Vector2(400, 100);
                InventoryWin.Visible = false;
                AddChild(InventoryWin);
            }

            // NPC Window
            var npcRes = GD.Load<PackedScene>("res://Scenes/Windows/NPCWindow.tscn");
            if (npcRes != null)
            {
                NPCWin = npcRes.Instantiate<NPCWindow>();
                NPCWin.Position = new Vector2(200, 100);
                NPCWin.Visible = false;
                AddChild(NPCWin);
            }
        }

        public override void Process()
        {
            MapControl?.Process();
            User?.Process();

            if (InventoryWin != null && InventoryWin.Visible)
                InventoryWin.Process();
        }

        public override void _Input(InputEvent @event)
        {
            // Toggle Inventory
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.F9)
            {
                if (InventoryWin != null) InventoryWin.Visible = !InventoryWin.Visible;
            }

            if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
            {
                if (MapControl == null || User == null) return;
                Vector2 mousePos = mouseEvent.Position;
                Point gridPos = MapControl.GetMapLocation(mousePos);

                if (mouseEvent.ButtonIndex == MouseButton.Right)
                {
                    // Right Click -> Move
                    GD.Print($"Moving to {gridPos}");
                    User.MoveTo(gridPos);
                }
                else if (mouseEvent.ButtonIndex == MouseButton.Left)
                {
                    // Left Click -> Target or Attack
                    var target = MapControl.GetObjectAt(gridPos);
                    if (target != null)
                    {
                        GD.Print($"Target: {target.Name}");

                        if (target is ItemObject item)
                        {
                            // If close, pick up
                            if (ClientGodot.Scripts.MirGraphics.Functions.InRange(User.CurrentLocation, item.CurrentLocation, 0))
                            {
                                NetworkManager.Enqueue(new ClientPackets.PickUp());
                            }
                            else
                            {
                                // Move to item
                                User.MoveTo(item.CurrentLocation);
                            }
                        }
                        else if (target is NPCObject npc)
                        {
                            // Move to NPC then Call
                            // User.MoveTo(npc.CurrentLocation);
                            // Simple: Just call immediately if close
                            NetworkManager.Enqueue(new ClientPackets.CallNPC { ObjectID = npc.ObjectID, Key = "[@Main]" });
                            if (NPCWin != null) NPCWin.NPCID = npc.ObjectID;
                        }
                        else
                        {
                            User.Attack(target);
                        }
                    }
                    else
                    {
                        // Shift+Click or just attack direction?
                        // For now simple directional attack towards mouse
                        User.Attack(null, gridPos);
                    }
                }
            }
        }

        public override void ProcessPacket(Packet p)
        {
            switch (p)
            {
                case ServerPackets.MapInformation mapInfo:
                    GD.Print($"Loading Map: {mapInfo.FileName} ({mapInfo.Title})");
                    MapControl.LoadMap(mapInfo.FileName);
                    break;

                case ServerPackets.UserInformation userInfo:
                    GD.Print($"User Info: {userInfo.Name} Loc: {userInfo.Location}");
                    User = new UserObject(userInfo.ObjectID)
                    {
                        Name = userInfo.Name,
                        CurrentLocation = userInfo.Location,
                        Direction = userInfo.Direction,
                        Class = userInfo.Class,
                        Gender = userInfo.Gender,
                        Hair = userInfo.Hair,
                        // Weapon = userInfo.Weapon, // The error says UserInformation doesn't have Weapon/Armour?
                        // Checking ServerPackets.cs: UserInformation (line 627) has:
                        // public short Weapon, WeaponEffect, Armour;
                        // Wait, looking at UserInformation packet...
                        // Ah, ServerPackets.UserInformation (class UserInformation)
                        // has NO Weapon/Armour fields directly?
                        // Let's re-read the file carefully.
                        // Line 627: public class ObjectPlayer has Weapon/Armour.
                        // Line 502: public class UserInformation has:
                        // UserItem[] Inventory, Equipment...
                        // It does NOT have Weapon/Armour fields. It has Equipment array!
                        // We need to parse Equipment array to find Weapon/Armour.
                    };

                    // Parse Equipment
                    if (userInfo.Equipment != null)
                    {
                        // Assuming Equipment Slots:
                        // Weapon = 1, Armour = 0? Need Enums.
                        // Standard Mir2 Enums:
                        // EquipmentSlot: Armour=0, Weapon=1...
                        // Let's look up EquipmentSlot in Shared/Enums.cs if possible, or guess.
                        // Since we can't see Enums.cs right now, let's look at `Shared` project structure again or safely assume indices.
                        // Or better: Iterate.
                        // Actually, let's just comment them out or set to 0 for now to fix build.
                        // We will fix equipment visuals later when we implement Inventory.

                        // User.Weapon = ...
                        // User.Armour = ...

                        // Copy Inventory Data
                        if (userInfo.Inventory != null)
                        {
                            for(int i = 0; i < userInfo.Inventory.Length; i++)
                            {
                                if (i < User.Inventory.Length)
                                    User.Inventory[i] = userInfo.Inventory[i];
                            }
                        }
                    }

                    MapControl.SetUserLocation(userInfo.Location);
                    HUD.Instance?.UpdateBars(userInfo.HP, userInfo.HP, userInfo.MP, userInfo.MP); // Initial Set
                    break;

                case ServerPackets.ObjectWalk walk:
                    // Handle movement
                    if (User != null && walk.ObjectID == User.ObjectID)
                    {
                        // Server confirmed walk
                        MapControl.SetUserLocation(walk.Location);
                        HUD.Instance?.UpdateCoordinates(walk.Location.X + ":" + walk.Location.Y);
                    }
                    break;

                case ServerPackets.Chat chat:
                    HUD.Instance?.AddChatMessage(chat.Message, chat.Type);
                    break;

                case ServerPackets.ObjectMonster monster:
                    var mob = new MonsterObject(monster.ObjectID)
                    {
                        Name = monster.Name,
                        CurrentLocation = monster.Location,
                        Direction = monster.Direction,
                        Image = monster.Image
                    };
                    MapControl.AddObject(mob);
                    break;

                case ServerPackets.ObjectPlayer player:
                    var otherPlayer = new PlayerObject(player.ObjectID)
                    {
                        Name = player.Name,
                        CurrentLocation = player.Location,
                        Direction = player.Direction,
                        Class = player.Class,
                        Gender = player.Gender,
                        Hair = player.Hair,
                        Weapon = player.Weapon,
                        Armour = player.Armour
                    };
                    MapControl.AddObject(otherPlayer);
                    break;

                case ServerPackets.ObjectRemove remove:
                    MapControl.RemoveObject(remove.ObjectID);
                    break;

                case ServerPackets.ObjectItem item:
                    var itemObj = new ItemObject(item.ObjectID)
                    {
                        Name = item.Name,
                        CurrentLocation = item.Location,
                        ImageIndex = item.Image,
                        ItemName = item.Name
                    };
                    MapControl.AddObject(itemObj);
                    break;

                case ServerPackets.ObjectNPC npc:
                    var npcObj = new NPCObject(npc.ObjectID)
                    {
                        Name = npc.Name,
                        CurrentLocation = npc.Location,
                        ImageIndex = npc.Image,
                        Direction = npc.Direction
                    };
                    MapControl.AddObject(npcObj);
                    break;

                case ServerPackets.DamageIndicator damage:
                    var targetObj = MapControl.MapObjects.Find(x => x.ObjectID == damage.ObjectID);
                    if (targetObj != null)
                    {
                        MapControl.CreateDamageIndicator(damage.Damage, targetObj.CurrentLocation);
                    }
                    else if (User != null && damage.ObjectID == User.ObjectID)
                    {
                        MapControl.CreateDamageIndicator(damage.Damage, User.CurrentLocation);
                    }
                    break;

                case ServerPackets.ObjectStruck struck:
                    var struckObj = MapControl.MapObjects.Find(x => x.ObjectID == struck.ObjectID);
                    if (struckObj != null)
                    {
                        struckObj.Direction = struck.Direction;
                        struckObj.CurrentAction = MirAction.Struck;
                        struckObj.AnimationCount = 0;
                        struckObj.NextFrameTime = CMain.Time + 200;
                    }
                    break;

                case ServerPackets.ObjectDied died:
                    var diedObj = MapControl.MapObjects.Find(x => x.ObjectID == died.ObjectID);
                    if (diedObj != null)
                    {
                        diedObj.CurrentAction = MirAction.Die;
                        diedObj.Direction = died.Direction;
                        diedObj.AnimationCount = 0;
                        diedObj.Dead = true;
                    }
                    break;

                case ServerPackets.GainedItem gained:
                    if (User != null)
                    {
                        // Add to first empty slot or server sends slot?
                        // Standard: GainedItem sends the UserItem directly. We need to find a place.
                        // Actually server logic usually ensures we have space before sending.
                        // But wait, where does it go?
                        // ServerPackets.GainedItem (Line 1325) has public UserItem Item.
                        // We usually append it to User.Inventory.
                        for (int i = 0; i < User.Inventory.Length; i++)
                        {
                            if (User.Inventory[i] == null)
                            {
                                User.Inventory[i] = gained.Item;
                                break;
                            }
                        }
                        HUD.Instance?.AddChatMessage($"Gained Item: {gained.Item.Info.Name}", ChatType.System);
                    }
                    break;

                case ServerPackets.NPCResponse npcRes:
                    if (NPCWin != null)
                    {
                        NPCWin.Visible = true;
                        NPCWin.UpdateText(npcRes.Page);
                        // We also need to set the NPCID on the window so clicks work
                        // But NPCResponse doesn't contain the NPCID?
                        // It assumes context.
                        // The original client tracks "CurrentNPC".
                        // For simplicity, we just assume the last clicked NPC is the active one?
                        // Or we can find the closest NPC.

                        // Let's assume we clicked the NPC recently.
                        // Refactor: Store LastClickedNPC in GameScene.
                    }
                    break;
            }
        }
    }

    // Temporary placeholder for UserObject until fully implemented
    public class UserObject : PlayerObject
    {
        public UserItem[] Inventory = new UserItem[46];
        public UserItem[] Equipment = new UserItem[14];

        public UserObject(uint id) : base(id) { }
    }
}
