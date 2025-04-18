function [a, b] = RandRange(x, y)
% Funkcja generuje dwie losowe liczby z przedziału [x, y] i zwraca je 
% w porządku rosnącym, 
% liczby odpowiadają przedziałowi zwierającemu się w [x, y]
%
% WEJŚCIE:
%   x, y - liczby rzeczywiste określające przedział losowania
%
% WYJŚCIE:
%   [a, b] - losowy przedział zawierający się w [x, y]

a = 0;
b = 0;

% losowanie [a, b] dopóki nie uzyskamy przedziału zajmującego co najmniej
% 1/10 miejsca
while b - a < (y - x)/10

% losowanie a i b
a = x + (y - x) * rand();
b = x + (y - x) * rand();

% zamienianie a i b, jeżeli jest to potrzebne
if a > b
    [a, b] = deal(b, a);
end

end

end % function