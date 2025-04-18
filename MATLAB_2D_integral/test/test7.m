function test7()
% Funkcja testująca dla programu P1Z29_MKO_integral2D
% Funkcja testuje poprawność działania programu P1Z29_MKO_integral2D
% obliczającej całkę podwójną losowej funkcji 
% na obszarze D = [a, b] x [c, d], takim, że a = b lub c = d
% Test składa się z 3 dwóch etapów:
% 1. Obliczenia dla a = b
% 2. Obliczenia dla c = d
% 3. Obliczenia dla a = b i c = d
%
% Działanie funkcji:
% Wynik uzyskany przez P1Z29_MKO_integral2D porównywany jest z teoretycznym
% wynikiem analitycznym (0). Różnica pomiędzy wynikami oraz czas obliczeń
% prezentowane są w formie tabeli.
%
% Funkcja nie posiada wejśća, ani wyjścia.

% stałe
[rl, ru] = deal(-5, 5); % ograniczenie na przedziały [a, b] i [c, d]
[nl, nu] = deal(10, 200); % ograniczenie na n1 i n2
num_tests = 9; % ilość testów dla każdego z przypadków
rowLength = 75; % maksymalna długość wiersza
test_desc_path = 'desc_test7.txt'; % ścieżka do pliku z opisem testu

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

% testy

f = @(x, y) rand; % losowa funkcja

wyn = 0; % w tym teście wynik to zawsze 0

for i = 1:num_tests
    if i <= num_tests/3
        % losowanie a=b, c, d
        a = rl + (ru - rl)*rand();
        b = a;
        [c, d] = RandRange(rl, ru);
    elseif i <= num_tests * (2/3)
        % losowanie a, b, c=d
        [a, b] = RandRange(rl, ru);        
        c = rl + (ru - rl)*rand();
        d = c;
    else
        % losowanie a=b, c=d
        a = rl + (ru - rl)*rand();
        b = a;       
        c = rl + (ru - rl)*rand();
        d = c;     
    end

    % losowanie n1 i n2
    n1 = randi([nl, nu]);
    n2 = randi([nl, nu]);

    DispWithPause(sprintf('[a, b] = [%f, %f], [c, d] = [%f, %f]', ...
        a, b, c, d));

    % test
    tic;
    [q] = P1Z29_MKO_integral2D(f, a, b, c, d, n1, n2);
    exec_time = toc;

    % wyświetlanie wyniku
    DispWithPause(sprintf(['n1\tn2\tWynik Programu\tPoprawny ' ...
    'Wynik\tRóżnica\t\tCzas']));
    DispWithPause(sprintf('%i\t%i\t%f\t%f\t%f\t%fs', ...
        n1, n2, q, wyn, abs(q-wyn), exec_time));

    DispWithPause(repmat('-', 1, rowLength));
end

end % function