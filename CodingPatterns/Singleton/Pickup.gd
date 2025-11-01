extends Area2D

func _process(delta):
	# bob up and down over time
	var val = sin(Time.get_unix_time_from_system() * 5)
	position.y += val * delta * 5

func _on_body_entered(body):
	# has the player hit the pickup?
	if body.is_in_group("Player"):
		#$"../../ScoreManager".score += 1
		ScoreManager.score += 1
		queue_free()
