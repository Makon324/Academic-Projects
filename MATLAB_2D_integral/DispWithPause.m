function DispWithPause(text)
% Funkcja pomocnicza do wyświetlania tekstu z automatycznym zatrzymaniem 
% po osiągnięciu limitu wierszy.
% gdy argument to zero to robi clc; i zeruje licznik
% text - tekst do wyświetlenia (string lub char array)
%
% WEJŚCIE:
%   text - tekst do wyświetlenia

persistent currentLineCount; % Licznik bieżących wierszy

if isempty(currentLineCount)
    currentLineCount = 0; % Inicjalizacja
end

maxLines = 21; % maksymalna liczba wierszy na ekranie
rowLength = 75; % maksymalna długość wiersza
pause_message = ' Naciśnij dowolny klawisz, aby kontynuować '; % teskst ...
% ... wyświetlany podczas pauzy

padding_length = rowLength - length(pause_message);
pause_padding = repmat('-', 1, idivide(uint32(padding_length), 2));
pause_text = [pause_padding, pause_message, pause_padding];

text = [RemoveTabs(text), newline]; % chagnes all tabs to equivalent ...
% ... number of ' ', to make sure tables are displayed properly

% Wyświetlenie tekstu
while ~isempty(text)
    nl_index = find(text == newline, 1);
    if length(text) > rowLength && nl_index > rowLength + 1
        str = [text(1:rowLength-4), ' ...'];
        text = ['... ', text(rowLength-3:end)];
    elseif nl_index <= rowLength + 1
        str = text(1:nl_index-1);
        text = text(nl_index+1:end);
    else
        str = text;
        text = '';
    end

    disp(str);
    currentLineCount = currentLineCount + 1; % Zwiększ licznik wierszy
    
    % Jeśli liczba wierszy osiągnęła limit, czekaj na klawisz
    if currentLineCount >= maxLines - 1
        disp(pause_text);
        pause();
        fprintf(repmat('\b', 1, length(pause_text)+1));
        currentLineCount = 0; % Zresetuj licznik po pauzie
    end
end

end % function