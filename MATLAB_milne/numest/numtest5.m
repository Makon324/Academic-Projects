function numtest5()
% Celem testu jest zbadanie, jak metoda Milne'a sprawdza się w przypadku, 
% gdy rozwiązaniem równania różniczkowego jest funkcja szybko oscylująca.
%
% Funkcja nie posiada wejśća, ani wyjścia.s

% stałe
b = @(x) 0; % funkcja, prawa strona równania
[x0, xN] = deal(0, 1); % przedział rozwiązania
y0 = [1, 0]; % warunki początkowe [y, y']
N = 1000; % ilość podprzedziałów
rowLength = 75; % maksymalna długość wiersza
test_desc_path = 'desc_numtest5.txt'; % ścieżka do pliku z opisem testu

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

% test 
DispWithPause(sprintf('a0   \terror      \terror_gill'));

a0 = 1000;
while(a0 < 1e6)
    a = {@(x) a0, @(x) 0, @(x) 1}; % tablica komórkowa współczynników

    [y, x] = P2Z40_MKO_milne(b, a, x0, xN, y0, N);

    % Liczenie błędu
    y_exact = cos(sqrt(a0)*x); % rozwiązanie analityczne
    error_gill = max(abs(y(1:4) - y_exact(1:4)));
    error = max(abs(y - y_exact));

    DispWithPause(sprintf('%i\t%.5e\t%.5e', a0, error, error_gill));

    a0 = 2 * a0; % aktualizacja a0
end

end % function