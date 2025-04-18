function numtest6()
% Celem testu jest zbadanie, jak dokładność metody numerycznej zmienia się 
% w zależności od liczby punktów podziału (N) w przypadku, gdy współczynnik
% a2 w równaniu różniczkowym jest bardzo bliski 0 w pewnym punkcie.
%
% Funkcja nie posiada wejśća, ani wyjścia.

% Stałe
b = @(x) -(x - 0.5)^2 * sin(x); % funkcja, prawa strona równania
a = {@(x) 1e-10, @(x) 0, @(x) ((x-0.5)^2 + 1e-10)}; % tablica komórkowa współczynników
[x0, xN] = deal(0, 1); % przedział rozwiązania
y0 = [0, 1]; % warunki początkowe
rowLength = 75; % maksymalna długość wiersza
test_desc_path = 'desc_numtest6.txt'; % ścieżka do pliku z opisem testu

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
DispWithPause(sprintf('N\terror'));

N = 3;
k = ceil(log2(1e4 / N));
error_values = zeros(1, k);
N_values = zeros(1, k);
for i = 1:k
    [y, x] = P2Z40_MKO_milne(b, a, x0, xN, y0, N);

    % Liczenie błędu
    y_exact = sin(x);
    error = max(abs(y - y_exact));

    DispWithPause(sprintf('%i\t%.5e\t%.5e', N, error));

    % Aktualizacja tablic
    error_values(i) = error;
    N_values(i) = N;

    N = 2 * N; % aktualizacja N
end

% Wykres
fig = figure;
set(fig, 'Name', 'numtest6', 'NumberTitle', 'off');
loglog(N_values, error_values, '-o', 'DisplayName', 'Error');
xlabel('Liczba podprzedziałów (N)');
ylabel('Error');
title('Błąd w zależności od liczby podprzedziałów (N)');
legend;
grid on;

end % function