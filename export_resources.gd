@tool
extends EditorScript

func _run():
	print("Exporting Ironclad card images...")
	
	var pck_path = "G:/SteamLibrary/steamapps/common/Slay the Spire 2/SlayTheSpire2.pck"
	var output_dir = "F:/personal/game_t/firefly/reference/ironclad_cards"
	
	# 创建输出目录
	DirAccess.make_dir_recursive_absolute(output_dir)
	
	# 尝试加载 PCK
	var success = ProjectSettings.load_resource_pack(pck_path)
	print("Load PCK: ", success)
	
	if success:
		# 查找卡牌资源
		var card_paths = [
			"res://assets/cards/ironclad/",
			"res://assets/images/cards/",
			"res://textures/cards/"
		]
		
		for base_path in card_paths:
			print("Checking: ", base_path)
			_export_directory(base_path, output_dir)

func _export_directory(path: String, output_dir: String):
	var dir = DirAccess.open(path)
	if dir:
		dir.list_dir_begin()
		var file_name = dir.get_next()
		while file_name != "":
			if dir.current_is_dir():
				_export_directory(path + file_name + "/", output_dir)
			else:
				if file_name.ends_with(".png") or file_name.ends_with(".jpg"):
					print("Found: ", path + file_name)
					# 加载并保存
					var img = load(path + file_name)
					if img:
						var output_path = output_dir + "/" + file_name
						img.save_png(output_path)
						print("Exported: ", output_path)
			file_name = dir.get_next()
		dir.list_dir_end()
