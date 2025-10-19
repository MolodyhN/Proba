import tkinter as tk
from tkinter import ttk
from geometry_calculations import find_bisector, find_median, find_altitude

class GeometryApp:
    def __init__(self, root):
        self.root = root
        self.root.title("Геометрические вычисления в пространстве")
        self.root.geometry("600x500")
        
        self.create_widgets()
    
    def create_widgets(self):
        # Создаем notebook для вкладок
        self.notebook = ttk.Notebook(self.root)
        self.notebook.pack(fill='both', expand=True, padx=10, pady=10)
        
        # Создаем вкладки
        self.create_bisector_tab()
        self.create_median_tab()
        self.create_altitude_tab()
        
        # Кнопка выхода
        ttk.Button(self.root, text="Выход", command=self.root.quit).pack(pady=5)
    
    def create_bisector_tab(self):
        """Вкладка для вычисления биссектрис"""
        frame = ttk.Frame(self.notebook)
        self.notebook.add(frame, text="Биссектрисы")
        
        # Заголовок
        ttk.Label(frame, text="Нахождение биссектрис двух прямых в пространстве", 
                 font=('Arial', 11, 'bold')).pack(pady=10)
        
        # Ввод данных
        input_frame = ttk.LabelFrame(frame, text="Входные данные")
        input_frame.pack(fill='x', padx=10, pady=5)
        
        ttk.Label(input_frame, text="Коэффициенты прямых α и β (A1 B1 C1 A2 B2 C2):").pack(pady=5)
        self.bisector_entry = ttk.Entry(input_frame, width=50, font=('Arial', 10))
        self.bisector_entry.pack(pady=5)
        self.bisector_entry.insert(0, "1 0 0 0 1 0")
        
        # Кнопка расчета
        ttk.Button(frame, text="Рассчитать биссектрисы", 
                  command=self.calculate_bisector).pack(pady=10)
        
        # Вывод результатов
        output_frame = ttk.LabelFrame(frame, text="Результаты")
        output_frame.pack(fill='both', expand=True, padx=10, pady=5)
        
        self.bisector_result = tk.Text(output_frame, height=8, width=70, font=('Arial', 10))
        scrollbar = ttk.Scrollbar(output_frame, orient='vertical', command=self.bisector_result.yview)
        self.bisector_result.configure(yscrollcommand=scrollbar.set)
        
        self.bisector_result.pack(side='left', fill='both', expand=True)
        scrollbar.pack(side='right', fill='y')
    
    def create_median_tab(self):
        """Вкладка для вычисления медианы"""
        frame = ttk.Frame(self.notebook)
        self.notebook.add(frame, text="Медиана")
        
        # Заголовок
        ttk.Label(frame, text="Нахождение медианы треугольника в пространстве", 
                 font=('Arial', 11, 'bold')).pack(pady=10)
        
        # Ввод данных
        input_frame = ttk.LabelFrame(frame, text="Входные данные")
        input_frame.pack(fill='x', padx=10, pady=5)
        
        ttk.Label(input_frame, text="Координаты точек A, B и Q (x1 y1 x2 y2 x3 y3):").pack(pady=5)
        ttk.Label(input_frame, text="(точка Q вводится последней)").pack()
        self.median_entry = ttk.Entry(input_frame, width=50, font=('Arial', 10))
        self.median_entry.pack(pady=5)
        self.median_entry.insert(0, "4 0 0 4 0 0")
        
        # Кнопка расчета
        ttk.Button(frame, text="Рассчитать медиану", 
                  command=self.calculate_median).pack(pady=10)
        
        # Вывод результатов
        output_frame = ttk.LabelFrame(frame, text="Результаты")
        output_frame.pack(fill='both', expand=True, padx=10, pady=5)
        
        self.median_result = tk.Text(output_frame, height=8, width=70, font=('Arial', 10))
        scrollbar = ttk.Scrollbar(output_frame, orient='vertical', command=self.median_result.yview)
        self.median_result.configure(yscrollcommand=scrollbar.set)
        
        self.median_result.pack(side='left', fill='both', expand=True)
        scrollbar.pack(side='right', fill='y')
    
    def create_altitude_tab(self):
        """Вкладка для вычисления высоты"""
        frame = ttk.Frame(self.notebook)
        self.notebook.add(frame, text="Высота")
        
        # Заголовок
        ttk.Label(frame, text="Нахождение высоты к прямой из точки в пространстве", 
                 font=('Arial', 11, 'bold')).pack(pady=10)
        
        # Ввод данных
        input_frame = ttk.LabelFrame(frame, text="Входные данные")
        input_frame.pack(fill='x', padx=10, pady=5)
        
        ttk.Label(input_frame, text="Коэффициенты прямой (A B C) и координаты точки (x y):").pack(pady=5)
        self.altitude_entry = ttk.Entry(input_frame, width=50, font=('Arial', 10))
        self.altitude_entry.pack(pady=5)
        self.altitude_entry.insert(0, "1 0 0 2 3")
        
        # Кнопка расчета
        ttk.Button(frame, text="Рассчитать высоту", 
                  command=self.calculate_altitude).pack(pady=10)
        
        # Вывод результатов
        output_frame = ttk.LabelFrame(frame, text="Результаты")
        output_frame.pack(fill='both', expand=True, padx=10, pady=5)
        
        self.altitude_result = tk.Text(output_frame, height=8, width=70, font=('Arial', 10))
        scrollbar = ttk.Scrollbar(output_frame, orient='vertical', command=self.altitude_result.yview)
        self.altitude_result.configure(yscrollcommand=scrollbar.set)
        
        self.altitude_result.pack(side='left', fill='both', expand=True)
        scrollbar.pack(side='right', fill='y')
    
    def calculate_bisector(self):
        """Вычисление биссектрис"""
        try:
            data = self.bisector_entry.get().split()
            if len(data) != 6:
                self.show_error("Введите 6 чисел через пробел: A1 B1 C1 A2 B2 C2")
                return
            
            a1, b1, c1, a2, b2, c2 = map(float, data)
            bis1, bis2 = find_bisector(a1, b1, c1, a2, b2, c2)
            
            result_text = "РЕЗУЛЬТАТЫ РАСЧЕТА БИССЕКТРИС:\n"
            result_text += "=" * 50 + "\n"
            result_text += f"Прямая α: ({a1:.2f}; {b1:.2f}; {c1:.2f})\n"
            result_text += f"Прямая β: ({a2:.2f}; {b2:.2f}; {c2:.2f})\n\n"
            result_text += f"Биссектриса 1: {bis1}\n"
            result_text += f"Биссектриса 2: {bis2}\n\n"
            result_text += "Формат: (A; B; C) для уравнения Ax + By + C = 0"
            
            self.bisector_result.delete(1.0, tk.END)
            self.bisector_result.insert(1.0, result_text)
            
        except Exception as e:
            self.show_error(f"Ошибка расчета: {str(e)}")
    
    def calculate_median(self):
        """Вычисление медианы"""
        try:
            data = self.median_entry.get().split()
            if len(data) != 6:
                self.show_error("Введите 6 чисел через пробел: x1 y1 x2 y2 x3 y3")
                return
            
            x1, y1, x2, y2, x3, y3 = map(float, data)
            line, point = find_median(x1, y1, x2, y2, x3, y3)
            
            if "Ошибка" in line:
                self.show_error(line)
                return
            
            result_text = "РЕЗУЛЬТАТЫ РАСЧЕТА МЕДИАНЫ:\n"
            result_text += "=" * 50 + "\n"
            result_text += f"Точка A: ({x1:.2f}; {y1:.2f})\n"
            result_text += f"Точка B: ({x2:.2f}; {y2:.2f})\n"
            result_text += f"Точка Q: ({x3:.2f}; {y3:.2f})\n\n"
            result_text += f"Координаты точки M (середина AB): {point}\n"
            result_text += f"Уравнение медианы p: {line}\n\n"
            result_text += "Формат прямой: (A; B; C) для уравнения Ax + By + C = 0"
            
            self.median_result.delete(1.0, tk.END)
            self.median_result.insert(1.0, result_text)
            
        except Exception as e:
            self.show_error(f"Ошибка расчета: {str(e)}")
    
    def calculate_altitude(self):
        """Вычисление высоты"""
        try:
            data = self.altitude_entry.get().split()
            if len(data) != 5:
                self.show_error("Введите 5 чисел через пробел: A B C x y")
                return
            
            a, b, c, x, y = map(float, data)
            line, point = find_altitude(a, b, c, x, y)
            
            if "Ошибка" in line:
                self.show_error(line)
                return
            
            result_text = "РЕЗУЛЬТАТЫ РАСЧЕТА ВЫСОТЫ:\n"
            result_text += "=" * 50 + "\n"
            result_text += f"Прямая: ({a:.2f}; {b:.2f}; {c:.2f})\n"
            result_text += f"Точка: ({x:.2f}; {y:.2f})\n\n"
            result_text += f"Уравнение высоты: {line}\n"
            result_text += f"Точка пересечения с основанием: {point}\n\n"
            result_text += "Формат: (A; B; C) для уравнения Ax + By + C = 0"
            
            self.altitude_result.delete(1.0, tk.END)
            self.altitude_result.insert(1.0, result_text)
            
        except Exception as e:
            self.show_error(f"Ошибка расчета: {str(e)}")
    
    def show_error(self, message):
        """Показывает сообщение об ошибке"""
        error_window = tk.Toplevel(self.root)
        error_window.title("Ошибка")
        error_window.geometry("400x150")
        error_window.transient(self.root)
        error_window.grab_set()
        
        ttk.Label(error_window, text=message, wraplength=350, justify='center').pack(pady=20)
        ttk.Button(error_window, text="OK", command=error_window.destroy).pack(pady=10)

def main():
    root = tk.Tk()
    app = GeometryApp(root)
    root.mainloop()

if __name__ == "__main__":
    main()