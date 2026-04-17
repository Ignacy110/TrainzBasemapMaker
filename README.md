# Trainz Basemap Maker

The Trainz Basemap Maker application allows you to quickly and easily create basemaps (satellite basemaps) for Trainz games. Currently, the application can only generate basemaps within Polish geographic coordinates.

The application is based on the EPSG:2180 coordinate system and downloads satellite basemaps from https://www.geoportal.gov.pl.

As the current version is primarily tailored for the Polish region and its specific geographic services, the documentation is provided in Polish.

<br>

### Third-Party Libraries

The application utilizes the following open-source library for geographic calculations:

* ProjNET (LGPL v2.1 license) – authored by Morten Nielsen and the NetTopologySuite-Team. This library is used for point-to-point coordinate conversions between the WGS84 and EPSG:2180 systems.

The full text of the LGPL license can be found here: https://www.gnu.org/licenses/old-licenses/lgpl-2.1.html

<br>

# Dokumentacja [PL]

Aplikacja *Trainz Basemap Maker* umożliwia szybkie i proste tworzenie basemap (podkładów satelitarnych) do gier serii Trainz.

Aplikacja opiera się o system koordynatów EPSG:2180 i pobiera podkłady satelitarne z serwisu https://www.geoportal.gov.pl **tylko na terenie Polski**.

>[!IMPORTANT]
>Ze względu na obciążenie strony georportalu, w pewnych godzinach (zazwyczaj z rana) oczekiwanie na pobranie podkładu może być znacznie wydłużone lub jego pobranie może być nie możliwe. Należy wtedy spróbować później najlepiej w godzinach popołudniowych lub wieczornych.

<br>

### Wykorzystane biblioteki (Third-party software)

Aplikacja wykorzystuje bibliotekę open-source do obliczeń geograficznych:

* **ProjNET** (licencja LGPL v2.1) – autorstwa Mortena Nielsena oraz NetTopologySuite-Team. Biblioteka służy do konwersji współrzędnych punktowych między systemami WGS84 a EPSG:2180.

