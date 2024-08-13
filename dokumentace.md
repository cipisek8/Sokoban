# Overview
XAML je rozd�len� na 2 ��sti:
- Menus - kde jsou v�echna menu
- Game - kde je samotn� hra

Tla��tka v menu volaj� metody, kter� p�ep�naj� _state.

Hern� pole je vytvo�en� z gridu, kter� m� stejnou velikost jako mapa. Ka�d� pole v n�m je vlastn� Image.

---
# Prom�n�
### MainWindow._state
Ur�uje stav aplikace (v hlavn�m menu, ve h�e, v menu vyb�r�n� levelu,...).

### MainWindow._rows
Ur�uje po�et ��dk�.

### MainWindow._columns
Ur�uje po�et sloupc�.

### MainWindow._filePath
Ukl�d� cestu k souboru na�ten�ho levelu.

### MainWindow._tiles
Ukl�d� v�echna pole ve h�e (zdi, g�ly, hr��e, krabice).

### Tile.Coordinates
Ur�uje kde se pole nach�z�.

### Tile._listReference
Reference na list, kde se nach�z� v�echna pol��ka.

### Tile.Item
Ukl�d� obr�zek, kter� se d�v� do TileGrid.

---

# Metody
### MainWindow.LoadGame
Na�te hru ze souboru z cesty _filePath.

### MainWindow.ShowError
Uk�e error, �e se level nemohl na��st.

### MainWindow.GenerateMap
Zm�n� velikost grid� v XAML na pot�ebnou velikost a p�id� grafiku tr�v do GrassGrid.

### MainWindow.KeyboardControls
Kontroluje vstupy z kl�vesnice a podle _state d�l� v�ci.

### MainWindow.CheckGoals
Zkontroluje, jestli v�echny g�ly maj� na sob� krabici, a jestli jo tak zavol� MainWindow.ChangeState na zm�n�n� _state na AppState.Victory.

### MainWindow.ChangeState
M�n� _state. 

### MainWindow.ChangeMenuControls
Zavol� MainWindow.ChangeState na zm�n�n� _state na AppState.Controls.

### MainWindow.ChangeMenuLevel
Zavol� MainWindow.ChangeState na zm�n�n� _state na AppState.LevelSelect.

### MainWindow.LoadFile
Otev�e FileDialog a pokus� se na��st level.

### MainWindow.LoadLevelButton
Na�te level ze souboru Resources\Levels.

---
## T��da Tile a potomci

T��dy Player, Box, Goal, Wall d�d� z abstarktn� t��dy Tile, kter� zaji��uje chovan� pol� ve h�e.

### Tile.Move
Zkontroluje, jestli se m��e pohnout v dan�m sm�ru (Tile.CanMove), a jestli m��e, pohne se (Tile.ChangePosition).

### Tile.CanMove
Zkontroluje, jestli se m��e pohnout v dan�m sm�ru.

### Tile.ChangePosition
Pohne se na danou pozici.
