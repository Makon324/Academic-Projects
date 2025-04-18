function q = integral_xnym(n, m, a, b, c, d)
% Funkcja oblicza całkę podwójną z x^n*y^m na obszarze D = [a,b]x[c,d]
%
% WEJŚCIE:
%   n, m - potęgi przy x i y
%   a, b - granice całkowania dla zmiennej x
%   c, d - granice całkowania dla zmiennej y
%
% WYJŚCIE:
%   q - wartość całki

q = ((b^(n+1) - a^(n+1))/(n+1)) * ((d^(m+1) - c^(m+1))/(m+1));

end