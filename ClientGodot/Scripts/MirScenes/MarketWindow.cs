using System.Collections.Generic;
using Godot;
using ClientPackets;
using ServerPackets;

namespace ClientGodot.Scripts.MirScenes
{
    public partial class MarketWindow : Panel
    {
        private LineEdit _searchBox;
        private Button _searchButton;
        private VBoxContainer _listContainer;
        private TabContainer _tabs;

        public override void _Ready()
        {
            _searchBox = GetNode<LineEdit>("Tabs/Consign/SearchRow/SearchBox");
            _searchButton = GetNode<Button>("Tabs/Consign/SearchRow/SearchButton");
            _listContainer = GetNode<VBoxContainer>("Tabs/Consign/Scroll/List");
            _tabs = GetNode<TabContainer>("Tabs");

            _searchButton.Pressed += OnSearchPressed;
        }

        private void OnSearchPressed()
        {
            string text = _searchBox.Text;
            // Send Search Packet
            ClientGodot.Scripts.NetworkManager.Enqueue(new ClientPackets.MarketSearch
            {
                Match = text,
                MarketType = MarketPanelType.Consign
            });
        }

        public void UpdateList(List<ClientAuction> listings)
        {
            foreach(Node child in _listContainer.GetChildren()) child.QueueFree();

            foreach(var auction in listings)
            {
                var row = new HBoxContainer();
                var lbl = new Label();
                lbl.Text = $"{auction.Item.Info.Name} - {auction.Price} Gold";
                row.AddChild(lbl);

                var btn = new Button();
                btn.Text = "Buy";
                btn.Pressed += () =>
                {
                    ClientGodot.Scripts.NetworkManager.Enqueue(new ClientPackets.MarketBuy { AuctionID = auction.AuctionID, BidPrice = auction.Price });
                };
                row.AddChild(btn);

                _listContainer.AddChild(row);
            }
        }
    }
}
