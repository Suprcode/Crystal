using System;
using System.Drawing;
using System.Windows.Forms;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirObjects;
using Client.MirScenes;
using Client.MirSounds;
using Client.MirScenes.Dialogs;
using C = ClientPackets;

namespace Client.MirControls
{
    public sealed class MirItemCell : MirImageControl
    {

        public UserItem Item
        {
            get
            {
                if (GridType == MirGridType.DropPanel)
                    return NPCDropDialog.TargetItem;

                if (GridType == MirGridType.TrustMerchant)
                    return TrustMerchantDialog.Selected != null ? TrustMerchantDialog.Selected.Listing.Item : null;

                if (GridType == MirGridType.Renting)
                    return ItemRentingDialog.RentalItem;

                if (GridType == MirGridType.GuestRenting)
                    return GuestItemRentingDialog.GuestLoanItem;

                if (ItemArray != null && _itemSlot >= 0 && _itemSlot < ItemArray.Length)
                    return ItemArray[_itemSlot];
                return null;
            }
            set
            {
                if (GridType == MirGridType.DropPanel)
                    NPCDropDialog.TargetItem = value;
                else if (GridType == MirGridType.Renting)
                    ItemRentingDialog.RentalItem = value;
                else if (GridType == MirGridType.GuestRenting)
                    GuestItemRentingDialog.GuestLoanItem = value;
                else if (ItemArray != null && _itemSlot >= 0 && _itemSlot < ItemArray.Length)
                    ItemArray[_itemSlot] = value;

                SetEffect();
                Redraw();
            }
        }

        public UserItem ShadowItem
        {
            get
            {
                if (GridType == MirGridType.Craft && _itemSlot >= 0 && _itemSlot < ItemArray.Length)
                    return CraftDialog.ShadowItems[_itemSlot];

                return null;
            }
        }

        public UserItem[] ItemArray
        {
            get
            {
                switch (GridType)
                {
                    case MirGridType.Inventory:
                        return MapObject.User.Inventory;
                    case MirGridType.Equipment:
                        return MapObject.User.Equipment;
                    case MirGridType.BuyBack:
                        //return BuyBackPanel.Goods;
                    case MirGridType.Storage:
                        return GameScene.Storage;
                    case MirGridType.Inspect:
                        return InspectDialog.Items;
                    case MirGridType.GuildStorage:
                        return GameScene.GuildStorage;
                    case MirGridType.Trade:
                        return GameScene.User.Trade;
                    case MirGridType.GuestTrade:
                        return GuestTradeDialog.GuestItems;
                    case MirGridType.Mount:
                        return MapObject.User.Equipment[(int)EquipmentSlot.Mount].Slots;
                    case MirGridType.Fishing:
                        return MapObject.User.Equipment[(int)EquipmentSlot.Weapon].Slots;
                    case MirGridType.QuestInventory:
                        return MapObject.User.QuestInventory;
                    case MirGridType.AwakenItem:
                        return NPCAwakeDialog.Items;
                    case MirGridType.Mail:
                        return MailComposeParcelDialog.Items;
                    case MirGridType.Refine:
                        return GameScene.Refine;
                    case MirGridType.Craft:
                        return CraftDialog.IngredientSlots;

                    default:
                        throw new NotImplementedException();
                }

            }
        }

        public override bool Border
        {
            get { return (GameScene.SelectedCell == this || MouseControl == this || Locked) && GridType != MirGridType.DropPanel && (GridType != MirGridType.Craft); }
        }

        private bool _locked;

        public bool Locked
        {
            get { return _locked; }
            set
            {
                if (_locked == value) return;
                _locked = value;
                Redraw();
            }
        }



        #region GridType

        private MirGridType _gridType;
        public event EventHandler GridTypeChanged;
        public MirGridType GridType
        {
            get { return _gridType; }
            set
            {
                if (_gridType == value) return;
                _gridType = value;
                OnGridTypeChanged();
            }
        }

