extends CharacterBody2D

@export var move_speed : float = 50.0
@onready var sprite : Sprite2D = get_node("Sprite")

func _process(delta):
	# reload the scene when ENTER is pressed
	if Input.is_action_just_pressed("ui_accept"):
		get_tree().reload_current_scene()

func _physics_process(delta):
	# player movement
	var dir = Input.get_vector("ui_left", "ui_right", "ui_up", "ui_down")
	velocity = dir * move_speed
	move_and_slide()
	
	# flip sprite based on movement
	if dir.x > 0:
		sprite.flip_h = false
	elif dir.x < 0:
		sprite.flip_h = true
