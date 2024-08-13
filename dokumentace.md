# Overview
XAML je rozdìlený na 2 èásti:
- Menus - kde jsou všechna menu
- Game - kde je samotná hra

Tlaèítka v menu volají metody, které pøepínají GameState.

Herní pole je vytvoøené z gridu, který má stejnou velikost jako mapa. Každá kostka v nìm je vlastní Image.

---
# Promìné
### MainWindow ---- AppState _state
Urèuje stádium aplikace (v hlavním menu, ve høe, v menu vybírání levelu,...).
### MainWindow ---- int _rows
Urèuje poèet øádkù.
### MainWindow ---- int _columns
Urèuje poèet sloupcù.
### MainWindow ---- string _filePath
Ukládá cestu k souboru naèteného levelu.
### MainWindow ---- Tile[] _tiles;
Ukládá všechna pole ve høe (zdi, góly, hráèe, krabice).

### Tile ---- Point Coordinates;
Urèuje kde se pole nachází.
### Tile ---- Tile[] _listReference;
Reference na list, kde se nachází všechna políèka.
### Tile ---- Image Item;
Ukládá obrázek, který se dává do TileGrid.

---

# Metody
### MainWindow.LoadGame
Naète hru ze souboru z cesty _filePath.

### MainWindow.ShowError
Ukáže error, že se level nemohl naèíst.

### MainWindow.GenerateMap
Zmìní velikost gridù v XAML na potøebnou velikost a pøidá grafiku tráv do GrassGrid.

### MainWindow.KeyboardControls
Kontroluje vstupy z klávesnice a podle _state dìlá vìci.

### MainWindow.CheckGoals
Zkontroluje, jestli všechny góly mají na sobì krabici, a jestli jo tak zavolá MainWindow.ChangeState na zmìnìní _state na AppState.Victory.

### MainWindow.ChangeState
Mìní _state. 

### MainWindow.ChangeMenuControls
Zavolá MainWindow.ChangeState na zmìnìní _state na AppState.Controls.

### MainWindow.ChangeMenuLevel
Zavolá MainWindow.ChangeState na zmìnìní _state na AppState.LevelSelect.

### MainWindow.LoadFile
Otevøe FileDialog a pokusí se naèíst level.

### MainWindow.LoadLevelButton
Naète level ze souboru Resources\Levels.

---

### Tile.Move
Zkontroluje, jestli se mùže pohnout v daném smìru (Tile.CanMove), a jestli mùže, pohne se (Tile.ChangePosition).
### Tile.CanMove
Zkontroluje, jestli se mùže pohnout v daném smìru.
### Tile.ChangePosition
Pohne se na danou pozici.