        private void OnGridTypeChanged()
        {
            if (GridTypeChanged != null)
                GridTypeChanged.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region ItemSlot

        private int _itemSlot;
        public event EventHandler ItemSlotChanged;
        public int ItemSlot
        {
            get { return _itemSlot; }
            set
            {
                if (_itemSlot == value) return;
                _itemSlot = value;
                OnItemSlotChanged();
            }
        }

        private void OnItemSlotChanged()
        {
            if (ItemSlotChanged != null)
                ItemSlotChanged.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Count Label

        private MirLabel CountLabel { get; set; }

        #endregion

        public MirItemCell()
        {
            Size = new Size(36, 32);
            GridType = MirGridType.None;
            DrawImage = false;

            BorderColour = Color.Lime;

            BackColour = Color.FromArgb(255, 255, 125, 125);
            Opacity = 0.5F;
            DrawControlTexture = true;
            Library = Libraries.Items;

        }

        public void SetEffect()
        {
            //put effect stuff here??
        }


        public override void OnMouseClick(MouseEventArgs e)
        {
            if (Locked) return;

            if (GameScene.PickedUpGold || GridType == MirGridType.Inspect || GridType == MirGridType.TrustMerchant || GridType == MirGridType.QuestInventory) return;

            if(GameScene.SelectedCell == null && (GridType == MirGridType.Mail)) return;

            base.OnMouseClick(e);
            
            Redraw();

            switch (e.Button)
            {
                case MouseButtons.Right:
                    UseItem();
                    break;
                case MouseButtons.Left:
                    if (Item != null && GameScene.SelectedCell == null)
                        PlayItemSound();
                    if (CMain.Shift)
                    {
                        if (GridType == MirGridType.Inventory || GridType == MirGridType.Storage)
                        {
                            if (GameScene.SelectedCell == null && Item != null)
                            {
                                if (FreeSpace() == 0)
                                {
                                    GameScene.Scene.ChatDialog.ReceiveChat("No room to split stack.", ChatType.System);
                                    return;
                                }

                                if (Item.Count > 1)
                                {
                                    MirAmountBox amountBox = new MirAmountBox("Split Amount:", Item.Image, Item.Count - 1);

                                    amountBox.OKButton.Click += (o, a) =>
                                    {
                                        if (amountBox.Amount == 0 || amountBox.Amount >= Item.Count) return;
                                        Network.Enqueue(new C.SplitItem { Grid = GridType, UniqueID = Item.UniqueID, Count = amountBox.Amount });
                                        Locked = true;
                                    };

                                    amountBox.Show();
                                }
                            }
                        }
                    }
                    else MoveItem();
                    break;
            }
        }
        public override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (Locked) return;

            if (GameScene.PickedUpGold || GridType == MirGridType.Inspect || GridType == MirGridType.TrustMerchant || GridType == MirGridType.Craft) return;

            base.OnMouseClick(e);

            Redraw();

            GameScene.SelectedCell = null;
            UseItem();
        }


        private void BuyItem()
        {
            if (Item == null || Item.Price() * GameScene.NPCRate > GameScene.Gold) return;

            MirAmountBox amountBox;
            if (Item.Count > 1)
            {
                amountBox = new MirAmountBox("Purchase Amount:", Item.Image, Item.Count);

                amountBox.OKButton.Click += (o, e) =>
                {
                    Network.Enqueue(new C.BuyItemBack { UniqueID = Item.UniqueID, Count = amountBox.Amount });
                    Locked = true;
                };
            }
            else
            {
                amountBox = new MirAmountBox("Purchase", Item.Image, string.Format("Value: {0:#,##0} Gold", Item.Price()));

                amountBox.OKButton.Click += (o, e) =>
                {
                    Network.Enqueue(new C.BuyItemBack { UniqueID = Item.UniqueID, Count = 1 });
                    Locked = true;
                };
            }

            amountBox.Show();
        }
        
        public void UseItem()
        {
            if (Locked || GridType == MirGridType.Inspect || GridType == MirGridType.TrustMerchant || GridType == MirGridType.GuildStorage || GridType == MirGridType.Craft) return;

            if (MapObject.User.Fishing) return;
            if (MapObject.User.RidingMount && Item.Info.Type != ItemType.Scroll && Item.Info.Type != ItemType.Potion && Item.Info.Type != ItemType.Torch) return;

            if (GridType == MirGridType.BuyBack)
            {
                BuyItem();
                return;
            }

            if (GridType == MirGridType.Equipment || GridType == MirGridType.Mount || GridType == MirGridType.Fishing)
            {
                RemoveItem();
                return;
            }

            if ((GridType != MirGridType.Inventory && GridType != MirGridType.Storage) || Item == null || !CanUseItem() || GameScene.SelectedCell == this) return;

            CharacterDialog dialog = GameScene.Scene.CharacterDialog;

            if ((Item.SoulBoundId != -1)  && (MapObject.User.Id != Item.SoulBoundId))
                return;
            switch (Item.Info.Type)
            {
                case ItemType.Weapon:
                    if (dialog.Grid[(int)EquipmentSlot.Weapon].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)EquipmentSlot.Weapon });
                        dialog.Grid[(int)EquipmentSlot.Weapon].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Armour:
                    if (dialog.Grid[(int)EquipmentSlot.Armour].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)EquipmentSlot.Armour });
                        dialog.Grid[(int)EquipmentSlot.Armour].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Helmet:
                    if (dialog.Grid[(int)EquipmentSlot.Helmet].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)EquipmentSlot.Helmet });
                        dialog.Grid[(int)EquipmentSlot.Helmet].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Necklace:
                    if (dialog.Grid[(int)EquipmentSlot.Necklace].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)EquipmentSlot.Necklace });
                        dialog.Grid[(int)EquipmentSlot.Necklace].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Bracelet:
                    if ((dialog.Grid[(int)EquipmentSlot.BraceletR].Item == null || dialog.Grid[(int)EquipmentSlot.BraceletR].Item.Info.Type == ItemType.Amulet) && dialog.Grid[(int)EquipmentSlot.BraceletR].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)EquipmentSlot.BraceletR });
                        dialog.Grid[(int)EquipmentSlot.BraceletR].Locked = true;
                        Locked = true;
                    }
                    else if (dialog.Grid[(int)EquipmentSlot.BraceletL].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)EquipmentSlot.BraceletL });
                        dialog.Grid[(int)EquipmentSlot.BraceletL].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Ring:
                    if (dialog.Grid[(int)EquipmentSlot.RingR].Item == null && dialog.Grid[(int)EquipmentSlot.RingR].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)EquipmentSlot.RingR });
                        dialog.Grid[(int)EquipmentSlot.RingR].Locked = true;
                        Locked = true;
                    }
                    else if (dialog.Grid[(int)EquipmentSlot.RingL].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)EquipmentSlot.RingL });
                        dialog.Grid[(int)EquipmentSlot.RingL].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Amulet:
                    //if (Item.Info.Shape == 0) return;

                    if (dialog.Grid[(int)EquipmentSlot.Amulet].Item != null && Item.Info.Type == ItemType.Amulet)
                    {
                        if (dialog.Grid[(int)EquipmentSlot.Amulet].Item.Info == Item.Info && dialog.Grid[(int)EquipmentSlot.Amulet].Item.Count < dialog.Grid[(int)EquipmentSlot.Amulet].Item.Info.StackSize)
                        {
                            Network.Enqueue(new C.MergeItem { GridFrom = GridType, GridTo = MirGridType.Equipment, IDFrom = Item.UniqueID, IDTo = dialog.Grid[(int)EquipmentSlot.Amulet].Item.UniqueID });

                            Locked = true;
                            return;
                        }
                    }

                    if (dialog.Grid[(int)EquipmentSlot.Amulet].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)EquipmentSlot.Amulet });
                        dialog.Grid[(int)EquipmentSlot.Amulet].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Belt:
                    if (dialog.Grid[(int)EquipmentSlot.Belt].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)EquipmentSlot.Belt });
                        dialog.Grid[(int)EquipmentSlot.Belt].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Boots:
                    if (dialog.Grid[(int)EquipmentSlot.Boots].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)EquipmentSlot.Boots });
                        dialog.Grid[(int)EquipmentSlot.Boots].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Stone:
                    if (dialog.Grid[(int)EquipmentSlot.Stone].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)EquipmentSlot.Stone });
                        dialog.Grid[(int)EquipmentSlot.Stone].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Torch:
                    if (dialog.Grid[(int)EquipmentSlot.Torch].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)EquipmentSlot.Torch });
                        dialog.Grid[(int)EquipmentSlot.Torch].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Potion:
                case ItemType.Scroll:
                case ItemType.Book:
                case ItemType.Food:
                case ItemType.Script:
                case ItemType.Pets:
                case ItemType.Transform:
                    if (CanUseItem() && GridType == MirGridType.Inventory)
                    {
                        if (CMain.Time < GameScene.UseItemTime) return;
                        Network.Enqueue(new C.UseItem { UniqueID = Item.UniqueID });

                        if (Item.Count == 1 && ItemSlot < 6)
                        {
                            for (int i = GameScene.User.BeltIdx; i < GameScene.User.Inventory.Length; i++)
                                if (ItemArray[i] != null && ItemArray[i].Info == Item.Info)
                                {
                                    Network.Enqueue(new C.MoveItem { Grid = MirGridType.Inventory, From = i, To = ItemSlot });
                                    GameScene.Scene.InventoryDialog.Grid[i - GameScene.User.BeltIdx].Locked = true;
                                    break;
                                }
                        }

                        Locked = true;
                    }
                    break;
                case ItemType.Mount:
                    if (dialog.Grid[(int)EquipmentSlot.Mount].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)EquipmentSlot.Mount });
                        dialog.Grid[(int)EquipmentSlot.Mount].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Reins:
                case ItemType.Bells:
                case ItemType.Ribbon:
                case ItemType.Saddle:
                case ItemType.Mask:
                case ItemType.Hook:
                case ItemType.Float:
                case ItemType.Bait:
                case ItemType.Finder:
                case ItemType.Reel:
                    UseSlotItem();
                    break;
            }

            GameScene.UseItemTime = CMain.Time + 300;
            PlayItemSound();
        }
        public void UseSlotItem()
        {
            MountDialog mountDialog = null;
            FishingDialog fishingDialog = null;

            if (!CanUseItem()) return;

            switch (Item.Info.Type)
            {
                case ItemType.Reins:
                    mountDialog = GameScene.Scene.MountDialog;
                    if (mountDialog.Grid[(int)MountSlot.Reins].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipSlotItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)MountSlot.Reins, GridTo = MirGridType.Mount });
                        mountDialog.Grid[(int)MountSlot.Reins].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Bells:
                    mountDialog = GameScene.Scene.MountDialog;
                    if (mountDialog.Grid[(int)MountSlot.Bells].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipSlotItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)MountSlot.Bells, GridTo = MirGridType.Mount });
                        mountDialog.Grid[(int)MountSlot.Bells].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Ribbon:
                    mountDialog = GameScene.Scene.MountDialog;
                    if (mountDialog.Grid[(int)MountSlot.Ribbon].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipSlotItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)MountSlot.Ribbon, GridTo = MirGridType.Mount });
                        mountDialog.Grid[(int)MountSlot.Ribbon].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Saddle:
                    mountDialog = GameScene.Scene.MountDialog;
                    if (mountDialog.Grid[(int)MountSlot.Saddle].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipSlotItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)MountSlot.Saddle, GridTo = MirGridType.Mount });
                        mountDialog.Grid[(int)MountSlot.Saddle].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Mask:
                    mountDialog = GameScene.Scene.MountDialog;
                    if (mountDialog.Grid[(int)MountSlot.Mask].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipSlotItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)MountSlot.Mask, GridTo = MirGridType.Mount });
                        mountDialog.Grid[(int)MountSlot.Mask].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Hook:
                    fishingDialog = GameScene.Scene.FishingDialog;
                    if (fishingDialog.Grid[(int)FishingSlot.Hook].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipSlotItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)FishingSlot.Hook, GridTo = MirGridType.Fishing });
                        fishingDialog.Grid[(int)FishingSlot.Hook].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Float:
                    fishingDialog = GameScene.Scene.FishingDialog;
                    if (fishingDialog.Grid[(int)FishingSlot.Float].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipSlotItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)FishingSlot.Float, GridTo = MirGridType.Fishing });
                        fishingDialog.Grid[(int)FishingSlot.Float].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Bait:
                    fishingDialog = GameScene.Scene.FishingDialog;

                    if (fishingDialog.Grid[(int)FishingSlot.Bait].Item != null && Item.Info.Type == ItemType.Bait)
                    {
                        if (fishingDialog.Grid[(int)FishingSlot.Bait].Item.Info == Item.Info && fishingDialog.Grid[(int)FishingSlot.Bait].Item.Count < fishingDialog.Grid[(int)FishingSlot.Bait].Item.Info.StackSize)
                        {
                            Network.Enqueue(new C.MergeItem { GridFrom = GridType, GridTo = MirGridType.Fishing, IDFrom = Item.UniqueID, IDTo = fishingDialog.Grid[(int)FishingSlot.Bait].Item.UniqueID });

                            Locked = true;
                            GameScene.SelectedCell.Locked = true;
                            GameScene.SelectedCell = null;
                            return;
                        }
                    }

                    if (fishingDialog.Grid[(int)FishingSlot.Bait].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipSlotItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)FishingSlot.Bait, GridTo = MirGridType.Fishing });
                        fishingDialog.Grid[(int)FishingSlot.Bait].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Finder:
                    fishingDialog = GameScene.Scene.FishingDialog;
                    if (fishingDialog.Grid[(int)FishingSlot.Finder].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipSlotItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)FishingSlot.Finder, GridTo = MirGridType.Fishing });
                        fishingDialog.Grid[(int)FishingSlot.Finder].Locked = true;
                        Locked = true;
                    }
                    break;
                case ItemType.Reel:
                    fishingDialog = GameScene.Scene.FishingDialog;
                    if (fishingDialog.Grid[(int)FishingSlot.Reel].CanWearItem(Item))
                    {
                        Network.Enqueue(new C.EquipSlotItem { Grid = GridType, UniqueID = Item.UniqueID, To = (int)FishingSlot.Reel, GridTo = MirGridType.Fishing });
                        fishingDialog.Grid[(int)FishingSlot.Reel].Locked = true;
                        Locked = true;
                    }
                    break;
            }
        }
        public void RemoveItem()
        {
            int count = 0;

            for (int i = 0; i < GameScene.User.Inventory.Length; i++)
            {
                MirItemCell itemCell = i < GameScene.User.BeltIdx ? GameScene.Scene.BeltDialog.Grid[i] : GameScene.Scene.InventoryDialog.Grid[i - GameScene.User.BeltIdx];

                if (itemCell.Item == null) count++;
            }

            if (Item == null || count < 1 || (MapObject.User.RidingMount && Item.Info.Type != ItemType.Torch)) return;

            if (Item.Info.StackSize > 1)
            {
                UserItem item = null;

                for (int i = 0; i < GameScene.User.Inventory.Length; i++)
                {
                    MirItemCell itemCell = i < GameScene.User.BeltIdx ? GameScene.Scene.BeltDialog.Grid[i] : GameScene.Scene.InventoryDialog.Grid[i - GameScene.User.BeltIdx];

                    if (itemCell.Item == null || itemCell.Item.Info != Item.Info) continue;

                    item = itemCell.Item;
                }

                if (item != null && ((item.Count + Item.Count) <= item.Info.StackSize))
                {
                    //Merge.
                    Network.Enqueue(new C.MergeItem { GridFrom = GridType, GridTo = MirGridType.Inventory, IDFrom = Item.UniqueID, IDTo = item.UniqueID });

                    Locked = true;

                    PlayItemSound();
                    return;
                }
            }


            for (int i = 0; i < GameScene.User.Inventory.Length; i++)
            {
                MirItemCell itemCell = null;

                if (Item.Info.Type == ItemType.Amulet)
                {
                    itemCell = i < GameScene.User.BeltIdx ? GameScene.Scene.BeltDialog.Grid[i] : GameScene.Scene.InventoryDialog.Grid[i - GameScene.User.BeltIdx];
                }
                else
                {
                    itemCell = i < (GameScene.User.Inventory.Length - GameScene.User.BeltIdx) ? GameScene.Scene.InventoryDialog.Grid[i] : GameScene.Scene.BeltDialog.Grid[i - GameScene.User.Inventory.Length];
                }

                if (itemCell.Item != null) continue;

                if (GridType != MirGridType.Equipment)
                {
                    Network.Enqueue(new C.RemoveSlotItem { Grid = GridType, UniqueID = Item.UniqueID, To = itemCell.ItemSlot, GridTo = MirGridType.Inventory });
                }
                else
                {
                    Network.Enqueue(new C.RemoveItem { Grid = MirGridType.Inventory, UniqueID = Item.UniqueID, To = itemCell.ItemSlot });
                }

                Locked = true;

                PlayItemSound();
                break;
            }

        }

        private void MoveItem()
        {
            if (GridType == MirGridType.BuyBack || GridType == MirGridType.DropPanel || GridType == MirGridType.Inspect || GridType == MirGridType.TrustMerchant || GridType == MirGridType.Craft) return;

            if (GameScene.SelectedCell != null)
            {
                if (GameScene.SelectedCell.Item == null || GameScene.SelectedCell == this)
                {
                    GameScene.SelectedCell = null;
                    return;
                }

                switch (GridType)
                {
                    #region To Inventory
                    case MirGridType.Inventory: // To Inventory
                        switch (GameScene.SelectedCell.GridType)
                        {
                            #region From Inventory
                            case MirGridType.Inventory: //From Invenotry
                                if (Item != null)
                                {
                                    if (CMain.Ctrl)
                                    {
                                        MirMessageBox messageBox = new MirMessageBox("Do you want to try and combine these items?", MirMessageBoxButtons.YesNo);
                                        messageBox.YesButton.Click += (o, e) =>
                                        {
                                            //Combine
                                            Network.Enqueue(new C.CombineItem { IDFrom = GameScene.SelectedCell.Item.UniqueID, IDTo = Item.UniqueID });
                                            Locked = true;
                                            GameScene.SelectedCell.Locked = true;
                                            GameScene.SelectedCell = null;
                                        };

                                        messageBox.Show();
                                        return;
                                    }

                                    if (GameScene.SelectedCell.Item.Info == Item.Info && Item.Count < Item.Info.StackSize)
                                    {
                                        //Merge
                                        Network.Enqueue(new C.MergeItem { GridFrom = GameScene.SelectedCell.GridType, GridTo = GridType, IDFrom = GameScene.SelectedCell.Item.UniqueID, IDTo = Item.UniqueID });

                                        Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                }

                                Network.Enqueue(new C.MoveItem { Grid = GridType, From = GameScene.SelectedCell.ItemSlot, To = ItemSlot });

                                Locked = true;
                                GameScene.SelectedCell.Locked = true;
                                GameScene.SelectedCell = null;
                                return;
                            #endregion
                            #region From Equipment
                            case MirGridType.Equipment: //From Equipment
                                if (Item != null && GameScene.SelectedCell.Item.Info.Type == ItemType.Amulet)
                                {
                                    if (GameScene.SelectedCell.Item.Info == Item.Info && Item.Count < Item.Info.StackSize)
                                    {
                                        Network.Enqueue(new C.MergeItem { GridFrom = GameScene.SelectedCell.GridType, GridTo = GridType, IDFrom = GameScene.SelectedCell.Item.UniqueID, IDTo = Item.UniqueID });

                                        Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                }

                                if (!CanRemoveItem(GameScene.SelectedCell.Item))
                                {
                                    GameScene.SelectedCell = null;
                                    return;
                                }
                                if (Item == null)
                                {
                                    Network.Enqueue(new C.RemoveItem { Grid = GridType, UniqueID = GameScene.SelectedCell.Item.UniqueID, To = ItemSlot });

                                    Locked = true;
                                    GameScene.SelectedCell.Locked = true;
                                    GameScene.SelectedCell = null;
                                    return;
                                }

                                for (int x = 6; x < ItemArray.Length; x++)
                                    if (ItemArray[x] == null)
                                    {
                                        Network.Enqueue(new C.RemoveItem { Grid = GridType, UniqueID = GameScene.SelectedCell.Item.UniqueID, To = x });

                                        MirItemCell temp = x < GameScene.User.BeltIdx ? GameScene.Scene.BeltDialog.Grid[x] : GameScene.Scene.InventoryDialog.Grid[x - GameScene.User.BeltIdx];

                                        if (temp != null) temp.Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                break;
                            #endregion
                            #region From Storage
                            case MirGridType.Storage: //From Storage
                                if (Item != null && GameScene.SelectedCell.Item.Info.Type == ItemType.Amulet)
                                {
                                    if (GameScene.SelectedCell.Item.Info == Item.Info && Item.Count < Item.Info.StackSize)
                                    {
                                        Network.Enqueue(new C.MergeItem { GridFrom = GameScene.SelectedCell.GridType, GridTo = GridType, IDFrom = GameScene.SelectedCell.Item.UniqueID, IDTo = Item.UniqueID });

                                        Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                }

                                if (GameScene.SelectedCell.Item.Weight + MapObject.User.CurrentBagWeight > MapObject.User.MaxBagWeight)
                                {
                                    GameScene.Scene.ChatDialog.ReceiveChat("Too heavy to get back.", ChatType.System);
                                    GameScene.SelectedCell = null;
                                    return;
                                }

                                if (Item != null)
                                {
                                    if (GameScene.SelectedCell.Item.Info == Item.Info && Item.Count < Item.Info.StackSize)
                                    {
                                        Network.Enqueue(new C.MergeItem { GridFrom = GameScene.SelectedCell.GridType, GridTo = GridType, IDFrom = GameScene.SelectedCell.Item.UniqueID, IDTo = Item.UniqueID });

                                        Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                }


                                if (Item == null)
                                {
                                    Network.Enqueue(new C.TakeBackItem { From = GameScene.SelectedCell.ItemSlot, To = ItemSlot });

                                    Locked = true;
                                    GameScene.SelectedCell.Locked = true;
                                    GameScene.SelectedCell = null;
                                    return;
                                }

                                for (int x = 6; x < ItemArray.Length; x++)
                                    if (ItemArray[x] == null)
                                    {
                                        Network.Enqueue(new C.TakeBackItem { From = GameScene.SelectedCell.ItemSlot, To = x });

                                        MirItemCell temp = x < GameScene.User.BeltIdx ? GameScene.Scene.BeltDialog.Grid[x] : GameScene.Scene.InventoryDialog.Grid[x - GameScene.User.BeltIdx];

                                        if (temp != null) temp.Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                break;
                            #endregion
                            #region From Guild Storage
                            case MirGridType.GuildStorage:
                                if (Item != null)
                                {
                                    GameScene.Scene.ChatDialog.ReceiveChat("You cannot swap items.", ChatType.System);
                                    return;
                                }
                                if (!GuildDialog.MyOptions.HasFlag(RankOptions.CanRetrieveItem))
                                {
                                    GameScene.Scene.ChatDialog.ReceiveChat("Insufficient rights to retrieve items.", ChatType.System);
                                    return;
                                }
                                Network.Enqueue(new C.GuildStorageItemChange { Type = 1, From = GameScene.SelectedCell.ItemSlot, To = ItemSlot });
                                Locked = true;
                                GameScene.SelectedCell.Locked = true;
                                GameScene.SelectedCell = null;
                                break;
                            #endregion
                            #region From Trade
                            case MirGridType.Trade: //From Trade
                                if (Item != null && GameScene.SelectedCell.Item.Info.Type == ItemType.Amulet)
                                {
                                    if (GameScene.SelectedCell.Item.Info == Item.Info && Item.Count < Item.Info.StackSize)
                                    {
                                        Network.Enqueue(new C.MergeItem { GridFrom = GameScene.SelectedCell.GridType, GridTo = GridType, IDFrom = GameScene.SelectedCell.Item.UniqueID, IDTo = Item.UniqueID });

                                        Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                }

                                if (GameScene.SelectedCell.Item.Weight + MapObject.User.CurrentBagWeight > MapObject.User.MaxBagWeight)
                                {
                                    GameScene.Scene.ChatDialog.ReceiveChat("Too heavy to get back.", ChatType.System);
                                    GameScene.SelectedCell = null;
                                    return;
                                }

                                if (Item != null)
                                {
                                    if (GameScene.SelectedCell.Item.Info == Item.Info && Item.Count < Item.Info.StackSize)
                                    {
                                        Network.Enqueue(new C.MergeItem { GridFrom = GameScene.SelectedCell.GridType, GridTo = GridType, IDFrom = GameScene.SelectedCell.Item.UniqueID, IDTo = Item.UniqueID });

                                        Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                }


                                if (Item == null)
                                {
                                    Network.Enqueue(new C.RetrieveTradeItem { From = GameScene.SelectedCell.ItemSlot, To = ItemSlot });

                                    Locked = true;
                                    GameScene.SelectedCell.Locked = true;
                                    GameScene.SelectedCell = null;
                                    return;
                                }

                                for (int x = 6; x < ItemArray.Length; x++)
                                    if (ItemArray[x] == null)
                                    {
                                        Network.Enqueue(new C.RetrieveTradeItem { From = GameScene.SelectedCell.ItemSlot, To = x });

                                        MirItemCell temp = x < GameScene.User.BeltIdx ? GameScene.Scene.BeltDialog.Grid[x] : GameScene.Scene.InventoryDialog.Grid[x - GameScene.User.BeltIdx];

                                        if (temp != null) temp.Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                break;
                            #endregion
                            #region From AwakenItem
                            case MirGridType.AwakenItem: //From AwakenItem
                                Network.Enqueue(new C.MoveItem { Grid = GridType, From = NPCAwakeDialog.ItemsIdx[GameScene.SelectedCell.ItemSlot], To = NPCAwakeDialog.ItemsIdx[GameScene.SelectedCell.ItemSlot] });
                                GameScene.SelectedCell.Locked = false;
                                GameScene.SelectedCell.Item = null;
                                NPCAwakeDialog.ItemsIdx[GameScene.SelectedCell.ItemSlot] = 0;

                                if (GameScene.SelectedCell.ItemSlot == 0)
                                    GameScene.Scene.NPCAwakeDialog.ItemCell_Click();
                                GameScene.SelectedCell = null;
                                break;
                             #endregion

                            #region From Refine
                            case MirGridType.Refine: //From AwakenItem
                                if (Item != null && GameScene.SelectedCell.Item.Info.Type == ItemType.Amulet)
                                {
                                    if (GameScene.SelectedCell.Item.Info == Item.Info && Item.Count < Item.Info.StackSize)
                                    {
                                        Network.Enqueue(new C.MergeItem { GridFrom = GameScene.SelectedCell.GridType, GridTo = GridType, IDFrom = GameScene.SelectedCell.Item.UniqueID, IDTo = Item.UniqueID });

                                        Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                }

                                if (GameScene.SelectedCell.Item.Weight + MapObject.User.CurrentBagWeight > MapObject.User.MaxBagWeight)
                                {
                                    GameScene.Scene.ChatDialog.ReceiveChat("Too heavy to get back.", ChatType.System);
                                    GameScene.SelectedCell = null;
                                    return;
                                }

                                if (Item != null)
                                {
                                    if (GameScene.SelectedCell.Item.Info == Item.Info && Item.Count < Item.Info.StackSize)
                                    {
                                        Network.Enqueue(new C.MergeItem { GridFrom = GameScene.SelectedCell.GridType, GridTo = GridType, IDFrom = GameScene.SelectedCell.Item.UniqueID, IDTo = Item.UniqueID });

                                        Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                }


                                if (Item == null)
                                {
                                    Network.Enqueue(new C.RetrieveRefineItem { From = GameScene.SelectedCell.ItemSlot, To = ItemSlot });

                                    Locked = true;
                                    GameScene.SelectedCell.Locked = true;
                                    GameScene.SelectedCell = null;
                                    return;
                                }

                                for (int x = 6; x < ItemArray.Length; x++)
                                    if (ItemArray[x] == null)
                                    {
                                        Network.Enqueue(new C.RetrieveRefineItem { From = GameScene.SelectedCell.ItemSlot, To = x });

                                        MirItemCell temp = x < GameScene.User.BeltIdx ? GameScene.Scene.BeltDialog.Grid[x] : GameScene.Scene.InventoryDialog.Grid[x - GameScene.User.BeltIdx];

                                        if (temp != null) temp.Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                break;
                            #endregion

                            #region From Item Renting Dialog

                            case MirGridType.Renting:
                                if (GameScene.User.RentalItemLocked)
                                {
                                    GameScene.Scene.ChatDialog.ReceiveChat("Unable to remove locked item, cancel item rental and try again.", ChatType.System);
                                    GameScene.SelectedCell = null;
                                    return;
                                }

                                if (GameScene.SelectedCell.Item.Weight + MapObject.User.CurrentBagWeight > MapObject.User.MaxBagWeight)
                                {
                                    GameScene.Scene.ChatDialog.ReceiveChat("Too heavy to get back.", ChatType.System);
                                    GameScene.SelectedCell = null;
                                    return;
                                }

                                if (Item == null)
                                {
                                    Network.Enqueue(new C.RetrieveRentalItem { From = GameScene.SelectedCell.ItemSlot, To = ItemSlot });

                                    Locked = true;
                                    GameScene.SelectedCell.Locked = true;
                                    GameScene.SelectedCell = null;
                                    return;
                                }

                                break;
                                #endregion
                        }
                        break;
                    #endregion
                    #region To Equipment
                    case MirGridType.Equipment: //To Equipment

                        if (GameScene.SelectedCell.GridType != MirGridType.Inventory && GameScene.SelectedCell.GridType != MirGridType.Storage) return;


                        if (Item != null && GameScene.SelectedCell.Item.Info.Type == ItemType.Amulet)
                        {
                            if (GameScene.SelectedCell.Item.Info == Item.Info && Item.Count < Item.Info.StackSize)
                            {
                                Network.Enqueue(new C.MergeItem { GridFrom = GameScene.SelectedCell.GridType, GridTo = GridType, IDFrom = GameScene.SelectedCell.Item.UniqueID, IDTo = Item.UniqueID });

                                Locked = true;
                                GameScene.SelectedCell.Locked = true;
                                GameScene.SelectedCell = null;
                                return;
                            }
                        }

                        if (CorrectSlot(GameScene.SelectedCell.Item))
                        {
                            if (CanWearItem(GameScene.SelectedCell.Item))
                            {
                                Network.Enqueue(new C.EquipItem { Grid = GameScene.SelectedCell.GridType, UniqueID = GameScene.SelectedCell.Item.UniqueID, To = ItemSlot });
                                Locked = true;
                                GameScene.SelectedCell.Locked = true;
                            }
                            GameScene.SelectedCell = null;
                        }
                        return;
                    #endregion
                    #region To Storage
                    case MirGridType.Storage: //To Storage
                        switch (GameScene.SelectedCell.GridType)
                        {
                            #region From Inventory
                            case MirGridType.Inventory: //From Invenotry
                                if (Item != null)
                                {
                                    if (GameScene.SelectedCell.Item.Info == Item.Info && Item.Count < Item.Info.StackSize)
                                    {
                                        Network.Enqueue(new C.MergeItem { GridFrom = GameScene.SelectedCell.GridType, GridTo = GridType, IDFrom = GameScene.SelectedCell.Item.UniqueID, IDTo = Item.UniqueID });

                                        Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                }


                                if (ItemArray[ItemSlot] == null)
                                {
                                    Network.Enqueue(new C.StoreItem { From = GameScene.SelectedCell.ItemSlot, To = ItemSlot });
                                    Locked = true;
                                    GameScene.SelectedCell.Locked = true;
                                    GameScene.SelectedCell = null;
                                    return;
                                }

                                for (int x = 0; x < ItemArray.Length; x++)
                                    if (ItemArray[x] == null)
                                    {
                                        Network.Enqueue(new C.StoreItem { From = GameScene.SelectedCell.ItemSlot, To = x });

                                        MirItemCell temp = GameScene.Scene.StorageDialog.Grid[x];
                                        if (temp != null) temp.Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                break;
                            #endregion
                            #region From Equipment
                            case MirGridType.Equipment: //From Equipment
                                if (Item != null)
                                {
                                    if (GameScene.SelectedCell.Item.Info == Item.Info && Item.Count < Item.Info.StackSize)
                                    {
                                        //Merge.
                                        Network.Enqueue(new C.MergeItem { GridFrom = GameScene.SelectedCell.GridType, GridTo = GridType, IDFrom = GameScene.SelectedCell.Item.UniqueID, IDTo = Item.UniqueID });

                                        Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                }

                                if (!CanRemoveItem(GameScene.SelectedCell.Item))
                                {
                                    GameScene.SelectedCell = null;
                                    return;
                                }

                                if (Item == null)
                                {
                                    Network.Enqueue(new C.RemoveItem { Grid = GridType, UniqueID = GameScene.SelectedCell.Item.UniqueID, To = ItemSlot });

                                    Locked = true;
                                    GameScene.SelectedCell.Locked = true;
                                    GameScene.SelectedCell = null;
                                    return;
                                }

                                for (int x = 0; x < ItemArray.Length; x++)
                                    if (ItemArray[x] == null)
                                    {
                                        Network.Enqueue(new C.RemoveItem { Grid = GridType, UniqueID = GameScene.SelectedCell.Item.UniqueID, To = x });

                                        MirItemCell temp = GameScene.Scene.StorageDialog.Grid[x];
                                        if (temp != null) temp.Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                break;
                            #endregion
                            #region From Storage
                            case MirGridType.Storage:
                                if (Item != null)
                                {
                                    if (GameScene.SelectedCell.Item.Info == Item.Info && Item.Count < Item.Info.StackSize)
                                    {
                                        //Merge.
                                        Network.Enqueue(new C.MergeItem { GridFrom = GameScene.SelectedCell.GridType, GridTo = GridType, IDFrom = GameScene.SelectedCell.Item.UniqueID, IDTo = Item.UniqueID });

                                        Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                }

                                Network.Enqueue(new C.MoveItem { Grid = GridType, From = GameScene.SelectedCell.ItemSlot, To = ItemSlot });
                                Locked = true;
                                GameScene.SelectedCell.Locked = true;
                                GameScene.SelectedCell = null;
                                return;
                            #endregion
                            
                        }
                        break;

                    #endregion
                    #region To guild storage
                    case MirGridType.GuildStorage: //To Guild Storage
                        switch (GameScene.SelectedCell.GridType)
                        {
                            case MirGridType.GuildStorage: //From Guild Storage
                                if (GameScene.SelectedCell.GridType == MirGridType.GuildStorage)
                                {                                    
                                    if (!GuildDialog.MyOptions.HasFlag(RankOptions.CanStoreItem))
                                    {
                                        GameScene.Scene.ChatDialog.ReceiveChat("Insufficient rights to store items.", ChatType.System);
                                        return;
                                    }
                                       
                                    //if (ItemArray[ItemSlot] == null)
                                    //{
                                        Network.Enqueue(new C.GuildStorageItemChange { Type = 2, From = GameScene.SelectedCell.ItemSlot, To = ItemSlot });
                                        Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    //}
                                }
                                return;

                            case MirGridType.Inventory:

                                if (GameScene.SelectedCell.GridType == MirGridType.Inventory)
                                {
                                    if (Item != null)
                                    {
                                        GameScene.Scene.ChatDialog.ReceiveChat("You cannot swap items.", ChatType.System);
                                        return;
                                    }
                                    if (!GuildDialog.MyOptions.HasFlag(RankOptions.CanStoreItem))
                                    {
                                        GameScene.Scene.ChatDialog.ReceiveChat("Insufficient rights to store items.", ChatType.System);
                                        return;
                                    }
                                    if (ItemArray[ItemSlot] == null)
                                    {
                                        Network.Enqueue(new C.GuildStorageItemChange { Type = 0, From = GameScene.SelectedCell.ItemSlot, To = ItemSlot });
                                        Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                }
                                return;
                        }
                        break;
                    #endregion
                    #region To Trade

                    case MirGridType.Trade:
                        if (Item != null && Item.Info.Bind.HasFlag(BindMode.DontTrade)) return;

                        switch (GameScene.SelectedCell.GridType)
                        {
                            #region From Trade
                            case MirGridType.Trade: //From Trade
                                if (Item != null)
                                {
                                    if (GameScene.SelectedCell.Item.Info == Item.Info && Item.Count < Item.Info.StackSize)
                                    {
                                        //Merge.
                                        Network.Enqueue(new C.MergeItem { GridFrom = GameScene.SelectedCell.GridType, GridTo = GridType, IDFrom = GameScene.SelectedCell.Item.UniqueID, IDTo = Item.UniqueID });

                                        Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                }

                                Network.Enqueue(new C.MoveItem { Grid = GridType, From = GameScene.SelectedCell.ItemSlot, To = ItemSlot });

                                Locked = true;
                                GameScene.SelectedCell.Locked = true;
                                GameScene.SelectedCell = null;
                                return;
                            #endregion

                            #region From Inventory
                            case MirGridType.Inventory: //From Inventory
                                if (Item != null)
                                {
                                    if (GameScene.SelectedCell.Item.Info == Item.Info && Item.Count < Item.Info.StackSize)
                                    {
                                        Network.Enqueue(new C.MergeItem { GridFrom = GameScene.SelectedCell.GridType, GridTo = GridType, IDFrom = GameScene.SelectedCell.Item.UniqueID, IDTo = Item.UniqueID });

                                        Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                }


                                if (ItemArray[ItemSlot] == null)
                                {
                                    Network.Enqueue(new C.DepositTradeItem { From = GameScene.SelectedCell.ItemSlot, To = ItemSlot });
                                    Locked = true;
                                    GameScene.SelectedCell.Locked = true;
                                    GameScene.SelectedCell = null;
                                    return;
                                }

                                for (int x = 0; x < ItemArray.Length; x++)
                                    if (ItemArray[x] == null)
                                    {
                                        Network.Enqueue(new C.DepositTradeItem { From = GameScene.SelectedCell.ItemSlot, To = x });

                                        MirItemCell temp = GameScene.Scene.TradeDialog.Grid[x];
                                        if (temp != null) temp.Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                break;
                            #endregion
                        }
                        break;

                    #endregion

                    #region To Refine 
                  
                    case MirGridType.Refine:

                        switch (GameScene.SelectedCell.GridType)
                        {
                            #region From Refine
                            case MirGridType.Refine: //From Refine
                                if (Item != null)
                                {
                                    if (GameScene.SelectedCell.Item.Info == Item.Info && Item.Count < Item.Info.StackSize)
                                    {
                                        //Merge.
                                        Network.Enqueue(new C.MergeItem { GridFrom = GameScene.SelectedCell.GridType, GridTo = GridType, IDFrom = GameScene.SelectedCell.Item.UniqueID, IDTo = Item.UniqueID });

                                        Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                }

                                Network.Enqueue(new C.MoveItem { Grid = GridType, From = GameScene.SelectedCell.ItemSlot, To = ItemSlot });
                                Locked = true;
                                GameScene.SelectedCell.Locked = true;
                                GameScene.SelectedCell = null;
                                return;
                            #endregion

                            #region From Inventory
                            case MirGridType.Inventory: //From Inventory
                                if (Item != null)
                                {
                                    if (GameScene.SelectedCell.Item.Info == Item.Info && Item.Count < Item.Info.StackSize)
                                    {
                                        Network.Enqueue(new C.MergeItem { GridFrom = GameScene.SelectedCell.GridType, GridTo = GridType, IDFrom = GameScene.SelectedCell.Item.UniqueID, IDTo = Item.UniqueID });

                                        Locked = true;
                                        GameScene.SelectedCell.Locked = true;
                                        GameScene.SelectedCell = null;
                                        return;
                                    }
                                }

                                Network.Enqueue(new C.DepositRefineItem { From = GameScene.SelectedCell.ItemSlot, To = ItemSlot });
                                Locked = true;
                                GameScene.SelectedCell.Locked = true;
                                GameScene.SelectedCell = null;
                                return;
                            #endregion
                        }
                        break;

                    #endregion

                    #region To Item Renting Dialog

                    case MirGridType.Renting:
                        switch (GameScene.SelectedCell.GridType)
                        {
                            case MirGridType.Inventory:
             
                                if (Item == null)
                                {
                                    Network.Enqueue(new C.DepositRentalItem { From = GameScene.SelectedCell.ItemSlot, To = ItemSlot });
                                    Locked = true;
                                    GameScene.SelectedCell.Locked = true;
                                    GameScene.SelectedCell = null;
                                    return;
                                }

                                break;
                        }

                        break;

                    #endregion

                    #region To Awakening
                    case MirGridType.AwakenItem:
                        {
                            int errorCode = 0;

                            if (GameScene.SelectedCell.GridType != MirGridType.Inventory && _itemSlot < 1) return;

                            switch (_itemSlot)
                            {
                                    //baseitem
                                case 0:
                                    {
                                        if ((GameScene.SelectedCell.Item.Info.Type == ItemType.Weapon ||
                                            GameScene.SelectedCell.Item.Info.Type == ItemType.Helmet ||
                                            GameScene.SelectedCell.Item.Info.Type == ItemType.Armour) &&
                                            GameScene.SelectedCell.Item.Info.Grade != ItemGrade.None &&
                                            _itemSlot == 0)
                                        {
                                            if (Item == null)
                                            {
                                                Item = GameScene.SelectedCell.Item;
                                                GameScene.SelectedCell.Locked = true;
                                                NPCAwakeDialog.ItemsIdx[_itemSlot] = GameScene.SelectedCell._itemSlot;
                                            }
                                            else
                                            {
                                                Network.Enqueue(new C.AwakeningLockedItem { UniqueID = Item.UniqueID, Locked = false });

                                                Item = GameScene.SelectedCell.Item;
                                                GameScene.SelectedCell.Locked = true;
                                                NPCAwakeDialog.ItemsIdx[_itemSlot] = GameScene.SelectedCell._itemSlot;
                                            }
                                            GameScene.Scene.NPCAwakeDialog.ItemCell_Click();
                                            GameScene.Scene.NPCAwakeDialog.OnAwakeTypeSelect(0);
                                        }
                                        else
                                        {
                                            errorCode = -2;
                                        }
                                    }
                                    break;
                                    //view materials
                                case 1:
                                case 2:
                                    break;
                                    //materials
                                case 3:
                                case 4:
                                    {
                                        switch (GameScene.SelectedCell.GridType)
                                        {
                                            case MirGridType.Inventory:
                                                {
                                                    if (GameScene.SelectedCell.Item.Info.Type == ItemType.Awakening &&
                                                        GameScene.SelectedCell.Item.Info.Shape < 200 && NPCAwakeDialog.ItemsIdx[_itemSlot] == 0)
                                                    {
                                                        Item = GameScene.SelectedCell.Item;
                                                        GameScene.SelectedCell.Locked = true;
                                                        NPCAwakeDialog.ItemsIdx[_itemSlot] = GameScene.SelectedCell._itemSlot;
                                                    }
                                                    else
                                                    {
                                                        errorCode = -2;
                                                    }
                                                }
                                                break;
                                            case MirGridType.AwakenItem:
                                                {
                                                    if (GameScene.SelectedCell.ItemSlot == ItemSlot || GameScene.SelectedCell.ItemSlot == 0)
                                                    {
                                                        Locked = false;
                                                        GameScene.SelectedCell = null;
                                                    }
                                                    else
                                                    {
                                                        GameScene.SelectedCell.Locked = false;
                                                        Locked = false;

                                                        int beforeIdx = NPCAwakeDialog.ItemsIdx[GameScene.SelectedCell._itemSlot];
                                                        NPCAwakeDialog.ItemsIdx[GameScene.SelectedCell._itemSlot] = NPCAwakeDialog.ItemsIdx[_itemSlot];
                                                        NPCAwakeDialog.ItemsIdx[_itemSlot] = beforeIdx;

                                                        UserItem item = GameScene.SelectedCell.Item;
                                                        GameScene.SelectedCell.Item = Item;
                                                        Item = item;
                                                        GameScene.SelectedCell = null;
                                                    }
                                                }
                                                break;
                                        }
                                        
                                    }
                                    break;
                                    //SuccessRateUpItem or RandomValueUpItem or CancelDestroyedItem etc.
                                    //AllCashItem Korea Server Not Implementation.
                                case 5:
                                case 6:
                                    if (GameScene.SelectedCell.Item.Info.Type == ItemType.Awakening &&
                                            GameScene.SelectedCell.Item.Info.Shape == 200)
                                    {
                                        Item = GameScene.SelectedCell.Item;
                                        GameScene.SelectedCell.Locked = true;
                                        NPCAwakeDialog.ItemsIdx[_itemSlot] = GameScene.SelectedCell._itemSlot;
                                    }
                                    else
                                    {
                                        errorCode = -2;
                                    }
                                    break;
                                default:
                                    break;
                            }

                            GameScene.SelectedCell = null;
                            MirMessageBox messageBox;
                           
                            switch (errorCode)
                            {
                                //case -1:
                                //    messageBox = new MirMessageBox("Item must be in your inventory.", MirMessageBoxButtons.OK);
                                //    messageBox.Show();
                                //    break;
                                case -2:
                                    //messageBox = new MirMessageBox("Cannot awaken this item.", MirMessageBoxButtons.OK);
                                    //messageBox.Show();
                                    break;
                            }
                        }
                        return;
                    #endregion
                    #region To Mail
                    case MirGridType.Mail: //To Mail
                        if (GameScene.SelectedCell.GridType == MirGridType.Inventory)
                        {
                            if (Item != null)
                            {
                                GameScene.Scene.ChatDialog.ReceiveChat("You cannot swap items.", ChatType.System);
                                return;
                            }

                            if (GameScene.SelectedCell.Item.Info.Bind.HasFlag(BindMode.DontTrade))
                            {
                                GameScene.Scene.ChatDialog.ReceiveChat("You cannot mail this item.", ChatType.System);
                                return;
                            }

                            if (ItemArray[ItemSlot] == null)
                            {
                                Item = GameScene.SelectedCell.Item;
                                GameScene.SelectedCell.Locked = true;
                                MailComposeParcelDialog.ItemsIdx[_itemSlot] = GameScene.SelectedCell.Item.UniqueID;
                                GameScene.SelectedCell = null;
                                GameScene.Scene.MailComposeParcelDialog.CalculatePostage();

                                return;
                            }
                        }
                        break;
                    #endregion
                }

                return;
            }

            if (Item != null)
            {
                GameScene.SelectedCell = this;
            }
        }
        private void PlayItemSound()
        {
            if (Item == null) return;

            switch (Item.Info.Type)
            {
                case ItemType.Weapon:
                    SoundManager.PlaySound(SoundList.ClickWeapon);
                    break;
                case ItemType.Armour:
                    SoundManager.PlaySound(SoundList.ClickArmour);
                    break;
                case ItemType.Helmet:
                    SoundManager.PlaySound(SoundList.ClickHelmet);
                    break;
                case ItemType.Necklace:
                    SoundManager.PlaySound(SoundList.ClickNecklace);
                    break;
                case ItemType.Bracelet:
                    SoundManager.PlaySound(SoundList.ClickBracelet);
                    break;
                case ItemType.Ring:
                    SoundManager.PlaySound(SoundList.ClickRing);
                    break;
                case ItemType.Boots:
                    SoundManager.PlaySound(SoundList.ClickBoots);
                    break;
                case ItemType.Potion:
                    SoundManager.PlaySound(SoundList.ClickDrug);
                    break;
                default:
                    SoundManager.PlaySound(SoundList.ClickItem);
                    break;
            }
        }

        private int FreeSpace()
        {
            int count = 0;

            for (int i = 0; i < ItemArray.Length; i++)
                if (ItemArray[i] == null) count++;

            return count;
        }


        private bool CanRemoveItem(UserItem i)
        {
            if(MapObject.User.RidingMount && i.Info.Type != ItemType.Torch)
            {
                return false;
            }
            //stuck
            return FreeSpace() > 0;
        }

        private bool CorrectSlot(UserItem i)
        {
            ItemType type = i.Info.Type;

            switch ((EquipmentSlot)ItemSlot)
            {
                case EquipmentSlot.Weapon:
                    return type == ItemType.Weapon;
                case EquipmentSlot.Armour:
                    return type == ItemType.Armour;
                case EquipmentSlot.Helmet:
                    return type == ItemType.Helmet;
                case EquipmentSlot.Torch:
                    return type == ItemType.Torch;
                case EquipmentSlot.Necklace:
                    return type == ItemType.Necklace;
                case EquipmentSlot.BraceletL:
                    return i.Info.Type == ItemType.Bracelet;
                case EquipmentSlot.BraceletR:
                    return i.Info.Type == ItemType.Bracelet || i.Info.Type == ItemType.Amulet;
                case EquipmentSlot.RingL:
                case EquipmentSlot.RingR:
                    return type == ItemType.Ring;
                case EquipmentSlot.Amulet:
                    return type == ItemType.Amulet;// && i.Info.Shape > 0;
                case EquipmentSlot.Boots:
                    return type == ItemType.Boots;
                case EquipmentSlot.Belt:
                    return type == ItemType.Belt;
                case EquipmentSlot.Stone:
                    return type == ItemType.Stone;
                case EquipmentSlot.Mount:
                    return type == ItemType.Mount;
                default:
                    return false;
            }

        }
        private bool CanUseItem()
        {
            if (Item == null) return false;

            switch (MapObject.User.Gender)
            {
                case MirGender.Male:
                    if (!Item.Info.RequiredGender.HasFlag(RequiredGender.Male))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You are not Female.", ChatType.System);
                        return false;
                    }
                    break;
                case MirGender.Female:
                    if (!Item.Info.RequiredGender.HasFlag(RequiredGender.Female))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You are not Male.", ChatType.System);
                        return false;
                    }
                    break;
            }

            switch (MapObject.User.Class)
            {
                case MirClass.Warrior:
                    if (!Item.Info.RequiredClass.HasFlag(RequiredClass.Warrior))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("Warriors cannot use this item.", ChatType.System);
                        return false;
                    }
                    break;
                case MirClass.Wizard:
                    if (!Item.Info.RequiredClass.HasFlag(RequiredClass.Wizard))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("Wizards cannot use this item.", ChatType.System);
                        return false;
                    }
                    break;
                case MirClass.Taoist:
                    if (!Item.Info.RequiredClass.HasFlag(RequiredClass.Taoist))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("Taoists cannot use this item.", ChatType.System);
                        return false;
                    }
                    break;
                case MirClass.Assassin:
                    if (!Item.Info.RequiredClass.HasFlag(RequiredClass.Assassin))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("Assassins cannot use this item.", ChatType.System);
                        return false;
                    }
                    break;
                case MirClass.Archer:
                    if (!Item.Info.RequiredClass.HasFlag(RequiredClass.Archer))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("Archers cannot use this item.", ChatType.System);
                        return false;
                    }
                    break;
            }

            switch (Item.Info.RequiredType)
            {
                case RequiredType.Level:
                    if (MapObject.User.Level < Item.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You are not a high enough level.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MaxAC:
                    if (MapObject.User.MaxAC < Item.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough AC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MaxMAC:
                    if (MapObject.User.MaxMAC < Item.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough MAC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MaxDC:
                    if (MapObject.User.MaxDC < Item.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough DC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MaxMC:
                    if (MapObject.User.MaxMC < Item.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough MC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MaxSC:
                    if (MapObject.User.MaxSC < Item.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough SC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MaxLevel:
                    if (MapObject.User.Level > Item.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You have exceeded the maximum level.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MinAC:
                    if (MapObject.User.MinAC < Item.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough Base AC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MinMAC:
                    if (MapObject.User.MinMAC < Item.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough Base MAC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MinDC:
                    if (MapObject.User.MinDC < Item.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough Base DC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MinMC:
                    if (MapObject.User.MinMC < Item.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough Base MC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MinSC:
                    if (MapObject.User.MinSC < Item.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough Base SC.", ChatType.System);
                        return false;
                    }
                    break;
            }

            switch (Item.Info.Type)
            {
                case ItemType.Saddle:
                case ItemType.Ribbon:
                case ItemType.Bells:
                case ItemType.Mask:
                case ItemType.Reins:
                    if (MapObject.User.Equipment[(int)EquipmentSlot.Mount] == null)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have a mount equipped.", ChatType.System);
                        return false;
                    }
                    break;
                case ItemType.Hook:
                case ItemType.Float:
                case ItemType.Bait:
                case ItemType.Finder:
                case ItemType.Reel:
                    if (MapObject.User.Equipment[(int)EquipmentSlot.Weapon] == null || 
                        (MapObject.User.Equipment[(int)EquipmentSlot.Weapon].Info.Shape != 49 && MapObject.User.Equipment[(int)EquipmentSlot.Weapon].Info.Shape != 50))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have a fishing rod equipped.", ChatType.System);
                        return false;
                    }
                    break;
            }
            return true;
        }

        private bool CanWearItem(UserItem i)
        {
            if (i == null) return false;

            //If Can remove;

            switch (MapObject.User.Gender)
            {
                case MirGender.Male:
                    if (!i.Info.RequiredGender.HasFlag(RequiredGender.Male))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You are not Female.", ChatType.System);
                        return false;
                    }
                    break;
                case MirGender.Female:
                    if (!i.Info.RequiredGender.HasFlag(RequiredGender.Female))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You are not Male.", ChatType.System);
                        return false;
                    }
                    break;
            }

            switch (MapObject.User.Class)
            {
                case MirClass.Warrior:
                    if (!i.Info.RequiredClass.HasFlag(RequiredClass.Warrior))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("Warriors cannot use this item.", ChatType.System);
                        return false;
                    }
                    break;
                case MirClass.Wizard:
                    if (!i.Info.RequiredClass.HasFlag(RequiredClass.Wizard))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("Wizards cannot use this item.", ChatType.System);
                        return false;
                    }
                    break;
                case MirClass.Taoist:
                    if (!i.Info.RequiredClass.HasFlag(RequiredClass.Taoist))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("Taoists cannot use this item.", ChatType.System);
                        return false;
                    }
                    break;
                case MirClass.Assassin:
                    if (!i.Info.RequiredClass.HasFlag(RequiredClass.Assassin))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("Assassins cannot use this item.", ChatType.System);
                        return false;
                    }
                    break;
                case MirClass.Archer:
                    if (!i.Info.RequiredClass.HasFlag(RequiredClass.Archer))
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("Archers cannot use this item.", ChatType.System);
                        return false;
                    }
                    break;
            }

            switch (i.Info.RequiredType)
            {
                case RequiredType.Level:
                    if (MapObject.User.Level < i.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You are not a high enough level.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MaxAC:
                    if (MapObject.User.MaxAC < i.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough AC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MaxMAC:
                    if (MapObject.User.MaxMAC < i.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough MAC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MaxDC:
                    if (MapObject.User.MaxDC < i.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough DC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MaxMC:
                    if (MapObject.User.MaxMC < i.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough MC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MaxSC:
                    if (MapObject.User.MaxSC < i.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough SC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MaxLevel:
                    if (MapObject.User.Level > i.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You have exceeded the maximum level.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MinAC:
                    if (MapObject.User.MinAC < i.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough Base AC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MinMAC:
                    if (MapObject.User.MinMAC < i.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough Base MAC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MinDC:
                    if (MapObject.User.MinDC < i.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough Base DC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MinMC:
                    if (MapObject.User.MinMC < i.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough Base MC.", ChatType.System);
                        return false;
                    }
                    break;
                case RequiredType.MinSC:
                    if (MapObject.User.MinSC < i.Info.RequiredAmount)
                    {
                        GameScene.Scene.ChatDialog.ReceiveChat("You do not have enough Base SC.", ChatType.System);
                        return false;
                    }
                    break;
            }

            if (i.Info.Type == ItemType.Weapon || i.Info.Type == ItemType.Torch)
            {
                if (i.Weight - (Item != null ? Item.Weight : 0) + MapObject.User.CurrentHandWeight > MapObject.User.MaxHandWeight)
                {
                    GameScene.Scene.ChatDialog.ReceiveChat("It is too heavy to Hold.", ChatType.System);
                    return false;
                }
            }
            else
            {
                if (i.Weight - (Item != null ? Item.Weight : 0) + MapObject.User.CurrentWearWeight > MapObject.User.MaxWearWeight)
                {
                    GameScene.Scene.ChatDialog.ReceiveChat("It is too heavy to wear.", ChatType.System);
                    return false;
                }
            }

            return true;
        }

        protected internal override void DrawControl()
        {
            /*
            if (GameScene.SelectedCell == this || Locked)
            {
                base.DrawControl();
            }

            if (Locked) return;
            */
            if (Item != null && GameScene.SelectedCell != this && Locked != true)
            {
                CreateDisposeLabel();

                if (Library != null)
                {
                    ushort image = Item.Image;

                    Size imgSize = Library.GetTrueSize(image);

                    Point offSet = new Point((Size.Width - imgSize.Width) / 2, (Size.Height - imgSize.Height) / 2);

                    Library.Draw(image, DisplayLocation.Add(offSet), ForeColour, UseOffSet, 1F);
                }
            }
            else if (Item != null && (GameScene.SelectedCell == this  || Locked))
            {
                CreateDisposeLabel();

                if (Library != null)
                {
                    ushort image = Item.Image;

                    Size imgSize = Library.GetTrueSize(image);

                    Point offSet = new Point((Size.Width - imgSize.Width) / 2, (Size.Height - imgSize.Height) / 2);

                    Library.Draw(image, DisplayLocation.Add(offSet), Color.DimGray, UseOffSet, 0.8F);
                }
            }
            else if (ShadowItem != null)
            {
                CreateDisposeLabel();

                if (Library != null)
                {
                    ushort image = ShadowItem.Info.Image;

                    Size imgSize = Library.GetTrueSize(image);

                    Point offSet = new Point((Size.Width - imgSize.Width) / 2, (Size.Height - imgSize.Height) / 2);

                    Library.Draw(image, DisplayLocation.Add(offSet), Color.DimGray, UseOffSet, 0.8F);
                }
            }
            else
                DisposeCountLabel();
        }

        protected override void OnMouseEnter()
        {
            base.OnMouseEnter();
            if (GridType == MirGridType.Inspect)
                GameScene.Scene.CreateItemLabel(Item, true);
            else
            {
                if (Item != null)
                    GameScene.Scene.CreateItemLabel(Item);
                else if (ShadowItem != null)
                    GameScene.Scene.CreateItemLabel(ShadowItem);
            }
        }
        protected override void OnMouseLeave()
        {
            base.OnMouseLeave();
            GameScene.Scene.DisposeItemLabel();
            GameScene.HoverItem = null;
        }

        private void CreateDisposeLabel()
        {
            if (Item == null && ShadowItem == null)
                return;

            if (Item != null && ShadowItem == null && Item.Info.StackSize <= 1)
            {
                DisposeCountLabel();
                return;
            }

            if (CountLabel == null || CountLabel.IsDisposed)
            {
                CountLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.Yellow,
                    NotControl = true,
                    OutLine = false,
                    Parent = this,
                };
            }

            if (ShadowItem != null)
            {
                CountLabel.ForeColour = (Item == null || ShadowItem.Count > Item.Count) ? Color.Red : Color.LimeGreen;

                CountLabel.Text = string.Format("{0}/{1}", Item == null ? 0 : Item.Count, ShadowItem.Count);
            }
            else
            {
                CountLabel.Text = Item.Count.ToString("###0");
            }

            CountLabel.Location = new Point(Size.Width - CountLabel.Size.Width, Size.Height - CountLabel.Size.Height);
        }
        private void DisposeCountLabel()
        {
            if (CountLabel != null && !CountLabel.IsDisposed)
                CountLabel.Dispose();
            CountLabel = null;
        }
    }
}
