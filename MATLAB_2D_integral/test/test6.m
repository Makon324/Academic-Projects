function test6()
% Funkcja testująca dla programu P1Z29_MKO_integral2D
% Funkcja testuje poprawność działania programu P1Z29_MKO_integral2D
% przy obliczaniu całki podwójnej wielomianu 2 argumentowego 5 stopnia
% na obszarze D = [a, b] x [c, d]. Współczynniki wielomianu są generowane
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
n = 5; % stopień wielomianu
[rl, ru] = deal(-5, 5); % ograniczenie na przedziały [a, b] i [c, d]
[pl, pu] = deal(-5, 5); % ograniczenie parametrów wielomianu
[nl, nu] = deal(10, 200); % ograniczenie na n1 i n2 w ostatnich testach
num_norm_tests = 10; % ilość normalnych testów
num_rand_tests = 3; % ilość testów z losowymi n1 i n2
ni = 2; % mnożnik pomiędzy kolejnymi n1 i n2
rowLength = 75; % maksymalna długość wiersza
test_desc_path = 'desc_test6.txt'; % ścieżka do pliku z opisem testu

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
p = pl + (pu - pl) * rand(1, (n+2)*(n+1)/2);

DispWithPause(sprintf('Wylosowano: p = [%s]', strjoin(string(p), ', ')));
DispWithPause(sprintf('[a, b] = [%f, %f], [c, d] = [%f, %f]', a, b, c, d));
DispWithPause(sprintf(['-> liczenie całki z f(x, y) = %fx^5 + %fx^4y +' ...
    ' %fx^3y^2 + %fx^2y^3 + %fxy^4 +%fy^5 + %fx^4 + %fx^3y + %fx^2y^2 ' ...
    '+ %fxy^3 + %fy^4 + %fx^3 + %fx^2y + %fxy^2 + %fy^3 + %fx^2 + ' ...
    '%fxy + %fy^2 + %fx + %fy + %f'], p(21), p(20), p(19), p(18), ...
    p(17), p(16), p(15), p(14), p(13), p(12), p(11), p(10), p(9), p(8), ...
    p(7), p(6), p(5), p(4), p(3), p(2), p(1)));
DispWithPause(sprintf('na obszarze [%f, %f] x [%f, %f]', a, b, c, d));
DispWithPause(repmat('-', 1, rowLength));

% funckja
f = @(x, y) p(21)*x^5 + p(20)*x^4*y + p(19)*x^3*y^2 + p(18)*x^2*y^3 + ...
p(17)*x*y^4 + p(16)*y^5 + p(15)*x^4 + p(14)*x^3*y +p(13)*x^2*y^2 + ...
p(12)*x*y^3 + p(11)*y^4 + p(10)*x^3 + p(9)*x^2*y + p(8)*x*y^2 + ...
p(7)*y^3 + p(6)*x^2 + p(5)*x*y + p(4)*y^2 + p(3)*x + p(2)*y + p(1);

% wynik analityczny
wyn = p(21)*integral_xnym(5, 0, a, b, c, d) + ...
    p(20)*integral_xnym(4, 1, a, b, c, d) + ...
    p(19)*integral_xnym(3, 2, a, b, c, d) + ...
    p(18)*integral_xnym(2, 3, a, b, c, d) + ...
    p(17)*integral_xnym(1, 4, a, b, c, d) + ...
    p(16)*integral_xnym(0, 5, a, b, c, d) + ...
    p(15)*integral_xnym(4, 0, a, b, c, d) + ...
    p(14)*integral_xnym(3, 1, a, b, c, d) + ...
    p(13)*integral_xnym(2, 2, a, b, c, d) + ...
    p(12)*integral_xnym(1, 3, a, b, c, d) + ...
    p(11)*integral_xnym(0, 4, a, b, c, d) + ...
    p(10)*integral_xnym(3, 0, a, b, c, d) + ...
    p(9)*integral_xnym(2, 1, a, b, c, d) + ...
    p(8)*integral_xnym(1, 2, a, b, c, d) + ...
    p(7)*integral_xnym(0, 3, a, b, c, d) + ...
    p(6)*integral_xnym(2, 0, a, b, c, d) + ...
    p(5)*integral_xnym(1, 1, a, b, c, d) + ...
    p(4)*integral_xnym(0, 2, a, b, c, d) + ...
    p(3)*integral_xnym(1 , 0, a, b, c, d) + ...
    p(2)*integral_xnym(0, 1, a, b, c, d) + p(1)*(b-a)*(d-c);

% test 

popr_test(f, wyn, a, b, c, d, ni, nl, nu, num_norm_tests, num_rand_tests);

end % function