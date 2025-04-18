function popr_test(f, wyn, a, b, c, d, ni, nl, nu, n_norm_tests, n_rand_tests)
% Funkcja oblicza wyniki wywołanie funckji gauss_legendre_2d
% oraz porównuje je z podanymi wynikami analitycznymi
%
% WEJŚCIE:
%   f   - Uchwyt do funkcji f(x, y), którą należy całkować
%   wyn - analityczny wynik całkowania f
%   a, b - Granice całkowania dla zmiennej x
%   c, d - Granice całkowania dla zmiennej y
%   ni - mnożnik pomiędzy kolejnymi n1 i n2
%   nl  - dolna granica liczby podprzedziałów wzdłuż osi X
%   nu  - dolna granica liczby podprzedziałów wzdłuż osi Y
%   n_norm_tests - ilość normalnych testów
%   n_rand_test - ilość testów z losowymi n1 i n2


rowLength = 75; % maksymalna długość wiersza

% test

n1 = 1; % początkowa wartość n1
n2 = 1; % początkowa wartość n2

DispWithPause(sprintf(['n1\tn2\tWynik Programu\tPoprawny ' ...
    'Wynik\tRóżnica\t\tCzas']));

% testy dla stałych n1 i n2

for i = 1:n_norm_tests
    % test
    tic;
    [q] = P1Z29_MKO_integral2D(f, a, b, c, d, n1, n2);
    exec_time = toc;

    % wyświetlanie wyniku
    DispWithPause(sprintf('%i\t%i\t%f\t%f\t%f\t%fs', ...
        n1, n2, q, wyn, abs(q-wyn), exec_time));

    % zwiększanie n1 i n2
    n1 = round(n1 * ni);
    n2 = round(n2 * ni);
end

if n_norm_tests ~= 0 && n_rand_tests ~= 0
    DispWithPause(repmat('-', 1, rowLength));
end

% testy dla losowych n1 i n2

for i = 1:n_rand_tests
    % losowanie n1 i n2
    n1 = randi([nl, nu]);
    n2 = randi([nl, nu]);

    % test
    tic;
    [q] = P1Z29_MKO_integral2D(f, a, b, c, d, n1, n2);
    exec_time = toc;

    % wyświetlanie wyniku
    DispWithPause(sprintf('%i\t%i\t%f\t%f\t%f\t%fs', ...
        n1, n2, q, wyn, abs(q-wyn), exec_time));
end

DispWithPause(repmat('-', 1, rowLength));

end % function