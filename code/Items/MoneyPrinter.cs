using System;
using FPSGame;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

namespace FPSGame.Items
{
	


	partial class MoneyPrinter : Item
	{
		public override string Model => "models/basemodel.vmdl";



		public override void Spawn()
		{
			base.Spawn();

			PlaySound( "gun/mone" );

		}





	}
}
