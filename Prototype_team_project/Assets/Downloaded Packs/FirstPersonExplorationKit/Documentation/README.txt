//
// First Person Exploration Kit
// A complete package to create a First Person Exploration game.
// Copyright 2016 While Fun Games
// http://whilefun.com
//

===================
>Setup Instructions:
===================

Here are the steps to go from a new scene to one that works with First Person Exploration Kit.

Player Controller:
=================

You can use the included "FPEPlayerController" prefab, or build from scratch using the Unity Standard Asset "First Person Controller"

Option 1:
--------

1) Add some level geometry to your scene for the player to walk on (a plane is fine), and direction light to light the scene.
2) To use the "FPEPlayerController", just drag it into your scene and you're done.


Option 2:
--------
To rebuild using another player controller (such as the Standard Asset version of "First Person Controller"):

1) Add some level geometry to your scene for the player to walk on (a plane is fine), and direction light to light the scene.
2) Add a Standard Asset "First Person Controller" to your scene (Standard Assets > Characters > FirstPersonCharacter > Prefabs folder), and run to ensure standard asset works.
2a) Change the Layer of the object to FPEPlayer (Click "Yes, change children" if prompted)
3) In the Inspector, add the following prefabs as children of the First Person Controller:
3a) FPEAudioDiaryPlayer prefab at offset location (0,0,0)
3b) FPEJournalSFX prefab at offset location (0,0,0)
4) In the Inspector in the First Person Controller, add the following prefabs as children of the Main Camera:
4a) ObjectPickupLocation prefab at offset location (0.39,-0.406,0.7)
4b) ObjectTossLocation prefab at offset location (0,-0.03,0.48)
4c) ObjectExamineLocation prefab at offset location (0,0,0.557)
4d) ExaminationCamera prefab at offset (0,0,0)
5) Set mouse sensitivity on the "Mouse Look (Script)" Component to be X Sensitivity = 5, Y Sensitivity = 5. Set Minimum X to -80, Maximum X to 80 (this controls look pitch).
6) Assign the Tag "Player" to the Player Controller Game Object
7) Run the scene, and move around.

>Custom Layers:
==============

There are 5 new Layers that are included in First Person Exploration Kit. It is possible that the Layers may not be imported correctly when you download the package. To confirm, check the Layers dropdown, or check under Edit > Project Settings > Tags and Layers, and ensure the following Layers are defined, in order, beginning with Layer 8:

FPEPutBackObjects
FPEPickupObjects
FPEPlayer
FPEObjectExamination
FPEIgnore

Finally, in Edit > Project Settings > Physics, ensure the checkbox for FPEPlayer and FPEPickupObjects is unchecked to prevent Player<->Pickup Object collisions. If you encounter weird physics behaviour, confirm that this checkbox is indeed unchecked.

For a further explanation of what these layers are for, please see the detailed section titled "Custom Layers Added for First Person Exploration Kit" below. 

Note:
----
If you have existing layers and cannot start the FPE Layers at Layer 8, please ensure the following prefabs have their layer variables updated:

>FPE Interaction Manager: Variables "Putback Layer Mask" and "Interaction Layer Mask". See Inspector Tooltips for instructions on setting the values to specific layers.


>Other First Person Exploration Kit Objects:
===========================================

Before you can start adding Interactable objects to your scene, add the following Prefabs:

1) FPEInteractionManager prefab
2) FPEUICamera prefab at position (0,0,0)
3) FPEEventSystem prefab at position (0,0,0)
4) In order to test that everything works as expected, also add the demoCardboardBox prefab (from the DemoPrefabs folder) to your scene.
5) Ensure the demoCardboardBox prefab is just above the ground so its put back location is generated correctly.
6) Run the scene, and test picking up, putting back, examining, and dropping the demoCardboardBox object.

If the demoCardboardBox test worked, you're done. If not, please double check the steps above and/or refer to the demoScene.unity Scene for further guidance.


Setup is now complete! Now that the scene is up and running, proceed to adding included interactable objects, or create your own. See below for details.


=====================
>Interactable Objects:
=====================

Overview:
--------
An Interactable object is an object that can be interacted with using First Person Exploration Kit. 

All Interactable objects have a flag called canInteractWithWhileHoldingObject. This flag controls how Static and Activate types behave when the player is holding an object in their hand. By default, Journal and Pickup type objects will always require both of the player's hands for interaction. See demoCabinet in the demo scene for an example of an Activate type whose interaction requires both of the player's hands (i.e. that the player is not holding any Pickup type objects).

