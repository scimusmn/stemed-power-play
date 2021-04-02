# Power Play

Educational mini-game about personal energy use and energy conservation.

Built in Unity, exported for WebGL/HTML5.

There are two packages in this repo.
- `build` contains the exported HTML5 files for hosting.
- `loading-screen` contains the assets used to customize the loading screen.
- `source` contains the source Unity project.



#### Loading screen customization
1. Build for WebGL/HTML5. In Project Settings -> Player -> Resolution and Presentation -> WebGL Template, Default should already be selected.

2. Unity will spit out a `Build` folder, an `index.html` file, and a `TemplateData` folder. The `TemplateData` folder will contain the load screen assets--such as the style sheet and all the default (Unity) icons.

3. Overwrite the following four files using the same inside `/loading-screen`:
  - `favicon.ico`
  - `unity-logo-dark.png`
  - `unity-log-light.png`
  - `webgl-logo.png`
