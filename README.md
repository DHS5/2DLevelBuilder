# 2DLevelBuilder
## In-Game 2D Level Builder

### This project is an In-Game 2D Level Builder that can be easily added to any 2D game project. It is based on Unity Tilemaps system and works a lot alike.

![Thumbnail](https://user-images.githubusercontent.com/94963203/225274693-640fdc02-f710-4759-82d1-bd661be3bc35.png)

The level builder is highly customizable :
- it uses a style system and a save system completely independant from one another, allowing the visualization of any level with any tile styles
- the level builder can be used in editor play mode as well as in a build
- in editor play mode, the user can choose whether to save a level as an asset (scriptabe object) or as a text file located at the project persistent data path
- asset level saves allow the developer to create base levels directly in the build
- text file level saves allow players to save levels on their device and potentially post their creation online thanks to a third party
- for text file level save, a custom serialization system, optimized for very large levels is used to ensure that only important data is saved and that save text file size stay low
- the command in the level builder are entirely re-bindable
- every action in the level builder can be undo or redo and are usable with re-bindable shortcuts
- a style sheet system handles all of the level builder UI allowing the developer to simply and quickly personalize the whole project's UI thanks to this package : https://github.com/DHS5/AdvancedUI
