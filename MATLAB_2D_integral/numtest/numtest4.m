function numtest4()
% Funkcja testuje skuteczność złożonych 2 punktowych kwadratur
% Gaussa-Legendre'a przy obliczaniu funkcji z nieosobliwościami
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
[a, b] = deal(0, 1); % przedział [a, b]
[c, d] = deal(0, 1); % przedział [c, d]
[nl, nu] = deal(10, 1000); % ograniczenie na n1 i n2
num_norm_tests = 12; % ilość normalnych testów
num_rand_tests = 3; % ilość testów z losowymi n1 i n2
ni = 2; % mnożnik pomiędzy kolejnymi n1 i n2
rowLength = 75; % maksymalna długość wiersza
test_desc_path = 'desc_numtest4.txt'; % ścieżka do pliku z opisem testu

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

function result = f2(x, y) 
    if (x + y) < 1
        result = 1/(x*y);
    else
        result = -1/((1-x)*(1-y));
    end
end % function f2

f = {@(x, y) 1/(x*y), @f2 };

fstr = {'1/xy', 'x+y<1 -> 1/xy; x+y>=1 -> 1/(1-x)(1-y)'};

wyn = [inf, 0];

% testy

for i = 1:length(f)

    DispWithPause(sprintf(['f = %s, [a, b] = [%i, %i], ' ...
        '[c, d] = [%i, %i]'], fstr{i}, a, b, c, d));

    popr_test(f{i}, wyn(i), a, b, c, d, ni, nl, nu, ...
        num_norm_tests, num_rand_tests);

end

end % function