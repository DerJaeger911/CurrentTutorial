extends Command
class_name MoveCommand

var player_mover : PlayerMover
var move_dir : Vector2

static func create (pm : PlayerMover, dir : Vector2) -> MoveCommand:
	var i = MoveCommand.new()
	i.player_mover = pm
	i.move_dir = dir
	return i

func execute ():
	player_mover.move(move_dir)

func undo ():
	player_mover.move(-move_dir)
