using System;
using System.Drawing;
using System.Windows.Forms;

namespace Лаба1._0_GID
{
    public class TransformGroupForm : Form
    {
        public float RotationX { get; private set; }
        public float RotationY { get; private set; }
        public float RotationZ { get; private set; }
        public float TranslationX { get; private set; }
        public float TranslationY { get; private set; }
        public float TranslationZ { get; private set; }
        public bool MirrorX { get; private set; }
        public bool MirrorY { get; private set; }
        public bool MirrorZ { get; private set; }

        private TabControl tabControl;
        private NumericUpDown rotateX, rotateY, rotateZ;
        private NumericUpDown translateX, translateY, translateZ;
        private CheckBox mirrorX, mirrorY, mirrorZ;

        public TransformGroupForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "Трансформация группы";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Padding = new Padding(10);
            this.Controls.Add(mainPanel);

            // Создаем TabControl для разделения типов трансформаций
            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(tabControl);

            // Вкладка вращения
            var rotateTab = new TabPage("Вращение");
            rotateTab.Padding = new Padding(10);
            tabControl.Controls.Add(rotateTab);
            CreateRotationTab(rotateTab);

            // Вкладка смещения
            var translateTab = new TabPage("Смещение");
            translateTab.Padding = new Padding(10);
            tabControl.Controls.Add(translateTab);
            CreateTranslationTab(translateTab);

            // Вкладка зеркалирования
            var mirrorTab = new TabPage("Зеркалирование");
            mirrorTab.Padding = new Padding(10);
            tabControl.Controls.Add(mirrorTab);
            CreateMirrorTab(mirrorTab);

            // Кнопки применения
            var buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Height = 50;
            mainPanel.Controls.Add(buttonPanel);

            var applyButton = new Button();
            applyButton.Text = "Применить";
            applyButton.Location = new Point(120, 10);
            applyButton.Size = new Size(80, 30);
            applyButton.Click += (s, e) =>
            {
                // Сохраняем значения из активной вкладки
                SaveValues();
                this.DialogResult = DialogResult.OK;
                this.Close();
            };
            buttonPanel.Controls.Add(applyButton);

            var cancelButton = new Button();
            cancelButton.Text = "Отмена";
            cancelButton.Location = new Point(210, 10);
            cancelButton.Size = new Size(80, 30);
            cancelButton.DialogResult = DialogResult.Cancel;
            buttonPanel.Controls.Add(cancelButton);
        }

        private void CreateRotationTab(TabPage parent)
        {
            var label = new Label();
            label.Text = "Вращение вокруг осей (градусы):";
            label.Location = new Point(10, 10);
            label.Size = new Size(200, 20);
            parent.Controls.Add(label);

            CreateCoordinateControl(parent, "Ось X:", 50, out rotateX, -180, 180, 0);
            CreateCoordinateControl(parent, "Ось Y:", 90, out rotateY, -180, 180, 0);
            CreateCoordinateControl(parent, "Ось Z:", 130, out rotateZ, -180, 180, 0);

            var infoLabel = new Label();
            infoLabel.Text = "Положительные углы - против часовой стрелки";
            infoLabel.Location = new Point(10, 180);
            infoLabel.Size = new Size(300, 30);
            infoLabel.ForeColor = Color.Blue;
            parent.Controls.Add(infoLabel);
        }

        private void CreateTranslationTab(TabPage parent)
        {
            var label = new Label();
            label.Text = "Смещение по осям:";
            label.Location = new Point(10, 10);
            label.Size = new Size(200, 20);
            parent.Controls.Add(label);

            CreateCoordinateControl(parent, "По X:", 50, out translateX, -1000, 1000, 0);
            CreateCoordinateControl(parent, "По Y:", 90, out translateY, -1000, 1000, 0);
            CreateCoordinateControl(parent, "По Z:", 130, out translateZ, -1000, 1000, 0);

            var infoLabel = new Label();
            infoLabel.Text = "Введите значения смещения в единицах координат";
            infoLabel.Location = new Point(10, 180);
            infoLabel.Size = new Size(300, 30);
            infoLabel.ForeColor = Color.Blue;
            parent.Controls.Add(infoLabel);
        }

        private void CreateMirrorTab(TabPage parent)
        {
            var label = new Label();
            label.Text = "Зеркалирование относительно плоскостей:";
            label.Location = new Point(10, 10);
            label.Size = new Size(300, 20);
            parent.Controls.Add(label);

            mirrorX = CreateCheckBox(parent, "Зеркально относительно плоскости YZ (ось X)", 50);
            mirrorY = CreateCheckBox(parent, "Зеркально относительно плоскости XZ (ось Y)", 90);
            mirrorZ = CreateCheckBox(parent, "Зеркально относительно плоскости XY (ось Z)", 130);

            var infoLabel = new Label();
            infoLabel.Text = "Отражение относительно выбранных плоскостей координат";
            infoLabel.Location = new Point(10, 180);
            infoLabel.Size = new Size(300, 30);
            infoLabel.ForeColor = Color.Blue;
            parent.Controls.Add(infoLabel);
        }

        private void CreateCoordinateControl(Control parent, string label, int y, out NumericUpDown numeric,
                                           decimal min, decimal max, decimal defaultValue)
        {
            var labelControl = new Label();
            labelControl.Text = label;
            labelControl.Location = new Point(20, y);
            labelControl.Size = new Size(80, 20);
            parent.Controls.Add(labelControl);

            numeric = new NumericUpDown();
            numeric.Location = new Point(120, y);
            numeric.Size = new Size(100, 20);
            numeric.Minimum = min;
            numeric.Maximum = max;
            numeric.Value = defaultValue;
            numeric.DecimalPlaces = 2;
            parent.Controls.Add(numeric);
        }

        private CheckBox CreateCheckBox(Control parent, string text, int y)
        {
            var checkBox = new CheckBox();
            checkBox.Text = text;
            checkBox.Location = new Point(20, y);
            checkBox.Size = new Size(300, 20);
            parent.Controls.Add(checkBox);
            return checkBox;
        }

        private void SaveValues()
        {
            // Сохраняем значения из всех вкладок
            RotationX = (float)rotateX.Value;
            RotationY = (float)rotateY.Value;
            RotationZ = (float)rotateZ.Value;

            TranslationX = (float)translateX.Value;
            TranslationY = (float)translateY.Value;
            TranslationZ = (float)translateZ.Value;

            MirrorX = mirrorX.Checked;
            MirrorY = mirrorY.Checked;
            MirrorZ = mirrorZ.Checked;
        }
    }
}