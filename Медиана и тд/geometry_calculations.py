import math

def find_bisector(a1, b1, c1, a2, b2, c2):
    """Находит биссектрисы двух прямых в пространстве"""
    n1 = math.sqrt(a1**2 + b1**2)
    n2 = math.sqrt(a2**2 + b2**2)
    
    a_m1 = a1 * n2 + a2 * n1
    b_m1 = b1 * n2 + b2 * n1
    c_m1 = c1 * n2 + c2 * n1
    
    a_m2 = a1 * n2 - a2 * n1
    b_m2 = b1 * n2 - b2 * n1
    c_m2 = c1 * n2 - c2 * n1
    
    bis1 = f"({a_m1:.2f}; {b_m1:.2f}; {c_m1:.2f})"
    bis2 = f"({a_m2:.2f}; {b_m2:.2f}; {c_m2:.2f})"
    
    return bis1, bis2

def find_median(xA, yA, xB, yB, xQ, yQ):
    """Находит медиану треугольника в пространстве"""
    # Порядок ввода: A(xA,yA), B(xB,yB), Q(xQ,yQ)
    
    # Проверяем, не лежит ли точка Q на прямой AB
    # Уравнение прямой через A и B: (y - yA)(xB - xA) = (x - xA)(yB - yA)
    # Подставляем Q(xQ,yQ): (yQ - yA)(xB - xA) - (xQ - xA)(yB - yA) = 0
    collinear_check = (yQ - yA) * (xB - xA) - (xQ - xA) * (yB - yA)
    if abs(collinear_check) < 1e-10:
        return "Ошибка: точка Q лежит на прямой AB", ""
    
    # Середина стороны AB
    xM = (xA + xB) / 2
    yM = (yA + yB) / 2
    
    # Уравнение медианы через точку Q(xQ,yQ) и середину M(xM, yM)
    # Используем формулу прямой через две точки
    if abs(xM - xQ) < 1e-10:
        # Вертикальная прямая
        A = 1
        B = 0
        C = -xQ
    else:
        k = (yM - yQ) / (xM - xQ)
        A = k
        B = -1
        C = yQ - k * xQ
    
    return f"({A:.2f}; {B:.2f}; {C:.2f})", f"({xM:.2f}; {yM:.2f})"

def find_altitude(a, b, c, x, y):
    """Находит высоту к прямой из точки в пространстве"""
    # Перпендикулярная прямая
    a2 = b
    b2 = -a
    c2 = -a2*x - b2*y
    
    # Точка пересечения
    denominator = a*b2 - a2*b
    if abs(denominator) < 1e-10:
        return "Прямая и высота параллельны", ""
    
    x2 = (b*c2 - b2*c) / denominator
    y2 = (a2*c - a*c2) / denominator
    
    return f"({a2:.2f}; {b2:.2f}; {c2:.2f})", f"({x2:.2f}; {y2:.2f})"