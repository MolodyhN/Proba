using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лаба1._0_GID
{
    public static class SelectionManager
    {
        public static event EventHandler SelectionChanged;

        private static Group _selectedGroup;
        private static Line3D _selectedLine;

        public static Group SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                if (_selectedGroup != value)
                {
                    _selectedGroup = value;
                    OnSelectionChanged();
                }
            }
        }

        public static Line3D SelectedLine
        {
            get => _selectedLine;
            set
            {
                if (_selectedLine != value)
                {
                    _selectedLine = value;
                    OnSelectionChanged();
                }
            }
        }

        public static void ClearSelection()
        {
            _selectedGroup = null;
            _selectedLine = null;
            OnSelectionChanged();
        }

        private static void OnSelectionChanged()
        {
            SelectionChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
