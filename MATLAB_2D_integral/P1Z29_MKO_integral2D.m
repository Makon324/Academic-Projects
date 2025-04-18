function [q] = P1Z29_MKO_integral2D(f, a, b, c, d, n1, n2)
% Funkcja oblicza całkę podwójną z f(x, y) na obszarze D = [a,b]x[c,d]
% używając złożonych, 2-punktowych kwadratur Gaussa-Legendre'a.
%
% WEJŚCIE:
%   f   - uchwyt do funkcji f(x, y), którą należy całkować
%   a, b - granice całkowania dla zmiennej x (a<b)
%   c, d - granice całkowania dla zmiennej y (c<d)
%   n1  - liczba podprzedziałów wzdłuż osi X (>=1)
%   n2  - liczba podprzedziałów wzdłuż osi Y (>=1)
%
% WYJŚCIE:
%   q - przybliżona wartość całki

hx = (b-a)/n1; % długości podprzedziałów wzdłuż X
hy = (d-c)/n2; % długości podprzedziałów wzdłuż Y

% Węzły Gaussa-Legendre'a
xNodes = [a + hx*0.5*(1-sqrt(1/3)) + (0:n1-1) * hx; ...
          a + hx*0.5*(1+sqrt(1/3)) + (0:n1-1) * hx];
yNodes = [c + hy*0.5*(1-sqrt(1/3)) + (0:n2-1) * hy; ...
          c + hy*0.5*(1+sqrt(1/3)) + (0:n2-1) * hy];

% Obliczenie sumy dla kwadratury Gaussa-Legendre'a
q = 0; % wynik całki

for i = 1:n1
    for j = 1:n2
        for k=1:2
            for l=1:2
                % dodawanie wartości funkcji w węzłach
                q = q + f(xNodes(k,i), yNodes(l,j));
            end
        end
    end
end

% skalowanie wyniku
q = q * hx * hy / 4;

end % function