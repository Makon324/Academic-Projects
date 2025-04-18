import sys
import os
from PIL import Image

# USAGE: python <ścieżka do skryptu> <ścieżka do folderu sourcowego> <ścieżka do folderu docelowego> ...
#   ... <docelowa szerokość> <docelowa wysokość> <docelowe rozszerzenie>

def rozszerzenie(path):  # zwraca rozszeżenie pliku o danej ścieżce - NULL - brak rozszerzenia
    ext = os.path.splitext(path)[1][1:].lower()  # [1:] pomija kropkę
    return ext if ext else "NULL"

def nazwa(path):  # zwraca nazwę pliku bez rozszerzenia
    return os.path.splitext(os.path.basename(path))[0]


dostepne_formaty = ["png", "jpg", "jpeg", "ppm", "gif", "tiff", "bmp"]

# sprawdzanie czy argumenty są dobre

ilosc_argumentow = len(sys.argv)

if ilosc_argumentow != 6:
    print("zła ilość argumentów")
    print("podaj: \t source folder \t folder docelowy \t docelową wysokość \t docelową szerokość \t docelowy format")
    exit()

if sys.argv[3].isnumeric()==False or sys.argv[4].isnumeric()==False:
    print("szerokość i wyskokość muszą być liczbami")
    exit()

docwidth = int(sys.argv[3])  # docelowa szerokość
docheight = int(sys.argv[4])  # docelowa wysokość

if docwidth >= 5000 or docheight >= 5000:    # i tak nie ma sensu robić większych jak 4k, a tak dla pewności, aby ktoś 10^12 nie pdoał
    print("szerokość i wyskokość powiny być mniejsze niż 5000")
    exit()

if dostepne_formaty.count(sys.argv[5]) == 0:
    print("podaj jeden z formatów: ", end = '')
    print(", ".join(dostepne_formaty))
    exit()

if os.path.isdir(sys.argv[1]) == False:
    print("source folder nie istnieje")
    exit()

#

if os.path.isdir(sys.argv[2]) == False:  # gdy docelowy katakolg nie istnieje, to go tworzy
    os.mkdir(sys.argv[2])

pliki = os.listdir(sys.argv[1])  # pliki w katalogu

i = 0
for plik in pliki:
    if dostepne_formaty.count(rozszerzenie(plik)) == 0:  # gdy dany plik nie jest akceptowalnego rozszerzenia, to nic z nim nie rób
        continue

    path = os.path.join(sys.argv[1], plik)  # ścieżka do pliku

    image = Image.open(path)

    # zmienianie rozmiaru obrazka
    width, height = image.size
    newwidth = 0  # nowa szerokość
    newheight = 0  # nowa wysokość
    top = 0  # górny margines
    left = 0  # lewy margines

    if width * docheight > height * docwidth:
        newwidth = docwidth
        newheight = round((docwidth/width)*height)
        top = round((docheight - newheight)/2)
    else:
        newheight = docheight
        newwidth = round((docheight / height) * width)
        left = round((docwidth - newwidth)/2)

    newpath = os.path.join(sys.argv[2], nazwa(plik) + '.' + sys.argv[5])  # ścieżka do zapisania obrazka

    if top == 0 and left == 0:  # gdy nie ma marginesów (oryginalna wielkość i nowa mają taki sam stosunek szerokość/wysokosć)
        image = image.resize((newwidth, newheight))
        image.save(newpath)
        image.close()
        i += 1
        continue

    else:
        image = image.resize((newwidth, newheight))
        result = Image.new(image.mode, (docwidth, docheight), (0, 0, 0))
        result.paste(image, (left, top))
        result.save(newpath)
        image.close()
        result.close()
        i += 1

print(f"z sukcesem zamieniono {i} obrazków")



