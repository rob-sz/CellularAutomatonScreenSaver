using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using ScreenSaver.Windows.Forms;

namespace ScreenSaver.Options
{
    public partial class UserOptionsForm : Form
    {
        private readonly ColorDialogExtended _colorDialog;
        private readonly UserOptionsProfileRepository _userOptionsProfileRepository;
        private Color[] _cellStateColors;
        private Panel[] _cellStateColorPanels;

        public UserOptionsForm(Point location)
        {
            _colorDialog = new ColorDialogExtended {Location = new Point(location.X + 342, location.Y + 170)};
            _userOptionsProfileRepository = new UserOptionsProfileRepository();

            InitializeComponent();

            if (location == Point.Empty)
            {
                StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                StartPosition = FormStartPosition.Manual;
                Location = location;
            }

            InitializeColorPanels();
            InitializeForm();
            InitializeOptions();
        }

        private void InitializeForm()
        {
            InitializeComboBox(NumberOfStates, UserOptionsData.NumberOfStatesRange);
            InitializeComboBox(NumberOfStatesMinimum, UserOptionsData.NumberOfStatesRange);
            InitializeComboBox(NumberOfStatesMaximum, UserOptionsData.NumberOfStatesRange);

            InitializeComboBox(IsotropicStates, UserOptionsData.IsIsotropicOptions);

            InitializeComboBox(CellSize, UserOptionsData.CellSizeRange);
            InitializeComboBox(CellSizeMinimum, UserOptionsData.CellSizeRange);
            InitializeComboBox(CellSizeMaximum, UserOptionsData.CellSizeRange);
        }

        private void InitializeOptions()
        {
            if (!_userOptionsProfileRepository.ProfileExists(UserOptionsData.DefaultProfileName))
            {
                var defaultProfile = new UserOptionsProfile
                {
                    Name = UserOptionsData.DefaultProfileName,
                    Options = new UserOptions()
                };

                _userOptionsProfileRepository.Add(defaultProfile);

                SaveProfile(defaultProfile.Name);
            }

            OptionsProfile.DataSource = _userOptionsProfileRepository.GetAllProfileNames();
            OptionsProfile.SelectedText = _userOptionsProfileRepository.GetSelectedProfileName();
        }

        private void InitializeComboBox(ComboBox comboBox, byte[] valueRange)
        {
            comboBox.DataSource = valueRange.Clone();
            comboBox.MaxDropDownItems = valueRange.Length;
        }

        private void InitializeComboBox(ComboBox comboBox, string[] valueRange)
        {
            comboBox.DataSource = valueRange.Clone();
            comboBox.MaxDropDownItems = valueRange.Length;
        }

        private void InitializeColorPanels()
        {
            _cellStateColorPanels = new[]
            {
                Color0,
                Color1,
                Color2,
                Color3,
                Color4,
                Color5,
                Color6,
                Color7,
                Color8,
                Color9,
                Color10,
                Color11,
                Color12,
                Color13,
                Color14,
                Color15,
                Color16,
                Color17,
                Color18,
                Color19,
                Color20,
                Color21,
                Color22,
                Color23,
                Color24,
                Color25,
                Color26,
                Color27,
                Color28,
                Color29,
                Color30,
                Color31
            };

            _cellStateColors = new Color[_cellStateColorPanels.Length];
            for (var i = 0; i < _cellStateColorPanels.Length; i++)
            {
                _cellStateColors[i] = _cellStateColorPanels[i].BackColor;
            }
        }

        private bool AssertRange(string a, string b, string message)
        {
            int ai;
            int.TryParse(a, out ai);

            int bi;
            int.TryParse(b, out bi);

            if (bi >= ai)
            {
                return true;
            }

            MessageDialog.Show(this, message, Text);
            return false;
        }

        private bool AssertRange(decimal a, decimal b, string message)
        {
            if (b >= a)
            {
                return true;
            }

            MessageDialog.Show(this, message, Text);
            return false;
        }