There are several types of Interactable object:

>Base: This is the Base type or class, and should never be used on its own. (see FPEInteractableBaseScript)

>Activate: Activate type Interactable objects are objects that the player can activate. It is a child of Base type, but should also never be used directly. Please refer to DemoSwitchActivationScript in the DemoScripts folder for an example of how to create your own custom Activate type Interactable objects. DemoRadioSimpleScript and DemoRadioComplexScript show 2 more examples of things you can do with this type. The demoToilet prefab and associated scripts show how a even more complex Activate type object can be created. The demoToilet uses multiple interaction targets (flush handle, toilet seat) to manipulate different parts of the same model to perform distinct actions. The demoUnlocableDoor prefab shows how you can require a key or other inventory item to activate an object.

>Audio Diary: This type triggers an audio diary to be played when the player sees it for the first time. Refer to demoAudioDiary prefab in the demo scene for example use. (See also:  FPEInteractableAudioDiaryScript)

>Journal: This type houses journal pages, and when the player interacts with it, the journal opens for the player to read. Refer to demoJournalPaper prefab in the demo scene for example use. (See also:  FPEInteractableJournalScript)

>Pickup: This type of object allows the player to pick it up, examine it, put it back, and drop it. See "Put Back" directly below for more details. Note that a put back location is not always required. (Refer to demoBall in demo scene for example of this). Pickup objects can also be used to create complex scripted events, as shown with the "demoArtifactPickupObject" prefab in the demoScene. (See also:  FPEInteractablePickupScript)

->Put Back: This is not an Interactable type, but is related to the Pickup type. This script designates an area for the player to Put Back a Pickup type object. In the Pickup script, there is a flag to auto-generate Put Back locations (see demoSoup prefab). These locations can also be manually created if you wish for them to differ from the object's initial location in the world (See demoManuallyPlacedPutbackObjectForCardboardBox prefab in demo scene). This flexibility allows for a more interesting environment to explore and interact with. (See also:  FPEPutBackScript)

Note: Auto-generating Put Back locations from Mesh Colliders may result in weird scales and orientations, depending on which package was used to create the models, and how they were imported. This can happen if the imported Mesh Collider geometry orientation differs fron Unity's coordinate system orientation, or the object was scaled during import. If that's the case, either adjust mesh collider accordingly, or switch to a Box Collider or other native Collider.

>Static: This type is used for any static object in the game world that you also want to be discoverable or otherwise have context. For example, a painting on a wall is not something that the player can activate or pickup or use, but it may be significant to your game. Refer to demoPainting in demo scene for example use. (See also:  FPEInteractableStaticScript)

>Inventory: This type is used to allow the player to add an object to their inventory. This is useful for things like keys, access cards, clues, and other collectable objects. Unlike the other object types, they are consumed (put into inventory) when interacted with.

All types above have at least one example in the Demo Prefabs folder. Refer to the demo scene for further configuration details and example use.

All script variables and options are explained in the Inspector Tooltips and Headers.

==============================================
>Important Notes about Pickup Object Rotations:
==============================================

There are 4 rotation types for Pickup objects. They are:

>Free: Rotate freely in both X and Y axes

>Horizontal: Rotate in the horizontal axis only

>Vertical: Rotate in the vertical axis only

>None: Full lock, no rotation allowed (ideal for single sided objects like documents)

When setting up an Interactable object as a Pickup type, the Local Forward transform handle (blue arrow in scene editor) indicates the side of the object that will face the player when it is examined. For example, the demoFlatPaper prefab in the demo scene faces with Forward arrow pointing up away from the table it is sitting on. When the player picks the object up and examines it, the paper's front faces the camera. This is especially important for flat objects and those with the NONE rotation type.

=======================
>Interaction Management:
=======================

The FPEInteractionManager prefab contains the UI Canvas as well as FPEInteractionManagerScript. This is the core part of the player interaction with the various types of Interactable objects in the game world. Here is a breakdown of the child objects of FPEInteractionManager, and corresponding variable sections of FPEInteractionManagerScript:

