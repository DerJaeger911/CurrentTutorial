extends Sprite2D
class_name PlayerMover

var move_dist : float = 15.0

func move (dir : Vector2):
	global_position += dir * move_dist

func adjust_scale (amount : float):
	scale.x += amount
	scale.y += amount
