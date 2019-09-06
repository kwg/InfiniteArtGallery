# InfiniteArtGallery

## About
The Infinite Art Gallery is an experimental project to generate various forms of artwork using evoloving neural networks. This work expands on methods developed by Picbreeder: A Case Study in Collaborative Evolutionary Exploration of Design Space (Secretan et al 2008), Evolving 3D objects with a generative encoding inspired by developmental biology (Clune and Lipson 2011), and AnimationBreeder and ThreeDimensionalAnimationBreeder (Tweaser, Gillespie, and Schrum 2018) in its curent form, and has planned features based on Interactively Evolving Compositional Sound Synthesis Networks (Jónsson, Hoover, and Risi 2015) and Unshackling Evolution: Evolving Soft Robots with Multiple Materials and a Powerful Generative Encoding (Cheney, et al 2013)

This project includes code that was directly derived from MM-NEAT (c) 2014 The University of Texas and 2016 Southwestern University

Infinite Art Gallery is open sourced under the GPL3 license (notices are being applied to all source files as time allows)

## Installation
This repository holds a Unity project that should open directly in the Unity Editor. It was developed using version 2018.x and has only been tested in those environments. This can be imported into the editor and built from there for the target platform. Currenly, the alpha builds have been tested and known to work on PC (x64) and Mac.

## Usage
The main menu allows for the selection of various test chambers for 2d and 3d artworks, as well as the test for the Art Gallery. These tests have different control schemes:

### 2d and 3d test chambers
There is no character movement in these tests
Left-click to mutate the artwork
Right-click to create a new artwork with a new network
(3d objects only) Middle-click to toggle transparency in the artwork
  - note: this does not redraw the current form and a new sculpture will need to be generated to see the transparency
Escape brings up the menu

### Art Gallery
Character movement is based on FPS controls (W, A, S, D with mouselook)
Left-click on a portal to save the genome to your invetory
Right-click a portal to replace that portal's genome with the selected invenetory item
* note: sculptres are non interactive as of version 0.05.1

Functions will spawn on cubes in the middle of the room randomly. walking over a cube will pick it up and replace the selected function from the invetory unless the player already has that function on their bar.