>Reticle (Optional): An Image centered on screen that changes when the player highlights an Interactable object. See Reticle section of Inspector variables.
>InteractionTextLabel (Optional): A text object below the reticle that changes to reflect the current object the player is interacting with. See Reticle section of Inspector variables.
>CloseButton: A UI Canvas Image-based Button that closes the journal.
>PreviousButton: A UI Canvas Image-based Button that goes to previous journal page.
>NextButton: A UI Canvas Image-based Button that goes to next journal page.
>JournalBackground: A background image that appears below the journal pages. You can change it to be blank or a new image as desired.
>JournalPage: This UI Canvas Image object houses the sprite of the current journal page.
>AudioDiaryTitleLabel: This UI Canvas Text object houses the title of the currently playing Audio Diary and skip instruction, if there is one.
>Mouse(L/R)MBHelper(Icon/Text): These objects house the icons and text for Mouse Hints. These are optional, see Control Hints UI section of Inspector variables.

The FPEInteractionManager also has the FPEInventoryManagerScript attached. You can modify this script to add new inventory types to the game world.

=====================================
>Creating Custom Interactable Objects:
=====================================

To make your own custom Interactable objects, there are a few key elements to cover for each type:

Pickup:
------
1) Drag any 3D object into the scene. 
2) Ensure the Collider is either correctly scaled and rotated (e.g. if using imported Mesh Collider), or remove it completely.
3) Add a RigidBody
4) If Collider was removed in step 2, add a new collider (e.g. Box Collider)
5) Size the Collider (either imported, or added in step 4 above - e.g. Box Collider) to fit for the object you're making.
6) Run the scene, and see how the physics behave. Sometimes collider bounds don't quite match geometry. Adjust as needed.
7) Set the Layer of the object to be FPEPickupObjects
8) Add FPEInteractablePickupScript to your object (this will also add an AudioSource), and set the following variables to suit your object:
   >Interaction String: For example "Grab Coffee Mug"
   >Put Back String: For example "Set down Coffee Mug"
   >If the object's "home" location is the same as its start location, Check "Auto Generate Put Back Object". Note that sometimes this can behave oddly if imported Mesh Collider geometry orientation differs fron Unity's coordinate system orientation. If that's the case, either adjust mesh collider, or switch to a Box or other native Collider.
   >Set Rotation Type, Examination offsets, etc. as required. Sometimes play testing helps judge the best values for oddly shaped objects.
   >If Enable Sounds is checked, assign sounds if you wish. If no sounds are assigned, generic sounds will be used. This is fine for most small objects.
9) Run the scene, and interact with your new Pickup object.

Optional Optimization: If you know your new pickup object will NEVER use sound, you can uncheck "Enable Sounds" and also Remove the Audio Source component from the Game Object. This is generally not recommended, unless perhaps you are making a Marshmallow object that can only touch other marshmallows :)

Note: There are many things you can create with this object type. For an example of something pretty complex and fun, see demoArtifactPickupObject prefab (or complete "find the artifact" in the demo scene). You can hook up the pick up / put down / drop actions of a Pickup type object to any arbitrary scripts you wish.


Put Back (Manually Placed):
--------------------------
Sometimes it is necessary to have the put back location for an object be in a different location than its starting location (See demoArtifactPickupObject and demoCardboardBoxSpecial in demo scene). These are also optimal for complex or small meshes (see demoPencil in demo scene), as it can provide a large target for the player, and can prevent "pixel hunting".

Here are the steps to create a manually placed Put Back location:

1) Add an empty Game Object to the scene
2) Add Collider type of your choice and size accordingly (Box Collider recommended)
3) Add FPEPutBackScript to the object
4) In the Inspector, assign Scene object to My Pickup Object variable for the associated object you want to be put back in this location
5) Run the scene and test it out. If your object is rotated weirdly when put back, ensure that the Put Back object has the same rotation as its assigned "My Pickup Object".

Note: The transform of the Put Back object will guide the Pickup object when it is put back. So, for example, you could rotate the put back transform and when the pickup object is put back it will assume that rotation as well. It is recommended that the transform's location by slightly "up" from any physics objects to prevent physics clipping and odd reactions when the object is put back. This will prevent physics problems, and also nicely simulate the object being set down from the player's hand.


Journal:
-------
1) Drag any 3D object into the scene. 
2) Ensure the Collider is either correctly scaled and rotated (e.g. if using imported Mesh Collider), or remove it completely.
3) If Collider was removed in step 2, add a new collider (e.g. Box Collider)
4) Size the Collider (either imported, or added in step 3 above - e.g. Box Collider) to fit for the object you're making, with consideration to reticle aiming (e.g. if tiny scrap of paper, make collider big enough to get reticle over easily for the player)
5) Add FPEInteractableJournalScript to your object, and set the following variables to suit your object:
   >Interaction String: For example "Read crumpled note"
   >Journal Pages: Set size to at least 1, and assign corresponding Sprites. See Graphics Guide for making your own Journal Pages.
