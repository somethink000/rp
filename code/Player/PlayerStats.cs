using Sandbox;
using System;
using System.Collections.Generic;

namespace FPSGame
{
	public partial class FPSPlayer
    {
		[Net] public float Armor { get; set; }
		[Net] public int MaxArmor { get; set; }


		[Net] public int Hunger { get; set; }

		[Net] public int Money { get; set; }


		

		
		
	}
}