Pełny tekst licencji LGPL dostępny jest tutaj: [https://www.gnu.org/licenses/old-licenses/lgpl-2.1.html](https://www.gnu.org/licenses/old-licenses/lgpl-2.1.html)

<br>

## Szybki Start:
1. Wpisz współrzędne (np. z Google Maps) w **Konwerterze**.
2. Kliknij **Konwertuj**.
3. Kliknij **Skonfiguruj i pobierz**. Gotowe - pobrałeś pierwszy podkład satelitarny do Trainz!
4. Klikając przyciski w panelu *Nawigacja* możesz pobierać kolejne podkłady.

<br>

## Obsługa aplikacji:

### 1. Konwerter:

Ze względu na działanie aplikacji na standardzie współrzędnych EPSG:2180, został zaimplementowany konwerter, który sam zamienia podane standardowe koordynaty (WGS84) na EPSG:2180.

Jeżeli znamy koordynaty w standardzie EPSG:2180, możemy pominąć tę sekcje i wprowadzić odpowiednie wartości w [sekcji konfiguracji](#2-konfiguracja).

#### 1.1 Współrzędne geograficzne (WGS84):

Wprowadzamy odpowiednio *szerokość* (latitude) i *długość* (longitude) geograficzną.

>[!IMPORTANT]
>Należy pamiętać aby wpisywać tylko cyfry oraz ewentualnie kropkę jako część po przecinku.

#### 1.2 Konwersja:

Po wprowadzeniu współrzędnych klikamy przycisk *Konwertuj na EPSG:2180*, co powoduje automatyczne wpisanie odpowiednich wartości do [sekcji konfiguracja](#2-konfiguracja).
<br>

### 2. Konfiguracja:

Sekcja konfiguracji jest przeznaczona do sprecyzowania parametrów pobieranych podkładów satelitarnych oraz tworzonych plików do gry Trainz.

#### 2.1 Współrzędne EPSG:2180:

Możemy skorzystać z [konwertera](#1-konwerter) lub ręcznie wprowadzić *szerokość* (latitude) i *długość* geograficzną (longitude).

>[!IMPORTANT]
>Należy pamiętać aby wpisywać tylko cyfry - te pola przyjmują liczby całkowite.

#### 2.2 Rozdzielczość podkładów satelitarnych:

Należy wybrać pożądaną rozdzielczość podkładów satelitarnych. Im wyższa rozdzielczość tym więcej szczegółów, lecz jednocześnie dłuższy czas pobierania podkładów satelitarnych.

#### 2.3 Rodzaj podkładów satelitarnych:

Należy wybrać rodzaj otrzymywanych podkładów satelitarnych.
Dostępne opcje:
- ortofotomapa
- cieniowanie

>[!TIP]
>Dla zlikwidowanych lub mocno zarośniętych lini kolejowych opcja pobierania podkładów typu *cieiowanie* może być lepsza.

#### 2.4 Rok podkładów satelitarnych:

Należy wpisać w polu rok (np. *2013*), z którego chcemy otrzymywać podkłady satelitarne.

>[!IMPORTANT]
>Jeżeli po pobraniu podkładu podgląd jest biały, należy wybrać późniejszy rok. Dzieje się tak ponieważ, dla wybranego roku nie wykonano do tej pory żadnych zdjęć lotniczych (dostępnych na geoportalu).

#### 2.5 Tworzenie plików Trainz:

>[!TIP]
>Tej sekcji zdecydowanie warto poświęcić uwagę, zwłaszcza przy większych projektach. Dzięki umiejętnemu zarządzaniu wpisywanymi parametrami można bez większych problemów oraz konfliktów odnajdywać się pośród dziesiątek pobieranych podkładów.

Jeżeli checkbox *Twórz foldery i pliki Trainz* został zaznaczony, to z każdym pobranym podkładem satelitarnym, podkład oraz pliki dla Trainz są zapisywane na dysku w **folderze *Kuids***. Folder ten tworzy się automatycznie w tej samej lokalizacji co uruchomiony plik *.exe*.

##### 2.4.1 Lista folderów:

Lista *twoje foldery* pokazuje wszystkie foldery w katalogu *Kuids*.

>[!NOTE]
>Program został stworzony z myślą o możliwości segragacji pobieranych podkładów.
>
>Możemy segregować - dzielić podkłady na trasy, nadając im odpowiednie nazwy.
>
>Na przykład: folder: *Opole-Ozimek* oraz folder: *Opole-Kluczbork*.
>
>Dzięki temu nie musimy trzymać wszystkich podkładów razem wymieszanych w jednym folderze, lecz dzielić je według preferencji (np na wspomniane trasy czy linie kolejowe).

Jeżeli mamy jakiś folder z podkładami wyświetli się on na liście. Klikając na dany folder z listy, *nazwa docelowego folderu* jest uzupełniana automatycznie tę samą nazwą.

Jeżeli nie mamy żadnego folderu lub chcemy stworzyć nowy zobacz [kolejny podpunkt](#242-nazwa-docelowego-folderu).

##### 2.4.2 Nazwa docelowego folderu:

W tym polu należy wpisać nazwę folderu, w którym będą się tworzyły pliki do gry Trainz. Po kliknięciu przycisku *Skonfiguruj i pobierz* folder o wpisanej nazwie utworzy się w katalogu *Kuids*, następnie do niego będą trafiały pobierane podkłady.

>[!NOTE]
>Przykład:
>Ponieważ będziemy pobierać podkłady na trasie między Opolem, a Nysą to folder nazwiemy *Opole-Nysa*.

##### 2.4.3 Oznaczenie podkładu:

W tym polu najlepiej jest wpisać skróconą nazwę folderu.

Oznaczenie to będzie widniało w nazwie podkładu satelitarnego w grze Trainz, co ma na celu ułatwić identyfikacje z jakiego folderu (grupy podkładów) korzystamy.

>[!NOTE]
>Przykład:
>Jeżeli mamy folder *Opole-Nysa* to można wpisać na przykład w tym polu: *OpNy*. Napotykając w grze Trainz z takim oznaczeniem w nazwie będziemy wiedzieli że jest to podkład obejmujący teren na tej trasie kolejowej.

##### 2.4.4 Numer podkładu:

W tym polu należy wpisać numer dla podkładu.

Numery podkładów dla ułatwienia korzystania powinny być unikatowe (nie powtarzające się).

>[!NOTE]
>W tym polu najczęściej wpisujemy tylko wartość od której chcemy zacząć naszą numerację podkładów, ponieważ pobierając kolejne podkłady korzystając z sekcji nawigacji, numer podkładu zwiększa się automatycznie.

##### 2.4.5 Oznaczenie kuidu:

Te pole jest uzupełniane automatycznie przez program.

Wartości wpisane w tych (dwóch) polach nadają identyfikator tak zwanej zależności do gry Trainz, który powinien być unikatowy spośród wszystkich dodatków jakich posiadasz w grze.

### 3. Nawigacja:

Po skonfigurowaniu i pobraniu pierwszego podkładu w zakładce [konfiguracja](#2-konfiguracja), kolejne możemy pobierać nawigując się przyciskami.

Podczas nawigowania pola koordynatów, numeru podkładu oraz oznaczenia oznaczenia kuidu są uzupełniane automatycznie.

### 4. Lista podkładów:

Lista podkładów pokazuje wszystkie pobrane podkłady dla danej grupy - folderu.

Klikając raz na dany podkład spośród listy wyświetlimy jego podgląd.
Klikając dwa razy na dany podkład spośród listy, wartości w sekcji konfiguracja uzupełnią się danymi danego podkładu, aby umożliwić nawigowanie od niego.

## Instalacja podkładów:

Aby zainstalować pobrane podkłady należy przeciągnąć cały katalog *Kuids* lub wybrane foldery z jego wnętrza do menadżera zawartości Trainz. Aby zatwierdzić zależności (zainstalowane podkłady) uruchamiamy grę Trainz z menadżera zawartości.