        private bool AssertFields()
        {
            if (NumberOfStatesRandom.Checked &&
                !AssertRange(NumberOfStatesMinimum.Text, NumberOfStatesMaximum.Text, "Invalid number of states range!"))
                return false;

            if (NumberOfNeighboursRandom.Checked &&
                !AssertRange(NumberOfNeighboursMinimum.Text, NumberOfNeighboursMaximum.Text, "Invalid number of neighbours range!"))
                return false;

            if (LambdaRandom.Checked &&
                !AssertRange(LambdaMinimum.Value, LambdaMaximum.Value, "Invalid lambda range!"))
                return false;

            if (CellSizeRandom.Checked &&
                !AssertRange(CellSizeMinimum.Text, CellSizeMaximum.Text, "Invalid cell size range!"))
                return false;

            if (RgbRangeRandom.Checked &&
                !AssertRange(RgbRangeMinimum.Value, RgbRangeMaximum.Value, "Invalid RGB range!"))
                return false;

            return true;
        }
        
        private bool AssertSaveProfile(string profileName)
        {
            if (AssertFields())
            {
                var dialogResult = MessageDialog.Show(this,
                    string.Format("Save changes to profile '{0}'?", profileName), Text, MessageBoxButtons.OKCancel);

                if (dialogResult == DialogResult.OK)
                {
                    SaveProfile(profileName);
                    return true;
                }
            }

            return false;
        }

        private bool AssertChangeProfile(string profileName)
        {
            var dialogResult = MessageDialog.Show(this,
                string.Format("Save changes to profile '{0}'?", profileName), Text, MessageBoxButtons.YesNoCancel);

            if (dialogResult == DialogResult.Yes && AssertFields())
            {
                SaveProfile(profileName);
                return true;
            }

            if (dialogResult == DialogResult.No)
            {
                return true;
            }

            return false;
        }

        private void SaveProfile(string profileName)
        {
            var options = _userOptionsProfileRepository.GetProfile(profileName).Options;

            options.SleepAfter = new TimeSpan(
                (int)SleepAfterHours.Value,
                (int)SleepAfterMinutes.Value,
                (int)SleepAfterSeconds.Value);
            options.IsSleepAfter = !SleepAfterNever.Checked;

            options.ChangeWorldFrequency = (int)ChangeWorldFrequency.Value;
            options.IsChangeWorldFrequency = !ChangeWorldFrequencyNever.Checked;

            options.ChangeRulesFrequency = (int)ChangeRulesFrequency.Value;
            options.IsChangeRulesFrequency = !ChangeRulesFrequencyNever.Checked;

            options.IterationInterval = (int)IterationInterval.Value;

            //options.IsSeedIteration = SeedIteration.Text == "Saved";

            options.NumberOfStates = byte.Parse(NumberOfStates.Text);
            options.NumberOfStatesMinimum = byte.Parse(NumberOfStatesMinimum.Text);
            options.NumberOfStatesMaximum = byte.Parse(NumberOfStatesMaximum.Text);
            options.IsNumberOfStatesRandom = NumberOfStatesRandom.Checked;

            options.NumberOfNeighbours = byte.Parse(NumberOfNeighbours.Text);
            options.NumberOfNeighboursMinimum = byte.Parse(NumberOfNeighboursMinimum.Text);
            options.NumberOfNeighboursMaximum = byte.Parse(NumberOfNeighboursMaximum.Text);
            options.IsNumberOfNeighboursRandom = NumberOfNeighboursRandom.Checked;

            options.Lambda = (int)Lambda.Value;
            options.LambdaMinimum = (int)LambdaMinimum.Value;
            options.LambdaMaximum = (int)LambdaMaximum.Value;
            options.IsLambdaRandom = LambdaRandom.Checked;

            options.IsIsotropic = IsotropicStates.Text == "Yes";
            options.IsIsotropicRandom = IsotropicStatesRandom.Checked;

            options.CellSize = byte.Parse(CellSize.Text);
            options.CellSizeMinimum = byte.Parse(CellSizeMinimum.Text);
            options.CellSizeMaximum = byte.Parse(CellSizeMaximum.Text);
            options.IsCellSizeRandom = CellSizeRandom.Checked;

            options.RgbRangeMinimum = (int)RgbRangeMinimum.Value;
            options.RgbRangeMaximum = (int)RgbRangeMaximum.Value;
            options.IsRgbRangeRandom = RgbRangeRandom.Checked;

            options.StateColors = new Color[_cellStateColorPanels.Length];
            for (int i = 0; i < options.StateColors.Length; i++)
            {
                options.StateColors[i] = _cellStateColors[i];
            }

            _userOptionsProfileRepository.SaveChanges();
        }

