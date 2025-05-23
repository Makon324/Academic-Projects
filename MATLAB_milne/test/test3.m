function test3()
% Funkcja testująca dla programu P2Z40_MKO_milne
% Funkcja testuje poprawność działania programu P2Z40_MKO_milne
% przy obliczaniu rozwiązania równania różniczkowego postaci:
% y'' + cos(x)y' + sin(x)y = 1 - sin(x)
% Na przedziale: [0, 2*pi]
% Przy warunkach początkowych: y(0) = 1, y'(0) = 0

% stałe
b = @(x) 1 - sin(x); % funkcja, prawa strona równania
a = {@(x) sin(x), @(x) cos(x), @(x) 1}; % tablica komórkowa współczynników
[x0, xN] = deal(0, 2*pi); % przedział rozwiązania
y0 = [0, 1]; % warunki początkowe
N = 1000; % ilość podprzedziałów
rowLength = 75; % maksymalna długość wiersza
test_desc_path = 'desc_test3.txt'; % ścieżka do pliku z opisem testu

% czyszczenie ekranu
clc;
clear DispWithPause;

% z jakiegoś powodu bez tego czasami nic się nie wyświetla przed 1 pauzą
disp('test start');
pause(1);
clc;
% -------------

% wyświetlanie opisu testu

DispWithPause(repmat('-', 1, rowLength));
DispWithPause(strrep(fileread(test_desc_path), char(13), ''));
DispWithPause(repmat('-', 1, rowLength));

% test 
[y, x] = P2Z40_MKO_milne(b, a, x0, xN, y0, N);
y_exact = sin(x); % rozwiązanie analityczne

% liczenie błędu
error = max(abs(y - y_exact));
error_gill = max(abs(y(1:4) - y_exact(1:4)));
error_milne = max(abs(y(5:end) - y_exact(5:end)));
DispWithPause(sprintf('gill error = %.5e', error_gill));
DispWithPause(sprintf('milne error = %.5e', error_milne));
DispWithPause(sprintf('error = %.5e', error));

% wyświetlanie wykresu
figure(1);
clf;
plot(x, y, 'DisplayName', 'Rozwiąznie metodą Milne''a', 'LineWidth', 3, 'Color', 'y');
hold on
plot(x, y_exact, '--', 'DisplayName', 'Rozwiąznie analityczne', 'LineWidth', 1.5, 'Color', 'b');
title('test3');
xlabel('x');
ylabel('y');
grid on;
legend;

end % function