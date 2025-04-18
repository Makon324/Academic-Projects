function numtest2()
% Celem testu jest zbadanie, jak liczba iteracji predyktora w metodzie
% Milne'a wpływa na dokładność rozwiązania oraz czas obliczeń.
%
% Funkcja nie posiada wejśća, ani wyjścia.

% Stałe
b = @(x) 0; % funkcja, prawa strona równania
a = {@(x) -1, @(x) 0, @(x) 1}; % tablica komórkowa współczynników
[x0, xN] = deal(0, 1); % przedział rozwiązania
y0 = [1, 0]; % warunki początkowe
N_ = [10, 500]; % ilość podprzedziałów
rowLength = 75; % maksymalna długość wiersza
test_desc_path = 'desc_numtest2.txt'; % ścieżka do pliku z opisem testu

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
DispWithPause(sprintf('N\tm\terror           \ttime (s)'));

k = ceil(log2(1e3));
error_values = zeros(2, k);
m_values = zeros(1, k);
for j = 1:2
    N = N_(j);
    m = 1;
    for i = 1:k
        tic;
        [y, x] = P2Z40_MKO_milne(b, a, x0, xN, y0, N, m);
        elapsed_time = toc;
    
        % Liczenie błędu
        y_exact = cosh(x);
        error = max(abs(y - y_exact));
    
        DispWithPause(sprintf('%i\t%i\t%.15e\t%.5e', N, m, error, ...
            elapsed_time));

        % Aktualizacja tablic
        error_values(j, i) = error;
        m_values(i) = m;
    
        m = 2 * m; % aktualizacja m
    end
    DispWithPause(repmat('-', 1, rowLength));
end

% Wykres
fig = figure;
set(fig, 'Name', 'numtest2', 'NumberTitle', 'off');

loglog(m_values, error_values(1, :), '-o', 'DisplayName', 'Error N = 10');
hold on;
loglog(m_values, error_values(2, :), '-o', 'DisplayName', 'Error N = 500');
xlabel('Liczba iteracji korektora (m)');
ylabel('Error');
title('Błąd w zależności od liczby iteracji korektora (m)');
legend;
grid on;


end % function