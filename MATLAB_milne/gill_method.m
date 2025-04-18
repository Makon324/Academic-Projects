function [y, z] = gill_method(x, y, z, f, h)
% Oblicza pierwsze 4 punkty za pomocą metody Gilla
%
% WEJŚCIE:
%   x, y, z - jak w głównej funkcji
%   f       - funkcja pomocnicza dla równania różniczkowego
%   h       - krok
%
% WYJŚCIE:
%   y, z - zaktualizowane wektory wartości rozwiązania i pochodnych

for i = 1:3
    % Liczenie wartości pośrednich
    k1y = h * z(i);
    k1z = h * f(x(i), y(i), z(i));

    k2y = h * (z(i) + 0.5 * k1z);
    k2z = h * f(x(i) + 0.5 * h, y(i) + 0.5 * k1y, z(i) + 0.5 * k1z);

    k3y = h * (z(i) + 0.5 * (-1 + sqrt(2)) * k1z + ...
        (1 - 0.5 * sqrt(2)) * k2z);
    k3z = h * f(x(i) + 0.5 * h, y(i) + 0.5 * (-1 + sqrt(2)) * k1y + ...
        (1 - 0.5 * sqrt(2)) * k2y, z(i) + 0.5 * (-1 + sqrt(2)) * k1z + ...
        (1 - 0.5 * sqrt(2)) * k2z);

    k4y = h * (z(i) - 0.5 * sqrt(2) * k2z + (1 + 0.5 * sqrt(2)) * k3z);
    k4z = h * f(x(i) + h, y(i) - 0.5 * sqrt(2) * k2y + ...
        (1 + 0.5 * sqrt(2)) * k3y, z(i) - 0.5 * sqrt(2) * k2z + ...
        (1 + 0.5 * sqrt(2)) * k3z);

    % Aktualizacja wyniku
    y(i + 1) = y(i) + (1 / 6) * (k1y + (2 - sqrt(2)) * k2y + ...
        (2 + sqrt(2)) * k3y + k4y);
    z(i + 1) = z(i) + (1 / 6) * (k1z + (2 - sqrt(2)) * k2z + ...
        (2 + sqrt(2)) * k3z + k4z);
end

end % function