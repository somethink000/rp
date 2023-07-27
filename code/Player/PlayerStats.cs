using Sandbox;
using System;
using System.Collections.Generic;

namespace MyGame
{
	public partial class Player
    {
		[Net] public float Armor { get; set; }
		[Net] public int MaxArmor { get; set; }


		[Net] public int Hunger { get; set; }

		[Net] public int Money { get; set; }


		public float TakeArmor( float amount )
		{
			if ( Armor == null ) return 0;

			var available = Armor;
			amount = Math.Min( available, amount );

			Armor = available - amount;
			return amount;
		}

	}
}
