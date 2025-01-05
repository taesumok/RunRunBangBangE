-------------------------------------
2D Plane Shooting Starter (Unity2D)
Version 1.0.0
Copyright 2014 RonnieJ
Jangseyun@gmail.com
-------------------------------------

Thank you for buying 2D Plane Shooting Starter (Unity2D).
If you have any questions, suggestions, comments or feature requests, please contact to jangseyun@gmail.com.

-------------------------------------
NOTE
-------------------------------------
This package based on Unity2D so you should use Unity 4.3.

--------------------------------------
This Package includes:
--------------------------------------
- Resources folder : Includes all sprite textures, audio clips and prefabs used this game.

- Simple_GUI folder
  = Button_UI (Button_UI.cs) : provides color change when hover, click of touch event occurs, scale change when click of touch event occurs.
				   SeneMessages to the function of the message target.
  = Font_UI (Font_UI.cs) : based on dynamic font system of the Unity. No need to use Bitmap font texture for label.
  = Slider (UISlider.cs) : can be used for gauge ui. based on Unity2D.
			      pivots should be set ¡°Left¡± for Horizontal slide.
			      pivots should be set ¡°Top¡± for Vertical slide.

- Scripts folder
  = GameManager (GameManager.cs) : Spawns enemies and deals with game starts and ends.
  = ScoreManager (ScoreManager.cs) : Manages score and coin count.
  = Resource Pool (ResourcePool.cs) : Manages resources for resource reuse. This is based on the name of each resources.
  = Player
    - Control (PlayerMover.cs) : player plane is controlled by your dragging. Collision detection with enemies.
    - Shoot bullet (PlayerShoot.cs) : shoots bullets.
  = Enemies
    - Spawned by GameManager with Random position between (-3.5f, 7f, 0f) ~ (3.5f, 7f, 0f).
    - Move (EnemyMove.cs) : move speed is set between 20 ~ 25 when enemy object enabled. 
				collision detection with player¡¯s bullet.
    - Shoot Bullet (EnemyShoot.cs) : shoots bullets every ¡°RepeatTime¡± value.

---------------------------------------
Comments.
--------------------------------------
This package is based on Unity2D so you should use Unity 4.3 or upper version of Unity.
This Package is executable on all platform.

Tutorial Scenes will be updated soon.

---------------------------------------Version Infomation---------------------------------------Initial Version : First Released.