        private void LoadProfile(string profileName)
        {
            var options = _userOptionsProfileRepository.GetProfile(profileName).Options;

            SleepAfterHours.Value = options.SleepAfter.Hours;
            SleepAfterMinutes.Value = options.SleepAfter.Minutes;
            SleepAfterSeconds.Value = options.SleepAfter.Seconds;
            SleepAfterNever.Checked = !options.IsSleepAfter;

            ChangeWorldFrequency.Value = options.ChangeWorldFrequency;
            ChangeWorldFrequencyNever.Checked = !options.IsChangeWorldFrequency;

            ChangeRulesFrequency.Value = options.ChangeRulesFrequency;
            ChangeRulesFrequencyNever.Checked = !options.IsChangeRulesFrequency;

            IterationInterval.Value = options.IterationInterval;

            //SeedIteration.Text = options.IsSeedIteration ? "Saved" : "Random";

            NumberOfStates.Text = options.NumberOfStates.ToString(CultureInfo.InvariantCulture);
            NumberOfStatesMinimum.Text = options.NumberOfStatesMinimum.ToString(CultureInfo.InvariantCulture);
            NumberOfStatesMaximum.Text = options.NumberOfStatesMaximum.ToString(CultureInfo.InvariantCulture);
            NumberOfStatesRandom.Checked = options.IsNumberOfStatesRandom;

            NumberOfNeighbours.Text = options.NumberOfNeighbours.ToString(CultureInfo.InvariantCulture);
            NumberOfNeighboursMinimum.Text = options.NumberOfNeighboursMinimum.ToString(CultureInfo.InvariantCulture);
            NumberOfNeighboursMaximum.Text = options.NumberOfNeighboursMaximum.ToString(CultureInfo.InvariantCulture);
            NumberOfNeighboursRandom.Checked = options.IsNumberOfNeighboursRandom;

            Lambda.Value = options.Lambda;
            LambdaMinimum.Value = options.LambdaMinimum;
            LambdaMaximum.Value = options.LambdaMaximum;
            LambdaRandom.Checked = options.IsLambdaRandom;

            IsotropicStates.Text = options.IsIsotropic ? "Yes" : "No";
            IsotropicStatesRandom.Checked = options.IsIsotropicRandom;

            CellSize.Text = options.CellSize.ToString(CultureInfo.InvariantCulture);
            CellSizeMinimum.Text = options.CellSizeMinimum.ToString(CultureInfo.InvariantCulture);
            CellSizeMaximum.Text = options.CellSizeMaximum.ToString(CultureInfo.InvariantCulture);
            CellSizeRandom.Checked = options.IsCellSizeRandom;

            // note: needs to happen before RgbRangeRandom.Checked,
            //   as checkbox will trigger colors to be enabled/disabled
            for (int i = 0; i < options.StateColors.Length; i++)
            {
                _cellStateColors[i] = _cellStateColorPanels[i].BackColor = options.StateColors[i];
            }

            RgbRangeMinimum.Value = options.RgbRangeMinimum;
            RgbRangeMaximum.Value = options.RgbRangeMaximum;
            RgbRangeRandom.Checked = options.IsRgbRangeRandom;
        }

