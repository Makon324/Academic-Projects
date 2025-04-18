function test8()
% Funkcja testująca dla programu P1Z29_MKO_integral2D
% Funkcja testuje poprawność działania programu P1Z29_MKO_integral2D
% obliczającej całki funkcji łatwo przybliżalnych wielomianami
% na obszarze D = [0, 1] x [0, 1].
% Test każdej funkcji składa się z dwóch etapów:
% 1. Obliczenia dla rosnących wartości parametrów n1 i n2.
% 2. Obliczenia dla losowych wartości parametrów n1 i n2.
%
% Działanie funkcji:
% Wynik uzyskany przez P1Z29_MKO_integral2D porównywany jest z teoretycznym
% wynikiem analitycznym. Różnica pomiędzy wynikami oraz czas obliczeń
% prezentowane są w formie tabeli.
%
% Funkcja nie posiada wejśća, ani wyjścia.

% stałe
[a, b] = deal(0, 1); % przedział [a, b]
[c, d] = deal(0, 1); % przedział [c, d]
[nl, nu] = deal(5, 20); % ograniczenie na n1 i n2
num_norm_tests = 4; % ilość normalnych testów
num_rand_tests = 2; % ilość testów z losowymi n1 i n2
ni = 2; % mnożnik pomiędzy kolejnymi n1 i n2
rowLength = 75; % maksymalna długość wiersza
test_desc_path = 'desc_test8.txt'; % ścieżka do pliku z opisem testu

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

f = {@(x, y) sin(x+y), @(x, y) sin(x)*cos(y), @(x, y) exp(x+y), ...
    @(x, y) sin(x)+x*y^2};

fstr = {'sin(x+y)', 'sin(x)cos(y)', 'exp(x+y)', 'sin(x) + xy^2'};

wyn = [2*sin(1)-sin(2), (1-cos(1))*sin(1), (exp(1)-1)^2, 7/6-cos(1)];

% test

% testy, gdzie a = b
for i = 1:length(f)

    DispWithPause(sprintf(['f = %s, [a, b] = [%i, %i], ' ...
        '[c, d] = [%i, %i]'], fstr{i}, a, b, c, d));

    popr_test(f{i}, wyn(i), a, b, c, d, ni, nl, nu, num_norm_tests, ...
        num_rand_tests);

end

end % function