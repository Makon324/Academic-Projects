function numtest3()
% Celem testu jest zbadanie, w jaki sposób błędy obliczeń pierwszych 
% wartości za pomocą metody Gilla wpływają na błąd całego rozwiązania.
%
% Funkcja nie posiada wejśća, ani wyjścia.

% Stałe
b = @(x) 0; % funkcja, prawa strona równania
a = {@(x) -1, @(x) 0, @(x) 1}; % tablica komórkowa współczynników
[x0, xN] = deal(0, 1); % przedział rozwiązania
y0 = [1, 0]; % warunki początkowe
N = 100; % ilość podprzedziałów
rowLength = 75; % maksymalna długość wiersza
test_desc_path = 'desc_numtest3.txt'; % ścieżka do pliku z opisem testu

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

% Funkcje współczynników równania
a2 = a{3};
a1 = a{2};
a0 = a{1};
    
% Definicja równań jako pierwszego rzędu (y', z')
f = @(x, y, z) (b(x) - a0(x) * y - a1(x) * z) / a2(x);

% Test
DispWithPause(sprintf('err_mn\terror   \tgill error'));
err_mn = 0.000001;
err_mn_values = zeros(1, 15);
error_values = zeros(1, 15);

for i = 1:15
    % Parametry
    h = (xN - x0) / N; % Krok czasowy
    x = linspace(x0, xN, N + 1);
    
    % Inicjalizacja
    y = zeros(1, N + 1); % Rozwiązanie
    z = zeros(1, N + 1); % z = y'
    y(1) = y0(1);
    z(1) = y0(2);
    
    % Obliczanie pierwszych punktów metodą Gilla
    [y, z] = gill_method(x, y, z, f, h);
    error_gill_y = y(1:4) - cosh(x(1:4));
    error_gill_z = z(1:4) - sinh(x(1:4));

    % Zmniejszanie błędu
    y(1:4) = cosh(x(1:4)) + error_gill_y * err_mn;
    z(1:4) = sinh(x(1:4)) + error_gill_z * err_mn;
    
    % Iteracja metodą Milne'a
    [y, ~] = milne_method(x, y, z, f, h, N, 1);

    % Liczenie błędu
    y_exact = cosh(x);
    error = max(abs(y - y_exact));
    error_gill = max(abs(y(1:4) - y_exact(1:4)));

    DispWithPause(sprintf('%.1e\t%.5e\t%.5e', err_mn, error, error_gill));

    % Aktualizacja tablic
    err_mn_values(i) = err_mn;
    error_values(i) = error;

    err_mn = 10 * err_mn; % aktualizacja err_mn
end

% Wykres
fig = figure;
set(fig, 'Name', 'numtest3', 'NumberTitle', 'off');
loglog(err_mn_values, error_values, '-o', 'DisplayName', 'Error');
xlabel('Mnożnik błędu metody Gilla (err\_mn)');
ylabel('Error');
title('Błąd w zależności od mnożnika błędu metody Gilla (err\_mn)');
legend;
grid on;

end % function