6) Run the scene, and interact with the Journal to read it.


Audio Diary:
-----------
1) Follow Journal steps above up to and including Step 4.
2) Add FPEInteractableAudioDiaryScript to your object and set the following variables to suit your object:
   >Interaction String: For example "Woah, what happened to that ship?"
   >Audio Diary Title: For example, "Captain's Log - We crash landed"
   >Audio Diary Clip: The audio clip for the diary. See audioDiaryDemo.ogg in Sounds folder for an example.
   >Post Playback Interaction String (Optional): For example, once the diary is played, the string would read "That's the captain's ship that crash landed"
3) Run the scene, and move the reticle over the new Audio Diary to trigger play back.

Static:
------
1) Follow Journal steps above up to and including Step 4.
2) Add FPEInteractableStaticScript to your object and set the following variables to suit your object:
   >Interaction String: For example "It's a picture of the old mill"
   >Optionally, for some objects it makes sense to un-check "Highlight on mouse over", and adjust Interaction Range. For example, see demoMoon and demoPainting in demo scene.
3) Run the scene, and interact with new static object.


Activate:
--------
Activate type Interactable objects can be as simple or complex as you want. The basic requirement is to create a new script/class as an extension of FPEInteractableActivateScript, and assign the appropriate Base type variables as seen in Static above (interactionString, etc.). You must implement the activate function, which will be triggered when the player interacts with your new script. This function can do anything you can imagine, as simply or complex as you wish. Please refer to DemoRadioComplexScript in the Demo Scripts folder, and the demoRadioComplex prefab in the demo scene for a good example of how to make custom Activate type scripts and objects.

Inventory:
---------
1) Follow Journal steps above up to and including Step 4.
2) Add FPEInteractableInventoryItemScript to your object and set the following variables to suit your object:
   >Interaction String: For example "Grab the key card."
   >Specify the Inventory Item Type (e.g. KEYCARD)
   >Optionally, specify a unique sound for this inventory item, or disable sounds by unchecking the "Enable Sounds" checkbox.
3) Run the scene, and interact with new static object.

Note: To add more Inventory Types to your game, simply extend the eInventoryItems enum inside FPEInventoryManagerScript.cs. For example, if you wanted 3 different keycard types and some batteries, it would look something like this:

public enum eInventoryItems { KEYCARD_A=0,KEYCARD_B=1,KEYCARD_C=2 };
private int inventoryItemCount = 3;

===============
>Graphics Guide:
===============

The following areas cover one or more graphics key to the UI of First Person Exploration Kit. If you wish to create your own custom graphics, please refer to the Photoshop templates included in the FirstPersonExplorationKit > Textures > Templates folder. The image types below can be assigned inside FPEInteractionManager to replace the default graphics in the prefab.

>Graphical Element Breakdown:
============================

Note:
----
The resolutions are a guideline only. You can make these different sizes if you wish.

Reticle(64x64): The center target "cursor" of the player's view. It is recommended that both an Inactive and Active version be used for a better experience.

Journal Background(800x450): The center static background image that appears under the Journal Pages.

Journal Buttons(64x64): There are three buttons. Previous Page, Next Page, and Close.

Journal Pages(773x1000): A portait orientation journal page layout. One required for each page of a journal.

Mouse Hint Icons(64x64): Two key images: Left Mouse Button and Right Mouse Button.

============
>Sound Guide:
============

There are 3 key sounds in FPEInteractionManager: JournalOpen, JournalClose, and JournalPageTurn. If you replace these sounds, be sure to keep the filenames the same as they are loaded at runtime from the Resources folder.

Each Pickup type Interactable object has 3 sound banks that can be optionally specified: Pickup Sounds, Put Back Sounds, and Impact Sounds. If more than one sound of a given type is specified, a random sound will be played from the sound bank.

Impact Sounds are played any time the there is a physics interaction with the object. For example if you drop the demoBall object in the demo, it plays a sound when the ball bounces. The sound's volume is adjusted based on the stength of the impact.

If no sounds are specified, generic sounds are loaded from the Resources folder. These sounds apply to all Pickup objects that have sounds enabled but no sounds specified.

