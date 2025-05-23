function numtest5()
% Funkcja testuje skuteczność złożonych 2 punktowych kwadratur
% Gaussa-Legendre'a przy obliczaniu funkcji nieparzystej
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
[a, b] = deal(-1, 1); % przedział [a, b]
[c, d] = deal(-1, 1); % przedział [c, d]
[pl, pu] = deal(-5, 5); % ograniczenie parametrów
[nl, nu] = deal(1, 1000); % ograniczenie na n1 i n2
[n1, n2] = deal(10, 10); % wartości n1 i n2 do stałych testów
num_norm_tests = 18; % ilość normalnych testów
num_rand_tests = 3; % ilość testów z losowymi n1 i n2
ni = 1.5; % mnożnik pomiędzy kolejnymi n1 i n2
rowLength = 75; % maksymalna długość wiersza
test_desc_path = 'desc_numtest5.txt'; % ścieżka do pliku z opisem testu

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

Mx = pl + (pu-pl)*rand(1, 5142);
My = pl + (pu-pl)*rand(1, 7345);

function result = f(x, y) 
    if (x - y) < 0
        result = -1*Mx(ceil(abs(x)/5142)) + -1*My(ceil(abs(y)/7345));
    elseif x == y
        result = 0;
    else
        result = Mx(ceil(abs(x)/5142)) + My(ceil(abs(y)/7345));
    end
end % function f

fstr = 'losowa funkcja nieparzysta';

wyn = 0;

% test

DispWithPause(sprintf(['f = %s, [a, b] = [%i, %i], ' ...
    '[c, d] = [%i, %i]'], fstr, a, b, c, d));

popr_test(@f, wyn, a, b, c, d, ni, nl, nu, num_norm_tests, num_rand_tests);

end % function