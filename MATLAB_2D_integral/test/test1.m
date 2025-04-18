function test1()
% Funkcja testująca dla programu P1Z29_MKO_integral2D
% Funkcja testuje poprawność działania programu P1Z29_MKO_integral2D
% przy obliczaniu całki podwójnej funkcji stałej 
% na obszarze D = [a, b] x [c, d]. Stała wartość funkcji jest generowana
% losowo z zakresu [-5, 5], tak samo jak przedziały [a, b] i [c, d]. 
% Test składa się z dwóch etapów:
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
[rl, ru] = deal(-5, 5); % ograniczenie na przedziały [a, b] i [c, d]
[pl, pu] = deal(-5, 5); % ograniczenie parametrów wielomianu
[nl, nu] = deal(10, 200); % ograniczenie na n1 i n2 w ostatnich testach
num_norm_tests = 10; % ilość normalnych testów
num_rand_tests = 3; % ilość testów z losowymi n1 i n2
ni = 2; % mnożnik pomiędzy kolejnymi n1 i n2
rowLength = 75; % maksymalna długość wiersza
test_desc_path = 'desc_test1.txt'; % ścieżka do pliku z opisem testu

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

% losowanie

[a, b] = RandRange(rl, ru);
[c, d] = RandRange(rl, ru);
p0 = pl + (pu - pl) * rand();

DispWithPause(sprintf(['Wylosowano: p0 = %f, [a, b] = [%f, %f], [c, d]' ...
    ' = [%f, %f]'], p0, a, b, c, d));
DispWithPause(sprintf('-> liczenie całki z f(x, y) = %f', p0));
DispWithPause(sprintf('na obszarze [%f, %f] x [%f, %f]', a, b, c, d));
DispWithPause(repmat('-', 1, rowLength));

f = @(x, y) p0;

wyn = p0*(b-a)*(d-c);

% test 

popr_test(f, wyn, a, b, c, d, ni, nl, nu, num_norm_tests, num_rand_tests);

end % function