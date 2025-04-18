function numtest3()
% Funkcja testuje skuteczność złożonych 2 punktowych kwadratur
% Gaussa-Legendre'a przy obliczaniu funkcji nieregulnej złożonej ze 100
% losowych funkcji stałych
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
[pl, pu] = deal(-5, 5); % ograniczenie parametrów
[nl, nu] = deal(1, 10000); % ograniczenie na n1 i n2
[n1, n2] = deal(10, 10); % wartości n1 i n2 do stałych testów
num_norm_tests = 18; % ilość normalnych testów
num_rand_tests = 0; % ilość testów z losowymi n1 i n2
num_const_tests = 3; % ilość testów z losowymi n1 i n2
ni = 1.5; % mnożnik pomiędzy kolejnymi n1 i n2
rowLength = 75; % maksymalna długość wiersza
test_desc_path = 'desc_numtest3.txt'; % ścieżka do pliku z opisem testu

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

% funckja

f1_matrix = pl + (pu-pl)*rand(10, 10);

f = @(x, y) f1_matrix(ceil(x*10), ceil(y*10));

fstr = 'f1_matrix(ceil(x*10), ceil(y*10))';

wyn = sum(f1_matrix(:))/100;

% test

DispWithPause(sprintf(['f = %s, [a, b] = [%i, %i], ' ...
    '[c, d] = [%i, %i]'], fstr, a, b, c, d));

popr_test(f, wyn, a, b, c, d, ni, nl, nu, num_norm_tests, num_rand_tests);

% stałe testy
for j = -1:num_const_tests-2
    tic;
    [q] = P1Z29_MKO_integral2D(f, a, b, c, d, n1+j, n2+j);
    exec_time = toc;
    
    DispWithPause(sprintf('%i\t%i\t%f\t%f\t%f\t%fs', ...
    n1+j, n2+j, q, wyn, abs(q-wyn), exec_time));
end

DispWithPause(repmat('-', 1, rowLength));

end % function