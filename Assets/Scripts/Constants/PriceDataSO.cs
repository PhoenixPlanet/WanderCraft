using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TH.Core {

[ManageableData]
[CreateAssetMenu(fileName = "PriceDataSO", menuName = "SO/PriceDataSO", order = 1)]
public class PriceDataSO : ScriptableObject
{
    #region PublicVariables
	[SerializeField]
	public List<PropertyData> _priceList = new List<PropertyData>();
	#endregion

	#region PrivateVariables
	#endregion

	#region PublicMethod
	#endregion
    
	#region PrivateMethod
	#endregion
}

[System.Serializable]
public class PropertyData {
		public int red;
		public int green;
		public int blue;
		
		public int money;

		public PropertyData(int red, int blue, int green, int money) {
			this.red = red;
			this.blue = blue;
			this.green = green;
			this.money = money;
		}

		public static bool operator >=(PropertyData a, PropertyData b) {
			return a.red >= b.red && a.blue >= b.blue && a.green >= b.green && a.money >= b.money;
		}

		public static bool operator <=(PropertyData a, PropertyData b) {
			return a.red <= b.red && a.blue <= b.blue && a.green <= b.green && a.money <= b.money;
		}

		public static bool operator <(PropertyData a, PropertyData b) {
			return a.red < b.red || a.blue < b.blue || a.green < b.green || a.money < b.money;
		}

		public static bool operator >(PropertyData a, PropertyData b) {
			return a.red > b.red && a.blue > b.blue && a.green > b.green && a.money > b.money;
		}

		public static bool operator ==(PropertyData a, PropertyData b) {
			return a.red == b.red && a.blue == b.blue && a.green == b.green && a.money == b.money;
		}

		public static bool operator !=(PropertyData a, PropertyData b) {
			return a.red != b.red || a.blue != b.blue || a.green != b.green || a.money != b.money;
		}

		public static PropertyData operator +(PropertyData a, PropertyData b) {
			return new PropertyData(a.red + b.red, a.blue + b.blue, a.green + b.green, a.money + b.money);
		}

		public static PropertyData operator -(PropertyData a, PropertyData b) {
			return new PropertyData(a.red - b.red, a.blue - b.blue, a.green - b.green, a.money - b.money);
		}

		public static PropertyData operator *(PropertyData a, int b) {
			return new PropertyData(a.red * b, a.blue * b, a.green * b, a.money * b);
		}

		public static PropertyData operator *(int b, PropertyData a) {
			return new PropertyData(a.red * b, a.blue * b, a.green * b, a.money * b);
		}

		public static PropertyData operator /(PropertyData a, int b) {
			return new PropertyData(a.red / b, a.blue / b, a.green / b, a.money / b);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			PropertyData other = (PropertyData)obj;
			return red == other.red && blue == other.blue && green == other.green && money == other.money;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + red.GetHashCode();
				hash = hash * 23 + blue.GetHashCode();
				hash = hash * 23 + green.GetHashCode();
				hash = hash * 23 + money.GetHashCode();
				return hash;
			}
		}

		public static PropertyData Zero => new PropertyData(0, 0, 0, 0);

		public PropertyData SetProperty(ESourceType sourceType, int value) {
			switch (sourceType) {
				case ESourceType.Red:
					red = value;
					break;
				case ESourceType.Blue:
					blue = value;
					break;
				case ESourceType.Green:
					green = value;
					break;
				default:
					break;
			}

			return this;
		}

		public int GetPropertyValue(ESourceType sourceType) {
			switch (sourceType) {
				case ESourceType.Red:
					return red;
				case ESourceType.Blue:
					return blue;
				case ESourceType.Green:
					return green;
				default:
					return 0;
			}
		}
	}

}