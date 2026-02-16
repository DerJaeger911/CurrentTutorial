shader_type spatial;
render_mode cull_disabled;

uniform vec3 color: source_color;
uniform vec3 shadow_color: source_color;

uniform sampler2D curve_sampler: repeat_disable;
uniform sampler2D wind_sampler: repeat_enable, filter_linear;

void vertex() {
	vec2 world_pos = vec2(NODE_POSITION_WORLD.x, NODE_POSITION_WORLD.z);
	float curve = texture(curve_sampler, vec2(UV.y, 0.0)).x;
	float wind = texture(wind_sampler, world_pos * 0.08 + TIME * 0.05).x - 0.5;
	VERTEX += vec3(wind * curve);
}

void fragment() {
	ALBEDO = mix(color, shadow_color, UV.y);
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
