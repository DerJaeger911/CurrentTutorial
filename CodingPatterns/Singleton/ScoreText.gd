extends Label

#@onready var score_manager = $"../ScoreManager"

func _process(delta):
	text = "Score: " + str(ScoreManager.score)
