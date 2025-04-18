function numtest4()
% Celem testu jest zbadanie poprawności działania funkcji przy
% rozwiązywaniu równania różniczkowego drugiego rzędu, gdzie wynik
% ma asymptotę.
%
% Funkcja nie posiada wejśća, ani wyjścia.

% Stałe
b = @(x) 2/(x^3); % funkcja, prawa strona równania
a = {@(x) x, @(x) x^2, @(x) 1}; % tablica komórkowa współczynników
xN = 1; % koniec przedziału rozwiązania
N = 2000; % ilość podprzedziałów
rowLength = 75; % maksymalna długość wiersza
test_desc_path = 'desc_numtest4.txt'; % ścieżka do pliku z opisem testu

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
DispWithPause(sprintf('x0      \terror'));

x0 = 0.01;
while(x0 > 1e-5)
    y0 = [1/x0, -1/(x0^2)]; % aktualizacja warunków początkowych

    [y, x] = P2Z40_MKO_milne(b, a, x0, xN, y0, N);

    % Liczenie błędu
    y_exact = 1./x;
    error = max(abs(y - y_exact));

    DispWithPause(sprintf('%f\t%.5e', x0, error));

    x0 = x0/2; % aktualizacja x0
end

end % function