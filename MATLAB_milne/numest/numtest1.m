function numtest1()
% Celem testu jest zbadanie złożoności metody Milne’a, w której pierwsze 
% współczynniki są obliczane za pomocą metody Gilla. Analizowane są zarówno 
% dokładność rozwiązania, jak i czas obliczeń w zależności od liczby 
% punktów podziału (N).
%
% Funkcja nie posiada wejśća, ani wyjścia.

% Stałe
b = @(x) 0; % funkcja, prawa strona równania
a = {@(x) -1, @(x) 0, @(x) 1}; % tablica komórkowa współczynników
[x0, xN] = deal(0, 1); % przedział rozwiązania
y0 = [1, 0]; % warunki początkowe
rowLength = 75; % maksymalna długość wiersza
test_desc_path = 'desc_numtest1.txt'; % ścieżka do pliku z opisem testu

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
DispWithPause(sprintf('N\terror   \tgill error\ttime'));

N = 3;
k = ceil(log2(1e6 / N));
error_values = zeros(1, k);
N_values = zeros(1, k);
for i = 1:k
    tic;
    [y, x] = P2Z40_MKO_milne(b, a, x0, xN, y0, N);
    elapsed_time = toc;
 
    % Liczenie błędu
    y_exact = cosh(x);
    error = max(abs(y - y_exact));
    error_gill = max(abs(y(1:4) - y_exact(1:4)));
    
    DispWithPause(sprintf('%i\t%.5e\t%.5e\t%.5e', N, error, ...
        error_gill, elapsed_time));

    % Aktualizacja tablic
    error_values(i) = error;
    N_values(i) = N;

    N = 2 * N; % aktualizacja N
end

% Wykres
fig = figure;
set(fig, 'Name', 'numtest1', 'NumberTitle', 'off');
loglog(N_values, error_values, '-o', 'DisplayName', 'Error');
xlabel('Liczba podprzedziałów (N)');
ylabel('Error');
title('Błąd w zależności od liczby podprzedziałów (N)');
legend;
grid on;

end % function