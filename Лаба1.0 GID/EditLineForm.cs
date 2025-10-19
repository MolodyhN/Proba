using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Лаба1._0_GID;

namespace Лаба1._0_GID
{
    public class EditLineForm : Form
    {
        public Color LineColor { get; private set; }
        public int LineWidth { get; private set; }
        public DashStyle LineStyle { get; private set; }

        private ComboBox colorComboBox;
        private NumericUpDown widthNumeric;
        private ComboBox styleComboBox;

        public EditLineForm(Line line)
        {
            InitializeComponents();
            colorComboBox.SelectedItem = line.Color.Name;
            widthNumeric.Value = line.Width;
            styleComboBox.SelectedIndex = LineHelper.GetStyleIndex(line.DashStyle);
        }

        private void InitializeComponents()
        {
            this.Text = "Редактирование линии";
            this.Size = new Size(300, 250);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Padding = new Padding(20);
            this.Controls.Add(mainPanel);

            var colorLabel = new Label();
            colorLabel.Text = "Цвет:";
            colorLabel.Location = new Point(20, 20);
            colorLabel.Size = new Size(80, 20);
            mainPanel.Controls.Add(colorLabel);

            colorComboBox = new ComboBox();
            colorComboBox.Location = new Point(120, 20);
            colorComboBox.Size = new Size(140, 20);
            colorComboBox.Items.AddRange(new string[] { "Black", "Red", "Green", "Blue", "Yellow", "Purple" });
            mainPanel.Controls.Add(colorComboBox);

            var widthLabel = new Label();
            widthLabel.Text = "Ширина:";
            widthLabel.Location = new Point(20, 60);
            widthLabel.Size = new Size(80, 20);
            mainPanel.Controls.Add(widthLabel);

            widthNumeric = new NumericUpDown();
            widthNumeric.Location = new Point(120, 60);
            widthNumeric.Size = new Size(140, 20);
            widthNumeric.Minimum = 1;
            widthNumeric.Maximum = 20;
            mainPanel.Controls.Add(widthNumeric);

            var styleLabel = new Label();
            styleLabel.Text = "Стиль:";
            styleLabel.Location = new Point(20, 100);
            styleLabel.Size = new Size(80, 20);
            mainPanel.Controls.Add(styleLabel);

            styleComboBox = new ComboBox();
            styleComboBox.Location = new Point(120, 100);
            styleComboBox.Size = new Size(140, 20);
            styleComboBox.Items.AddRange(new string[] { "Сплошная", "Пунктир", "Точечная", "Штрих-пунктир" });
            mainPanel.Controls.Add(styleComboBox);

            var okButton = new Button();
            okButton.Text = "OK";
            okButton.Location = new Point(60, 150);
            okButton.Size = new Size(80, 30);
            okButton.DialogResult = DialogResult.OK;
            okButton.Click += (s, e) =>
            {
                LineColor = Color.FromName(colorComboBox.SelectedItem.ToString());
                LineWidth = (int)widthNumeric.Value;
                LineStyle = LineHelper.GetDashStyleFromIndex(styleComboBox.SelectedIndex);
            };
            mainPanel.Controls.Add(okButton);

            var cancelButton = new Button();
            cancelButton.Text = "Отмена";
            cancelButton.Location = new Point(160, 150);
            cancelButton.Size = new Size(80, 30);
            cancelButton.DialogResult = DialogResult.Cancel;
            mainPanel.Controls.Add(cancelButton);
        }
    }
}

