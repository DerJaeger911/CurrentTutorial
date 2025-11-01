extends Command
class_name ScaleCommand

var player_mover : PlayerMover
var scale : float

static func create (pm : PlayerMover, s : float) -> ScaleCommand:
	var i = ScaleCommand.new()
	i.player_mover = pm
	i.scale = s
	return i

func execute ():
	player_mover.adjust_scale(scale)

func undo ():
	player_mover.adjust_scale(-scale)