        private void UpdateNumberOfRules()
        {
            byte numberOfStatesMinimum;
            if (NumberOfStatesRandom.Checked)
            {
                byte.TryParse(NumberOfStatesMinimum.Text, out numberOfStatesMinimum);
            }
            else
            {
                byte.TryParse(NumberOfStates.Text, out numberOfStatesMinimum);
            }

            byte numberOfStatesMaximum = numberOfStatesMinimum;
            if (NumberOfStatesRandom.Checked)
            {
                byte.TryParse(NumberOfStatesMaximum.Text, out numberOfStatesMaximum);
            }

            byte numberOfNeighboursMinimum;
            if (NumberOfNeighboursRandom.Checked)
            {
                byte.TryParse(NumberOfNeighboursMinimum.Text, out numberOfNeighboursMinimum);
            }
            else
            {
                byte.TryParse(NumberOfNeighbours.Text, out numberOfNeighboursMinimum);
            }

            byte numberOfNeighboursMaximum = numberOfNeighboursMinimum;
            if (NumberOfNeighboursRandom.Checked)
            {
                byte.TryParse(NumberOfNeighboursMaximum.Text, out numberOfNeighboursMaximum);
            }

            int lambdaMinimum = LambdaRandom.Checked ? (int)LambdaMinimum.Value : (int)Lambda.Value;
            int lambdaMaximum = LambdaRandom.Checked ? (int)LambdaMaximum.Value : lambdaMinimum;

            long numberOfRulesTotal = (long)Math.Pow(numberOfStatesMinimum, numberOfNeighboursMinimum);
            long numberOfRulesTotalMaximum = (long)Math.Pow(numberOfStatesMaximum, numberOfNeighboursMaximum);

            long numberOfRulesUsed = (long)(lambdaMinimum / 100d * numberOfRulesTotal);
            long numberOfRulesUsedMaximum = (long)(lambdaMaximum / 100d * numberOfRulesTotalMaximum);

            NumberOfRulesTotal.Text = numberOfRulesTotal == numberOfRulesTotalMaximum
                ? string.Format("{0}", numberOfRulesTotal)
                : string.Format("{0} - {1}", numberOfRulesTotal, numberOfRulesTotalMaximum);

            NumberOfRulesUsed.Text = numberOfRulesUsed == numberOfRulesUsedMaximum
                ? string.Format("{0}", numberOfRulesUsed)
                : string.Format("{0} - {1}", numberOfRulesUsed, numberOfRulesUsedMaximum);
        }

        private void UpdateNumberOfNeighbours()
        {
            byte numberOfStates;
            byte.TryParse(NumberOfStates.Text, out numberOfStates);

            byte numberOfNeighbours;
            byte.TryParse(NumberOfNeighbours.Text, out numberOfNeighbours);

            byte[] numberOfNeighboursRange =
                UserOptionsData.NumberOfNeighboursRange(numberOfStates);

            if (NumberOfNeighbours.Items.Count != numberOfNeighboursRange.Length)
            {
                InitializeComboBox(NumberOfNeighbours, numberOfNeighboursRange);
                if (numberOfNeighboursRange.Contains(numberOfNeighbours))
                {
                    NumberOfNeighbours.Text = string.Format("{0}", numberOfNeighbours);
                }
            }
        }

