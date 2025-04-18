function test4()
% Funkcja testująca dla programu P1Z29_MKO_integral2D
% Funkcja testuje poprawność działania programu P1Z29_MKO_integral2D
% przy obliczaniu całki podwójnej wielomianu 2 argumentowego 3 stopnia
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
n = 3; % stopień wielomianu
[rl, ru] = deal(-5, 5); % ograniczenie na przedziały [a, b] i [c, d]
[pl, pu] = deal(-5, 5); % ograniczenie parametrów wielomianu
[nl, nu] = deal(10, 200); % ograniczenie na n1 i n2 w ostatnich testach
num_norm_tests = 10; % ilość normalnych testów
num_rand_tests = 3; % ilość testów z losowymi n1 i n2
ni = 2; % mnożnik pomiędzy kolejnymi n1 i n2
rowLength = 75; % maksymalna długość wiersza
test_desc_path = 'desc_test4.txt'; % ścieżka do pliku z opisem testu

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
DispWithPause(sprintf(['-> liczenie całki z f(x, y) = %fx^3 + %fx^2y +' ...
    ' %fxy^2 + %fy^3 + %fx^2 + %fxy + %fy^2 + %fx + %fy + %f'], p(10), ...
    p(9), p(8), p(7), p(6), p(5), p(4), p(3), p(2), p(1)));
DispWithPause(sprintf('na obszarze [%f, %f] x [%f, %f]', a, b, c, d));
DispWithPause(repmat('-', 1, rowLength));

% funkcja
f = @(x, y) p(10)*x^3 + p(9)*x^2*y + p(8)*x*y^2 + p(7)*y^3 + ...
p(6)*x^2 + p(5)*x*y + p(4)*y^2 + p(3)*x + p(2)*y + p(1);

% wynik analityczny
wyn = p(10)*integral_xnym(3, 0, a, b, c, d) + ...
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