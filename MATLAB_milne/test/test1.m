function test1()
% Funkcja testująca dla programu P2Z40_MKO_milne
% Funkcja testuje poprawność działania programu P1Z29_MKO_milne
% przy obliczaniu rozwiązania równania różniczkowego postaci:
% y'' - y = 0
% Na przedziale: [0, 1]
% Przy warunkach początkowych: y(0) = 1, y'(0) = 0
%
% Funkcja nie posiada wejśća, ani wyjścia.

% Stałe
b = @(x) 0; % funkcja, prawa strona równania
a = {@(x) -1, @(x) 0, @(x) 1}; % tablica komórkowa współczynników
[x0, xN] = deal(0, 1); % przedział rozwiązania
y0 = [1, 0]; % warunki początkowe
N = 1000; % ilość podprzedziałów
rowLength = 75; % maksymalna długość wiersza
test_desc_path = 'desc_test1.txt'; % ścieżka do pliku z opisem testu

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
y_exact = cosh(x);

% Liczenie błędu
error = max(abs(y - y_exact));
error_gill = max(abs(y(1:4) - y_exact(1:4)));
error_milne = max(abs(y(5:end) - y_exact(5:end)));

% Wyświetlanie błędu
DispWithPause(sprintf('gill error = %.5e', error_gill));
DispWithPause(sprintf('milne error = %.5e', error_milne));
DispWithPause(sprintf('error = %.5e', error));

% Wykres
fig = figure;
set(fig, 'Name', 'test1', 'NumberTitle', 'off');

plot(x, y, 'DisplayName', 'Rozwiąznie P2Z40\_MKO\_milne', ...
    'LineWidth', 3, 'Color', 'y');
hold on

plot(x, y_exact, '--', 'DisplayName', 'Rozwiąznie analityczne', ...
    'LineWidth', 1.5, 'Color', 'b');

title('test1');
xlabel('x');
ylabel('y');
grid on;
legend;

end % function