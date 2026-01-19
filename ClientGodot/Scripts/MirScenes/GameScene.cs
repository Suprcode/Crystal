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
        public CharacterWindow CharWin;
        public MagicWindow MagicWin;
        public OptionWindow OptionWin;
        public MarketWindow MarketWin;

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

            // Windows Container (CanvasLayer maybe? For now simple children)
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

            // Sound Manager
            AddChild(new ClientGodot.Scripts.SoundManager());

            // Character Window
            var charRes = GD.Load<PackedScene>("res://Scenes/Windows/CharacterWindow.tscn");
            if (charRes != null)
            {
                CharWin = charRes.Instantiate<CharacterWindow>();
                CharWin.Position = new Vector2(100, 100);
                CharWin.Visible = false;
                AddChild(CharWin);
            }

            // Magic Window
            var magRes = GD.Load<PackedScene>("res://Scenes/Windows/MagicWindow.tscn");
            if (magRes != null)
            {
                MagicWin = magRes.Instantiate<MagicWindow>();
                MagicWin.Position = new Vector2(500, 100);
                MagicWin.Visible = false;
                AddChild(MagicWin);
            }

            // Option Window
            var optRes = GD.Load<PackedScene>("res://Scenes/Windows/OptionWindow.tscn");
            if (optRes != null)
            {
                OptionWin = optRes.Instantiate<OptionWindow>();
                OptionWin.Position = new Vector2(300, 150);
                OptionWin.Visible = false;
                AddChild(OptionWin);
            }

            // Market Window
            var marketRes = GD.Load<PackedScene>("res://Scenes/Windows/MarketWindow.tscn");
            if (marketRes != null)
            {
                MarketWin = marketRes.Instantiate<MarketWindow>();
                MarketWin.Position = new Vector2(200, 100);
                MarketWin.Visible = false;
                AddChild(MarketWin);
            }
        }

        public override void Process()
        {
            // WASD Movement
            // MirAction is in global namespace or ClientPackets? No, it's an Enum in Shared.
            // But we might have wrapped it or using MirObjects.MirAction?
            // In MapObject.cs we used 'public MirAction CurrentAction;'
            // If it's Shared.MirAction, we don't need MirObjects prefix if it's not there.
            // Let's check MapObject definition in previous steps.
            // It seems we used `public MirAction CurrentAction`.
            // If MirAction is in Shared, then just `MirAction`.

            if (User != null && !User.Dead && User.MovementQueue.Count == 0 && User.CurrentAction != MirAction.Attack1 && User.CurrentAction != MirAction.Spell)
            {
                // Check Inputs
                if (Input.IsKeyPressed(Key.W)) User.MoveTo(ClientGodot.Scripts.MirGraphics.Functions.PointMove(User.CurrentLocation, MirDirection.Up, 1));
                else if (Input.IsKeyPressed(Key.S)) User.MoveTo(ClientGodot.Scripts.MirGraphics.Functions.PointMove(User.CurrentLocation, MirDirection.Down, 1));
                else if (Input.IsKeyPressed(Key.A)) User.MoveTo(ClientGodot.Scripts.MirGraphics.Functions.PointMove(User.CurrentLocation, MirDirection.Left, 1));
                else if (Input.IsKeyPressed(Key.D)) User.MoveTo(ClientGodot.Scripts.MirGraphics.Functions.PointMove(User.CurrentLocation, MirDirection.Right, 1));

                // Diagonals?
                // E = UpRight, Q = UpLeft?
                // Traditional Mir uses mouse mainly, but modern WASD often ignores diagonals or uses 2 keys.
                // Godot Input.GetVector is better but we need Grid direction.
                // Simple 4-way for now.
            }

            MapControl?.Process();
            User?.Process();

            if (InventoryWin != null && InventoryWin.Visible)
                InventoryWin.Process();

            if (CharWin != null && CharWin.Visible)
                CharWin.Process();

            if (MagicWin != null && MagicWin.Visible)
                MagicWin.Process();

            // MarketWin logic is mostly event based
        }

        public void HandleItemClick(ItemCell cell)
        {
            // Determine context
            // In a real app, cells would know their GridType (Inventory/Equip) and Slot index.
            // We need to pass this info to ItemCell.
            // For now, let's search.

            if (User == null) return;

            // Search Inventory
            for(int i = 0; i < User.Inventory.Length; i++)
            {
                if (User.Inventory[i] == cell.Item)
                {
                    // Found in Inventory -> Equip
                    // To? Need to know item type.
                    // For now, send to default (0). Server handles slot logic?
                    // ClientPackets.EquipItem requires 'To' slot.
                    // We need ClientItemInfo to know type.
                    // UserItem.Info (ItemInfo) has Type.
                    // Simplified: Just try sending to slot 0 or assume server smarts?
                    // Protocol requires Slot.
                    // Let's blindly try slot 0 (Weapon) for test if we don't have ItemInfo data fully mapped.
                    // Actually UserItem has Info.

                    int slot = 0;
                    if (cell.Item.Info.Type == ItemType.Armour) slot = 1;
                    else if (cell.Item.Info.Type == ItemType.Helmet) slot = 2;
                    // ...

                    NetworkManager.Enqueue(new ClientPackets.EquipItem { UniqueID = cell.Item.UniqueID, To = slot });
                    return;
                }
            }

            // Search Equipment
            for(int i = 0; i < User.Equipment.Length; i++)
            {
                if (User.Equipment[i] == cell.Item)
                {
                    // Found in Equipment -> Remove
                    // To? Slot 0 of inventory?
                    NetworkManager.Enqueue(new ClientPackets.RemoveItem { UniqueID = cell.Item.UniqueID, To = 0 });
                    return;
                }
            }
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventKey key && key.Pressed)
            {
                if (key.Keycode == Key.Escape && OptionWin != null)
                    OptionWin.Visible = !OptionWin.Visible;

                if (key.Keycode == Key.F9 && InventoryWin != null)
                    InventoryWin.Visible = !InventoryWin.Visible;
                if (key.Keycode == Key.F10 && CharWin != null)
                    CharWin.Visible = !CharWin.Visible;
                if (key.Keycode == Key.F11 && MagicWin != null)
                    MagicWin.Visible = !MagicWin.Visible;

                // Market W
                if (key.Keycode == Key.W && MarketWin != null && !Input.IsKeyPressed(Key.Ctrl)) // Avoid conflict with WASD?
                {
                    // W is usually WASD forward. Market is usually @Market or button.
                    // Let's bind it to M or just handle via NPC button later.
                    // Or bind to 'P' (Place).
                }

                // Casting Keys F1-F8
                if (key.Keycode >= Key.F1 && key.Keycode <= Key.F8)
                {
                    int spellKey = (int)(key.Keycode - Key.F1) + 1;
                    User.CastSpell(spellKey);
                }
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
                    if (mapInfo.Music > 0)
                        SoundManager.Instance?.PlayMusic(mapInfo.Music.ToString());
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
                        // Standard Mir2 Equipment Slots:
                        // 0: Weapon
                        // 1: Armour
                        // 2: Helmet
                        // ...

                        for(int i = 0; i < userInfo.Equipment.Length; i++)
                        {
                            if (i < User.Equipment.Length)
                                User.Equipment[i] = userInfo.Equipment[i];
                        }

                        // Update Visuals
                        if (User.Equipment[0] != null) User.Weapon = User.Equipment[0].Info.Shape;
                        else User.Weapon = -1;

                        if (User.Equipment[1] != null) User.Armour = User.Equipment[1].Info.Shape;
                        // Else keep default armour (usually based on class/gender in constructor)

                        // Hair is usually not an item but a property, already set.
                    }

                    // Copy Magic Data
                    if (userInfo.Magics != null)
                    {
                        User.Magics.Clear();
                        foreach(var mag in userInfo.Magics)
                            User.Magics.Add(mag);
                    }

                    // Copy Inventory Data
                    if (userInfo.Inventory != null)
                    {
                        for(int i = 0; i < userInfo.Inventory.Length; i++)
                        {
                            if (i < User.Inventory.Length)
                                User.Inventory[i] = userInfo.Inventory[i];
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
                    }
                    break;

                case ServerPackets.MountUpdate mount:
                    var mountObj = MapControl.MapObjects.Find(x => x.ObjectID == mount.ObjectID);
                    if (mountObj is PlayerObject pObj)
                    {
                        pObj.MountType = mount.MountType;
                        pObj.RidingMount = mount.RidingMount;
                    }
                    else if (User != null && mount.ObjectID == User.ObjectID)
                    {
                        User.MountType = mount.MountType;
                        User.RidingMount = mount.RidingMount;
                    }
                    break;

                case ServerPackets.ObjectEffect effect:
                    var spellObj = new SpellObject(effect.ObjectID)
                    {
                        Effect = effect.Effect,
                        EffectType = effect.EffectType,
                        // Location is not in ObjectEffect packet?
                        // ObjectEffect is usually attached to an ObjectID?
                        // Ah, ServerPackets.ObjectEffect has ObjectID.
                        // We need to find the target object to attach to?
                        // Or if ObjectID is new, it's a standalone effect?
                        // Line 1699: public sealed class ObjectEffect ... uint ObjectID; ...
                        // If ObjectID matches an existing MapObject, it might be attached.
                        // If it's a standalone effect (like FireWall), it has its own ID and Location.
                        // Wait, ObjectEffect in ServerPackets.cs line 1705:
                        // ReadPacket: ObjectID, Effect, EffectType, Delay, Time.
                        // It does NOT have Location.
                        // So it must be attached to an existing object.
                    };

                    var targetForEffect = MapControl.MapObjects.Find(x => x.ObjectID == effect.ObjectID);
                    if (targetForEffect != null)
                    {
                        // Add spell object to map?
                        // Or add to target?
                        // MapControl manages objects. SpellObject inherits MapObject.
                        // We need to give it a location.
                        spellObj.CurrentLocation = targetForEffect.CurrentLocation;
                        MapControl.AddObject(spellObj);
                    }
                    else if (User != null && effect.ObjectID == User.ObjectID)
                    {
                        spellObj.CurrentLocation = User.CurrentLocation;
                        MapControl.AddObject(spellObj);
                    }
                    // Else: Effect on location? Maybe ObjectMagic packet handles location effects.
                    break;

                case ServerPackets.ObjectMagic magic:
                    // Projectiles or static ground effects
                    var magicObj = new SpellObject(magic.ObjectID)
                    {
                        CurrentLocation = magic.Location,
                        EffectType = (uint)magic.Spell // Simplified mapping
                    };
                    MapControl.AddObject(magicObj);
                    break;

                case ServerPackets.NPCMarket market:
                    if (MarketWin != null)
                    {
                        MarketWin.Visible = true;
                        MarketWin.UpdateList(market.Listings);
                    }
                    break;

                case ServerPackets.UserSlotsRefresh refresh:
                    // Full refresh of inventory/equipment
                    if (refresh.Inventory != null)
                    {
                        for (int i = 0; i < refresh.Inventory.Length; i++)
                            if (i < User.Inventory.Length) User.Inventory[i] = refresh.Inventory[i];
                    }
                    if (refresh.Equipment != null)
                    {
                        for (int i = 0; i < refresh.Equipment.Length; i++)
                            if (i < User.Equipment.Length) User.Equipment[i] = refresh.Equipment[i];

                        // Refresh Visuals
                        if (User.Equipment[0] != null) User.Weapon = User.Equipment[0].Info.Shape; else User.Weapon = -1;
                        if (User.Equipment[1] != null) User.Armour = User.Equipment[1].Info.Shape;
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
        public List<ClientMagic> Magics = new List<ClientMagic>();

        public UserObject(uint id) : base(id) { }
    }
}
