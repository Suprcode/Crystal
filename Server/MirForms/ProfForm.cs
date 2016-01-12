using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Server.MirEnvir;
using Server.MirDatabase;

namespace Server
{
    public partial class CraftingForm : Form
    {
        private ProfessionInfo selectedProf;
        private RecipeInfo selectedRecipe;
        private IngredientInfo selectedIngredient;

        private Dictionary<ItemInfo, int> tmpIngredients = new Dictionary<ItemInfo, int>();
        public CraftingForm()
        {
            InitializeComponent();

            LoadInterface();
        }

        public Envir Envir
        {
            get { return SMain.EditEnvir; }
        }

        private void CraftingForm_Load(object sender, EventArgs e)
        {
        }

        private void LoadInterface()
        {
            Profs_lb.Items.Clear();
            ProfType_cb.Items.Clear();

            if (Ingredient_cb.Items.Count != Envir.ItemInfoList.Count)
            {
                Ingredient_cb.Items.Clear();
                CraftItem_cb.Items.Clear();
                for (int i = 0; i < Envir.ItemInfoList.Count; i++)
                {
                    Ingredient_cb.Items.Add(Envir.ItemInfoList[i]);
                    CraftItem_cb.Items.Add(Envir.ItemInfoList[i]);
                }

            }

            for (int i = 0; i < Envir.ProfessionList.Count; i++)
            {
                Profs_lb.Items.Add(Envir.ProfessionList[i]);
            }

            ProfType_cb.Items.AddRange(Enum.GetValues(typeof(ProfType)).Cast<object>().ToArray());
        }

        private void AddProf_button_Click(object sender, EventArgs e)
        {
            Envir.AddProfession();
            Envir.SaveDB();
            LoadInterface();
        }

        private void Profs_lb_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedProf = (ProfessionInfo)Profs_lb.SelectedItem;
            SelectProf();
        }

        private void SelectProf()
        {
            //Reset form to empty state
            ProfName_tb.Text = string.Empty;
            ProfIndex_tb.Text = string.Empty;
            Index_tb.Text = string.Empty;
            Name_tb.Text = string.Empty;
            Level_tb.Text = string.Empty;
            Exp_tb.Text = string.Empty;
            CraftTime_tb.Text = string.Empty;
            StartRecipe_cb.Checked = false;
            CraftItem_cb.SelectedIndex = -1;
            Ingredient_cb.SelectedIndex = -1;
            Ingredients_lb.Items.Clear();
            Recipes_lb.SelectedIndex = -1;
            Recipe_gb.Enabled = false;
            Recipes_lb.Items.Clear();
            Profs_tab.Enabled = false;
            selectedRecipe = null;
            //End

            //Fill form with Profession data
            if (selectedProf != null)
            {
                ProfName_tb.Text = selectedProf.Name;
                ProfIndex_tb.Text = selectedProf.Index.ToString();
                Profs_tab.Enabled = true;
                ProfType_cb.SelectedItem = (ProfType)selectedProf.Type;
                
                for (int i = 0; i < Envir.RecipeList.Count; i++)
                {
                    if (Envir.RecipeList[i].ProfIndex == selectedProf.Index)
                        Recipes_lb.Items.Add(Envir.RecipeList[i]);
                }
                    
            }
            //End
        }

