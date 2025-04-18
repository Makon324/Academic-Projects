function numtest2()
% Funkcja testuje skuteczność złożonych 2 punktowych kwadratur
% Gaussa-Legendre'a przy obliczaniu funkcji ciężko przybliżalnych
% wielomianem niskiego stopnia
% na obszarze D = [0, 1] x [0, 1].
% Test każdej funkcji składa się z dwóch etapów:
% 1. Obliczenia dla rosnących wartości parametrów n1 i n2.
% 2. Obliczenia dla stałuch wartości parametrów n1 i n2.
%
% Działanie funkcji:
% Wynik uzyskany kwadraturą porównywany jest z teoretycznym
% wynikiem analitycznym. Różnica pomiędzy wynikami oraz czas obliczeń
% prezentowane są w formie tabeli.
%
% Funkcja nie posiada wejśća, ani wyjścia.

% stałe
[a, b] = deal(0.01, 1); % przedział [a, b]
[c, d] = deal(0.01, 1); % przedział [c, d]
[nl, nu] = deal(10, 1000); % ograniczenie na n1 i n2
num_norm_tests = 7; % ilość normalnych testów
num_rand_tests = 3; % ilość testów z losowymi n1 i n2
ni = 4; % mnożnik pomiędzy kolejnymi n1 i n2
rowLength = 75; % maksymalna długość wiersza
test_desc_path = 'desc_numtest2.txt'; % ścieżka do pliku z opisem testu

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

% funckje

f = {@(x, y) 100*log(x*y), @(x, y) 1/(x*y), @(x, y) x^12*y^13};

fstr = {'100ln(x*y)', '1/xy', 'x^12y^13'};

wyn = [100*2*0.99*(log(1)-1-0.01*log(0.01)+0.01), (log(1)-log(0.01))^2, ...
integral_xnym(12,13, a, b, c, d)];

% testy

for i = 1:length(f)

    DispWithPause(sprintf(['f = %s, [a, b] = [%f, %i], ' ...
        '[c, d] = [%f, %i]'], fstr{i}, a, b, c, d));

    popr_test(f{i}, wyn(i), a, b, c, d, ni, nl, nu, ...
        num_norm_tests, num_rand_tests);

end

end % function