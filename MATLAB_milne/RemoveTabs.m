function str = RemoveTabs(str)
% Funkcja zamienia znaki tabulacji w ciągu na odpowiadającą liczbę spacji,
% aby zapewnić, że długość tekstu o długości <=75 nie przekroczy rozmiaru
% na ekranie. 
%
% WEJŚCIE:
%   str - ciąg znaków, w którym mają zostać zamienione tabulacje
%
% WYJŚCIE:
%   str - ciąg znaków z zamienionymi tabulacjami na spacje
 
tabWidth = 8; % Szerokość tabulacji w liczbie znaków

% zamienianie tabulacji na spacje
i = 1;
i_in_line = 1;
while i <= length(str) 
    if str(i) == char(9) % if str(i) == '\t'
        str = [str(1:i-1), blanks(8 - mod(i_in_line-1, tabWidth)), ...
str(i+1:end)];
        i = i + 7 - mod(i_in_line-1, tabWidth);
        i_in_line = i_in_line + 7 - mod(i_in_line-1, tabWidth);
    elseif str(i) == newline % if str(i) == '\n'
        i = i + 1;
        i_in_line = 1;
    else
        i = i + 1;
        i_in_line = i_in_line + 1;
    end
end

end % fucntion