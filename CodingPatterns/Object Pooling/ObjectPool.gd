extends Node

@export var scene_to_spawn : PackedScene
@export var initial_amount : int

var pool : Array = []

func _ready():
	for i in range(initial_amount):
		_create_new_node()

func _create_new_node ():
	var node = scene_to_spawn.instantiate()
	get_tree().get_root().add_child.call_deferred(node)
	node.visible = false
	pool.append(node)
	return node

func spawn ():
	var node = null
	
	for i in len(pool):
		if pool[i].visible == false:
			node = pool[i]
			break
	
	if node == null:
		node = _create_new_node()
	
	node.visible = true
	return node
