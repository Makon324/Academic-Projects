function [y, x] = P2Z40_MKO_milne(b, a, x0, xN, y0, N, m)
% Rozwiązywanie równania różniczkowego:
%   a{3}(x)y'' + a{2}(x)y' + a{1}(x)y = b(x)
% na przedziale [x0, xN] z warunkami początkowymi y0 = [y(0), y'(0)].
%
% WEJŚCIE:
%   b   - funkcja, prawa strona równania
%   a   - tablica komórkowa z funkcjami współczynników a{1}(x), ...
%   ... a{2}(x), a{3}(x)
%   x0  - początek przedziału rozwiązania
%   xN  - koniec przedziału rozwiązania
%   y0  - warunki początkowe, wektor [y(0), y'(0)]
%   N   - liczba podprzedziałów (>= 3)
%   m   - liczba iteracji korekcji Milne'a (>= 1), domyślnie 1
%
% WYJŚCIE:
%   y   - wektor wartości rozwiązania
%   x   - wektor punktów rozwiązania

% Ustawienie domyślnej wartości m
if nargin < 7
    m = 1;
end

% Parametry
h = (xN - x0) / N; % Krok czasowy
x = linspace(x0, xN, N + 1);

% Inicjalizacja
y = zeros(1, N + 1); % Rozwiązanie
z = zeros(1, N + 1); % z = y'
y(1) = y0(1);
z(1) = y0(2);

% Funkcje współczynników równania
a2 = a{3};
a1 = a{2};
a0 = a{1};

% Definicja równań jako pierwszego rzędu (y', z')
f = @(x, y, z) (b(x) - a0(x) * y - a1(x) * z) / a2(x);

% Obliczanie pierwszych punktów metodą Gilla
[y, z] = gill_method(x, y, z, f, h);

% Iteracja metodą Milne'a
[y, ~] = milne_method(x, y, z, f, h, N, m);

end % function
