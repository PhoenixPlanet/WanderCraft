using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH.Core {

	namespace Constants {
		namespace ResourcesPath {
			public static class Prefabs {
				public const string CENTER_BLOCK_PREFAB_PATH = "Prefabs/CenterBlock";
				public const string SIDE_BLOCK_PREFAB_PATH = "Prefabs/SideBlockVarient/DiningFish";
				public const string BLOCK_MOUSE_SENSOR_PREFAB_PATH = "Prefabs/BlockMouseSensor";
				public const string BUILDING_LEVEL_PREFAB_PATH = "Prefabs/BuildingLevel";
			}

			public static class Materials {
				public const string BLOCK_NORMAL_MATERIAL_PATH = "Materials/BlockNormal";
				public const string BLOCK_TRANSPARENT_MATERIAL_PATH = "Materials/BlockTransparent";

				public static class BlockColor {
					public const string BLOCK_MEAT_0 = "Materials/BlockIntensity/Meat/Meat0";
					public const string BLOCK_MEAT_1 = "Materials/BlockIntensity/Meat/Meat1";
					public const string BLOCK_MEAT_2 = "Materials/BlockIntensity/Meat/Meat2";
					public const string BLOCK_MEAT_3 = "Materials/BlockIntensity/Meat/Meat3";

					public const string BLOCK_FISH_0 = "Materials/BlockIntensity/Fish/Fish0";
					public const string BLOCK_FISH_1 = "Materials/BlockIntensity/Fish/Fish1";
					public const string BLOCK_FISH_2 = "Materials/BlockIntensity/Fish/Fish2";
					public const string BLOCK_FISH_3 = "Materials/BlockIntensity/Fish/Fish3";

					public const string BLOCK_PLANT_0 = "Materials/BlockIntensity/Plant/Plant0";
					public const string BLOCK_PLANT_1 = "Materials/BlockIntensity/Plant/Plant1";
					public const string BLOCK_PLANT_2 = "Materials/BlockIntensity/Plant/Plant2";
					public const string BLOCK_PLANT_3 = "Materials/BlockIntensity/Plant/Plant3";

					public static readonly string[] MEAT = {
                        BLOCK_MEAT_0,
						BLOCK_MEAT_1,
						BLOCK_MEAT_2,
						BLOCK_MEAT_3,
					};

					public static readonly string[] FISH = {
						BLOCK_FISH_0,
						BLOCK_FISH_1,
						BLOCK_FISH_2,
						BLOCK_FISH_3,
					};

					public static readonly string[] PLANT = {
						BLOCK_PLANT_0,
						BLOCK_PLANT_1,
						BLOCK_PLANT_2,
						BLOCK_PLANT_3,
					};

					public static readonly Dictionary<ESourceType, string[]> INTENSITY = new Dictionary<ESourceType, string[]>() {
						{ESourceType.Meat, MEAT},
						{ESourceType.Fish, FISH},
						{ESourceType.Plant, PLANT},
					};
				}
			}
		}

		namespace Helper {
			public static class Organizer {
				public const string CENTER_BLOCK_PARENT_NAME = "=========== CenterBlockParent ===========";
				public const string FLOATING_BLOCK_PARENT_NAME = "=========== FloatingBlockParent ===========";
			}
		}

		namespace Colors {
			public static class BlockLinking {
				public readonly static Color LINE_COLOR = Color.red; 
			}
		}

		namespace GameSetting {
			public static class SourceProduction {
				public readonly static Dictionary<ESourceType, int> SOURCE_PRODUCTION = new Dictionary<ESourceType, int>() {
					{ESourceType.Meat, 1},
					{ESourceType.Fish, 2},
					{ESourceType.Plant, 4},
				};
			}
		}
	}

}