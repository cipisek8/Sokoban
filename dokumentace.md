# Overview
XAML je rozdìlenı na 2 èásti:
- Menus - kde jsou všechna menu
- Game - kde je samotná hra

Tlaèítka v menu volají metody, které pøepínají _state.

Herní pole je vytvoøené z gridu, kterı má stejnou velikost jako mapa. Kadé pole v nìm je vlastní Image.

---
# Promìné
### MainWindow._state
Urèuje stav aplikace (v hlavním menu, ve høe, v menu vybírání levelu,...).

### MainWindow._rows
Urèuje poèet øádkù.

### MainWindow._columns
Urèuje poèet sloupcù.

### MainWindow._filePath
Ukládá cestu k souboru naèteného levelu.

### MainWindow._tiles
Ukládá všechna pole ve høe (zdi, góly, hráèe, krabice).

### Tile.Coordinates
Urèuje kde se pole nachází.

### Tile._listReference
Reference na list, kde se nachází všechna políèka.

### Tile.Item
Ukládá obrázek, kterı se dává do TileGrid.

---

# Metody
### MainWindow.LoadGame
Naète hru ze souboru z cesty _filePath.

### MainWindow.ShowError
Ukáe error, e se level nemohl naèíst.

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
## Tøída Tile a potomci

Tøídy Player, Box, Goal, Wall dìdí z abstarktní tøídy Tile, která zajišuje chovaní polí ve høe.

### Tile.Move
Zkontroluje, jestli se mùe pohnout v daném smìru (Tile.CanMove), a jestli mùe, pohne se (Tile.ChangePosition).

### Tile.CanMove
Zkontroluje, jestli se mùe pohnout v daném smìru.

### Tile.ChangePosition
Pohne se na danou pozici.
