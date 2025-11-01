extends Node

var undo_stack : Array[Command] = []
var redo_stack : Array[Command] = []

@onready var player : PlayerMover = $"../Player"

func _process(delta):
	if Input.is_action_just_pressed("ui_up"):
		execute_command(MoveCommand.create(player, Vector2(0, -1)))
	if Input.is_action_just_pressed("ui_down"):
		execute_command(MoveCommand.create(player, Vector2(0, 1)))
	if Input.is_action_just_pressed("ui_left"):
		execute_command(MoveCommand.create(player, Vector2(-1, 0)))
	if Input.is_action_just_pressed("ui_right"):
		execute_command(MoveCommand.create(player, Vector2(1, 0)))
	
	if Input.is_action_just_pressed("ui_accept"):
		execute_command(ScaleCommand.create(player, 0.2))
	
func execute_command (cmd : Command):
	cmd.execute()
	undo_stack.push_front(cmd)
	redo_stack.clear()

func undo_command ():
	if len(undo_stack) == 0:
		return
	
	var cmd = undo_stack.pop_front()
	redo_stack.push_front(cmd)
	cmd.undo()

func redo_command ():
	if len(redo_stack) == 0:
		return
	
	var cmd = redo_stack.pop_front()
	undo_stack.push_front(cmd)
	cmd.execute()
