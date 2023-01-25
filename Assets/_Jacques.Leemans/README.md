# Nexefy-Unity-Skills-Test-Project

Requirements: https://docs.google.com/document/d/1CEn0DT63ul2wpoj5FX8zkkAQzW5QVePh-CIaSHT_A3Q/edit

* As mentioned above, each model needs 16 screenshots in png format capture, rotating the model by 22.5 degrees each frame.
* Outputs should be in an output folder named Output relative to the project root. I.e. the directory above the Asset folder.
* Each model should have outputs placed in a separate folder as per their name. I.e. what is shown below.
* The frames should use this naming syntax “frame{index}” . The index should be formatted to be made of 4 digits e.g. frame0000, frame0001, frame0002, frame0010 etc.
* The output dimensions of each frame must be 512x512 pixels.
* Once play mode has been entered, the export process must not require ‘user input’ to advance the routine. No key presses or interacting with ui buttons.
* Once all of the frames have been exported for each model, the program must exit playmode.
* The model must be completely contained within the frame with a small gap from the edge but remain as large as possible. 
* The model must remain a consistent distance from the camera in each frame. We don’t want the model to be jumping around between frames.
* Use an orthographic projection for rendering the models.
* The model should be lit nicely in each frame. We don’t want to be rendering a silhouette
* The prefabs in the Prefabs folder should not be modified nor any of the supporting assets in the AssetStore-Content folder. This includes any transform scale and positioning.
* The functionality is not required to work inside a built version of the project (ie windows player) but the solution should not prevent built executable from occurring.
  * Be mindful of UnityEditor libraries and use define detectives as required.
* Continue to use addressables as the means to load in assets.

Extra Requirements

* Each output frame renders to a transparent background.
* The Unity Editor GameView should display what the output frame looks like. It doesn’t need to match in resolution/aspect ratio but camera position and orthographic size should be consistent.
* Provide commenting and tooltips where beneficial.
* Once all of the frames of a model have been captured, unload the model from memory before moving onto the next item for export.

# Notes

## Bounds

Car-1203-Black = Center: (0.00, 0.31, -0.01), Extents: (1.03, 0.81, 2.26)
Car-1203-Yellow = Center: (0.00, 0.31, -0.01), Extents: (1.03, 0.81, 2.26)
Street-Lamp-Short = Center: (0.00, 1.53, 0.11), Extents: (0.50, 2.03, 0.61)
Street-Lamp-Tall = Center: (0.00, 2.32, 0.00), Extents: (0.50, 2.82, 0.95)
Super-Spitfire = Center: (-0.04, 2.40, -0.53), Extents: (12.29, 2.91, 10.46)
Train-Industrial-Dr14 = Center: (0.00, 2.11, 0.22), Extents: (1.56, 2.61, 6.85)
Trashbin-Green = Center: (0.00, 0.37, 0.00), Extents: (0.50, 0.87, 0.50)