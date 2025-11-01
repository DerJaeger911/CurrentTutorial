extends Area2D

var speed : float = 300.0

func _process(delta):
	translate(-transform.y * speed * delta)

# called when the Timer node timeouts
func end_of_lifetime ():
	#queue_free()
	visible = false
	print("Timer timeout")

func _on_visibility_changed():
	if visible == true:
		$DestroyTimer.start.call_deferred()
