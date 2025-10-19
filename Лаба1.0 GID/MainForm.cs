using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Лаба1._0_GID
{
    public partial class MainForm : Form
    {
        private enum ToolMode { Add, Edit, Delete, Select, Grouping, Ungrouping, MovingGroup, TransformingGroup, MovingLineEnd }
        private bool isMovingLineEnd = false;
        private bool isMovingStartPoint = false;
        private ToolMode currentMode = ToolMode.Select;
        private List<Line> lines = new List<Line>();
        private List<Group> groups = new List<Group>();
        private Line selectedLine = null;
        private Group selectedGroup = null;
        private Point startPoint;
        private Point currentPoint;
        private bool isDrawing = false;
        private bool isMovingGroup = false;
        private Point groupMoveStart;

        // Компоненты
        private Panel canvasPanel;
        private Panel infoPanel;
        private Panel rightPanel;
        private Panel mainContainer;
        private ListBox groupListBox;
        private ListBox elementListBox;

        public MainForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Редактор линий";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Главный контейнер для canvas и info
            mainContainer = new Panel();
            mainContainer.Dock = DockStyle.Fill;
            this.Controls.Add(mainContainer);

            // 1. Сначала создаем правую панель с функциями
            rightPanel = new Panel();
            rightPanel.Dock = DockStyle.Right;
            rightPanel.Width = 300;
            rightPanel.BackColor = SystemColors.Control;
            rightPanel.AutoScroll = true;
            this.Controls.Add(rightPanel);

            // 2. Затем создаем нижнюю информационную панель
            infoPanel = new Panel();
            infoPanel.Dock = DockStyle.Bottom;
            infoPanel.Height = 150;
            infoPanel.BackColor = SystemColors.Control;
            infoPanel.BorderStyle = BorderStyle.FixedSingle;
            mainContainer.Controls.Add(infoPanel);

            // 3. Canvas занимает оставшееся пространство
            canvasPanel = new Panel();
            canvasPanel.Dock = DockStyle.Fill;
            canvasPanel.BackColor = Color.White;
            canvasPanel.BorderStyle = BorderStyle.FixedSingle;
            canvasPanel.Paint += CanvasPanel_Paint;
            canvasPanel.MouseDown += CanvasPanel_MouseDown;
            canvasPanel.MouseMove += CanvasPanel_MouseMove;
            canvasPanel.MouseUp += CanvasPanel_MouseUp;
            mainContainer.Controls.Add(canvasPanel);

            // 4. Настраиваем правую панель
            SetupRightPanel();

            // 5. Обновляем информацию
            SetupInfoPanel();
            UpdateGroupList();
        }
        private void SetupRightPanel()
        {
            // Сначала создаем панель для кнопок действий
            var actionsPanel = new Panel();
            actionsPanel.Dock = DockStyle.Top;
            actionsPanel.AutoSize = true; // Автоматическая высота по содержимому
            actionsPanel.BackColor = SystemColors.Control;
            rightPanel.Controls.Add(actionsPanel);

            // Контейнер для кнопок
            var buttonsContainer = new FlowLayoutPanel();
            buttonsContainer.FlowDirection = FlowDirection.TopDown;
            buttonsContainer.AutoSize = true;
            buttonsContainer.WrapContents = false;
            buttonsContainer.Padding = new Padding(5);
            actionsPanel.Controls.Add(buttonsContainer);

            // Создаем кнопки 
            var addButton = CreateActionButton("Добавить", buttonsContainer);
            addButton.Click += (s, e) => { currentMode = ToolMode.Add; selectedGroup = null; selectedLine = null; canvasPanel.Invalidate(); };

            var editButton = CreateActionButton("Редактировать", buttonsContainer);
            editButton.Click += (s, e) => {
                if (selectedLine != null) { EditButton_Click(s, e); }
                else {currentMode = ToolMode.Select; selectedGroup = null; canvasPanel.Invalidate(); MessageBox.Show("Выберите линию для редактирования");}
            };

            var deleteButton = CreateActionButton("Удалить", buttonsContainer);
            deleteButton.Click += DeleteButton_Click;

            var selectButton = CreateActionButton("Выбрать", buttonsContainer);
            selectButton.Click += (s, e) => { currentMode = ToolMode.Select; selectedGroup = null; selectedLine = null; canvasPanel.Invalidate(); };

            // Кнопки действий - РАБОТА С ГРУППАМИ
            var groupButton = CreateActionButton("Группировать", buttonsContainer);
            groupButton.Click += GroupButton_Click;

            var ungroupButton = CreateActionButton("Разгруппировать", buttonsContainer);
            ungroupButton.Click += UngroupButton_Click;

            var moveGroupButton = CreateActionButton("Переместить группу", buttonsContainer);
            moveGroupButton.Click += (s, e) => { if (selectedGroup != null) currentMode = ToolMode.MovingGroup; };

            var transformGroupButton = CreateActionButton("Трансформировать группу", buttonsContainer);
            transformGroupButton.Click += (s, e) => { if (selectedGroup != null) currentMode = ToolMode.TransformingGroup; };

            // Кнопки действий - СОХРАНЕНИЕ/ЗАГРУЗКА
            var saveButton = CreateActionButton("Сохранить группу", buttonsContainer);
            saveButton.Click += SaveGroupButton_Click;

            var loadButton = CreateActionButton("Загрузить группу", buttonsContainer);
            loadButton.Click += LoadGroupButton_Click;

            // Затем создаем панель для групп и элементов
            var groupsPanel = new Panel();
            groupsPanel.Dock = DockStyle.Top; 
            groupsPanel.Height = 200;
            groupsPanel.BackColor = SystemColors.Control;
            groupsPanel.BorderStyle = BorderStyle.FixedSingle;
            rightPanel.Controls.Add(groupsPanel);

            // Левая панель для групп
            var leftGroupPanel = new Panel();
            leftGroupPanel.Dock = DockStyle.Left;
            leftGroupPanel.Width = 145;
            groupsPanel.Controls.Add(leftGroupPanel);

            //// Надпись "Группы"
            //var groupsLabel = new Label();
            //groupsLabel.Text = "Группы";
            //groupsLabel.Font = new Font("Arial", 10, FontStyle.Bold);
            //groupsLabel.Dock = DockStyle.Top;
            //groupsLabel.Height = 20;
            //groupsLabel.TextAlign = ContentAlignment.MiddleCenter;
            //leftGroupPanel.Controls.Add(groupsLabel);

            // ListBox для групп
            groupListBox = new ListBox();
            groupListBox.Dock = DockStyle.Fill;
            groupListBox.SelectedIndexChanged += GroupListBox_SelectedIndexChanged;
            leftGroupPanel.Controls.Add(groupListBox);

            // Правая панель для элементов группы
            var rightGroupPanel = new Panel();
            rightGroupPanel.Dock = DockStyle.Right;
            rightGroupPanel.Width = 145;
            groupsPanel.Controls.Add(rightGroupPanel);

            // Надпись "Элементы группы"
            var elementsLabel = new Label();
            elementsLabel.Text = "Элементы группы";
            elementsLabel.Font = new Font("Arial", 10, FontStyle.Bold);
            elementsLabel.Dock = DockStyle.Top;
            elementsLabel.Height = 20;
            elementsLabel.TextAlign = ContentAlignment.MiddleCenter;
            rightGroupPanel.Controls.Add(elementsLabel);

            // ListBox для элементов группы
            elementListBox = new ListBox();
            elementListBox.Dock = DockStyle.Fill;
            elementListBox.SelectedIndexChanged += ElementListBox_SelectedIndexChanged;
            rightGroupPanel.Controls.Add(elementListBox);
        }

        // Измененный метод для создания кнопок в FlowLayoutPanel
        private Button CreateActionButton(string text, FlowLayoutPanel container)
        {
            var button = new Button();
            button.Text = text;
            button.Width = 270; // Фиксированная ширина
            button.Height = 35;
            button.Margin = new Padding(5);
            container.Controls.Add(button);
            return button;
        }

        private void GroupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (groupListBox.SelectedIndex >= 0)
            {
                selectedGroup = groups[groupListBox.SelectedIndex];
                selectedLine = null;
                UpdateElementList();
                canvasPanel.Invalidate();
                SetupInfoPanel();
            }
        }

        private void ElementListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedGroup != null && elementListBox.SelectedIndex >= 0)
            {
                selectedLine = selectedGroup.Lines[elementListBox.SelectedIndex];
                canvasPanel.Invalidate();
                SetupInfoPanel();
            }
        }

        private void UpdateGroupList()
        {
            groupListBox.Items.Clear();
            foreach (var group in groups)
            {
                groupListBox.Items.Add(group.Name);
            }
        }

        private void UpdateElementList()
        {
            elementListBox.Items.Clear();
            if (selectedGroup != null)
            {
                for (int i = 0; i < selectedGroup.Lines.Count; i++)
                {
                    elementListBox.Items.Add($"Линия {i + 1}");
                }
            }
        }

        private void SetupInfoPanel()
        {
            infoPanel.Controls.Clear();

            var titleLabel = new Label();
            titleLabel.Text = "Информация";
            titleLabel.Font = new Font("Arial", 12, FontStyle.Bold);
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Height = 30;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            infoPanel.Controls.Add(titleLabel);

            if (selectedGroup != null)
            {
                var groupLabel = new Label();
                groupLabel.Text = $"Группа: {selectedGroup.Name}";
                groupLabel.Location = new Point(10, 40);
                groupLabel.Size = new Size(200, 20);
                infoPanel.Controls.Add(groupLabel);

                var countLabel = new Label();
                countLabel.Text = $"Элементов: {selectedGroup.Lines.Count}";
                countLabel.Location = new Point(10, 70);
                countLabel.Size = new Size(200, 20);
                infoPanel.Controls.Add(countLabel);

                var centerLabel = new Label();
                centerLabel.Text = $"Центр: ({selectedGroup.Center.X}, {selectedGroup.Center.Y})";
                centerLabel.Location = new Point(10, 100);
                centerLabel.Size = new Size(200, 20);
                infoPanel.Controls.Add(centerLabel);
            }
            else if (selectedLine != null)
            {
                var colorLabel = new Label();
                colorLabel.Text = $"Цвет: {selectedLine.Color.Name}";
                colorLabel.Location = new Point(10, 40);
                colorLabel.Size = new Size(200, 20);
                infoPanel.Controls.Add(colorLabel);

                var widthLabel = new Label();
                widthLabel.Text = $"Ширина: {selectedLine.Width}px";
                widthLabel.Location = new Point(10, 70);
                widthLabel.Size = new Size(200, 20);
                infoPanel.Controls.Add(widthLabel);

                var styleLabel = new Label();
                styleLabel.Text = $"Стиль: {LineHelper.GetDashStyleName(selectedLine.DashStyle)}";
                styleLabel.Location = new Point(10, 100);
                styleLabel.Size = new Size(200, 20);
                infoPanel.Controls.Add(styleLabel);

                var lengthLabel = new Label();
                lengthLabel.Text = $"Длина: {LineHelper.CalculateLineLength(selectedLine):F2}px";
                lengthLabel.Location = new Point(10, 130);
                lengthLabel.Size = new Size(200, 20);
                infoPanel.Controls.Add(lengthLabel);
            }
            else
            {
                var noSelectionLabel = new Label();
                noSelectionLabel.Text = "Объект не выбран";
                noSelectionLabel.Location = new Point(10, 40);
                noSelectionLabel.Size = new Size(200, 20);
                infoPanel.Controls.Add(noSelectionLabel);
            }
        }

        private void CanvasPanel_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Рисуем все линии
            foreach (var line in lines)
            {
                using (var pen = new Pen(line.Color, line.Width))
                {
                    pen.DashStyle = line.DashStyle;
                    g.DrawLine(pen, line.StartPoint, line.EndPoint);
                }
            }

            // Рисуем группы
            foreach (var group in groups)
            {
                foreach (var line in group.Lines)
                {
                    using (var pen = new Pen(line.Color, line.Width))
                    {
                        pen.DashStyle = line.DashStyle;
                        g.DrawLine(pen, line.StartPoint, line.EndPoint);
                    }
                }

                // Рисуем bounding box для выделенной группы
                if (group == selectedGroup)
                {
                    var bounds = GetGroupBounds(group);
                    using (var pen = new Pen(Color.Red, 2))
                    {
                        pen.DashStyle = DashStyle.Dash;
                        g.DrawRectangle(pen, bounds);
                    }

                    // Рисуем центр группы
                    g.FillEllipse(Brushes.Blue, group.Center.X - 5, group.Center.Y - 5, 10, 10);
                    g.DrawEllipse(Pens.Black, group.Center.X - 5, group.Center.Y - 5, 10, 10);
                }
            }

            // Рисуем выделенную линию с маркерами
            if (selectedLine != null)
            {
                using (var pen = new Pen(Color.Red, selectedLine.Width + 2))
                {
                    pen.DashStyle = DashStyle.Dash;
                    g.DrawLine(pen, selectedLine.StartPoint, selectedLine.EndPoint);
                }

                // Маркеры для редактирования
                DrawHandle(g, selectedLine.StartPoint);
                DrawHandle(g, selectedLine.EndPoint);
            }

            // Рисуем текущую линию при добавлении
            if (isDrawing && currentMode == ToolMode.Add)
            {
                using (var pen = new Pen(Color.Black, 2))
                {
                    g.DrawLine(pen, startPoint, currentPoint);
                }
            }
        }

        private Rectangle GetGroupBounds(Group group)
        {
            if (group.Lines.Count == 0) return Rectangle.Empty;

            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;

            foreach (var line in group.Lines)
            {
                minX = Math.Min(minX, Math.Min(line.StartPoint.X, line.EndPoint.X));
                minY = Math.Min(minY, Math.Min(line.StartPoint.Y, line.EndPoint.Y));
                maxX = Math.Max(maxX, Math.Max(line.StartPoint.X, line.EndPoint.X));
                maxY = Math.Max(maxY, Math.Max(line.StartPoint.Y, line.EndPoint.Y));
            }

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        private void DrawHandle(Graphics g, Point point)
        {
            g.FillRectangle(Brushes.Red, point.X - 4, point.Y - 4, 8, 8);
            g.DrawRectangle(Pens.Black, point.X - 4, point.Y - 4, 8, 8);
        }

        private void CanvasPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (currentMode)
                {
                    case ToolMode.Add:
                        startPoint = e.Location;
                        currentPoint = e.Location;
                        isDrawing = true;
                        break;

                    case ToolMode.Select:
                        selectedLine = LineHelper.FindLineAtPoint(e.Location, lines);
                        selectedGroup = FindGroupAtPoint(e.Location);

                        // Проверяем, был ли клик на конце линии
                        if (selectedLine != null)
                        {
                            float startDist = LineHelper.DistanceBetweenPoints(e.Location, selectedLine.StartPoint);
                            float endDist = LineHelper.DistanceBetweenPoints(e.Location, selectedLine.EndPoint);
                            float grabTolerance = 10f; // Радиус захвата конца линии

                            if (startDist <= grabTolerance)
                            {
                                isMovingLineEnd = true;
                                isMovingStartPoint = true;
                            }
                            else if (endDist <= grabTolerance)
                            {
                                isMovingLineEnd = true;
                                isMovingStartPoint = false;
                            }
                        }

                        SetupInfoPanel();
                        UpdateElementList();
                        canvasPanel.Invalidate();
                        break;

                    case ToolMode.Grouping:
                        var lineToAdd = LineHelper.FindLineAtPoint(e.Location, lines);
                        if (lineToAdd != null && !IsLineInAnyGroup(lineToAdd))
                        {
                            if (selectedGroup == null)
                            {
                                selectedGroup = new Group { Name = $"Группа {groups.Count + 1}" };
                                groups.Add(selectedGroup);
                                UpdateGroupList();
                            }

                            selectedGroup.Lines.Add(lineToAdd);
                            lines.Remove(lineToAdd);
                            selectedGroup.CalculateCenter();
                            UpdateElementList();
                            canvasPanel.Invalidate();
                        }
                        break;

                    case ToolMode.MovingGroup:
                        if (selectedGroup != null && IsPointInGroup(e.Location, selectedGroup))
                        {
                            isMovingGroup = true;
                            groupMoveStart = e.Location;
                        }
                        break;

                    case ToolMode.TransformingGroup:
                        if (selectedGroup != null)
                        {
                            ShowTransformDialog();
                        }
                        break;
                }
            }
        }

        private void CanvasPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && currentMode == ToolMode.Add)
            {
                currentPoint = e.Location;
                canvasPanel.Invalidate();
            }
            else if (isMovingGroup && selectedGroup != null)
            {
                int dx = e.Location.X - groupMoveStart.X;
                int dy = e.Location.Y - groupMoveStart.Y;

                selectedGroup.Move(dx, dy);
                groupMoveStart = e.Location;
                canvasPanel.Invalidate();
            }
            else if (isMovingLineEnd && selectedLine != null)
            {
                if (isMovingStartPoint)
                {
                    selectedLine.StartPoint = e.Location;
                }
                else
                {
                    selectedLine.EndPoint = e.Location;
                }
                canvasPanel.Invalidate();
            }
        }

        private void CanvasPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isDrawing && currentMode == ToolMode.Add)
            {
                isDrawing = false;
                lines.Add(new Line
                {
                    StartPoint = startPoint,
                    EndPoint = e.Location,
                    Color = Color.Black,
                    Width = 2,
                    DashStyle = DashStyle.Solid
                });
                canvasPanel.Invalidate();
            }
            else if (e.Button == MouseButtons.Left && isMovingGroup)
            {
                isMovingGroup = false;
            }
            else if (e.Button == MouseButtons.Left && isMovingLineEnd)
            {
                isMovingLineEnd = false;
            }
        }

        private Group FindGroupAtPoint(Point point)
        {
            foreach (var group in groups)
            {
                if (IsPointInGroup(point, group))
                    return group;
            }
            return null;
        }

        private bool IsPointInGroup(Point point, Group group)
        {
            foreach (var line in group.Lines)
            {
                if (LineHelper.IsPointOnLine(point, line, 5))
                    return true;
            }
            return false;
        }

        private bool IsLineInAnyGroup(Line line)
        {
            foreach (var group in groups)
            {
                if (group.Lines.Contains(line))
                    return true;
            }
            return false;
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (selectedLine != null)
            {
                var editForm = new EditLineForm(selectedLine);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    selectedLine.Color = editForm.LineColor;
                    selectedLine.Width = editForm.LineWidth;
                    selectedLine.DashStyle = editForm.LineStyle;
                    canvasPanel.Invalidate();
                    SetupInfoPanel();
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (selectedLine != null)
            {
                // Удаляем линию из группы или из общего списка
                if (IsLineInAnyGroup(selectedLine))
                {
                    foreach (var group in groups)
                    {
                        if (group.Lines.Contains(selectedLine))
                        {
                            group.Lines.Remove(selectedLine);
                            lines.Add(selectedLine);
                            group.CalculateCenter();
                            break;
                        }
                    }
                }
                else
                {
                    lines.Remove(selectedLine);
                }
                selectedLine = null;
                canvasPanel.Invalidate();
                SetupInfoPanel();
            }
            else if (selectedGroup != null)
            {
                // Возвращаем все линии группы в общий список
                foreach (var line in selectedGroup.Lines)
                {
                    lines.Add(line);
                }
                groups.Remove(selectedGroup);
                selectedGroup = null;
                UpdateGroupList();
                canvasPanel.Invalidate();
                SetupInfoPanel();
            }
        }

        private void GroupButton_Click(object sender, EventArgs e)
        {
            currentMode = ToolMode.Grouping;
            selectedGroup = null;
            selectedLine = null;
            canvasPanel.Invalidate();
        }
        private void SaveGroupButton_Click(object sender, EventArgs e)
        {
            if (selectedGroup != null)
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Group Files|*.grp";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Сериализация группы в файл
                        using (FileStream fs = new FileStream(saveDialog.FileName, FileMode.Create))
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            formatter.Serialize(fs, selectedGroup);
                        }
                        MessageBox.Show("Группа успешно сохранена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении группы: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите группу для сохранения", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadGroupButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Group Files|*.grp";
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Десериализация группы из файла
                    using (FileStream fs = new FileStream(openDialog.FileName, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        Group loadedGroup = (Group)formatter.Deserialize(fs);

                        // Добавляем загруженную группу в список групп
                        groups.Add(loadedGroup);

                        // Обновляем интерфейс
                        UpdateGroupList();
                        canvasPanel.Invalidate();

                        MessageBox.Show("Группа успешно загружена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке группы: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void UngroupButton_Click(object sender, EventArgs e)
        {
            if (selectedGroup != null)
            {
                // Возвращаем все линии группы в общий список
                foreach (var line in selectedGroup.Lines)
                {
                    lines.Add(line);
                }
                groups.Remove(selectedGroup);
                selectedGroup = null;
                UpdateGroupList();
                canvasPanel.Invalidate();
                SetupInfoPanel();
            }
        }

        private void ShowTransformDialog()
        {
            var transformForm = new Form();
            transformForm.Text = "Трансформация группы";
            transformForm.Size = new Size(300, 250);
            transformForm.StartPosition = FormStartPosition.CenterParent;
            transformForm.FormBorderStyle = FormBorderStyle.FixedDialog;

            var mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Padding = new Padding(20);
            transformForm.Controls.Add(mainPanel);

            var rotateLabel = new Label();
            rotateLabel.Text = "Поворот (градусы):";
            rotateLabel.Location = new Point(20, 20);
            rotateLabel.Size = new Size(120, 20);
            mainPanel.Controls.Add(rotateLabel);

            var rotateNumeric = new NumericUpDown();
            rotateNumeric.Location = new Point(150, 20);
            rotateNumeric.Size = new Size(100, 20);
            rotateNumeric.Minimum = -360;
            rotateNumeric.Maximum = 360;
            mainPanel.Controls.Add(rotateNumeric);

            var scaleLabel = new Label();
            scaleLabel.Text = "Масштаб:";
            scaleLabel.Location = new Point(20, 60);
            scaleLabel.Size = new Size(120, 20);
            mainPanel.Controls.Add(scaleLabel);

            var scaleNumeric = new NumericUpDown();
            scaleNumeric.Location = new Point(150, 60);
            scaleNumeric.Size = new Size(100, 20);
            scaleNumeric.Value = 1;
            scaleNumeric.Minimum = (decimal)0.1;
            scaleNumeric.Maximum = 10;
            scaleNumeric.Increment = (decimal)0.1;
            mainPanel.Controls.Add(scaleNumeric);

            var mirrorLabel = new Label();
            mirrorLabel.Text = "Зеркалирование:";
            mirrorLabel.Location = new Point(20, 100);
            mirrorLabel.Size = new Size(120, 20);
            mainPanel.Controls.Add(mirrorLabel);

            var mirrorCombo = new ComboBox();
            mirrorCombo.Location = new Point(150, 100);
            mirrorCombo.Size = new Size(100, 20);
            mirrorCombo.Items.AddRange(new string[] { "Нет", "По горизонтали", "По вертикали" });
            mirrorCombo.SelectedIndex = 0;
            mainPanel.Controls.Add(mirrorCombo);

            var okButton = new Button();
            okButton.Text = "Применить";
            okButton.Location = new Point(60, 150);
            okButton.Size = new Size(80, 30);
            okButton.Click += (s, e) =>
            {
                // Применяем трансформации
                if (rotateNumeric.Value != 0)
                {
                    selectedGroup.Rotate((float)rotateNumeric.Value);
                }

                if (scaleNumeric.Value != 1)
                {
                    selectedGroup.Scale((float)scaleNumeric.Value, (float)scaleNumeric.Value);
                }

                if (mirrorCombo.SelectedIndex == 1)
                {
                    selectedGroup.Mirror(true);
                }
                else if (mirrorCombo.SelectedIndex == 2)
                {
                    selectedGroup.Mirror(false);
                }

                canvasPanel.Invalidate();
                transformForm.DialogResult = DialogResult.OK;
                transformForm.Close();
            };
            mainPanel.Controls.Add(okButton);

            var cancelButton = new Button();
            cancelButton.Text = "Отмена";
            cancelButton.Location = new Point(160, 150);
            cancelButton.Size = new Size(80, 30);
            cancelButton.DialogResult = DialogResult.Cancel;
            mainPanel.Controls.Add(cancelButton);

            transformForm.ShowDialog();
        }
    }
}