        private void UpdateNumberOfNeighboursRandom()
        {
            byte numberOfStatesMinimum;
            byte.TryParse(NumberOfStatesMinimum.Text, out numberOfStatesMinimum);

            byte numberOfStatesMaximum;
            byte.TryParse(NumberOfStatesMaximum.Text, out numberOfStatesMaximum);

            byte numberOfNeighboursMinimum;
            byte.TryParse(NumberOfNeighboursMinimum.Text, out numberOfNeighboursMinimum);

            byte numberOfNeighboursMaximum;
            byte.TryParse(NumberOfNeighboursMaximum.Text, out numberOfNeighboursMaximum);

            byte[] numberOfNeighboursMinimumRange =
                UserOptionsData.NumberOfNeighboursRange(numberOfStatesMinimum);
            byte[] numberOfNeighboursMaximumRange =
                UserOptionsData.NumberOfNeighboursRange(numberOfStatesMaximum);

            if (numberOfNeighboursMaximumRange.Length < numberOfNeighboursMinimumRange.Length)
            {
                numberOfNeighboursMinimumRange = numberOfNeighboursMaximumRange;
            }
            else
            {
                numberOfNeighboursMaximumRange = numberOfNeighboursMinimumRange;
            }

            if (NumberOfNeighboursMinimum.Items.Count != numberOfNeighboursMinimumRange.Length)
            {
                InitializeComboBox(NumberOfNeighboursMinimum, numberOfNeighboursMinimumRange);
                if (numberOfNeighboursMinimumRange.Contains(numberOfNeighboursMinimum))
                {
                    NumberOfNeighboursMinimum.Text = string.Format("{0}", numberOfNeighboursMinimum);
                }
            }

            if (NumberOfNeighboursMaximum.Items.Count != numberOfNeighboursMaximumRange.Length)
            {
                InitializeComboBox(NumberOfNeighboursMaximum, numberOfNeighboursMaximumRange);
                if (numberOfNeighboursMaximumRange.Contains(numberOfNeighboursMaximum))
                {
                    NumberOfNeighboursMaximum.Text = string.Format("{0}", numberOfNeighboursMaximum);
                }
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                Close();
                return true;
            }

            return base.ProcessDialogKey(keyData);
        }

        private void EnableCellStateColors(bool enabled)
        {
            if (enabled)
            {
                _cellStateColorPanels[0].BackColor = _cellStateColors[0];
                for (int i = 1; i < _cellStateColors.Length; i++)
                {
                    _cellStateColorPanels[i].BackColor = _cellStateColors[i];
                    _cellStateColorPanels[i].Enabled = true;
                }
            }
            else
            {
                _cellStateColorPanels[0].BackColor = Color.FromArgb(100, _cellStateColorPanels[0].BackColor);
                for (var i = 1; i < _cellStateColorPanels.Length; i++)
                {
                    _cellStateColorPanels[i].BackColor = Color.FromArgb(100, _cellStateColorPanels[i].BackColor);
                    _cellStateColorPanels[i].Enabled = false;
                }
            }
        }

        private void Color_Click(object sender, EventArgs e)
        {
            var colorPanel = sender as Panel;
            if (colorPanel == null)
            {
                return;
            }

            colorPanel.BorderStyle = BorderStyle.Fixed3D;

            _colorDialog.Text = string.Format("Color: {0}", colorPanel.Tag);
            _colorDialog.Color = colorPanel.BackColor;
            if (_colorDialog.ShowDialog() == DialogResult.OK)
            {
                colorPanel.BackColor = _colorDialog.Color;
                for (int i = 0; i < _cellStateColorPanels.Length; i++)
                {
                    if (_cellStateColorPanels[i] == colorPanel)
                    {
                        _cellStateColors[i] = colorPanel.BackColor;
                        break;
                    }
                }
            }

            colorPanel.BorderStyle = BorderStyle.FixedSingle;
        }

        private void NumberOfStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateNumberOfNeighbours();
            UpdateNumberOfRules();
        }

        private void NumberOfStatesRandom_CheckedChanged(object sender, EventArgs e)
        {
            NumberOfStates.Enabled = !NumberOfStatesRandom.Checked;
            NumberOfStatesMinimum.Enabled = NumberOfStatesMaximum.Enabled = NumberOfStatesRandom.Checked;

            UpdateNumberOfRules();
        }