        private void SelectRecipe()
        {
            //Reset Recipe form to empty state
            Index_tb.Text = string.Empty;
            Name_tb.Text = string.Empty;
            Level_tb.Text = string.Empty;
            Exp_tb.Text = string.Empty;
            CraftTime_tb.Text = string.Empty;
            StartRecipe_cb.Checked = false;
            CraftItem_cb.SelectedIndex = -1;
            Ingredient_cb.SelectedIndex = -1;
            Ingredients_lb.Items.Clear();
            Recipe_gb.Enabled = false;
            IngredientCount_tb.Text = "1";
            RemoveIng_button.Enabled = false;
            //End

            //Fill form with Recipe data
            if (selectedRecipe != null)
            {
                Index_tb.Text = selectedRecipe.Index.ToString();
                Name_tb.Text = selectedRecipe.Name;
                Level_tb.Text = selectedRecipe.Level.ToString();
                Exp_tb.Text = selectedRecipe.Experience.ToString();
                CraftTime_tb.Text = selectedRecipe.CraftingTime.ToString();
                StartRecipe_cb.Checked = selectedRecipe.StartingRecipe;
                CraftItem_cb.SelectedItem = Envir.ItemInfoList.FirstOrDefault(x => x.Index == selectedRecipe.CraftItemIDX);
                for (int i = 0; i < selectedRecipe.Ingredients.Count; i++)
                {

                    ItemInfo temp = SMain.Envir.GetItemInfo(selectedRecipe.Ingredients[i].ItemIndex);
                    if (temp != null)
                        selectedRecipe.Ingredients[i].Name = temp.Name;

                    Ingredients_lb.Items.Add(selectedRecipe.Ingredients[i]);
                }
                    

                    Recipe_gb.Enabled = true;
            }
            //End
        }



        private void AddRecipe_button_Click(object sender, EventArgs e)
        {
            if (selectedProf == null) return;
            Envir.RecipeList.Add(new RecipeInfo { Index = ++Envir.RecipeIndex, ProfIndex = selectedProf.Index });
            SelectProf();
            Envir.SaveDB();
        }

        private void RemoveRecipe_button_Click(object sender, EventArgs e)
        {
            if (selectedRecipe != null)
                Envir.RecipeList.Remove(selectedRecipe);
            SelectProf();
        }

        private void Recipes_lb_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedRecipe = (RecipeInfo)Recipes_lb.SelectedItem;
            SelectRecipe();
        }

        private void ProfName_tb_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            string temp = ActiveControl.Text;
            selectedProf.Name = temp;
        }

        private void Name_tb_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            string temp = ActiveControl.Text;

            selectedRecipe.Name = temp;
        }

        private void Level_tb_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            selectedRecipe.Level = temp;
        }

        private void Exp_tb_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            long temp;

            if (!long.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            selectedRecipe.Experience = temp;
        }

        private void CraftTime_tb_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            short temp;

            if (!short.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            selectedRecipe.CraftingTime = temp;
        }

        private void StartRecipe_cb_CheckedChanged(object sender, EventArgs e)
        {
            selectedRecipe.StartingRecipe = StartRecipe_cb.Checked;
        }

        private void RemoveProf_button_Click(object sender, EventArgs e)
        {
            if (selectedProf != null)
                Envir.ProfessionList.Remove(selectedProf);
            LoadInterface();

            if (Envir.ProfessionList.Count == 0)
                Envir.CraftingIndex = 0;
        }

        private void AddIng_button_Click(object sender, EventArgs e)
        {
            int temp;
            if (!int.TryParse(IngredientCount_tb.Text, out temp))
            {
                return;
            }
            if (temp == 0) return;

            ItemInfo Ingredient = (ItemInfo)Ingredient_cb.SelectedItem;
            if (Ingredient == null) return;
            selectedRecipe.Ingredients.Add(new IngredientInfo { ItemIndex = Ingredient.Index, Quantity = temp, Name = Ingredient.Name });
            SelectRecipe();
        }

        private void Ingredients_lb_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedIngredient = (IngredientInfo)Ingredients_lb.SelectedItem;

            if (Ingredients_lb.SelectedIndex == -1)
                RemoveIng_button.Enabled = false;
            else
                RemoveIng_button.Enabled = true;
        }

        private void RemoveIng_button_Click(object sender, EventArgs e)
        {
            if (Ingredients_lb.SelectedItem != null)
                selectedRecipe.Ingredients.Remove(selectedIngredient);
            SelectRecipe();
        }

        private void Recipe_gb_Enter(object sender, EventArgs e)
        {

        }

        private void CraftItem_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CraftItem_cb.SelectedIndex == -1) return;

            int tempIndex = ((ItemInfo)CraftItem_cb.SelectedItem).Index;

            selectedRecipe.CraftItemIDX = tempIndex;
        }

        private void CraftingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Envir.SaveDB();
        }

        private void ProfType_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
                selectedProf.Type = (ProfType)ProfType_cb.SelectedItem;
        }
    }
}
