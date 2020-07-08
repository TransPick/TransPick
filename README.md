# TransPick
 
## Overview
TransPick is a translator that provides screen translation feature using OCR.

## Description
TransPick works with the process below.

### Area Highlighting&Selecting Features
Overlay methods that previews the area to be specified on the screen **(Highlighters)** and the methods that actually specifies and captures the area **(Selectors)** are implemented in different classes.
Selectors detect the location of the mouse and click events, and use them to provide area preview feature using highlighters. And if selecting area is complete, it returns information about that area.

### Capture Features
Each capturer captures and returns a bitmap image of the specified area.
Unmanaged functions included in "user32.dll" or "gdi32.dll" are also used in this process.

### Extension-related
All functions related to OCR and translator are implemented through the **TransPick.Extension** library.
Developers can use the interfaces included in the library to implement extension features.

## License
 * [TransPick License](LICENSE)(MIT License)
 * [GameOverlay.Net](https://github.com/michel-pi/GameOverlay.Net/blob/master/LICENSE)(MIT License)
 * [Json.NET License](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md)(MIT License)
 * [MaterialDesignInXamlToolkit](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit/blob/master/LICENSE)(MIT License)
 * [SharpDX](https://github.com/sharpdx/SharpDX/blob/master/LICENSE)(MIT License)

## Contributors
손형준(Hyeong-jun Son aka junimiso04)(junimiso04@naver.com) - **Main Developer**
