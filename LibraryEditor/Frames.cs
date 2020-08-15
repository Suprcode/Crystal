using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LibraryEditor
{
    public class Frame
    {
        public MirAction Action { get; set; }

        public int Start { get; set; }
        public int Count { get; set; }
        public int Skip { get; set; }
        public int Interval { get; set; }

        public int EffectStart { get; set; }
        public int EffectCount { get; set; }
        public int EffectSkip { get; set; }
        public int EffectInterval { get; set; }

        public bool Reverse { get; set; }
        public bool Blend { get; set; }


        DataGridViewComboBoxColumn CreateComboBoxWithEnums()
        {
            var cell = new DataGridViewComboBoxColumn
            {
                DataSource = Enum.GetValues(typeof(MirAction)),
                DataPropertyName = "Action",
                Name = "Action"
            };

            return cell;
        }

        DataGridViewCheckBoxColumn CreateCheckbox(string name)
        {
            var cell = new DataGridViewCheckBoxColumn
            {
                DataPropertyName = name,
                Name = name
            };

            return cell;
        }

        DataGridViewTextBoxColumn CreateTextbox(string name)
        {
            var cell = new DataGridViewTextBoxColumn
            {
                DataPropertyName = name,
                Name = name,
            };

            return cell;
        }
    }
}