=====================================================
>Custom Layers Added for First Person Exploration Kit:
=====================================================

There are 5 new Layers that are included in First Person Exploration Kit:

>FPEPutBackObjects: This layer is reserved for Put Back objects only. It ensures Put Back and Pickup game objects can have overlapping Colliders and still function properly. For example, the Put Back collider for small objects should be larger than the actual Pickup object collider to make it easier to put the object back (to prevent "pixel hunting").

>FPEPickupObjects: This layer is reserved for Pickup type objects. It is used to determine whether or not the reticle is highlighting an object that can be picked up.

>FPEPlayer: This layer houses the player and the player only. It prevents weird physics interactions with Interactables and the player controller.

Note: In Project Settings > Physics, the checkbox for FPEPlayer and FPEPickupObjects is unchecked to prevent Player<->Pickup Object collisions. If you encounter weird physics behaviour, confirm that this checkbox is indeed unchecked.

>FPEObjectExamination: This layer is used by the FPE Interaction Manager, and prevents depth clipping and other weirdness when you are examining a Pickup type object.

>FPEIgnore: This layer is reserved for any special Colliders or Trigger Colliders you may need that intersect with Pickup and Put Back object Colliders. For example, if you need to detect if a pickup game object was put back in a specific location to trigger some game events. See demoManuallyPlacedPutbackObjectForCardboardBox and demoBoxTriggerIndicator in demo scene for example use. The player can "look through" the collider for demoBoxTriggerIndicator, and still "see" the put back location for the special cardboard box.


==========================================================================
>Using another Unity Asset for your Character Controller (e.g. UFPS, etc.):
==========================================================================

If you wish to use another Character Controller prefab or Unity Asset (or write/use your own custom scripts for player control), the following functions may need to change inside FPEInteractionManagerScript. They are all very straightforward, and easy to "plumb in" to any other control scripts you may want to use.

rememberStartingMouseSensitivity(): This function is called on Start(), and simply records the mouse sensitivity on scene start.
----------------------------------

setMouseSensitivity(Vector2 sensitivity): This function simply updates the mouse sensitivity values as specified in the Vector2 argument's x/y values, and sets smoothMouseChange to false.
----------------------------------------

restorePreviousMouseSensitivity(bool smoothTransition): This function restores mouse sensitivity to the starting sensitivity, and sets smoothMouseChange to the value of the smoothTransition argument.
------------------------------------------------------

disableMouseLook(): This function disables mouse look. It is used to keep player view locked when examining an object or reading a journal.
------------------

enableMouseLook(): This function enables mouse look.
-----------------

disableMovement(): This function disables all "WASD" or other non-mouse movement. It keeps the player location in the game world static when examining an object or reading a journal.
-----------------

enableMovement(): This function enables player movement.
----------------

isMouseLookEnabled(): This function simply returns bool to indicate if mouse look is currently enabled or not
--------------------

Note: 
----
The smoothMouseChange boolean flag is used to smooth the transition if you're using "slow mouse" on object highlight. This helps the player target small game objects. The smoothing is in place to reduce any jarring feeling when the player moves their reticle away from the highlighted object. setMouseSensitivity() and restorePreviousMouseSensitivity() set this value, so any replacement functions you write/swap-in must also do this in order to preserve this smoothing functionality, if you wish to keep it.


===================================
> Using XBox 360 or similar gamepad:
===================================

The package contains Input definitions and code to handle basic XBox 360 controller support. Please refer to GamepadUpdateInstructions.html for a full breakdown of the Input definitions and code changes from v1.0 that handle this functionality.

====================
>Other Package Notes:
====================

The following files are provided to demonstrate examples and as a base to create your own game. However, you can discard them if you don't need them.

-All prefabs in the Prefabs > DemoPrefabs folder (e.g. "demoSoup") 
-All assets in the Materials > DemoMaterials folder
-All scripts in the Scripts > DemoScripts folder
-All textures in the Textures > DemoTextures folder
-All models and materials in the Models folder and sub folders
-All sounds in the Sounds folder

Feel free to delete the above items as you see fit.

Note: The Resources folder contains 4 sounds and 1 Material. These are critical for the package to function correctly. However, you may modify or replace them so long as the filename is not changed.




If you have any problems using this package, or have tweaks or new features you'd like to see included, please let me know.

Thanks,
Richard

@whilefun
info@whilefun.com



THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