        private void NumberOfStatesRandom_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateNumberOfNeighboursRandom();
            UpdateNumberOfRules();
        }

        private void NumberOfNeighbours_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateNumberOfRules();
        }

        private void NumberOfNeighboursRandom_CheckedChanged(object sender, EventArgs e)
        {
            NumberOfNeighbours.Enabled = !NumberOfNeighboursRandom.Checked;
            NumberOfNeighboursMinimum.Enabled = NumberOfNeighboursMaximum.Enabled = NumberOfNeighboursRandom.Checked;

            UpdateNumberOfRules();
        }

        private void NumberOfNeighboursRandom_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateNumberOfRules();
        }

        private void Lambda_ValueChanged(object sender, EventArgs e)
        {
            UpdateNumberOfRules();
        }

        private void LambdaRandom_CheckedChanged(object sender, EventArgs e)
        {
            Lambda.Enabled = !LambdaRandom.Checked;
            LambdaMinimum.Enabled = LambdaMaximum.Enabled = LambdaRandom.Checked;

            UpdateNumberOfRules();
        }

        private void LambdaRandom_ValueChanged(object sender, EventArgs e)
        {
            UpdateNumberOfRules();
        }

        private void OptionsProfile_SelectedIndexChanging(object sender, CancelEventArgs e)
        {
            if (OptionsProfile.LastSelectedIndex >= 0 &&
                !AssertChangeProfile(OptionsProfile.Text))
            {
                e.Cancel = true;
            }
        }

        private void OptionsProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProfile(OptionsProfile.Text);
        }

        private void ChangeWorldFrequency_ValueChanged(object sender, EventArgs e)
        {
            ChangeWorldFrequencyScreenLabel.Text =
                ChangeWorldFrequency.Value == 1 ? "screen" : "screens";
        }

        private void ChangeRulesFrequency_ValueChanged(object sender, EventArgs e)
        {
            ChangeRulesFrequencyScreenLabel.Text =
                ChangeRulesFrequency.Value == 1 ? "screen" : "screens";
        }

        private void IterationInterval_ValueChanged(object sender, EventArgs e)
        {
            IterationIntervalMillisecondsLabel.Text =
                IterationInterval.Value == 1 ? "millisecond" : "milliseconds";
        }

        private void IsotropicStatesRandom_CheckedChanged(object sender, EventArgs e)
        {
            IsotropicStates.Enabled = !IsotropicStatesRandom.Checked;
        }

        private void CellSizeRandom_CheckedChanged(object sender, EventArgs e)
        {
            CellSize.Enabled = !CellSizeRandom.Checked;
            CellSizeMinimum.Enabled = CellSizeMaximum.Enabled = CellSizeRandom.Checked;
        }

        private void RgbRangeRandom_CheckedChanged(object sender, EventArgs e)
        {
            EnableCellStateColors(!RgbRangeRandom.Checked);
            RgbRangeMinimum.Enabled = RgbRangeMaximum.Enabled = RgbRangeRandom.Checked;
        }

        private void SleepAfterNever_CheckedChanged(object sender, EventArgs e)
        {
            SleepAfterHours.Enabled = !SleepAfterNever.Checked;
            SleepAfterMinutes.Enabled = !SleepAfterNever.Checked;
            SleepAfterSeconds.Enabled = !SleepAfterNever.Checked;
        }

        private void ChangeWorldFrequencyNever_CheckedChanged(object sender, EventArgs e)
        {
            ChangeWorldFrequency.Enabled = !ChangeWorldFrequencyNever.Checked;
        }

        private void ChangeRulesFrequencyNever_CheckedChanged(object sender, EventArgs e)
        {
            ChangeRulesFrequency.Enabled = !ChangeRulesFrequencyNever.Checked;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (AssertSaveProfile(OptionsProfile.Text))
            {
                Close();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
