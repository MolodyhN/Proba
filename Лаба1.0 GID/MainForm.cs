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
        private List<Line3D> lines = new List<Line3D>();
        private List<Group> groups = new List<Group>();
        private Camera camera = new Camera();
        private Point startPoint;
        private Point currentPoint;
        private bool isDrawing = false;
        private bool isMovingGroup = false;
        private bool isRotatingCamera = false;
        private Point cameraStartRotation;
        private Point groupMoveStart;
        private FlowLayoutPanel buttonsContainer;
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
            InitializeSelectionManager();
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
            buttonsContainer = new FlowLayoutPanel();
            buttonsContainer.FlowDirection = FlowDirection.TopDown;
            buttonsContainer.AutoSize = true;
            buttonsContainer.WrapContents = false;
            buttonsContainer.Padding = new Padding(5);
            actionsPanel.Controls.Add(buttonsContainer);

            // Создаем кнопки 
            var addButton = CreateActionButton("Добавить", buttonsContainer);
            addButton.Click += (s, e) => {
                // Сразу открываем диалог для создания новой линии
                var addForm = new AddLineForm();
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    lines.Add(new Line3D
                    {
                        StartPoint = addForm.StartPoint,
                        EndPoint = addForm.EndPoint,
                        Color = addForm.LineColor,
                        Width = addForm.LineWidth,
                        DashStyle = addForm.LineStyle
                    });
                    canvasPanel.Invalidate();
                }
            };

            var editButton = CreateActionButton("Редактировать", buttonsContainer);
            editButton.Click += (s, e) => {
                if (SelectionManager.SelectedLine != null) { EditButton_Click(s, e); }
                else {currentMode = ToolMode.Select; SelectionManager.SelectedGroup = null; canvasPanel.Invalidate(); MessageBox.Show("Выберите линию для редактирования");}
            };

            var deleteButton = CreateActionButton("Удалить", buttonsContainer);
            deleteButton.Click += DeleteButton_Click;

            var selectButton = CreateActionButton("Выбрать", buttonsContainer);
            selectButton.Click += (s, e) => { currentMode = ToolMode.Select; SelectionManager.SelectedGroup = null; SelectionManager.SelectedLine = null; canvasPanel.Invalidate(); };

            // Кнопки действий - РАБОТА С ГРУППАМИ
            var groupButton = CreateActionButton("Группировать", buttonsContainer);
            groupButton.Click += GroupButton_Click;

            var ungroupButton = CreateActionButton("Разгруппировать", buttonsContainer);
            ungroupButton.Click += UngroupButton_Click;

            //var moveGroupButton = CreateActionButton("Переместить группу", buttonsContainer);
            //moveGroupButton.Click += (s, e) => { if (selectedGroup != null) currentMode = ToolMode.MovingGroup; };

            var transformGroupButton = CreateActionButton("Трансформировать группу", buttonsContainer);
            transformGroupButton.Click += (s, e) => { if (SelectionManager.SelectedGroup != null) currentMode = ToolMode.TransformingGroup;
                else{ MessageBox.Show("Сначала выберите группу для трансформации");}
            };

            // Кнопки действий - СОХРАНЕНИЕ/ЗАГРУЗКА
            var saveButton = CreateActionButton("Сохранить группу", buttonsContainer);
            saveButton.Click += SaveGroupButton_Click;

            var loadButton = CreateActionButton("Загрузить группу", buttonsContainer);
            loadButton.Click += LoadGroupButton_Click;
            SetupCameraControls();
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
            //var elementsLabel = new Label();
            //elementsLabel.Text = "Элементы группы";
            //elementsLabel.Font = new Font("Arial", 10, FontStyle.Bold);
            //elementsLabel.Dock = DockStyle.Top;
            //elementsLabel.Height = 20;
            //elementsLabel.TextAlign = ContentAlignment.MiddleCenter;
            //rightGroupPanel.Controls.Add(elementsLabel);

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
                SelectionManager.SelectedGroup = groups[groupListBox.SelectedIndex];
            }
            else
            {
                SelectionManager.SelectedGroup = null;
            }
            groupListBox.Invalidate();
            elementListBox.Invalidate();
        }

        private void ElementListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectionManager.SelectedGroup != null && elementListBox.SelectedIndex >= 0)
            {
                var selectedLine = SelectionManager.SelectedGroup.Lines[elementListBox.SelectedIndex];
                SelectionManager.SelectedLine = selectedLine;
                canvasPanel.Invalidate();
                SetupInfoPanel();
            }
            else
            {
                SelectionManager.SelectedLine = null;
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
            var currentSelectedGroup = SelectionManager.SelectedGroup;
            if (currentSelectedGroup != null)
            {
                for (int i = 0; i < currentSelectedGroup.Lines.Count; i++)
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

            // Используем SelectionManager вместо локальных переменных
            var currentSelectedGroup = SelectionManager.SelectedGroup;
            var currentSelectedLine = SelectionManager.SelectedLine;

            if (currentSelectedGroup != null)
            {
                var groupLabel = new Label();
                groupLabel.Text = $"Группа: {currentSelectedGroup.Name}";
                groupLabel.Location = new Point(10, 40);
                groupLabel.Size = new Size(200, 20);
                infoPanel.Controls.Add(groupLabel);

                var countLabel = new Label();
                countLabel.Text = $"Элементов: {currentSelectedGroup.Lines.Count}";
                countLabel.Location = new Point(10, 70);
                countLabel.Size = new Size(200, 20);
                infoPanel.Controls.Add(countLabel);

                var centerLabel = new Label();
                centerLabel.Text = $"Центр: ({currentSelectedGroup.Center.X:F1}, {currentSelectedGroup.Center.Y:F1}, {currentSelectedGroup.Center.Z:F1})";
                centerLabel.Location = new Point(10, 100);
                centerLabel.Size = new Size(200, 20);
                infoPanel.Controls.Add(centerLabel);
            }
            else if (currentSelectedLine != null)
            {
                var colorLabel = new Label();
                colorLabel.Text = $"Цвет: {currentSelectedLine.Color.Name}";
                colorLabel.Location = new Point(10, 40);
                colorLabel.Size = new Size(200, 20);
                infoPanel.Controls.Add(colorLabel);

                var widthLabel = new Label();
                widthLabel.Text = $"Ширина: {currentSelectedLine.Width}px";
                widthLabel.Location = new Point(10, 70);
                widthLabel.Size = new Size(200, 20);
                infoPanel.Controls.Add(widthLabel);

                var styleLabel = new Label();
                styleLabel.Text = $"Стиль: {LineHelper.GetDashStyleName(currentSelectedLine.DashStyle)}";
                styleLabel.Location = new Point(10, 100);
                styleLabel.Size = new Size(200, 20);
                infoPanel.Controls.Add(styleLabel);

                var lengthLabel = new Label();
                lengthLabel.Text = $"Длина: {LineHelper.CalculateLineLength3D(currentSelectedLine):F2}px";
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
            g.Clear(Color.White);
            DrawCoordinateAxes(g, camera, canvasPanel.Size);

            var currentSelectedGroup = SelectionManager.SelectedGroup;
            var currentSelectedLine = SelectionManager.SelectedLine;

            // 1. Сначала рисуем все обычные линии (не в группах)
            foreach (var line in lines)
            {
                ProjectionHelper.Draw3DLine(g, line.StartPoint, line.EndPoint,
                                          line.Color,
                                          line.Width,
                                          line.DashStyle,
                                          camera, canvasPanel.Size);
            }

            // 2. Затем рисуем все линии групп (обычным стилем)
            foreach (var group in groups)
            {
                foreach (var line in group.Lines)
                {
                    ProjectionHelper.Draw3DLine(g, line.StartPoint, line.EndPoint,
                                              line.Color,
                                              line.Width,
                                              line.DashStyle,
                                              camera, canvasPanel.Size);
                }
            }

            // 3. Теперь рисуем выделение ПОВЕРХ всех линий

            // Выделение обычных линий (красный пунктир)
            foreach (var line in lines)
            {
                if (line.IsSelected)
                {
                    ProjectionHelper.Draw3DLine(g, line.StartPoint, line.EndPoint,
                                              Color.Red,
                                              line.Width + 2,
                                              DashStyle.Dash,
                                              camera, canvasPanel.Size);
                }
            }

            // Выделение групп (красный пунктир для всех линий в группе)
            foreach (var group in groups)
            {
                if (group.IsSelected)
                {
                    foreach (var line in group.Lines)
                    {
                        ProjectionHelper.Draw3DLine(g, line.StartPoint, line.EndPoint,
                                                  Color.Red,
                                                  line.Width + 2,
                                                  DashStyle.Dash,
                                                  camera, canvasPanel.Size);
                    }
                }
            }

            // Выделение отдельных линий в группах
            foreach (var group in groups)
            {
                foreach (var line in group.Lines)
                {
                    if (line.IsSelected && !group.IsSelected)
                    {
                        ProjectionHelper.Draw3DLine(g, line.StartPoint, line.EndPoint,
                                                  Color.Red,
                                                  line.Width + 2,
                                                  DashStyle.Dash,
                                                  camera, canvasPanel.Size);
                    }
                }
            }

            // 4. Рисуем маркеры для выделенной линии (самый верхний слой)
            if (currentSelectedLine != null && currentSelectedGroup == null)
            {
                var start2D = ProjectionHelper.Project3DTo2D(currentSelectedLine.StartPoint, camera, canvasPanel.Size);
                var end2D = ProjectionHelper.Project3DTo2D(currentSelectedLine.EndPoint, camera, canvasPanel.Size);

                DrawHandle(g, start2D);
                DrawHandle(g, end2D);
            }

            if (isDrawing && currentMode == ToolMode.Add)
            {
                using (var pen = new Pen(Color.Black, 2))
                {
                    g.DrawLine(pen, startPoint, currentPoint);
                }
            }
        }
        private void InitializeSelectionManager()
        {
            SelectionManager.SelectionChanged += OnSelectionChanged;
        }
        private void OnSelectionChanged(object sender, EventArgs e)
        {
            SyncSelectionWithListBoxes();
            canvasPanel.Invalidate();
            SetupInfoPanel();
        }
        private void SyncSelectionWithListBoxes()
        {
            if (SelectionManager.SelectedGroup != null)
            {
                int groupIndex = groups.IndexOf(SelectionManager.SelectedGroup);
                if (groupIndex >= 0 && groupIndex < groupListBox.Items.Count)
                {
                    groupListBox.SelectedIndex = groupIndex;
                }
                UpdateElementList();
                if (SelectionManager.SelectedLine != null &&
                    SelectionManager.SelectedGroup.Lines.Contains(SelectionManager.SelectedLine))
                {
                    int lineIndex = SelectionManager.SelectedGroup.Lines.IndexOf(SelectionManager.SelectedLine);
                    if (lineIndex >= 0 && lineIndex < elementListBox.Items.Count)
                    {
                        elementListBox.SelectedIndex = lineIndex;
                    }
                }
                else
                {
                    elementListBox.ClearSelected();
                }
            }
            else
            {
                groupListBox.ClearSelected();
                elementListBox.ClearSelected();
            }
        }
        private Rectangle GetGroupBounds(Group group)
        {
            if (group.Lines.Count == 0) return Rectangle.Empty;

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            foreach (var line in group.Lines)
            {
                minX = Math.Min(minX, Math.Min(line.StartPoint.X, line.EndPoint.X));
                minY = Math.Min(minY, Math.Min(line.StartPoint.Y, line.EndPoint.Y));
                maxX = Math.Max(maxX, Math.Max(line.StartPoint.X, line.EndPoint.X));
                maxY = Math.Max(maxY, Math.Max(line.StartPoint.Y, line.EndPoint.Y));
            }

            // Преобразуем float в int для Rectangle
            return new Rectangle(
                (int)minX,
                (int)minY,
                (int)(maxX - minX),
                (int)(maxY - minY)
            );
        }

        private void DrawHandle(Graphics g, Point point)
        {
            g.FillRectangle(Brushes.Red, point.X - 4, point.Y - 4, 8, 8);
            g.DrawRectangle(Pens.Black, point.X - 4, point.Y - 4, 8, 8);
        }

        private void CanvasPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Вращение камеры при зажатой правой кнопке
                cameraStartRotation = new Point(e.X, e.Y);
                isRotatingCamera = true;
                canvasPanel.Cursor = Cursors.SizeAll;
            }
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
                        // Сначала проверяем клик на линию (вне групп)
                        var clickedLine = LineHelper.FindLineAtPoint(e.Location, lines, camera, canvasPanel.Size);
                        if (clickedLine != null)
                        {
                            SelectionManager.SelectedLine = clickedLine;
                            SelectionManager.SelectedGroup = null; // Сбрасываем выделение группы
                            break;
                        }

                        // Затем проверяем клик на линию внутри групп
                        foreach (var group in groups)
                        {
                            foreach (var line in group.Lines)
                            {
                                if (LineHelper.IsPointOn3DLine(e.Location, line, 5, camera, canvasPanel.Size))
                                {
                                    SelectionManager.SelectedLine = line;
                                    SelectionManager.SelectedGroup = group;
                                    break;
                                }
                            }
                            if (SelectionManager.SelectedLine != null) break;
                        }

                        // Если нашли линию в группе, выходим
                        if (SelectionManager.SelectedLine != null) break;

                        // Затем проверяем клик на саму группу (по bounding box или другим линиям)
                        var clickedGroup = FindGroupAtPoint(e.Location);
                        if (clickedGroup != null)
                        {
                            SelectionManager.SelectedGroup = clickedGroup;
                            SelectionManager.SelectedLine = null; // Сбрасываем выделение линии
                            break;
                        }

                        // Если клик мимо всего - сбрасываем выделение
                        SelectionManager.ClearSelection();
                        SetupInfoPanel();
                        UpdateElementList();
                        canvasPanel.Invalidate();
                        break;

                    case ToolMode.Grouping:
                        var lineToAdd = LineHelper.FindLineAtPoint(e.Location, lines, camera, canvasPanel.Size);
                        if (lineToAdd != null && !IsLineInAnyGroup(lineToAdd))
                        {
                            if (SelectionManager.SelectedGroup == null)
                            {
                                SelectionManager.SelectedGroup = new Group { Name = $"Группа {groups.Count + 1}" };
                                groups.Add(SelectionManager.SelectedGroup);
                                UpdateGroupList();
                            }

                            SelectionManager.SelectedGroup.Lines.Add(lineToAdd);
                            lines.Remove(lineToAdd);
                            SelectionManager.SelectedGroup.CalculateCenter();
                            UpdateElementList();
                            canvasPanel.Invalidate();
                        }
                        break;

                    case ToolMode.TransformingGroup:
                        if (SelectionManager.SelectedGroup != null)
                        {
                            ShowTransformDialog();
                        }
                        break;
                }
            }
        }

        private void CanvasPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRotatingCamera)
            {
                // Вращение камеры
                float deltaX = (cameraStartRotation.X - e.X) * 0.01f;
                float deltaY = (cameraStartRotation.Y - e.Y) * 0.01f;

                camera.Rotate(deltaX, deltaY);
                cameraStartRotation = new Point(e.X, e.Y);
                canvasPanel.Invalidate();
            }
            if (isDrawing && currentMode == ToolMode.Add)
            {
                currentPoint = e.Location;
                canvasPanel.Invalidate();
            }
            else if (isMovingGroup && SelectionManager.SelectedGroup != null)
            {
                int dx = e.Location.X - groupMoveStart.X;
                int dy = e.Location.Y - groupMoveStart.Y;

                float scaleFactor = 0.5f; 
                float scaledDx = dx * scaleFactor;
                float scaledDy = -dy * scaleFactor; 

                SelectionManager.SelectedGroup.Move3D(scaledDx, scaledDy, 0);
                groupMoveStart = e.Location;
                canvasPanel.Invalidate();
            }
            else if (isMovingLineEnd && SelectionManager.SelectedLine != null)
            {
                float x = e.Location.X - canvasPanel.Width / 2;
                float y = -(e.Location.Y - canvasPanel.Height / 2); 
                if (isMovingStartPoint)
                {
                    SelectionManager.SelectedLine.StartPoint = new Point3D(x, y, SelectionManager.SelectedLine.StartPoint.Z); ;
                }
                else
                {
                    SelectionManager.SelectedLine.EndPoint = new Point3D(x, y, SelectionManager.SelectedLine.EndPoint.Z);
                }
                canvasPanel.Invalidate();
            }
        }

        private void CanvasPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && isRotatingCamera)
            {
                isRotatingCamera = false;
                canvasPanel.Cursor = Cursors.Default;
            }
            if (e.Button == MouseButtons.Left && isDrawing && currentMode == ToolMode.Add)
            {
                isDrawing = false;
                float startX = startPoint.X - canvasPanel.Width / 2;
                float startY = -(startPoint.Y - canvasPanel.Height / 2); // Инвертируем Y
                float startZ = 0; // Z координата по умолчанию

                float endX = e.Location.X - canvasPanel.Width / 2;
                float endY = -(e.Location.Y - canvasPanel.Height / 2); // Инвертируем Y
                float endZ = 0; // Z координата по умолчанию

                lines.Add(new Line3D
                {
                    StartPoint = new Point3D(startX, startY, startZ), // Point3D вместо Point
                    EndPoint = new Point3D(endX, endY, endZ),         // Point3D вместо Point
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
                if (LineHelper.IsPointOn3DLine(point, line, 5, camera, canvasPanel.Size))
                    return true;
            }
            return false;
        }

        private bool IsLineInAnyGroup(Line3D line)
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
            if (SelectionManager.SelectedLine != null)
            {
                var editForm = new EditLineForm(SelectionManager.SelectedLine);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    SelectionManager.SelectedLine.Color = editForm.LineColor;
                    SelectionManager.SelectedLine.Width = editForm.LineWidth;
                    SelectionManager.SelectedLine.DashStyle = editForm.LineStyle;
                    canvasPanel.Invalidate();
                    SetupInfoPanel();
                }
            }
        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (SelectionManager.SelectedLine != null)
            {
                var lineToDelete = SelectionManager.SelectedLine;
                if (IsLineInAnyGroup(lineToDelete))
                {
                    foreach (var group in groups)
                    {
                        if (group.Lines.Contains(lineToDelete))
                        {
                            group.Lines.Remove(lineToDelete);
                            lines.Add(lineToDelete);
                            group.CalculateCenter();
                            break;
                        }
                    }
                }
                else
                {
                    lines.Remove(lineToDelete);
                }
                SelectionManager.ClearSelection();
                canvasPanel.Invalidate();
                SetupInfoPanel();
            }
            else if (SelectionManager.SelectedGroup != null)
            {
                var groupToDelete = SelectionManager.SelectedGroup;
                // Возвращаем все линии группы в общий список
                foreach (var line in groupToDelete.Lines)
                {
                    lines.Add(line);
                }
                groups.Remove(groupToDelete);
                SelectionManager.ClearSelection();
                UpdateGroupList();
                canvasPanel.Invalidate();
                SetupInfoPanel();
            }
        }
        private void GroupButton_Click(object sender, EventArgs e)
        {
            currentMode = ToolMode.Grouping;
            SelectionManager.SelectedGroup = null;
            SelectionManager.SelectedLine = null;
            canvasPanel.Invalidate();
        }
        private void SaveGroupButton_Click(object sender, EventArgs e)
        {
            if (SelectionManager.SelectedGroup != null)
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
                            formatter.Serialize(fs, SelectionManager.SelectedGroup);
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
            if (SelectionManager.SelectedGroup != null)
            {
                var groupToUngroup = SelectionManager.SelectedGroup;

                foreach (var line in groupToUngroup.Lines)
                {
                    lines.Add(line);
                }
                groups.Remove(groupToUngroup);
                SelectionManager.ClearSelection();
                UpdateGroupList();
                canvasPanel.Invalidate();
                SetupInfoPanel();
            }
        }
        private void SetupCameraControls()
        {
            var separator = new Label();
            separator.Text = "────────────────────";
            separator.Margin = new Padding(5, 10, 5, 5);
            separator.Width = 270;
            separator.Height = 15;
            separator.TextAlign = ContentAlignment.MiddleCenter;
            buttonsContainer.Controls.Add(separator);

            var cameraLabel = new Label();
            cameraLabel.Text = "Управление камерой";
            cameraLabel.Font = new Font("Arial", 9, FontStyle.Bold);
            cameraLabel.Margin = new Padding(5, 5, 5, 5);
            cameraLabel.Width = 270;
            cameraLabel.Height = 20;
            cameraLabel.TextAlign = ContentAlignment.MiddleCenter;
            buttonsContainer.Controls.Add(cameraLabel);

            var zoomInButton = CreateActionButton("Приблизить", buttonsContainer);
            zoomInButton.Click += (s, e) => { camera.Zoom *= 1.2f; canvasPanel.Invalidate(); };

            var zoomOutButton = CreateActionButton("Отдалить", buttonsContainer);
            zoomOutButton.Click += (s, e) => { camera.Zoom /= 1.2f; canvasPanel.Invalidate(); };

            var resetViewButton = CreateActionButton("Сброс вида", buttonsContainer);
            resetViewButton.Click += (s, e) => { ResetCamera(); canvasPanel.Invalidate(); };

            var testCubeButton = CreateActionButton("Тестовый куб", buttonsContainer);
            testCubeButton.Click += (s, e) => { CreateTestCube(); canvasPanel.Invalidate(); };
        }
        private void CreateTestCube()
        {
            var cubeGroup = new Group { Name = $"Куб {groups.Count + 1}" };

            float size = 50;
            Color cubeColor = Color.Black;

            // Нижняя грань (Z = 0)
            cubeGroup.Lines.Add(CreateCubeLine(0, 0, 0, size, 0, 0, cubeColor));
            cubeGroup.Lines.Add(CreateCubeLine(0, 0, 0, 0, 0, size, cubeColor));
            cubeGroup.Lines.Add(CreateCubeLine(size, 0, 0, size, 0, size, cubeColor));
            cubeGroup.Lines.Add(CreateCubeLine(0, 0, size, size, 0, size, cubeColor));

            // Верхняя грань (Z = size)
            cubeGroup.Lines.Add(CreateCubeLine(0, size, 0, size, size, 0, cubeColor));
            cubeGroup.Lines.Add(CreateCubeLine(0, size, 0, 0, size, size, cubeColor));
            cubeGroup.Lines.Add(CreateCubeLine(size, size, 0, size, size, size, cubeColor));
            cubeGroup.Lines.Add(CreateCubeLine(0, size, size, size, size, size, cubeColor));

            // Вертикальные ребра
            cubeGroup.Lines.Add(CreateCubeLine(0, 0, 0, 0, size, 0, cubeColor));
            cubeGroup.Lines.Add(CreateCubeLine(size, 0, 0, size, size, 0, cubeColor));
            cubeGroup.Lines.Add(CreateCubeLine(0, 0, size, 0, size, size, cubeColor));
            cubeGroup.Lines.Add(CreateCubeLine(size, 0, size, size, size, size, cubeColor));


            cubeGroup.CalculateCenter();
            groups.Add(cubeGroup);

            UpdateGroupList();
            UpdateElementList();
            SetupInfoPanel();
            canvasPanel.Invalidate();
        }

        private Line3D CreateCubeLine(float x1, float y1, float z1, float x2, float y2, float z2, Color color)
        {
            return new Line3D
            {
                StartPoint = new Point3D(x1, y1, z1),
                EndPoint = new Point3D(x2, y2, z2),
                Color = color,
                Width = 2,
                DashStyle = DashStyle.Solid
            };
        }

        private void ResetCamera()
        {
            camera.Reset();
        }
        private void DrawCoordinateAxes(Graphics g, Camera camera, Size viewportSize)
        {
            var origin = new Point3D(0, 0, 0);
            var xAxis = new Point3D(200, 0, 0);    
            var yAxis = new Point3D(0, 200, 0);   
            var zAxis = new Point3D(0, 0, 200); 

            ProjectionHelper.Draw3DLine(g, origin, xAxis, Color.Red, 3, DashStyle.Solid, camera, viewportSize);
            ProjectionHelper.Draw3DLine(g, origin, yAxis, Color.Green, 3, DashStyle.Solid, camera, viewportSize);
            ProjectionHelper.Draw3DLine(g, origin, zAxis, Color.Blue, 3, DashStyle.Solid, camera, viewportSize);

            DrawAxisLabel(g, "X", xAxis, Color.Red, camera, viewportSize);
            DrawAxisLabel(g, "Y", yAxis, Color.Green, camera, viewportSize);
            DrawAxisLabel(g, "Z", zAxis, Color.Blue, camera, viewportSize);
            DrawAxisTicks(g, camera, viewportSize);
        }
        private void DrawAxisLabel(Graphics g, string text, Point3D position, Color color, Camera camera, Size viewportSize)
        {
            var point2D = ProjectionHelper.Project3DTo2D(position, camera, viewportSize);
            using (var brush = new SolidBrush(color))
            using (var font = new Font("Arial", 12, FontStyle.Bold))
            {
                g.DrawString(text, font, brush, point2D.X + 8, point2D.Y + 8);
            }
            DrawAxisArrow(g, position, color, camera, viewportSize);
        }

        private void DrawAxisArrow(Graphics g, Point3D position, Color color, Camera camera, Size viewportSize)
        {
            var end2D = ProjectionHelper.Project3DTo2D(position, camera, viewportSize);
            var origin = new Point3D(0, 0, 0);
            var origin2D = ProjectionHelper.Project3DTo2D(origin, camera, viewportSize);

            float dx = end2D.X - origin2D.X;
            float dy = end2D.Y - origin2D.Y;
            float length = (float)Math.Sqrt(dx * dx + dy * dy);

            if (length > 0)
            {
                dx /= length;
                dy /= length;
                float arrowSize = 10f;
                float angle = (float)Math.PI / 6;

                PointF arrow1 = new PointF(
                    end2D.X - dx * arrowSize + dy * arrowSize * (float)Math.Tan(angle),
                    end2D.Y - dy * arrowSize - dx * arrowSize * (float)Math.Tan(angle)
                );

                PointF arrow2 = new PointF(
                    end2D.X - dx * arrowSize - dy * arrowSize * (float)Math.Tan(angle),
                    end2D.Y - dy * arrowSize + dx * arrowSize * (float)Math.Tan(angle)
                );

                using (var pen = new Pen(color, 2))
                using (var brush = new SolidBrush(color))
                {
                    g.DrawLine(pen, end2D, arrow1);
                    g.DrawLine(pen, end2D, arrow2);
                }
            }
        }
        private void DrawAxisTicks(Graphics g, Camera camera, Size viewportSize)
        {
            var tickColor = Color.DarkGray;
            float tickSize = 2.0f;
            int tickCount = 20; // Количество делений
            float tickStep = 10.0f; // Расстояние между делениями

            // Деления на оси X (красная)
            for (int i = 1; i <= tickCount; i++)
            {
                float pos = i * tickStep;

                // Деление на оси X
                var tickStartX = new Point3D(pos, -tickSize, 0);
                var tickEndX = new Point3D(pos, tickSize, 0);
                ProjectionHelper.Draw3DLine(g, tickStartX, tickEndX, Color.Red, 2, DashStyle.Solid, camera, viewportSize);

                if (i % 5 == 0)
                {
                    DrawTickLabel(g, pos.ToString(), new Point3D(pos, -12, 0), Color.Red, camera, viewportSize);
                }
            }

            // Деления на оси Y (зеленая)
            for (int i = 1; i <= tickCount; i++)
            {
                float pos = i * tickStep;

                // Деление на оси Y
                var tickStartY = new Point3D(-tickSize, pos, 0);
                var tickEndY = new Point3D(tickSize, pos, 0);
                ProjectionHelper.Draw3DLine(g, tickStartY, tickEndY, Color.Green, 2, DashStyle.Solid, camera, viewportSize);

                // Подпись деления (каждое 5-е)
                if (i % 5 == 0)
                {
                    DrawTickLabel(g, pos.ToString(), new Point3D(-25, pos, 0), Color.Green, camera, viewportSize);
                }
            }

            // Деления на оси Z (синяя)
            for (int i = 1; i <= tickCount; i++)
            {
                float pos = i * tickStep;

                // Деление на оси Z
                var tickStartZ = new Point3D(-tickSize, 0, pos);
                var tickEndZ = new Point3D(tickSize, 0, pos);
                ProjectionHelper.Draw3DLine(g, tickStartZ, tickEndZ, Color.Blue, 2, DashStyle.Solid, camera, viewportSize);

                // Подпись деления (каждое 5-е)
                if (i % 5 == 0)
                {
                    DrawTickLabel(g, pos.ToString(), new Point3D(-25, 0, pos), Color.Blue, camera, viewportSize);
                }
            }
        }

        private void DrawTickLabel(Graphics g, string text, Point3D position, Color color, Camera camera, Size viewportSize)
        {
            var point2D = ProjectionHelper.Project3DTo2D(position, camera, viewportSize);
            using (var brush = new SolidBrush(color))
            using (var font = new Font("Arial", 8, FontStyle.Bold))
            {
                // Центрируем текст
                var textSize = g.MeasureString(text, font);
                g.DrawString(text, font, brush, point2D.X - textSize.Width / 2, point2D.Y - textSize.Height / 2);
            }
        }
        private void ShowTransformDialog()
        {
            if (SelectionManager.SelectedGroup == null) return;

            var transformForm = new TransformGroupForm();
            if (transformForm.ShowDialog() == DialogResult.OK)
            {
                var selectedGroup = SelectionManager.SelectedGroup;

                // Применяем трансформации
                if (transformForm.TranslationX != 0 || transformForm.TranslationY != 0 || transformForm.TranslationZ != 0)
                {
                    SelectionManager.SelectedGroup.Translate(
                        transformForm.TranslationX,
                        transformForm.TranslationY,
                        transformForm.TranslationZ
                    );
                }

                if (transformForm.RotationX != 0 || transformForm.RotationY != 0 || transformForm.RotationZ != 0)
                {
                    selectedGroup.Rotate3D(
                        transformForm.RotationX,
                        transformForm.RotationY,
                        transformForm.RotationZ
                    );
                }

                if (transformForm.MirrorX || transformForm.MirrorY || transformForm.MirrorZ)
                {
                    selectedGroup.Mirror3D(
                        transformForm.MirrorX,
                        transformForm.MirrorY,
                        transformForm.MirrorZ
                    );
                }

                canvasPanel.Invalidate();
                SetupInfoPanel();
            }
        }
    }
}

