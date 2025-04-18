function numtest1()
% Funkcja testuje skuteczność złożonych 2 punktowych kwadratur
% Gaussa-Legendre'a przy obliczaniu funkcji szybko oscylującyh
% na obszarze D = [0, 1] x [0, 1].
% Test każdej funkcji składa się z dwóch etapów:
% 1. Obliczenia dla rosnących wartości parametrów n1 i n2.
% 2. Obliczenia dla losowych wartości parametrów n1 i n2.
%
% Działanie funkcji:
% Wynik uzyskany kwadraturą porównywany jest z teoretycznym
% wynikiem analitycznym. Różnica pomiędzy wynikami oraz czas obliczeń
% prezentowane są w formie tabeli.
%
% Funkcja nie posiada wejśća, ani wyjścia.

% stałe
[a, b] = deal(0, 1); % przedział [a, b]
[c, d] = deal(0, 1); % przedział [c, d]
[nl, nu] = deal(1, 10000); % ograniczenie na n1 i n2
num_norm_tests = 15; % ilość normalnych testów
num_rand_tests = 0; % ilość testów z losowymi n1 i n2
ni = 1.5; % mnożnik pomiędzy kolejnymi n1 i n2
rowLength = 75; % maksymalna długość wiersza
test_desc_path = 'desc_numtest1.txt'; % ścieżka do pliku z opisem testu

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

f = {@(x, y) 1000*sin(100*x)*cos(100*y), ...
@(x, y) 1000*sin(100*x)*sin(100*y)};

fstr = {'1000sin(100x)cos(100y)', '1000sin(100x)sin(100y)'};

wyn = [sin(100)*(1-cos(100))/10, (1-cos(100))^2/10];

% test

% testy, gdzie a = b
for i = 1:length(f)

    DispWithPause(sprintf(['f = %s, [a, b] = [%i, %i], ' ...
        '[c, d] = [%i, %i]'], fstr{i}, a, b, c, d));

    popr_test(f{i}, wyn(i), a, b, c, d, ni, nl, nu, ...
        num_norm_tests, num_rand_tests);

end

end % function