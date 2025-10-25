using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Лаба1._0_GID
{
    public class AddLineForm : Form
    {
        public Point3D StartPoint { get; private set; }
        public Point3D EndPoint { get; private set; }
        public Color LineColor { get; private set; }
        public int LineWidth { get; private set; }
        public DashStyle LineStyle { get; private set; }

        private NumericUpDown startX, startY, startZ, endX, endY, endZ;
        private ComboBox colorComboBox, styleComboBox;
        private NumericUpDown widthNumeric;

        public AddLineForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Добавить линию";
            this.Size = new Size(400, 450);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Padding = new Padding(20);
            this.Controls.Add(mainPanel);

            // Начальная точка
            var startLabel = new Label();
            startLabel.Text = "Начальная точка:";
            startLabel.Font = new Font("Arial", 10, FontStyle.Bold);
            startLabel.Location = new Point(20, 20);
            startLabel.Size = new Size(200, 20);
            mainPanel.Controls.Add(startLabel);

            CreateCoordinateControls(mainPanel, "X:", 50, out startX, 0);
            CreateCoordinateControls(mainPanel, "Y:", 80, out startY, 0);
            CreateCoordinateControls(mainPanel, "Z:", 110, out startZ, 0);

            // Конечная точка
            var endLabel = new Label();
            endLabel.Text = "Конечная точка:";
            endLabel.Font = new Font("Arial", 10, FontStyle.Bold);
            endLabel.Location = new Point(20, 150);
            endLabel.Size = new Size(200, 20);
            mainPanel.Controls.Add(endLabel);

            CreateCoordinateControls(mainPanel, "X:", 180, out endX, 100);
            CreateCoordinateControls(mainPanel, "Y:", 210, out endY, 0);
            CreateCoordinateControls(mainPanel, "Z:", 240, out endZ, 0);

            // Настройки линии
            var settingsLabel = new Label();
            settingsLabel.Text = "Настройки линии:";
            settingsLabel.Font = new Font("Arial", 10, FontStyle.Bold);
            settingsLabel.Location = new Point(20, 280);
            settingsLabel.Size = new Size(200, 20);
            mainPanel.Controls.Add(settingsLabel);

            // Цвет
            var colorLabel = new Label();
            colorLabel.Text = "Цвет:";
            colorLabel.Location = new Point(20, 310);
            colorLabel.Size = new Size(50, 20);
            mainPanel.Controls.Add(colorLabel);

            colorComboBox = new ComboBox();
            colorComboBox.Location = new Point(80, 310);
            colorComboBox.Size = new Size(120, 20);
            colorComboBox.Items.AddRange(new string[] { "Black", "Red", "Green", "Blue", "Yellow", "Purple" });
            colorComboBox.SelectedIndex = 0;
            mainPanel.Controls.Add(colorComboBox);

            // Ширина
            var widthLabel = new Label();
            widthLabel.Text = "Ширина:";
            widthLabel.Location = new Point(210, 310);
            widthLabel.Size = new Size(50, 20);
            mainPanel.Controls.Add(widthLabel);

            widthNumeric = new NumericUpDown();
            widthNumeric.Location = new Point(270, 310);
            widthNumeric.Size = new Size(50, 20);
            widthNumeric.Minimum = 1;
            widthNumeric.Maximum = 20;
            widthNumeric.Value = 2;
            mainPanel.Controls.Add(widthNumeric);

            // Стиль
            var styleLabel = new Label();
            styleLabel.Text = "Стиль:";
            styleLabel.Location = new Point(20, 340);
            styleLabel.Size = new Size(50, 20);
            mainPanel.Controls.Add(styleLabel);

            styleComboBox = new ComboBox();
            styleComboBox.Location = new Point(80, 340);
            styleComboBox.Size = new Size(120, 20);
            styleComboBox.Items.AddRange(new string[] { "Сплошная", "Пунктир", "Точечная", "Штрих-пунктир" });
            styleComboBox.SelectedIndex = 0;
            mainPanel.Controls.Add(styleComboBox);

            // Кнопки
            var okButton = new Button();
            okButton.Text = "Добавить";
            okButton.Location = new Point(100, 380);
            okButton.Size = new Size(80, 30);
            okButton.DialogResult = DialogResult.OK;
            okButton.Click += (s, e) =>
            {
                StartPoint = new Point3D((float)startX.Value, (float)startY.Value, (float)startZ.Value);
                EndPoint = new Point3D((float)endX.Value, (float)endY.Value, (float)endZ.Value);
                LineColor = Color.FromName(colorComboBox.SelectedItem.ToString());
                LineWidth = (int)widthNumeric.Value;
                LineStyle = LineHelper.GetDashStyleFromIndex(styleComboBox.SelectedIndex);
            };
            mainPanel.Controls.Add(okButton);

            var cancelButton = new Button();
            cancelButton.Text = "Отмена";
            cancelButton.Location = new Point(200, 380);
            cancelButton.Size = new Size(80, 30);
            cancelButton.DialogResult = DialogResult.Cancel;
            mainPanel.Controls.Add(cancelButton);
        }

        private void CreateCoordinateControls(Panel parent, string label, int y, out NumericUpDown numeric, decimal defaultValue = 0)
        {
            var labelText = new Label();
            labelText.Text = label;
            labelText.Location = new Point(20, y);
            labelText.Size = new Size(30, 20);
            parent.Controls.Add(labelText);

            numeric = new NumericUpDown();
            numeric.Location = new Point(60, y);
            numeric.Size = new Size(80, 20);
            numeric.Minimum = -1000;
            numeric.Maximum = 1000;
            numeric.Value = defaultValue;
            parent.Controls.Add(numeric);
        }
    }
}