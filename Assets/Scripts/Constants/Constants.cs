using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH.Core {

	namespace Constants {
		namespace ResourcesPath {
			public static class Prefabs {
				public const string CENTER_BLOCK_PREFAB_PATH = "Prefabs/CenterBlock";
				public const string SIDE_BLOCK_PREFAB_PATH = "Prefabs/SideBlock";
				public const string BLOCK_MOUSE_SENSOR_PREFAB_PATH = "Prefabs/BlockMouseSensor";
				public const string BUILDING_LEVEL_PREFAB_PATH = "Prefabs/BuildingLevel";
			}

			public static class Materials {
				public const string BLOCK_NORMAL_MATERIAL_PATH = "Materials/BlockNormal";
				public const string BLOCK_TRANSPARENT_MATERIAL_PATH = "Materials/BlockTransparent";
			}
		}

		namespace Helper {
			public static class Organizer {
				public const string CENTER_BLOCK_PARENT_NAME = "=========== CenterBlockParent ===========";
				public const string SIDE_BLOCK_PARENT_NAME = "=========== SideBlockParent ===========";
			}
		}

		namespace Colors {
			public static class BlockLinking {
				public readonly static Color LINE_COLOR = Color.red; 
			}
		}
	}

}