function [y, z] = milne_method(x, y, z, f, h, N, m)
% Iteruje metodą Milne'a z korekcją.
%
% WEJŚCIE:
%   x, y, z - jak w głównej funkcji
%   f       - funkcja pomocnicza dla równania różniczkowego
%   h       - krok
%   N       - liczba kroków iteracyjnych
%   m       - liczba iteracji korekcji Milne'a
%
% WYJŚCIE:
%   y, z - zaktualizowane wektory wartości rozwiązania i pochodnych

for i = 4:N
    % Licznie predykatora
    y(i + 1) = y(i - 3) + 4 * h / 3 * (2 * z(i) - z(i - 1) + 2 * z(i - 2));
    z(i + 1) = z(i - 3) + 4 * h / 3 * (2 * f(x(i), y(i), z(i)) - ...
        f(x(i - 1), y(i - 1), z(i - 1)) + ...
        2 * f(x(i - 2), y(i - 2), z(i - 2)));

    % Liczenie korektora
    for j = 1:m
        y(i + 1) = y(i - 1) + h / 3 * (z(i - 1) + 4 * z(i) + z(i + 1));
        z(i + 1) = z(i - 1) + h / 3 * (f(x(i - 1), y(i - 1), z(i - 1)) ...
            + 4 * f(x(i), y(i), z(i)) + f(x(i + 1), y(i + 1), z(i + 1)));
    end
end

end % function
