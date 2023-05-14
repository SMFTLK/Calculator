# Calculator
Этот калькулятор написан на C# с помощью WinForms.

# Функционал
На данный момент калькулятор умеет:
- Складывать
- Вычитать
- Умножать
- Делить
- Возводить во вторую степень
- Возводить в корень второй степени
- Находить процент от числа
- Менять знак у числа
- Находить синус угла
- Находить косинус угла
- Находить тангенс угла
- Находить котангенс угла

# Недостатки
На данный момент у калькулятора несколько недостатков (вернее, у него есть ошибки в коде, которые могут приводить к не совсем правильным действиям или последствиям).

~~Также не проработан алгоритм, который будет вычислять расположение кнопок и сносок при изменении размера окна, что приведет к растяжению кнопок.~~

~~Алгоритм пока что не умеет считать промежутки между боковыми сторонами кнопок. Вернее, у него это плохо получается.~~

Остался баг: при изменении размера формы кнопки уменьшаются, но не пропорционально размеру формы, а на отношение высоты и ширины формы

## Первый баг
Одним из багов является то, что если вы ввёдете первое число и, *не вводя второе число* и нажав на арифметическую операцию "вычесть", нажмёте кнопку "Равняется", так будет происходить до тех пор, пока вы не придёте к первому числу со знаком минус.

То есть, сначала вы получите "0" в строке, а затем отрицательное число. После этого, калькулятор будет думать, что вы приравниваете полученное число вместо того, чтобы вычитать из него первое число.

## Второй баг
Ещё один баг - вычисление чисел с помощью разных арифметических операций. Теперь я хочу прояснить ситуацию: вы можете не нажимать каждый раз кнопку "равняется", чтобы получить число, которое вы ожидали. Вместо этого я придумал алгоритм, по которому можно вычислять число, не нажимая кнопку "равняется".

Но, конечно, алгоритм не идеален. Нет, я *не хочу сказать*, что я сделал бесполезный код. Я хочу сказать, что этот алгоритм будет работать **правильно** только в том случае, если вы будете использовать одну и ту же арифметическую операцию. В ином случае, будет происходить следующее:

Представим, что я выбрал число "10", нажал кнопку "прибавить" и ввёл число "5". *НО:* я не нажимал кнопку "равняется", вместо этого я, после ввода числа "5", нажал кнопку "вычесть", и вместо первого числа я получаю не "15", а "5".

## Третий баг
Следующий баг: при вычитании от нуля какое-либо число, вы получаете ноль.
