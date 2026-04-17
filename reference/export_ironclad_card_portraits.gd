extends SceneTree

const PCK_PATH := "G:/SteamLibrary/steamapps/common/Slay the Spire 2/SlayTheSpire2.pck"
const SOURCE_DIR := "res://images/packed/card_portraits/ironclad"
const OUTPUT_DIR := "F:/personal/game_t/firefly/reference/ironclad_card_portraits"


func _initialize() -> void:
	var result := {
		"loaded_pack": false,
		"discovered": 0,
		"exported": 0,
		"failed": [],
	}

	result.loaded_pack = ProjectSettings.load_resource_pack(PCK_PATH)
	if not result.loaded_pack:
		push_error("Failed to load PCK: %s" % PCK_PATH)
		_write_report(result)
		quit(1)
		return

	DirAccess.make_dir_recursive_absolute(OUTPUT_DIR)

	var files := _discover_source_files()
	result.discovered = files.size()

	for source_path in files:
		var texture := ResourceLoader.load(source_path)
		if texture == null or not (texture is Texture2D):
			result.failed.append({"path": source_path, "reason": "load_failed"})
			continue

		var image := (texture as Texture2D).get_image()
		if image == null:
			result.failed.append({"path": source_path, "reason": "image_missing"})
			continue

		var output_path := OUTPUT_DIR.path_join(source_path.get_file())
		var save_err := image.save_png(output_path)
		if save_err != OK:
			result.failed.append({"path": source_path, "reason": "save_failed", "code": save_err})
			continue

		result.exported += 1

	_write_report(result)
	print(JSON.stringify(result))
	quit(0 if result.failed.is_empty() else 2)


func _discover_source_files() -> PackedStringArray:
	var dir := DirAccess.open(SOURCE_DIR)
	if dir == null:
		return PackedStringArray()

	var files: PackedStringArray = []
	dir.list_dir_begin()
	while true:
		var name := dir.get_next()
		if name.is_empty():
			break
		if dir.current_is_dir():
			continue
		if not name.ends_with(".png.import"):
			continue
		files.append(SOURCE_DIR.path_join(name.trim_suffix(".import")))
	dir.list_dir_end()
	files.sort()
	return files


func _write_report(result: Dictionary) -> void:
	var report_path := OUTPUT_DIR.path_join("_export_report.json")
	var file := FileAccess.open(report_path, FileAccess.WRITE)
	if file == null:
		push_error("Failed to write report: %s" % report_path)
		return
	file.store_string(JSON.stringify(result, "\t"))
	file.close()
