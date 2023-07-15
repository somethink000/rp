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
		public override string Model => "models/money_printer/money_printer.vmdl"; 



		public override void Spawn()
		{
			base.Spawn();
			PhysicsEnabled = true;
			UsePhysicsCollision = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;
			Tags.Add( "prop", "solid" );
		
			
			PlaySound( "mone" );

		}





	}
}
