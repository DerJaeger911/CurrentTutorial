extends CharacterBody2D

@export var move_speed : float = 50.0
@export var fire_rate : float = 0.05
var last_fire_time : float

@onready var sprite : Sprite2D = get_node("Sprite")
#@onready var projectile_scene = preload("res://Object Pooling/Projectile.tscn")
@onready var projectile_pool = $ObjectPool_Projectile

func _physics_process(delta):
	# player movement
	var dir = Input.get_vector("ui_left", "ui_right", "ui_up", "ui_down")
	velocity = dir * move_speed
	
	# flip sprite based on movement
	if dir.x > 0:
		sprite.flip_h = false
	elif dir.x < 0:
		sprite.flip_h = true
	
	move_and_slide()

func _process(delta):
	# try fire a projectile when left mouse button is pressed
	if Input.is_mouse_button_pressed(MOUSE_BUTTON_LEFT):
		_try_fire_projectile() 

func _try_fire_projectile ():
	# return if we're trying to fire too early
	if Time.get_unix_time_from_system() - last_fire_time < fire_rate:
		return
	
	last_fire_time = Time.get_unix_time_from_system()
	
	# calculate mouse direction from player
	var mouse_dir = position.direction_to(get_global_mouse_position())
	
	# spawn the projectile
	#var proj = projectile_scene.instantiate()
	#get_tree().get_root().add_child(proj)
	
	var proj = projectile_pool.spawn()
	
	# set position and rotation
	proj.global_position = global_position
	proj.rotation = mouse_dir.angle() + deg_to_rad(90 + randf_range(-5, 5))
