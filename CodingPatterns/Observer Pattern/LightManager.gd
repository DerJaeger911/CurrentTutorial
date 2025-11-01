extends Node

signal turn_on_lights
signal turn_off_lights

func _process(delta):
	if Input.is_action_just_pressed("ui_up"):
		turn_on_lights.emit()
	
	if Input.is_action_just_pressed("ui_down"):
		turn_off_lights.emit()
