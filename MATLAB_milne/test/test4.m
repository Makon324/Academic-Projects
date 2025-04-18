function test4()
% Funkcja testująca dla programu P2Z40_MKO_milne
% Funkcja testuje poprawność działania programu P2Z40_MKO_milne
% przy obliczaniu rozwiązania równania różniczkowego postaci:
% y'' + x^2y' + xy = 2 / (x^3)
% Na przedziale: [-1, 1]
% Przy warunkach początkowych: y(0) = -1, y'(0) = -1
%
% Funkcja nie posiada wejśća, ani wyjścia.

% Stałe
b = @(x) 2/(x^3); % funkcja, prawa strona równania
a = {@(x) x, @(x) x^2, @(x) 1}; % tablica komórkowa współczynników
[x0, xN] = deal(0.01, 1); % przedział rozwiązania
y0 = [100, -10000]; % warunki początkowe
N = 2000; % ilość podprzedziałów
rowLength = 75; % maksymalna długość wiersza
test_desc_path = 'desc_test4.txt'; % ścieżka do pliku z opisem testu

% Czyszczenie ekranu
clc;
clear DispWithPause;

% z jakiegoś powodu bez tego czasami nic się nie wyświetla przed 1 pauzą
disp('test start');
pause(1);
clc;
% -------------

% Wyświetlanie opisu testu
DispWithPause(repmat('-', 1, rowLength));
DispWithPause(strrep(fileread(test_desc_path), char(13), ''));
DispWithPause(repmat('-', 1, rowLength));

% Test 
[y, x] = P2Z40_MKO_milne(b, a, x0, xN, y0, N);
y_exact = 1./x; % rozwiązanie analityczne

% Liczenie błędu
error = max(abs(y - y_exact));
error_gill = max(abs(y(1:4) - y_exact(1:4)));
error_milne = max(abs(y(5:end) - y_exact(5:end)));
DispWithPause(sprintf('gill error = %.5e', error_gill));
DispWithPause(sprintf('milne error = %.5e', error_milne));
DispWithPause(sprintf('error = %.5e', error));

% Wyświetlanie wykresu
figure(1);
clf;
plot(x, y, 'DisplayName', 'Rozwiąznie metodą Milne''a', ...
    'LineWidth', 3, 'Color', 'y');
hold on
plot(x, y_exact, '--', 'DisplayName', 'Rozwiąznie analityczne', ...
    'LineWidth', 1.5, 'Color', 'b');
title('test4');
xlabel('x');
ylabel('y');
grid on;
legend;

end % function